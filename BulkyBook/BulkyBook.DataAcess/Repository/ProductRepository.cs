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
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext _context) : base(_context)
        {
                _dbContext = _context;
        }
        public void Update(Product product)
        {
            Product obj = _dbContext.Products.FirstOrDefault(x => x.Id == product.Id);
            if(obj != null)
            {
                obj.Name = product.Name;
                obj.Description = product.Description;
                obj.Price = product.Price;
                obj.Price50 = product.Price50;
                obj.ListPrice = product.ListPrice;
                obj.Price100 = product.Price100;
                obj.CategoryID = product.CategoryID;
                obj.Author = product.Author;
                obj.CoverTypeID = product.CoverTypeID;
                obj.ISBN = product.ISBN;
                if(product.ImageUrl != null)
                {
                    obj.ImageUrl = product.ImageUrl.ToString();
                }
            }
        }
    }
}
