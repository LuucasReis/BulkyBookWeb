using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.Web.Helpers;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviroment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
            };

            if (id == null || id == 0)
            {
                //Criar Produto
                return View(productVM);
            }
            else
            {
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnviroment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"Images\Products");
                    var extensions = Path.GetExtension(file.FileName);
                    if(obj.product.ImageUrl != null)
                    {
                        var OldImage = Path.Combine(wwwrootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(OldImage))
                        {
                            System.IO.File.Delete(OldImage);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, fileName + extensions), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.product.ImageUrl = @"\Images\Products\" + fileName + extensions;

                }
                if (obj.product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.product);
                    TempData["success"] = "Product created sucessfully";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                    TempData["success"] = "Product updated sucessfully";
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data=productlist});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(x=>x.Id == id);
            if(obj == null )
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var OldImage = Path.Combine(_hostEnviroment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldImage))
            {
                System.IO.File.Delete(OldImage);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product Deleted Sucessfully" });
        }

        #endregion

    }
}
