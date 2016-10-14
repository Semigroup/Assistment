using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Assistment.Texts;

namespace Assistment.Xml
{
    public static class XmlReaderErweiterer
    {
        public static void writeEnum<T>(this XmlWriter writer, string name, T value)
        {
            writer.writeAttribute(name, Enum.GetName(typeof(T), value));
        }
        public static T getEnum<T>(this XmlReader reader, string name) where T : struct
        {
            T result = default(T);
            Enum.TryParse<T>(reader.getString(name), out result);
            return result;
        }

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
        public static SizeF getSizeF(this XmlReader reader, string name)
        {
            return new SizeF(getFloat(reader, name + "_X"), getFloat(reader, name + "_Y"));
        }
        public static PointF getPointF(this XmlReader reader, string name)
        {
            return new PointF(getFloat(reader, name + "_X"), getFloat(reader, name + "_Y"));
        }
        public static Size getSize(this XmlReader reader, string name)
        {
            return new Size(getInt(reader, name + "_X"), getInt(reader, name + "_Y"));
        }
        public static Point getPoint(this XmlReader reader, string name)
        {
            return new Point(getInt(reader, name + "_X"), getInt(reader, name + "_Y"));
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

        public static xFont getFontX(this XmlReader reader, string name)
        {
            return new FontGraphicsMeasurer(reader.getString(name), reader.getFloat(name + "_Size"));
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
        /// <summary>
        /// schreibt nur, falls value != 0
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void writeInt(this XmlWriter writer, string name, int value)
        {
            if (value != 0)
                writer.WriteAttributeString(name, value.ToString());
        }
        public static void writeFloat(this XmlWriter writer, string name, float value)
        {
            if (value != 0)
                writer.WriteAttributeString(name, value.ToString());
        }
        public static void writeSize(this XmlWriter writer, string name, SizeF Size)
        {
            writeFloat(writer, name + "_X", Size.Width);
            writeFloat(writer, name + "_Y", Size.Height);
        }
        public static void writePoint(this XmlWriter writer, string name, PointF Point)
        {
            writeFloat(writer, name + "_X", Point.X);
            writeFloat(writer, name + "_Y", Point.Y);
        }
        public static void writeColorHexARGB(this XmlWriter writer, string name, Color Color)
        {
            writer.WriteAttributeString(name, Color.ToArgb().ToString("x8"));
        }

        public static Pen getPen(this XmlReader reader, string name)
        {
            return new Pen(reader.getColorHexARGB(name + "_color"), reader.getFloat(name + "_width"));
        }
        public static void writePen(this XmlWriter writer, string name, Pen Pen)
        {
            if (Pen != null)
            {
                writer.writeColorHexARGB(name + "_color", Pen.Color);
                writer.writeFloat(name + "_width", Pen.Width);
            }
        }
        public static Font getFont(this XmlReader reader, string name)
        {
            string s = reader.getString(name + "_name");
            if (s.Length == 0)
                return null;
            return new Font(s, reader.getFloat(name + "_size"), reader.getEnum<FontStyle>(name + "_style"));
        }
        public static void writeFont(this XmlWriter writer, string name, Font Font)
        {
            if (Font != null)
            {
                writer.writeAttribute(name + "_name", Font.Name);
                writer.writeEnum(name + "_style", Font.Style);
                writer.writeAttribute(name + "_size", Font.Size.ToString());
            }
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

        /// <summary>
        /// Liest bis zum nächsten Element oder EOF
        /// </summary>
        /// <param name="Reader"></param>
        public static bool Next(this XmlReader Reader)
        {
            Reader.Read();
            while (Reader.NodeType != XmlNodeType.Element && !Reader.EOF)
                Reader.Read();
            return !Reader.EOF;
        }
        /// <summary>
        /// Liest bis zum nächsten Node, der weder Whitespace noch None ist
        /// <para>gibt !EOF zurück</para>
        /// </summary>
        /// <param name="Reader"></param>
        public static bool NextRelevant(this XmlReader Reader)
        {
            Reader.Read();
            while ((Reader.NodeType == XmlNodeType.Whitespace || Reader.NodeType == XmlNodeType.None) && !Reader.EOF)
                Reader.Read();
            return !Reader.EOF;
        }

        public static string DumpInfo(this XmlReader Reader)
        {
            string s = Reader.Name + " (" + Reader.NodeType + ") in " + Reader.BaseURI + "\r\n";
            IXmlLineInfo impl = Reader as IXmlLineInfo;
            s += "Z: " + impl.LineNumber + ", S: " + impl.LinePosition;
            s += "\r\n";
            s += Reader.AttributeCount + " Attribute: \r\n";
            for (int i = 0; i < Reader.AttributeCount; i++)
                s += Reader.GetAttribute(i) + ", ";
            s += "\r\nTiefe: " + Reader.Depth;
            return s;
        }
        public static void Dump(this XmlReader Reader)
        {
            MessageBox.Show(Reader.DumpInfo());
        }

        public static XmlReader GetXmlReader(this string File)
        {
            XmlReader reader = XmlReader.Create(File);
            reader.NextRelevant();
            reader.NextRelevant();
            return reader;
        }
        public static XmlWriter GetXmlWriter(this string File)
        {
            XmlWriterSettings Settings = new XmlWriterSettings();
            Settings.CloseOutput = true;
            Settings.Indent = true;
            Settings.IndentChars = "    ";
            Settings.NewLineChars ="\r\n";
            Settings.NewLineHandling = NewLineHandling.Replace;
            Settings.NewLineOnAttributes = true;
            XmlWriter writer = XmlWriter.Create(File, Settings);
            return writer;
        }
    }
}
