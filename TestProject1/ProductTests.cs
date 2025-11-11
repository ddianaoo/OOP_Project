using ConsoleApp1;

namespace TestProject1
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string name = "Frying Pan";
            string desc = "Non-stick frying pan";
            int quantity = 10;
            decimal price = 25.5m;
            ProductCategory category = ProductCategory.Cookware;

            // Act
            Product product = new Product(name, desc, quantity, price, category);

            // Assert
            Assert.AreEqual(name, product.Name);
            Assert.AreEqual(desc, product.Description);
            Assert.AreEqual(quantity, product.Quantity);
            Assert.AreEqual(price, product.Price);
            Assert.AreEqual(category, product.Category);
            Assert.IsTrue(product.CreatedAt <= DateTime.Now);
            Assert.IsTrue(product.UpdatedAt <= DateTime.Now);
            Assert.AreNotEqual(Guid.Empty, product.Id);
        }

        [TestMethod]
        public void SetName_ShouldUpdateUpdatedAt()
        {
            // Arrange
            Product product = new Product("Pan", "Desc", 5, 10, ProductCategory.Cookware);
            DateTime oldUpdatedAt = product.UpdatedAt;

            // Act
            System.Threading.Thread.Sleep(10);
            product.Name = "New Pan";

            // Assert
            Assert.AreEqual("New Pan", product.Name);
            Assert.IsTrue(product.UpdatedAt > oldUpdatedAt);
        }

        [TestMethod]
        public void SetPrice_ShouldUpdateUpdatedAt()
        {
            Product product = new Product("Pan", "Desc", 5, 10, ProductCategory.Cookware);
            DateTime oldUpdatedAt = product.UpdatedAt;

            System.Threading.Thread.Sleep(10);
            product.Price = 20;

            Assert.AreEqual(20, product.Price);
            Assert.IsTrue(product.UpdatedAt > oldUpdatedAt);
        }

        [TestMethod]
        public void Parse_ValidString_ShouldReturnProduct()
        {
            // Arrange
            string input = "Pan;Non-stick;5;25;Cookware";

            // Act
            Product product = Product.Parse(input);

            // Assert
            Assert.AreEqual("Pan", product.Name);
            Assert.AreEqual("Non-stick", product.Description);
            Assert.AreEqual(5, product.Quantity);
            Assert.AreEqual(25m, product.Price);
            Assert.AreEqual(ProductCategory.Cookware, product.Category);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_EmptyString_ShouldThrow()
        {
            Product.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidQuantity_ShouldThrow()
        {
            string input = "Pan;Desc;abc;25;Cookware";
            Product.Parse(input);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidPrice_ShouldThrow()
        {
            string input = "Pan;Desc;5;abc;Cookware";
            Product.Parse(input);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidCategory_ShouldThrow()
        {
            string input = "Pan;Desc;5;25;UnknownCategory";
            Product.Parse(input);
        }

        [TestMethod]
        public void TryParse_ValidString_ShouldReturnTrue()
        {
            string input = "Pan;Non-stick;5;25;Cookware";
            bool result = Product.TryParse(input, out Product product);

            Assert.IsTrue(result);
            Assert.IsNotNull(product);
            Assert.AreEqual("Pan", product.Name);
        }

        [TestMethod]
        public void TryParse_InvalidString_ShouldReturnFalse()
        {
            string input = "Pan;Desc;abc;25;Cookware";
            bool result = Product.TryParse(input, out Product product);

            Assert.IsFalse(result);
            Assert.IsNull(product);
        }
    }
}