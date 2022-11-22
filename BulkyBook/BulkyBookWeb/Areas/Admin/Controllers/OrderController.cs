using BulkyBook.DataAcess.Repository;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Details(int orderId)
        {
            orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderId == orderId, includeProperties: "Product")
            };
            return View(orderVM);
        }
        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PayNow()
        {
            orderVM.OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            orderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderId == orderVM.OrderHeader.Id, includeProperties: "Product");


            //StripeSettings
            var domain = "https://localhost:44304/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={orderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={orderVM.OrderHeader.Id}",
            };

            foreach (var item in orderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {

                        UnitAmount = (long)item.Price * 100, //Multiplicar o preço por 100 e fazer o cast pra long
                        Currency = "brl",
                        ProductData = new SessionLineItemPriceDataProductDataOptions

                        {
                            Name = item.Product.Name,
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(orderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderHeaderId);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeaderId, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(orderHeaderId);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);

            orderHeaderDb.Name = orderVM.OrderHeader.Name;
            orderHeaderDb.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            orderHeaderDb.StreetAddress = orderVM.OrderHeader.StreetAddress;
            orderHeaderDb.City = orderVM.OrderHeader.City;
            orderHeaderDb.State = orderVM.OrderHeader.State;
            orderHeaderDb.PostalCode = orderVM.OrderHeader.PostalCode;

            if (orderVM.OrderHeader.Carrier != null)
            {
                orderHeaderDb.Carrier = orderVM.OrderHeader.Carrier;
            }
            if (orderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderDb);
            _unitOfWork.Save();
            TempData["success"] = "Detalhes do Pedido atualizados com sucesso!!";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderDb.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Pedido processando com sucesso!!";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.OrderHeader.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
            orderHeaderDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            orderHeaderDb.Carrier = orderVM.OrderHeader.Carrier;
            orderHeaderDb.OrderStatus = SD.StatusShipped;
            orderHeaderDb.ShippingDate = DateTime.Now;
            if (orderHeaderDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeaderDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeader.Update(orderHeaderDb);
            _unitOfWork.Save();
            TempData["success"] = "Pedido enviado com sucesso!!";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderDb.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
            if (orderHeaderDb.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderDb.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderDb.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Pedido cancelado com sucesso!!";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderDb.Id });
        }


        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimIdenytity = (ClaimsIdentity)User.Identity;
                var claim = claimIdenytity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;

                case "inprocess":
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == SD.StatusInProcess);
                    break;

                case "completed":
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == SD.StatusShipped);
                    break;

                case "approved":
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == SD.StatusApproved);
                    break;

                default:
                    break;
            }

            return Json(new {data=orderHeaders});
        }
        #endregion
    }
}
