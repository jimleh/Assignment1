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
        protected T[] m_items;
        //protected List<T> m_items;
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
        
        public void GetAllItems()
        {
            Console.WriteLine("\n--#Printing all the items in the item storage#--");
            foreach (var obj in m_items)
            {
                if (obj != null)
                {
                    Console.WriteLine(obj.ToString());
                }
            }
            Console.WriteLine("--#End of item storage#--\n");
        }

        // I prefer the name Print()
        public void Print()
        {
            Console.WriteLine("\n--#Printing all the items in the item storage#--");
            foreach (var obj in m_items)
            {
                if (obj != null)
                {
                    Console.WriteLine(obj.ToString());
                }
            }
            Console.WriteLine("--#End of item storage#--\n");
        }

        // Sorting and grouping methods
        protected void SortBy(string sortBy, string thenBy = null)
        {
            try
            {
                // if thenBy is null, then only order by the string sortBy
                m_items = (thenBy == null) ?
                    m_items.Where(x => x != null && GetPropertyFromObject(x, sortBy) != null)
                    .OrderBy(x => GetPropertyFromObject(x, sortBy)).ToArray() 
                    : // otherwise also order by the string thenBy
                    m_items.Where(x => x != null && GetPropertyFromObject(x, sortBy) != null && GetPropertyFromObject(x, thenBy) != null)
                    .OrderBy(x => GetPropertyFromObject(x, sortBy))
                    .ThenBy(x => GetPropertyFromObject(x, thenBy)).ToArray();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"No such property to order by in the objects used.\n Exception Message: {e.Message}");
            }
        }
        protected void GroupBy(string prop)
        {
            var tmp = m_items;

            try
            {
                var group = tmp
                    .Where(x=> x != null && GetPropertyValueFromObject(x, prop) != null)
                    .GroupBy(x => GetPropertyValueFromObject(x, prop));
                Clear();
                foreach (var item in group)
                {
                    AddElements(item.ToArray());
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"No such property to order by in the objects used.\n Exception Message: {e.Message}");
            }
        }
        protected void SortByAndGroupBy(string sortBy, string groupBy, string thenBy = null)
        {
            SortBy(sortBy, thenBy);
            GroupBy(groupBy);
        }

        // Specific Sorting methods (should probably be in derived classes)
        // These are a little ugly, but at least they work
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
            SortBy("Name", "Price");
        }
        public virtual void SortByPriceAndGroupByCategory()
        {
            SortByPrice();
            GroupBy("Category");
        }

        // Search Methods
        // I would like an even more generic way of searching, but this is taking too much time to do right now
        /*protected void SearchFor<T>(Expression<Func<T, int, bool>> predicate)
        {
            var items = m_items.AsQueryable().Where(predicate);
        }*/

        // These are also a little ugly, but they get the job done
        public void SearchForString(string prop, string value, bool exact = false)
        {
            T[] items;
            try
            {
                if (exact)
                {
                    items = m_items
                        .Where(x => x != null 
                        && Convert.ToString(GetPropertyValueFromObject(x, prop)).ToLower() == value.ToLower()).ToArray();
                }
                else
                {
                    items = m_items
                        .Where(x => x != null 
                        && Convert.ToString(GetPropertyValueFromObject(x, prop)).ToLower().Contains(value.ToLower())).ToArray();
                }
                SearchPrint(items);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($"The property could not be cast to the type double.\n Exception Message: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
        }
        public void SearchForExactName(string name)
        {
            SearchForString("Name", name, true);
        }
        public void SearchForNameContaining(string name)
        {
            SearchForString("Name", name);
        }

        protected void SearchForDouble(string prop, double value, bool smallerThan = false)
        {
            T[] items;
            try
            {
                if (smallerThan)
                {
                    items = m_items.Where(x => x != null && Convert.ToDouble(GetPropertyValueFromObject(x, prop)) <= value).ToArray();
                }
                else
                {
                    items = m_items.Where(x => x != null && Convert.ToDouble(GetPropertyValueFromObject(x, prop)) >= value).ToArray();
                }
                SearchPrint(items);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($"The property could not be cast to the type double.\n Exception Message: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
        }
        public void SearchForPriceLargerThanOrEqualTo(double price)
        {
            SearchForDouble("Price", price);
        }
        public void SearchForPriceSmallerThanOrEqualTo(double price)
        {
            SearchForDouble("Price", price, true);
        }

        // Ugly method
        public void SearchForPriceAndName(double price, string name, bool smallerThan = false, bool equals = false)
        {
            T[] items;
            try
            {
                if(smallerThan && equals)
                {
                    items = m_items
                        .Where(x => x != null 
                        && Convert.ToString(GetPropertyValueFromObject(x, "Name")).ToLower() == name.ToLower() 
                        && Convert.ToDouble(GetPropertyValueFromObject(x, "Price")) <= price)
                        .ToArray();
                }
                else if(smallerThan && !equals)
                {
                    items = m_items
                        .Where(x => x != null
                        && Convert.ToString(GetPropertyValueFromObject(x, "Name")).ToLower().Contains(name.ToLower())
                        && Convert.ToDouble(GetPropertyValueFromObject(x, "Price")) <= price)
                        .ToArray();
                }
                else if (equals && !smallerThan)
                {
                    items = m_items
                        .Where(x => x != null
                        && Convert.ToString(GetPropertyValueFromObject(x, "Name")).ToLower() == name.ToLower()
                        && Convert.ToDouble(GetPropertyValueFromObject(x, "Price")) >= price)
                        .ToArray();
                }
                else
                {
                    items = m_items
                        .Where(x => x != null
                        && Convert.ToString(GetPropertyValueFromObject(x, "Name")).ToLower().Contains(name.ToLower())
                        && Convert.ToDouble(GetPropertyValueFromObject(x, "Price")) >= price)
                        .ToArray();
                }

                SearchPrint(items);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($"The property could not be cast to the type double.\n Exception Message: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
            }
        }

        // Help method for searchnig, grouping and sorting
        // Prints the results of a search
        protected void SearchPrint(T[] items)
        {
            Console.WriteLine($"\n--Displaying all search matches--");
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }
        // Not sure if these make the code shorter or more readable at all...
        protected object GetPropertyFromObject(object obj, string prop)
        {
            var tmp = obj.GetType().GetProperty(prop);
            return tmp;
        }
        protected object GetPropertyValueFromObject(object obj, string prop)
        {
            var tmp = obj.GetType().GetProperty(prop).GetValue(obj);
            return tmp;
        }

        // Internal Storage
        // Some basic methods used to handle the internal storage array
        protected bool ContainsElement(T t)
        {
            return GetIndexOfElement(t) >= 0 ? true : false;
        }
        protected int GetIndexOfElement(T t)
        {
            if(t == null)
            {
                return -1;
            }

            for (int i = 0; i < Count; i++)
            {
                if (m_items[i].CompareTo(t) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        protected void AddElement(T t)
        {
            if(t == null)
            {
                //Console.WriteLine("Cannot Add a null object to the list.");
                return;
            }
            if (m_items == null)
            {
                m_items = new T[4];
                Capacity = 4;
            }

            if (Count == Capacity)
            {
                var tmp = new T[Capacity *= 2];
                for (int i = 0; i < Count; i++)
                {
                    tmp[i] = m_items[i];
                }
                m_items = tmp;
            }

            //Console.WriteLine($"Count: {Count}");
            m_items[Count++] = t;

        }
        protected void AddElements(T[] items)
        {
            foreach(var item in items)
            {
                AddElement(item);
            }

        }
        protected void RemoveElement(T t)
        {
            int index = GetIndexOfElement(t);
            if (index > 0)
            {
                return;
            }

            var tmp = new T[Capacity];

            for (int i = 0; i < Count; i++)
            {
                tmp[i] = m_items[(i < index ? i : i + 1)];
            }

            m_items = tmp;
            Count--;
        }
        protected void RemoveElements(T[] items)
        {
            foreach(var item in items)
            {
                RemoveElement(item);
            }
        }
        protected void RemoveAll()
        {
            if(m_items == null || Capacity == 0)
            {
                return;
            }
            m_items = null;
            m_items = new T[Capacity];
            Count = 0;
        }
        protected void Clear()
        {
            if(m_items == null || Capacity == 0)
            {
                return;
            }

            m_items = null;
            Capacity = 0;
            Count = 0;

        }

        // Implemented methods from interface

        // Implementation of public IEnumerator<T> GetEnumerator()
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in m_items)
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
