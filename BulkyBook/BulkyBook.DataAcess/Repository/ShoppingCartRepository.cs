using BulkyBook.DataAcess.Data;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count -= value;
            return shoppingCart.Count;
        }
        public int IncrementCount(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count += value;
            return shoppingCart.Count;
        }
    }
}
