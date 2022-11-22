using BulkyBook.DataAcess.Data;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(OrderHeader obj)
        {
            _dbContext.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderstatus, string? paymentStatus = null)
        {
            var order = _dbContext.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(order != null)
            {
                order.OrderStatus = orderstatus;
                if(paymentStatus != null)
                {
                    order.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentInentId)
        {
            var order = _dbContext.OrderHeaders.FirstOrDefault(x => x.Id == id);
            order.PaymentDate = DateTime.Now;
            order.SessionId = sessionId;
            order.PaymentIntentId = paymentInentId;

        }
    }
}
