using System.Collections.Generic;
using NUnit;
using NUnit.Framework;

namespace SupermarketReceipt.Test
{
    [TestFixture]
    public sealed class SupermarketNUnitTest
    {
        private SupermarketCatalog catalog;
        private SupermarketCatalog product;

        [SetUp]
        public void Setup()
        {
            catalog = new FakeCatalog();
            catalog.AddProduct(ProductData.ProductTootbrush, 0.99);
            catalog.AddProduct(ProductData.ProductApple, 1.99);
            catalog.AddProduct(ProductData.ProductRice, 2.49);
            catalog.AddProduct(ProductData.ProductToothpaste, 1.79);
            catalog.AddProduct(ProductData.ProductTomatoe, 0.69);
        }

        [Test]
        public void TenPercentDiscount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);
            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.AreEqual(4.975, receipt.GetTotalPrice());
            CollectionAssert.IsEmpty(receipt.GetDiscounts());
            Assert.AreEqual(1, receipt.GetItems().Count);
            var receiptItem = receipt.GetItems()[0];
            Assert.AreEqual(apples, receiptItem.Product);
            Assert.AreEqual(1.99, receiptItem.Price);
            Assert.AreEqual(2.5 * 1.99, receiptItem.TotalPrice);
            Assert.AreEqual(2.5, receiptItem.Quantity);
        }

        [Test]
        public void BuyTwoTootBrushesGetOneFreeDiscount()
        {
            var cart = new ShoppingCart();
            cart.AddItem(ProductData.ProductTootbrush);
            cart.AddItem(ProductData.ProductTootbrush);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, ProductData.ProductTootbrush, 1.0);

            var receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.That(receipt.GetTotalPrice, Is.EqualTo(0.99).Within(0.1));
        }

        [Test]
        public void BuyThreeTootBrushesGetOneFreeDiscount()
        {
            var cart = new ShoppingCart();
            cart.AddItem(ProductData.ProductTootbrush);
            cart.AddItem(ProductData.ProductTootbrush);
            cart.AddItem(ProductData.ProductTootbrush);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, ProductData.ProductTootbrush, 1.0);

            var receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.That(receipt.GetTotalPrice, Is.EqualTo(1.98).Within(0.1));
        }
    }
}