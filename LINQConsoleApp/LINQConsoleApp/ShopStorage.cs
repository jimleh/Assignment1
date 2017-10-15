using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQConsoleApp
{
    class ShopStorage : ItemStorage<Item>
    {
        Dictionary<int, int> stock;
        public ShopStorage()
        {
            InitStorage();
        }

        public void InitStorage()
        {
            Clear();
            AddElements(
                new Item[]
                {
                    new Item{ ID=1,       Category = Categories.Food,     Name= "Köttbullar",             Price = 39.50},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=3,       Category = Categories.Drinks,   Name= "Mjölk",                  Price = 10.50},
                    new Item{ ID=4,       Category = Categories.Drinks,   Name= "Juice",                  Price = 14.00},
                    new Item{ ID=5,       Category = Categories.Bread,    Name= "Frukostbröd",            Price = 18.00},
                    new Item{ ID=6,       Category = Categories.Sport,    Name= "Yogamatta",              Price = 99.00},
                    new Item{ ID=7,       Category = Categories.Sport,    Name= "Boxningshandskar",       Price = 200.00},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=8,       Category = Categories.Books,    Name= "Gudfadern",              Price = 59.00},
                    new Item{ ID=9,       Category = Categories.Books,    Name= "Bröderna Lejonhjärta",   Price = 71.00},
                    new Item{ ID=10,      Category = Categories.Books,    Name= "Ta bättre bilder",       Price = 150.00},
                    new Item{ ID=2,       Category = Categories.Food,     Name= "Pizza",                  Price = 29.50},
                    new Item{ ID=11,      Category = Categories.Bread,    Name= "Hårdbröd",               Price = 12.00},
                    new Item{ ID=12,      Category = Categories.Food,     Name= "Pizza",                  Price = 18.00},
                    new Item{ ID=12,      Category = Categories.Food,     Name= "Pizza",                  Price = 18.00},
                    new Item{ ID=13,      Category = Categories.Food,     Name= "Pizza",                  Price = 99.00},
                    new Item{ ID=14,      Category = Categories.Food,     Name= "Pizza",                  Price = 59.00},
                }
            );

            // Store the number of items in stock, although I'm too lazy to actually implement any checks for it right now
            stock = itemList.Where(x => x != null).GroupBy(x => x.ID).ToDictionary(x => x.Key, x => x.Count());

        }
    }
}