using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Product : ProductBase
    {
        public Guid Id { get; private set; }

        private string _name;
        private string _description;
        private int _quantity;
        private decimal _price;
        private ProductCategory _category;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateTimestamp();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                UpdateTimestamp();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                UpdateTimestamp();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                UpdateTimestamp();
            }
        }

        public ProductCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                UpdateTimestamp();
            }
        }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Product(string name, string description, int quantity, decimal price, ProductCategory category)
            : base(name, description, price)
        {
            Id = Guid.NewGuid();
            _name = name;
            _description = description;
            _quantity = quantity;
            _price = price;
            _category = category;
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;
        }

        private void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }

        public override string GetInfo()
        {
            return $"{Name} ({Category}) - {Price:C}, Qty: {Quantity}";
        }

        public override string ToString()
        {
            return $"{Name} ({Category}) - {Price:C} ({Quantity} pcs)\n" +
                   $"Created: {CreatedAt:G}, Updated: {UpdatedAt:G}";
        }

        public string ForTable()
        {
            string desc = Description.Length > 15 ? Description.Substring(0, 12) + "..." : Description;
            return $"{Id} | {Name,-18} | {Price,12:C} | {Quantity,5} | {Category,-16} | {desc,-15} | {CreatedAt:dd.MM.yyyy HH:mm:ss} | {UpdatedAt:dd.MM.yyyy HH:mm:ss}";
        }
        public static Product Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Рядок порожній.");

            string[] parts = s.Split(';');
            if (parts.Length < 5)
                throw new FormatException("Невірний формат рядка. Очікується 5 елементів через ';': Name;Description;Quantity;Price;Category.");

            try
            {
                string name = parts[0].Trim();
                string description = parts[1].Trim();

                if (!int.TryParse(parts[2].Trim(), out int quantity))
                    throw new FormatException($"Невірна кількість: {parts[2]}");

                if (!decimal.TryParse(parts[3].Trim(), out decimal price))
                    throw new FormatException($"Невірна ціна: {parts[3]}");

                if (!Enum.TryParse(parts[4].Trim(), out ProductCategory category))
                    throw new ArgumentException($"Невідома категорія: {parts[4]}");

                return new Product(name, description, quantity, price, category);
            }
            catch (Exception ex)
            {
                throw new FormatException("Помилка перетворення рядка у Product: " + ex.Message, ex);
            }
        }

        public static bool TryParse(string s, out Product? product)
        {
            try
            {
                product = Parse(s);
                return true;
            }
            catch
            {
                product = null;
                return false;
            }
        }
    }
}
