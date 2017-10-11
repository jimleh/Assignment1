using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQConsoleApp
{
    class ShoppingCart : ItemStorage<Item>
    {

        public ShoppingCart()
        {
        }

        public void AddToCart(Item item)
        {
        }
        public void RemoveFromCart(Item item)
        {
        }

        public void Checkout()
        {

            GetReceipt();
        }
        private void GetReceipt()
        {
            Console.WriteLine("\nReciept:");
            double totalPrice = 0;
            foreach(var obj in m_items)
            {
                obj.ToString();
                totalPrice += obj.Price;
            }
            Console.WriteLine($"Total price: {totalPrice}");
        }
    }
}
