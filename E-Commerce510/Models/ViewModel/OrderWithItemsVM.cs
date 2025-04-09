namespace E_Commerce510.Models.ViewModel
{
    public class OrderWithItemsVM
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
