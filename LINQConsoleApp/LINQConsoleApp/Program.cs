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

            // Perhaps I should tidy this up a little, or maybe just move this to a class of its own?
            // Maybe some other time...
            int input;
            string line;
            bool print = true;

            var shop = new ShopStorage();
            var cart = new ShoppingCart();

            do
            {
                if (shop.Count <= 0)
                {
                    Console.WriteLine("The shop is empty!\n");
                }

                if(print)
                {
                    Console.WriteLine("\nItems in the Shop:");
                    foreach (var item in shop.GetAllItems())
                    {
                        Console.WriteLine(item.ToString());
                    }
                    Console.WriteLine();
                }
                else
                {
                    print = true;
                }

                Console.WriteLine("Main Menu > "
                    + "\n1. Add an Item to the Shopping Cart"
                    + "\n2. Manage the Shopping Cart"
                    + "\n3. Sort the Shop"
                    + "\n4. Search the Shop"
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
                        print = AddItemToShoppingCart(shop, cart);
                        break;
                    case 2:
                        print = ManageTheShoppingCart(cart);
                        break;
                    case 3:
                        SortTheShop(shop);
                        break;
                    case 4:
                        SearchTheShop(shop);
                        break;
                    default:
                        Console.WriteLine($"Invalid input, you entered {input}! Please read and follow the instructions.\n");
                        break;
                }
            }
            while (input != 0);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View the Shop
        public static void SortTheShop(ShopStorage shop)
        {
            string line;
            int input;

            do
            {
                Console.WriteLine("Main Menu > View the Shop > "
                    + "\n1. Sort the Shop by name"
                    + "\n2. Sort the Shop by price"
                    + "\n3. Sort the Shop by price and name"
                    + "\n4. Sort the Shop by price, grouped by category"
                    + "\n0. Return"
                );
                // Make sure that the user entered a valid input
                while (!int.TryParse(line = Console.ReadLine().ToString(), out input))
                {
                    Console.WriteLine($"Invalid input, you entered {line}! Please read and follow the instructions.");
                }

                switch (input)
                {
                    case 0:
                        Console.WriteLine();
                        return;
                    case 1:
                        Console.WriteLine("\nView the Shop sorted by name:");
                        shop.SortByName();
                        foreach (var item in shop.GetAllItems())
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        return;
                    case 2:
                        Console.WriteLine("\nView the Shop sorted by price:");
                        shop.SortByPrice();
                        foreach (var item in shop.GetAllItems())
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        return;
                    case 3:
                        Console.WriteLine("\nView the Shop sorted by price and name:");
                        shop.SortByPriceAndName();
                        foreach (var item in shop.GetAllItems())
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        return;
                    case 4:
                        Console.WriteLine("\nView the Shop sorted by price, grouped by category:");
                        shop.SortByPriceAndGroupByCategory();
                        foreach (var item in shop.GetAllItems())
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        return;
                    default:
                        Console.WriteLine($"Invalid input, you entered {input}! Please read and follow the instructions.");
                        break;
                }
            }
            while (true);
        }

        // View The Shopping Cart
        public static bool ManageTheShoppingCart(ShoppingCart cart)
        {
            string line;
            int input;
            do
            {
                Console.WriteLine("Main Menu > Manage the Shopping Cart > "
                    + "\n1. View the Shopping Cart"
                    + "\n2. Checkout"
                    + "\n3. Remove an item from the Shopping Cart"
                    + "\n4. Reset the Shopping Cart"
                    + "\n0. Return"
                    );
                // Make sure that the user entered a valid input
                while (!int.TryParse(line = Console.ReadLine().ToString(), out input))
                {
                    Console.WriteLine($"Invalid input, you entered {line}! Please read and follow the instructions.");
                }

                switch (input)
                {
                    case 0:
                        Console.WriteLine();
                        return true;
                    case 1:
                        Console.WriteLine("View the Shopping Cart:");
                        if(cart.Count <= 0)
                        {
                            Console.WriteLine("The Shopping Cart is empty.\n");
                            return false;
                        }

                        foreach (var item in cart.GetAllItems())
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine($"The total price is: {cart.TotalPrice}.\n");
                        break;
                    case 2:
                        if (cart.Count <= 0)
                        {
                            Console.WriteLine("The Shopping Cart is empty.\n");
                            return false;
                        }
                        Console.WriteLine("Checkout: ");
                        Console.WriteLine(cart.Checkout());
                        Console.WriteLine();
                        break;
                    case 3:
                        if (cart.Count <= 0)
                        {
                            Console.WriteLine("The Shopping Cart is empty.\n");
                            return false;
                        }
                        Console.WriteLine("Remove an Item from the Shopping Cart:");
                        Console.WriteLine("Please enter the ID of the item you wish to remove:");
                        line = Console.ReadLine();
                        Console.WriteLine(cart.RemoveFromCart(line) + "\n");
                        break;
                    case 4:
                        if (cart.Count <= 0)
                        {
                            Console.WriteLine("The Shopping Cart is empty.\n");
                            return false;
                        }
                        Console.WriteLine("Reset the Shopping Cart:");
                        Console.WriteLine(cart.ResetShoppingCart() + "\n");

                        break;
                    default:
                        Console.WriteLine($"Invalid input, you entered {input}! Please read and follow the instructions.");
                        break;
                }
            }
            while (true);
        }
        // Add to the Shopping Cart
        public static bool AddItemToShoppingCart(ShopStorage shop, ShoppingCart cart)
        {
            Console.WriteLine($"\nAdd an item to the Shopping Cart");
            Console.WriteLine($"Please enter the ID of the item you wish to add: ");
            Console.WriteLine(cart.AddToCart(Console.ReadLine(), shop));
            return false;
        }

        // Search the Shop
        public static void SearchTheShop(ShopStorage shop)
        {
            string line;
            int input;
            Item[] items;

            if (shop.Count <= 0)
            {
                Console.WriteLine("The shop is empty!\n");
                return;
            }

            do
            {
                Console.WriteLine("Main Menu > Search the Shop > "
                    + "\n1. Search by exact name"
                    + "\n2. Search by name containing"
                    + "\n3. Search by price larger than"
                    + "\n4. Search by price smaller than"
                    + "\n5. Search by price and name"
                    + "\n6. Search by price within a category"
                    + "\n7. Search by name within a category"
                    + "\n0. Return"
                );
                // Make sure that the user entered a valid input
                while (!int.TryParse(line = Console.ReadLine().ToString(), out input))
                {
                    Console.WriteLine($"Invalid input, you entered {line}! Please read and follow the instructions.");
                }

                switch (input)
                {
                    case 0:
                        Console.WriteLine();
                        return;
                    case 1:
                        Console.WriteLine("\nSearch by exact name: ");
                        items = shop.SearchForExactName(Console.ReadLine());
                        if(items.Count() <= 0)
                        {
                            Console.WriteLine("No items found.\n");
                            continue;
                        }
                        Console.WriteLine("Items found:");
                        foreach (var item in items)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.WriteLine("\nSearch by name containing: ");
                        items = shop.SearchForNameContaining(Console.ReadLine());
                        if (items.Count() <= 0)
                        {
                            Console.WriteLine("No items found.\n");
                            continue;
                        }
                        Console.WriteLine("Items found:");
                        foreach (var item in items)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        Console.WriteLine();
                        break;
                    case 3:
                        Console.WriteLine("\nSearch by price larger than: ");
                        if(Double.TryParse(Console.ReadLine(), out double price))
                        {
                            items = shop.SearchForPriceLargerThan(price);
                            if (items.Count() <= 0)
                            {
                                Console.WriteLine("No items found.\n");
                                continue;
                            }
                            Console.WriteLine("Items found:");
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your didn't enter a valid price.");
                        }
                        Console.WriteLine();
                        break;
                    case 4:
                        Console.WriteLine("\nSearch by price smaller than: ");
                        if (Double.TryParse(Console.ReadLine(), out price))
                        {
                            items = shop.SearchForPriceSmallerThan(price);
                            if (items.Count() <= 0)
                            {
                                Console.WriteLine("No items found.\n");
                                continue;
                            }
                            Console.WriteLine("Items found:");
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your didn't enter a valid price.");
                        }
                        Console.WriteLine();
                        break;
                    case 5:
                        Console.WriteLine("\nSearch by price: ");
                        if (Double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("And name: ");
                            items = shop.SearchForPriceAndName(price, Console.ReadLine());
                            if (items.Count() <= 0)
                            {
                                Console.WriteLine("No items found.\n");
                                continue;
                            }
                            Console.WriteLine("Items found:");
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("You didn't enter a valid price.");
                        }
                        Console.WriteLine();
                        break;
                    case 6:
                        Console.WriteLine("\nSearch for price in a category: ");
                        foreach(var cat in Enum.GetValues(typeof(Categories)))
                        {
                            Console.WriteLine($"{cat.ToString()}");
                        }
                        Console.WriteLine("\nEnter a category:");

                        if(Enum.TryParse(Console.ReadLine(), out Categories category))
                        {

                            Console.WriteLine("Search by price: ");
                            if (Double.TryParse(Console.ReadLine(), out price))
                            {
                                items = shop.SearchForPriceAndCategory(price, category.ToString());
                                if (items.Count() <= 0)
                                {
                                    Console.WriteLine("No items found.\n");
                                    continue;
                                }
                                Console.WriteLine("Items found:");
                                foreach (var item in items)
                                {
                                    Console.WriteLine(item.ToString());
                                }
                            }
                            else
                            {
                                Console.WriteLine("Your didn't enter a valid price.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You didn¨t enter a valid category.");
                        }
                        Console.WriteLine();
                        break;
                    case 7:
                        Console.WriteLine("\nSearch for price in a category: ");
                        foreach (var cat in Enum.GetValues(typeof(Categories)))
                        {
                            Console.WriteLine($"{cat.ToString()}");
                        }
                        Console.WriteLine("\nEnter a category:");

                        if (Enum.TryParse(Console.ReadLine(), out category))
                        {
                            Console.WriteLine("Search by name: ");
                            items = shop.SearchForNameAndCategory(Console.ReadLine(), category.ToString());
                            if (items.Count() <= 0)
                            {
                                Console.WriteLine("No items found.\n");
                                continue;
                            }
                            Console.WriteLine("Items found:");
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("You didn¨t enter a valid category.");
                        }
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine($"Invalid input, you entered {input}! Please read and follow the instructions.");
                        break;
                }
            }
            while (input != 0);
        }
    }
}