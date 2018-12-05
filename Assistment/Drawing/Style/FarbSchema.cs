using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Drawing.Style
{
    public class FarbSchema<T> : IDictionary<FarbSchema<T>.Typ, T>
    {
        public enum Typ
        {
            Schrift,
            Rand,
            Hintergrund,
            Feld,
            Max
        }

        public const int N = (int)Typ.Max;
        private T[] Colors = new T[N];

        public FarbSchema<T> Clone()
        {
            FarbSchema<T> schema = new FarbSchema<T>();
            for (int i = 0; i < N; i++)
                schema.Colors[i] = Colors[i];
            return schema;
        }

        public void Add(Typ key, T value)
        {
            Colors[(int)key] = value;
        }
        public bool ContainsKey(Typ key)
        {
            return Colors[(int)key] != null;
        }
        public ICollection<Typ> Keys
        {
            get
            {
                Typ[] ts = new Typ[N];
                ts.CountMap(i => (Typ)i);
                return ts;
            }
        }
        public bool Remove(Typ key)
        {
            if (!Colors[(int)key].Equals(default(T)))
            {
                Colors[(int)key] = default(T);
                return true;
            }
            return false;
        }
        public bool TryGetValue(Typ key, out T value)
        {
            if (Colors[(int)key] != null)
            {
                value = Colors[(int)key];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
        public ICollection<T> Values
        {
            get { return Colors; }
        }
        public T this[Typ key]
        {
            get
            {
                return Colors[(int)key];
            }
            set
            {
                Colors[(int)key] = value;
            }
        }
        public void Add(KeyValuePair<Typ, T> item)
        {
            Colors[(int)item.Key] = item.Value;
        }
        public void Clear()
        {
            Colors.SelfMap(x => default(T));
        }
        public bool Contains(KeyValuePair<Typ, T> item)
        {
            return Colors[(int)item.Key].Equals(item.Value);
        }
        public void CopyTo(KeyValuePair<Typ, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public int Count
        {
            get { return N; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(KeyValuePair<Typ, T> item)
        {
            if (Colors[(int)item.Key].Equals(item))
            {
                Colors[(int)item.Key] = default(T);
                return true;
            }
            return false;
        }
        public IEnumerator<KeyValuePair<Typ, T>> GetEnumerator()
        {
            int i = 0;
            return Colors.Map(x => new KeyValuePair<Typ, T>((Typ)i++, x)).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
