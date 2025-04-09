﻿namespace E_Commerce510.Models
{
    public class OrderItem
    {
        public int ProductId {  get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

    }
}
