using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public List<Order> Orders { get; set; }

        public User(string firstName, string lastName, string email, UserRole role)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            Orders = new List<Order>();
        }

        public void CreateOrder(Order order)
        {
            Orders.Add(order);
            Console.WriteLine($"Order {order.Id} created for {FirstName} {LastName}");
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Role}) - {Email}";
        }
    }

}
