using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQConsoleApp
{
    class ShoppingCart : ItemStorage<Item>
    {
        public double TotalPrice { get; private set; }

        public ShoppingCart()
        {
            TotalPrice = 0;
        }

        // could probably have been done in a pettier way...
        public string AddToCart(string id, ShopStorage shop)
        {
            var item = shop.GetElement(id);
            if(item == null)
            {
                return $"No item found in the Shop with the ID: {ID}.\n";
            }

            AddElement(item);
            TotalPrice += item.Price;
            return $"{item.ToString()} was added to the Shopping Cart.\n";
        }

        public string RemoveFromCart(string id)
        {
            var item = GetElement(id);
            if(item == null)
            {
                return $"No item found in the Shopping cart with the ID: {ID}.\n";
            }

            TotalPrice -= item.Price;
            RemoveElement(item);
            return $"{item.ToString()} was removed from the Shopping Cart.\n";
        }

        public string ResetShoppingCart()
        {
            string result = $"The Shopping Cart was cleared, removing {Count} items.";
            Clear();
            return result;
        }

        public string Checkout()
        {
            string result = "Checkout...\n" + GetReceipt();
            Clear();
            return result;
        }

        private string GetReceipt()
        {
            string receipt = $"You bought a total of {Count} items for a total price of: {TotalPrice}:-."
                + "\n---------------------------------\n";

            foreach (var item in GetAllItems())
            {
                receipt += item.ToString() + "\n";
            }
            return receipt;
        }
    }
}
