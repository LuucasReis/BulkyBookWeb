using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimal caracteres required: {2}")]
        [Display(Name ="Nome")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "{0} must be between {1} and {2} only")]
        public int DisplayOrder { get; set; }
        [Display(Name ="Data de Criação")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public Category()
        {
        }
        public Category(string name, int displayOrder)
        {
            Name = name;
            DisplayOrder = displayOrder;
        }


    }
}
