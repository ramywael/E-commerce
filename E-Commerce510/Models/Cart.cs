using Microsoft.EntityFrameworkCore;

namespace E_Commerce510.Models
{
    public class Cart
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public Product product { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
