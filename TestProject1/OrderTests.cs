using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class OrderTests
    {
        private Product CreateSampleProduct(string name = "Product", decimal price = 10m)
        {
            return new Product(name, "Description", 5, price, ProductCategory.Cookware);
        }

        private User CreateSampleUser()
        {
            return new User("Іван", "Іваненко", "ivan@example.com", "password123", UserRole.Customer);
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            var user = CreateSampleUser();
            var order = new Order(user);

            Assert.AreEqual(OrderStatus.Created, order.Status);
            Assert.AreEqual(user, order.User);
            Assert.AreEqual(0, order.Items.Count);
            Assert.AreNotEqual(Guid.Empty, order.Id);
            Assert.AreEqual(order.CreatedAt, order.UpdatedAt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullUser_ShouldThrow()
        {
            var order = new Order(null);
        }

        [TestMethod]
        public void AddProduct_NewProduct_ShouldAddItem()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct();

            order.AddProduct(product, 2);

            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(product, order.Items[0].Product);
            Assert.AreEqual(2, order.Items[0].Quantity);
        }

        [TestMethod]
        public void AddProduct_ExistingProduct_ShouldIncreaseQuantity()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct();

            order.AddProduct(product, 2);
            order.AddProduct(product, 3);

            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(5, order.Items[0].Quantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProduct_NullProduct_ShouldThrow()
        {
            var user = CreateSampleUser();
            var order = new Order(user);

            order.AddProduct(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProduct_NonPositiveQuantity_ShouldThrow()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct();

            order.AddProduct(product, 0);
        }

        [TestMethod]
        public void ChangeStatus_ShouldUpdateStatusAndTimestamp()
        {
            var user = CreateSampleUser();
            var order = new Order(user);

            var oldUpdated = order.UpdatedAt;
            System.Threading.Thread.Sleep(10);
            order.ChangeStatus(OrderStatus.Paid);

            Assert.AreEqual(OrderStatus.Paid, order.Status);
            Assert.IsTrue(order.UpdatedAt > oldUpdated);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnCorrectSum()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product1 = CreateSampleProduct("P1", 10m);
            var product2 = CreateSampleProduct("P2", 15m);

            order.AddProduct(product1, 2);
            order.AddProduct(product2, 3);

            Assert.AreEqual(65m, order.GetTotal());
        }

        [TestMethod]
        public void OrderProduct_Constructor_ShouldInitializeProperties()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct();

            var orderProduct = new OrderProduct(product, order, 3);

            Assert.AreEqual(product, orderProduct.Product);
            Assert.AreEqual(order, orderProduct.Order);
            Assert.AreEqual(3, orderProduct.Quantity);
            Assert.AreNotEqual(Guid.Empty, orderProduct.Id);
            Assert.AreEqual(product.Price * 3, orderProduct.TotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderProduct_NullProduct_ShouldThrow()
        {
            var user = CreateSampleUser();
            var order = new Order(user);

            var op = new OrderProduct(null, order, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderProduct_NullOrder_ShouldThrow()
        {
            var product = CreateSampleProduct();
            var op = new OrderProduct(product, null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrderProduct_InvalidQuantity_ShouldThrow()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct();

            var op = new OrderProduct(product, order, 0);
        }

        [TestMethod]
        public void ForTable_ShouldReturnFormattedString()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product = CreateSampleProduct("Pan", 10m);
            order.AddProduct(product, 2);

            string table = order.ForTable();

            Assert.IsTrue(table.Contains(order.User.FirstName));
            Assert.IsTrue(table.Contains(order.User.LastName));
            Assert.IsTrue(table.Contains(order.Status.ToString()));
        }

        [TestMethod]
        public void ItemsForTable_ShouldReturnFormattedItems()
        {
            var user = CreateSampleUser();
            var order = new Order(user);
            var product1 = CreateSampleProduct("Pan", 10m);
            var product2 = CreateSampleProduct("Pot", 15m);

            order.AddProduct(product1, 2);
            order.AddProduct(product2, 1);

            string itemsTable = order.ItemsForTable();

            Assert.IsTrue(itemsTable.Contains("Pan"));
            Assert.IsTrue(itemsTable.Contains("Pot"));
            Assert.IsTrue(itemsTable.Contains("Total"));
        }
    }
}
