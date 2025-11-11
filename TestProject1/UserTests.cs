using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string firstName = "Іван";
            string lastName = "Іваненко";
            string email = "ivan@example.com";
            string password = "password123";
            UserRole role = UserRole.Customer;

            // Act
            User user = new User(firstName, lastName, email, password, role);

            // Assert
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(password, user.Password);
            Assert.AreEqual(role, user.Role);
            Assert.AreNotEqual(Guid.Empty, user.Id);
            Assert.IsNotNull(user.Orders);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FirstName_Empty_ShouldThrow()
        {
            new User("", "Last", "test@test.com", "password", UserRole.Customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LastName_Empty_ShouldThrow()
        {
            new User("First", " ", "test@test.com", "password", UserRole.Customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Email_Invalid_ShouldThrow()
        {
            new User("First", "Last", "invalidemail", "password", UserRole.Customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Password_Short_ShouldThrow()
        {
            new User("First", "Last", "test@test.com", "123", UserRole.Customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_Invalid_ShouldThrow()
        {
            var user = new User("First", "Last", "test@test.com", "password", (UserRole)999);
        }

        [TestMethod]
        public void Parse_ValidString_ShouldReturnUser()
        {
            string input = "Іван;Іваненко;ivan@example.com;password123;Customer";
            User user = User.Parse(input);

            Assert.AreEqual("Іван", user.FirstName);
            Assert.AreEqual("Іваненко", user.LastName);
            Assert.AreEqual("ivan@example.com", user.Email);
            Assert.AreEqual("password123", user.Password);
            Assert.AreEqual(UserRole.Customer, user.Role);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_EmptyString_ShouldThrow()
        {
            User.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidFormat_ShouldThrow()
        {
            string input = "Іван;Іваненко;ivan@example.com";
            User.Parse(input);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidRole_ShouldThrow()
        {
            string input = "Іван;Іваненко;ivan@example.com;password123;UnknownRole";
            User.Parse(input);
        }

        [TestMethod]
        public void TryParse_ValidString_ShouldReturnTrue()
        {
            string input = "Іван;Іваненко;ivan@example.com;password123;Customer";
            bool result = User.TryParse(input, out User user);

            Assert.IsTrue(result);
            Assert.IsNotNull(user);
            Assert.AreEqual("Іван", user.FirstName);
        }

        [TestMethod]
        public void TryParse_InvalidString_ShouldReturnFalse()
        {
            string input = "Іван;Іваненко;ivan@example.com;password123;UnknownRole";
            bool result = User.TryParse(input, out User user);

            Assert.IsFalse(result);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void PayOrder_ValidOrder_ShouldChangeStatus()
        {
            // Arrange
            var user = new User("Іван", "Іваненко", "ivan@example.com", "password123", UserRole.Customer);
            var order = new Order(user);
            user.CreateOrder(order);

            // Act
            user.PayOrder(order.Id);

            // Assert
            Assert.AreEqual(OrderStatus.Paid, order.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PayOrder_AlreadyPaid_ShouldThrow()
        {
            var user = new User("Іван", "Іваненко", "ivan@example.com", "password123", UserRole.Customer);
            var order = new Order(user);
            user.CreateOrder(order);
            user.PayOrder(order.Id);

            user.PayOrder(order.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CancelOrder_PaidOrder_ShouldThrow()
        {
            var user = new User("Іван", "Іваненко", "ivan@example.com", "password123", UserRole.Customer);
            var order = new Order(user);
            user.CreateOrder(order);
            user.PayOrder(order.Id);

            user.CancelOrder(order.Id);
        }

        [TestMethod]
        public void CancelOrder_Valid_ShouldSetCanceled()
        {
            var user = new User("Іван", "Іваненко", "ivan@example.com", "password123", UserRole.Customer);
            var order = new Order(user);
            user.CreateOrder(order);

            user.CancelOrder(order.Id);

            Assert.AreEqual(OrderStatus.Canceled, order.Status);
        }
    }
}
