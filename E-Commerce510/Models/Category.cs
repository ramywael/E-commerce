using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce510.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="لا يمكن ان يكون عدد الاحرف اقل من 3")]
        [MaxLength(100)]
        public string Name { get; set; }

        [ValidateNever]
        public List<Product> Products { get; set; }
    }
}
