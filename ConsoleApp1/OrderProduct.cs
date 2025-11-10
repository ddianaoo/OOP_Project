using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OrderProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public OrderProduct(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal TotalPrice => Product.Price * Quantity;

        public override string ToString()
        {
            return $"{Product.Name} x {Quantity} = {TotalPrice:C}";
        }
    }
}
