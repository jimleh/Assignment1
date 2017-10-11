using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQConsoleApp
{
    public enum Categories
    {
        Food,
        Drinks,
        Bread,
        Books,
        Sport
    }
    class Program
    {
        static void Main(string[] args)
        {
            int input;
            string line;

            var shop = new ShopStorage();
            var cart = new ShoppingCart();

            do
            {
                Console.WriteLine("Main Menu - "
                    + "\n1. View the Contents of the Store"
                    + "\n2. View The Contents of the Shopping Cart"
                    + "\n3. Add Item to the Shopping Cart"
                    + "\n4. Remove Item From the Shopping Cart"
                    + "\n5. Checkout"
                    + "\n6. Reset the Shopping Cart"
                    + "\n0. Exit"
                    );
                // Make sure that the user entered a valid input
                while (!int.TryParse(line = Console.ReadLine().ToString(), out input))
                {
                    Console.WriteLine($"Invalid input, you entered {line}! Please read and follow the instructions.");
                }

                switch (input)
                {
                    case 0:
                        Console.WriteLine($"You entered {input}! Exiting the application.");
                        break;
                    case 1:
                        ViewShop(shop);
                        break;
                    case 2:
                        ViewShoppingCart(cart);
                        break;
                    case 3:
                        AddToShoppingCart(cart);
                        break;
                    case 4:
                        RemoveFromShoppingCart(cart);
                        break;
                    case 5:
                        Checkout(cart);
                        break;
                    case 6:
                        ResetShoppingCart(cart);
                        break;
                    default:
                        Console.WriteLine($"Invalid input, you entered {input}! Please read and follow the instructions.");
                        break;
                }
            }
            while (input != 0);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View the Shop
        public static void ViewShop(ShopStorage shop)
        {
            Console.WriteLine($"\nEntering ViewShop()");
            shop.Print();
        }
        // View The Shopping Cart
        public static void ViewShoppingCart(ShoppingCart cart)
        {
            Console.WriteLine($"\nEntering ViewShoppingCart()");
            cart.Print();
        }
        // Add to the Shopping Cart
        public static void AddToShoppingCart(ShoppingCart cart)
        {
            Console.WriteLine($"\nEntering AddToShoppingCart()");
            //cart.AddToCart()
        }
        // Remove From the Shopping Cart
        public static void RemoveFromShoppingCart(ShoppingCart cart)
        {
            Console.WriteLine($"\nEntering RemoveFromShoppingCart()");
            //cart.RemoveFromCart();
        }

        public static void Checkout(ShoppingCart cart)
        {
            Console.WriteLine($"\nEntering Checkout()");
            cart.Checkout();
        }

        public static void ResetShoppingCart(ShoppingCart cart)
        {
            Console.WriteLine($"\nEntering ResetShoppingCart()");
            //cart.Reset();
        }
    }
}
