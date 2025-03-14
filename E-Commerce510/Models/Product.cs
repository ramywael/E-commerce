using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce510.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0, 1000000)]
        public double Price { get; set; }
        [ValidateNever]
        public string Img { get; set; }
        [Range(1,100)]
        public int Quntity { get; set; }
        public double Rate { get; set; }
        public double Discount { get; set; }

        public int? CompanyId { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CompanyId")]

        public Company Company { get; set; }

        [ValidateNever]

        public Category Category { get; set; }
    }
}
