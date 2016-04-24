using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Assistment.Xml
{
    public static class XmlReaderErweiterer
    {
        public static Image getImage(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return null;
            else
                return new Bitmap(s);
        }
        public static string getString(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null)
                return "";
            else
                return s;
        }
        public static int getInt(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return 0;
            else
                return int.Parse(s);
        }
        public static float getFloat(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return 0;
            else
                return float.Parse(s);
        }
        public static bool getBoolean(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return false;
            else
                return bool.Parse(s);
        }
        public static DateTime getDate(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return DateTime.Now;
            else
                return DateTime.Parse(s);
        }

        public static Color getColorHexARGB(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return Color.Empty;
            else
                return Color.FromArgb(int.Parse(s, System.Globalization.NumberStyles.HexNumber));
        }

        /// <summary>
        /// Vertauscht die Reihenfolge!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listOfLists"></param>
        /// <returns></returns>
        public static IEnumerable<T> multiConcat<T>(this IEnumerable<IEnumerable<T>> listOfLists)
        {
            IEnumerable<T> enumerable = new List<T>();
            foreach (var item in listOfLists)
                enumerable = item.Concat(enumerable);
            return enumerable;
        }

        /// <summary>
        /// flache Kopie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static SortedList<T, S> Clone<T, S>(this SortedList<T, S> list)
        {
            SortedList<T, S> l = new SortedList<T, S>();
            foreach (var item in list)
                l.Add(item.Key, item.Value);
            return l;
        }
        /// <summary>
        /// schreibt nur, falls value.length > 0
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void writeAttribute(this XmlWriter writer, string name, string value)
        {
            if (!(value == null || value == ""))
                writer.WriteAttributeString(name, value);
        }
        /// <summary>
        /// schreibt nur, falls value == true
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void writeBoolean(this XmlWriter writer, string name, bool value)
        {
            if (value)
                writer.WriteAttributeString(name, value.ToString());
        }

        public static void Add(this Control.ControlCollection ControlCollection, params Control[] Controls)
        {
            ControlCollection.AddRange(Controls);
        }
        public static void Add<T>(this ICollection<T> Collection, params T[] Values)
        {
            foreach (var item in Values)
            {
                Collection.Add(item);
            }
        }
    }
}
