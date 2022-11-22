using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		[Display(Name ="Endereço")]
		public string? StreetAddress { get; set; }
		[Display(Name ="Cidade")]
		public string? City { get; set; }
		[Display(Name ="Estado")]
		public string? State { get; set; }
		[Display(Name ="CEP")]
		public string? PostalCode { get; set; }
		public int? CompanyId { get; set; }
		[ForeignKey("CompanyId")]
		[ValidateNever]
		public Company Company { get; set; }
	}
}
