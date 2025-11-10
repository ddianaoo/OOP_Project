using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ConsoleApp1
{
    public class User
    {
        public Guid Id { get; private set; }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я не може бути порожнім.");
                _firstName = value.Trim();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище не може бути порожнім.");
                _lastName = value.Trim();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (!IsValidEmail(value))
                    throw new ArgumentException("Невірний формат email.");
                _email = value.Trim();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException("Пароль має містити мінімум 6 символів.");
                _password = value;
            }
        }

        private UserRole _role;
        public UserRole Role
        {
            get => _role;
            set
            {
                if (!Enum.IsDefined(typeof(UserRole), value))
                    throw new ArgumentException("Невірна роль користувача.");
                _role = value;
            }
        }

        public List<Order> Orders { get; private set; }

        public User(string firstName, string lastName, string email, string password, UserRole role)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            Orders = new List<Order>();
        }

        public void CreateOrder(Order order)
        {
            Orders.Add(order);
        }

        public void PayOrder(Guid orderId)
        {
            var order = Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Рядок порожній.");
            }

            if (order.Status != OrderStatus.Created)
            {
                throw new InvalidOperationException($"Неможливо оплатити замовлення зі статусом {order.Status}.");
            }

            order.ChangeStatus(OrderStatus.Paid);
        }
        public void CancelOrder(Guid orderId)
        {
            var order = Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new InvalidOperationException("Замовлення не знайдено.");

            if (order.Status == OrderStatus.Paid)
                throw new InvalidOperationException("Неможливо відмінити оплачений заказ.");

            if (order.Status == OrderStatus.Canceled)
                throw new InvalidOperationException("Замовлення вже відмінено.");

            order.ChangeStatus(OrderStatus.Canceled);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Role}) - {Email}";
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string ForTable()
        {
            return $"{Id} | {FirstName,-15} | {LastName,-15} | {Email,-25} | {Role,-12} | {"******"}";
        }

        public static User Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Рядок порожній.");

            string[] parts = s.Split(';');
            if (parts.Length < 5)
                throw new FormatException("Невірний формат рядка. Очікується 5 елементів через ';': FirstName;LastName;Email;Password;Role.");

            try
            {
                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string email = parts[2].Trim();
                string password = parts[3].Trim();

                if (!Enum.TryParse(parts[4].Trim(), out UserRole role))
                    throw new ArgumentException($"Невідома роль: {parts[4]}");

                return new User(firstName, lastName, email, password, role);
            }
            catch (Exception ex)
            {
                throw new FormatException("Помилка перетворення рядка у User: " + ex.Message, ex);
            }
        }

        public static bool TryParse(string s, out User? user)
        {
            try
            {
                user = Parse(s);
                return true;
            }
            catch
            {
                user = null;
                return false;
            }
        }
    }
}
