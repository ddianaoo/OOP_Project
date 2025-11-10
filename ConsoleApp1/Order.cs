using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private OrderStatus status;
        public OrderStatus Status
        {
            get => status;
            private set
            {
                if (!Enum.IsDefined(typeof(OrderStatus), value))
                    throw new ArgumentException("Невірне значення статусу замовлення.");
                status = value;
            }
        }

        public User User { get; private set; }
        public List<OrderProduct> Items { get; private set; }

        public Order(User user)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;
            Status = OrderStatus.Created;
            User = user ?? throw new ArgumentNullException(nameof(user));
            Items = new List<OrderProduct>();
        }

        private void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }

        public void AddProduct(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (quantity <= 0)
                throw new ArgumentException("Кількість має бути більше нуля.", nameof(quantity));

            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new OrderProduct(product, this, quantity));
            }
            UpdateTimestamp();
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            UpdateTimestamp();
        }

        public decimal GetTotal()
        {
            return Math.Round(Items.Sum(i => i.TotalPrice), 2);
        }

        public override string ToString()
        {
            return $"Order {Id} | Status: {Status} | Total: {GetTotal():C} | Created: {CreatedAt:dd.MM.yyyy HH:mm:ss} | Updated: {UpdatedAt:dd.MM.yyyy HH:mm:ss}";
        }

        public string ForTable()
        {
            return $"{Id} | {User.FirstName} {User.LastName,-25} | Status: {Status,-10} | Total: {GetTotal(),10:C} | Created: {CreatedAt:dd.MM.yyyy HH:mm:ss} | Updated: {UpdatedAt:dd.MM.yyyy HH:mm:ss}";
        }

        public string ItemsForTable()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Product Name         | Qty  | Total");
            sb.AppendLine("-------------------------------------");
            foreach (var item in Items)
                sb.AppendLine(item.ForTable());
            return sb.ToString();
        }
    }
}
