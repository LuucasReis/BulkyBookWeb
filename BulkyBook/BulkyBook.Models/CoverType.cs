using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display (Name ="Cover Type")]
        [StringLength (50, MinimumLength = 3, ErrorMessage ="{0} caracteres need to be between {1} and {2}")]
        public string Name { get; set; }

        public CoverType()
        {
        }
        public CoverType(string name)
        {
            Name = name;
        }
    }
}
