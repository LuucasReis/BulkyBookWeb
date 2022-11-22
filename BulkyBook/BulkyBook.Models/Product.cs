using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{ 
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Título")]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Descrição")]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Display(Name ="Autor")]
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        [Display(Name ="Preço da Lista")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1,10000)]
        [Display(Name ="Preço")]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name ="Preço para 50+")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name ="Preço para 100+")]
        public double Price100 { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [Required]
        [Display(Name ="Categoria")]
        public int CategoryID { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        [Display(Name ="Cover Type")]
        public int CoverTypeID { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }

        public Product()
        { 
        }
        public Product(string name, string description, string iSBN, string author, double listPrice, double price, double price50, double price100, string imageUrl, Category category, CoverType coverType)
        {
            Name = name;
            Description = description;
            ISBN = iSBN;
            Author = author;
            ListPrice = listPrice;
            Price = price;
            Price50 = price50;
            Price100 = price100;
            ImageUrl = imageUrl;
            Category = category;
            CoverType = coverType;
        }
    }
}
