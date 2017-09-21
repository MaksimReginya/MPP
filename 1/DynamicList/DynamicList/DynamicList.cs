using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicList
{
    public class DynamicList<T> : IEnumerable<T>, IEquatable<DynamicList<T>>
    {
        private T[] values;

        public int Count
        {
            get
            {
                return values.Length;
            }
        }

        public DynamicList()
        {
            values = new T[] { };
        }

        public DynamicList(int length)
        {
            if (length < 0)
                throw new ArgumentException();
            values = new T[length];
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= values.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                return (T)values[index];
            }
            set
            {
                if (index < 0 || index >= values.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                values[index] = value;
            }
        }

        public void Add(T item)
        {
            Array.Resize(ref values, values.Length + 1);
            values[values.Length - 1] = item;
        }

        public void RemoveAt(long index)
        {            
            if (values.Length < 1 || index < 0 || index >= values.Length)
            {
                throw new IndexOutOfRangeException();
            }                        
            var temp = new T[values.Length - 1];
            Array.Copy(values, 0, temp, 0, index);
            Array.Copy(values, index + 1, temp, index, values.Length - index - 1);
            values = new T[temp.Length];
            temp.CopyTo(values, 0);
        }

        public void Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException();
            int index = Array.FindIndex(values, (T x) => x.Equals(item));
            RemoveAt(index);
        }

        public void Clear()
        {
            values = new T[] { };
        }

        public IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }
       
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)values).GetEnumerator();         
        }

        public bool Equals(DynamicList<T> other)
        {
            if (other == null || this.Count != other.Count)
            {
                return false;
            }
            int i;
            for (i = 0; i < other.Count; i++)
            {
                if (!this[i].Equals(other[i]))
                    return false;
            }
            return true;
        }
    }    
}
