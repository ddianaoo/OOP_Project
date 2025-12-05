using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public abstract class ProductBase
    {
        public Guid Id { get; protected set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected ProductBase(string name, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;

            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;
        }

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }

        public abstract string GetInfo();

        public override string ToString()
        {
            return $"{Name} - {Price:C}\nCreated: {CreatedAt:G}, Updated: {UpdatedAt:G}";
        }
    }

}
