using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BulkyBook.Models
{
	public class Company
	{
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Nome")]
        public string Name { get; set; }
        [Display(Name = "Endereço")]
        public string? StreetAddress { get; set; }
        [Display(Name = "Cidade")]
        public string? City { get; set; }
        [Display(Name = "Estado")]
        public string? State { get; set; }
        [Display(Name = "CEP")]
        public string? PostalCode { get; set; }

        public Company()
        {
        }
        public Company(string name, string? streetAddress, string? city, string? state, string? postalCode)
        {
            Name = name;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
        }
    }
}

