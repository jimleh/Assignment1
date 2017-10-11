using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQConsoleApp
{
    class Item : IEquatable<Item>, IComparable<Item>, IComparable, IQueryable<Item>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Categories Category { get; set; }

        public Expression Expression => throw new NotImplementedException();

        public Type ElementType => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Category : {Category}, Price: {Price}";
        }

        // Implemented methods from interface

        public int CompareTo(Item other)
        {
            if (Equals(other))
            {
                return 0;
            }

            return ID.CompareTo(other.ID);

            // throw new NotImplementedException();
        }

        // Implementation of Equals
        // returns true if all properties of the items compared match
        // returns false otherwise
        public bool Equals(Item other)
        {
            // If all properties match; return true
            if (ID == other.ID && Name == other.Name && Price == other.Price && Category == other.Category)
            {
                return true;
            }
            // Otherwise; return false
            return false;

            // throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if(obj == null || !(obj is Item))
            {
                throw new NotImplementedException();
            }

            return CompareTo(obj as Item);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
