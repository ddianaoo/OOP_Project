using System;

namespace ConsoleApp1
{
    public class OrderProduct
    {
        public Guid Id { get; private set; }

        private Product _product;
        public Product Product
        {
            get => _product;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Product), "Product не може бути null.");
                _product = value;
            }
        }

        private Order _order;
        public Order Order
        {
            get => _order;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Order), "Order не може бути null.");
                _order = value;
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Кількість має бути більше нуля.");
                _quantity = value;
            }
        }

        public OrderProduct(Product product, Order order, int quantity)
        {
            Id = Guid.NewGuid();
            Product = product;
            Order = order;
            Quantity = quantity;
        }

        public decimal TotalPrice => Math.Round(Product.Price * Quantity, 2);

        public override string ToString()
        {
            return $"{Product.Name} x {Quantity} = {TotalPrice:C}";
        }

        public string ForTable()
        {
            return $"{Product.Name,-20} | {Quantity,5} | {TotalPrice,10:C}";
        }
    }
}
