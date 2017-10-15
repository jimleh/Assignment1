using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LINQConsoleApp
{
    abstract class ItemStorage<T> : IEnumerable<T>
        where T : IComparable
    {
        public string ID { get; private set; }
        protected T[] itemList;
        //protected List<T> itemList;
        public int Count { get; private set; }
        public int Capacity { get; private set; }


        public ItemStorage()
        {
            Count = 0;
            Capacity = 0;
        }

        public ItemStorage(T[] items)
        {
            Count = 0;
            Capacity = 0;
            foreach (var obj in items)
            {
                AddElement(obj);
            }
        }

        public List<string> GetAllItems()
        {
            return itemList.Where(x => x != null).Select(x=> x.ToString()).ToList();
        }

        // Base sorting methods
        protected void SortBy<TResult>(Func<T, TResult> sorter)
        {
            itemList = itemList
                .Where(x => x != null)
                .OrderBy(sorter)
                .ToArray();
        }
        protected void SortByThenBy<XElement, YElement>(Func<T, XElement> sorter, Func<T, YElement> otherSorter)
        {
            itemList = itemList
                .Where(x => x != null)
                .OrderBy(sorter)
                .ThenBy(otherSorter)
                .ToArray();
        }
        protected void SortByAndGroupBy<XElement, YElement>(Func<T, XElement> sorter, Expression<Func<T, YElement>> grouper)
        {
            itemList = itemList
                .Where(x => x != null)
                .OrderBy(sorter)
                .AsQueryable()
                .GroupBy(grouper)
                .SelectMany(x => x)
                .ToArray();
        }

        // A version that kinda uses reflection to get the desired property
        // also uses the more generic method listed above
        protected void SortBy(string prop)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, prop);

            // to make sure that the property actually exists
            if (predicate != null)
            {
                SortBy(x => GetPropertyValueFromObject(x, prop));
            }

        }
        protected void SortByThenBy(string prop1, string prop2)
        {
            Func<T, object> predicate1 = x => GetPropertyFromObject(x, prop1);
            Func<T, object> predicate2 = x => GetPropertyFromObject(x, prop2);

            if (predicate1 != null && predicate2 != null)
            {
                SortByThenBy(predicate1, predicate2);
            }
        }
        protected void SortByAndGroupBy(string sorter, string grouper)
        {
            Func<T, object> predicate1 = x => GetPropertyFromObject(x, sorter);
            Func<T, object> predicate2a = x => GetPropertyFromObject(x, grouper);
            Expression<Func<T, object>> predicate2b = x => GetPropertyValueFromObject(x, grouper);

            if (predicate1 != null && predicate2a != null)
            {
                SortByAndGroupBy(predicate1, predicate2b);
            }
        }

        // Specific sort methods 
        // can be implemented in the derived classes to make them less ugly
        // also uses the more generic sort methods listed above
        public virtual void SortByName()
        {
            SortBy("Name");
        }
        public virtual void SortByPrice()
        {
            SortBy("Price");
        }
        public virtual void SortByPriceAndName()
        {
            SortByThenBy("Price", "Name");
        }
        public virtual void SortByPriceAndGroupByCategory()
        {
            SortByAndGroupBy("Price", "Category");
        }

        // Base search method
        protected T[] SearchFor(Func<T, bool> predicate)
        {
            return itemList.Where(predicate).ToArray();
        }

        // Specific search methods 
        // can be implemented in the child classes to make them less ugly, but they can be used just as they are heres for this assignment
        // kinda uses reflection to do the job in a somewhat roundabout way
        // also uses the more generic search method listed above
        public virtual T[] SearchForExactName(string name)
        {
            Func<T, object> predicate = x=> GetPropertyFromObject(x, "Name");
            if(predicate != null)
            {
                return SearchFor(x => Convert.ToString(GetPropertyValueFromObject(x, "Name")) == name);
            }
            return null;
        }
        public virtual T[] SearchForNameContaining(string name)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Name");
            if (predicate != null)
            {
                return SearchFor(x => (Convert.ToString(GetPropertyValueFromObject(x, "Name")).Contains(name)));
            }
            return null;
        }
        public virtual T[] SearchForPriceLargerThan(double price)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Price");
            if (predicate != null)
            {
                return SearchFor(x => Double.TryParse(Convert.ToString(GetPropertyValueFromObject(x, "Price")), out double tmp)
                && tmp > price);
            }
            return null;
        }
        public virtual T[] SearchForPriceSmallerThan(double price)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Price");
            if (predicate != null)
            {
                return SearchFor(x => Double.TryParse(Convert.ToString(GetPropertyValueFromObject(x, "Price")), out double tmp)
                && tmp < price);
            }
            return null;
        }
        public virtual T[] SearchForPriceAndName(double price, string name)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Price");
            Func<T, object> predicate2 = x => GetPropertyFromObject(x, "Name");

            if (predicate != null && predicate2 != null)
            {
                return SearchFor(
                    x => Double.TryParse(Convert.ToString(GetPropertyValueFromObject(x, "Price")), out double tmp)
                    && tmp == price && Convert.ToString(GetPropertyValueFromObject(x, "Name")) == name);
            }
            return null;
        }
        public virtual T[] SearchForPriceAndCategory(double price, string category)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Price");
            Func<T, object> predicate2 = x => GetPropertyFromObject(x, "Category");

            if (predicate != null && predicate2 != null)
            {
                return SearchFor(
                    x => Double.TryParse(Convert.ToString(GetPropertyValueFromObject(x, "Price")), out double tmp)
                    && tmp == price && Convert.ToString((GetPropertyValueFromObject(x, "Category"))) == category);
            }
            return null;
        }
        public virtual T[] SearchForNameAndCategory(string name, string category)
        {
            Func<T, object> predicate = x => GetPropertyFromObject(x, "Price");
            Func<T, object> predicate2 = x => GetPropertyFromObject(x, "Category");

            if (predicate != null && predicate2 != null)
            {
                return SearchFor(
                    x => Convert.ToString(GetPropertyValueFromObject(x, "Name")) == name
                    && Convert.ToString((GetPropertyValueFromObject(x, "Category"))) == category);
            }
            return null;
        }

        // Help methods for searching, grouping and sorting
        // Not sure if these makes the code shorter or more readable at all...
        protected object GetPropertyFromObject(object obj, string prop)
        {
            var tmp = obj.GetType().GetProperty(prop);
            return tmp;
        }
        protected object GetPropertyValueFromObject(object obj, string prop)
        {
            return (obj == null) ? null : obj.GetType().GetProperty(prop).GetValue(obj);
        }

        // Internal Storage
        // Some basic methods used to handle the internal storage array
        public T GetElement(string id)
        {
            var items = SearchFor(x=> Convert.ToString(GetPropertyValueFromObject(x, "ID")) == id);

            if(items == null || items.Count() <= 0)
            {
                // will be null since we're dealing with reference types
                return default(T);
            }

            return items.First();
        }
        protected bool ContainsElement(T t)
        {
            return GetIndexOfElement(t) >= 0 ? true : false;
        }
        protected int GetIndexOfElement(T t)
        {
            if (t == null)
            {
                return -1;
            }

            for (int i = 0; i < Count; i++)
            {
                if (itemList[i].CompareTo(t) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        protected void AddElement(T t)
        {
            if (t == null)
            {
                //Console.WriteLine("Cannot Add a null object to the list.");
                return;
            }
            if (itemList == null)
            {
                itemList = new T[4];
                Capacity = 4;
            }

            if (Count == Capacity)
            {
                var tmp = new T[Capacity *= 2];
                for (int i = 0; i < Count; i++)
                {
                    tmp[i] = itemList[i];
                }
                itemList = tmp;
            }

            //Console.WriteLine($"Count: {Count}");
            itemList[Count++] = t;

        }
        protected void AddElements(T[] items)
        {
            foreach (var item in items)
            {
                AddElement(item);
            }

        }
        protected void RemoveElement(T t)
        {
            //int index = GetIndexOfElement(t);
            //if (index < 0)
            //{
            //    return;
            //}

            //var tmp = new T[Capacity];

            //for (int i = 0; i < Count; i++)
            //{
            //    tmp[i] = itemList[(i < index ? i : i + 1)];
            //}

            //itemList = tmp;

            // Much more elegant with LINQ!
            itemList = itemList.Where(x => !x.Equals(t)).ToArray();
            Count--;
        }
        protected void RemoveElements(T[] items)
        {
            foreach (var item in items)
            {
                RemoveElement(item);
            }
        }
        protected void RemoveAll()
        {
            if (itemList == null || Capacity == 0)
            {
                return;
            }
            itemList = null;
            itemList = new T[Capacity];
            Count = 0;
        }
        protected void Clear()
        {
            if (itemList == null || Capacity == 0)
            {
                return;
            }

            itemList = null;
            Capacity = 0;
            Count = 0;
        }

        // Implemented methods from interface

        // Implementation of public IEnumerator<T> GetEnumerator()
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in itemList)
            {
                if (t != null)
                    yield return t;
            }
            // throw new NotImplementedException();
        }

        // Implementation of IEnumerator IEnumerable.GetEnumerator()
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
            // throw new NotImplementedException();
        }
    }
}