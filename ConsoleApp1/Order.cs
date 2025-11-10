using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public List<OrderProduct> Items { get; private set; }

        public Order()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Status = OrderStatus.Created;
            Items = new List<OrderProduct>();
        }

        public void AddProduct(Product product, int quantity)
        {
            Items.Add(new OrderProduct(product, quantity));
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            Console.WriteLine($"Order {Id} status changed to {Status}");
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
                total += item.TotalPrice;
            return total;
        }

        public override string ToString()
        {
            return $"Order {Id} | Status: {Status} | Total: {GetTotal():C}";
        }
    }
}
