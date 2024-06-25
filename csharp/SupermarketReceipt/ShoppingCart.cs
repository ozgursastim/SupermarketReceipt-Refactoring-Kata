using System;
using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new List<ProductQuantity>();
        private readonly Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");


        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
        }

        public void AddItem(Product product)
        {
            AddItemQuantity(product, 1.0);
        }


        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
            if (_productQuantities.ContainsKey(product))
            {
                var newAmount = _productQuantities[product] + quantity;
                _productQuantities[product] = newAmount;
            }
            else
            {
                _productQuantities.Add(product, quantity);
            }
        }

        public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, SupermarketCatalog catalog)
        {
            foreach (var product in _productQuantities.Keys)
            {
                var quantity = _productQuantities[product];
                var quantityAsInt = (int) quantity;
                if (offers.ContainsKey(product))
                {
                    var offer = offers[product];
                    var unitPrice = catalog.GetUnitPrice(product);

                    Discount discount = null;

                    var amount = 1;
                    switch (offer.OfferType)
                    {
                        case SpecialOfferType.TwoForAmount:
                            amount = 2;
                            break;
                        case SpecialOfferType.ThreeForTwo:
                            amount = 3;
                            break;
                        case SpecialOfferType.FiveForAmount:
                            amount = 5;
                            break;
                        case SpecialOfferType.TenPercentDiscount:
                            break;
                        default:
                            amount = 1;
                            break;
                    }

                    var numberOfXs = quantityAsInt / amount;

                    if (offer.OfferType == SpecialOfferType.TenPercentDiscount)
                    {
                        discount = new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
                    }
                    else
                    {
                        if (quantityAsInt >= amount)
                        {
                            discount = GetAmountDiscount(amount, product, offer, quantityAsInt, unitPrice, numberOfXs);
                        }

                    }

                    if (discount != null)
                    {
                        receipt.AddDiscount(discount);
                    }
                }
            }
        }

        private string PrintPrice(double price)
        {
            return price.ToString("N2", Culture);
        }

        private Discount GetAmountDiscount(int amount, Product product, Offer offer, int quantity, double unitPrice, int numberOfXs)
        {
            var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantity % amount * unitPrice);
            return new Discount(product, amount + " for " + PrintPrice(offer.Argument), -discountTotal);
        }
    }
}