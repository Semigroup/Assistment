using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Assistment.Texts;
using System.Globalization;

namespace Assistment.Xml
{
    public static class XmlReaderErweiterer
    {
        public static CultureInfo PreferedCultureInfo = new CultureInfo(1031);

        public static void WriteEnum<T>(this XmlWriter writer, string name, T value)
        {
            writer.WriteAttribute(name, Enum.GetName(typeof(T), value));
        }
        public static T GetEnum<T>(this XmlReader reader, string name) where T : struct
        {
            T result = default(T);
            string enumIdentifier = reader.GetString(name);
            Enum.TryParse<T>(enumIdentifier, out result);
            return result;
        }

        public static Image GetImage(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return null;
            else
                return new Bitmap(s);
        }
        public static string GetString(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null)
                return "";
            else
                return s;
        }
        public static string[] GetStrings(this XmlReader reader, string name, string seperators)
        {
            return reader.GetString(name).Split(seperators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        public static int GetInt(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return 0;
            else
                return int.Parse(s, PreferedCultureInfo);
        }
        public static float GetFloat(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return 0;
            else
                return float.Parse(s, PreferedCultureInfo);
        }
        public static SizeF GetSizeF(this XmlReader reader, string name)
        {
            return new SizeF(GetFloat(reader, name + "_X"), GetFloat(reader, name + "_Y"));
        }
        public static PointF GetPointF(this XmlReader reader, string name)
        {
            return new PointF(GetFloat(reader, name + "_X"), GetFloat(reader, name + "_Y"));
        }
        public static Size GetSize(this XmlReader reader, string name)
        {
            return new Size(GetInt(reader, name + "_X"), GetInt(reader, name + "_Y"));
        }
        public static Point GetPoint(this XmlReader reader, string name)
        {
            return new Point(GetInt(reader, name + "_X"), GetInt(reader, name + "_Y"));
        }
        public static bool GetBoolean(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return false;
            else
                return bool.Parse(s);
        }
        public static DateTime GetDate(this XmlReader reader, string name)
        {
            string s = reader.GetAttribute(name);
            if (s == null || s == "")
                return DateTime.Now;
            else
                return DateTime.Parse(s, PreferedCultureInfo);
        }
        public static RectangleF GetRectangle(this XmlReader reader, string name)
        {
            return new RectangleF(
                reader.GetFloat(name + "_X"),
                reader.GetFloat(name + "_Y"),
                reader.GetFloat(name + "_Width"),
                reader.GetFloat(name + "_Height"));
        }

        public static IFontMeasurer GetFontX(this XmlReader reader, string name)
        {
            return new FontGraphicsMeasurer(reader.GetString(name), reader.GetFloat(name + "_Size"));
        }

        public static Color GetColorHexARGB(this XmlReader reader, string name)
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
        public static IEnumerable<T> MultiConcat<T>(this IEnumerable<IEnumerable<T>> listOfLists)
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
        public static void WriteAttribute(this XmlWriter writer, string name, string value)
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
        public static void WriteBoolean(this XmlWriter writer, string name, bool value)
        {
            if (value)
                writer.WriteAttributeString(name, value.ToString(PreferedCultureInfo));
        }
        /// <summary>
        /// schreibt nur, falls value != 0
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void WriteInt(this XmlWriter writer, string name, int value)
        {
            if (value != 0)
                writer.WriteAttributeString(name, value.ToString(PreferedCultureInfo));
        }
        public static void WriteFloat(this XmlWriter writer, string name, float value)
        {
            if (value != 0)
                writer.WriteAttributeString(name, value.ToString(PreferedCultureInfo));
        }
        public static void WriteSize(this XmlWriter writer, string name, SizeF Size)
        {
            WriteFloat(writer, name + "_X", Size.Width);
            WriteFloat(writer, name + "_Y", Size.Height);
        }
        public static void WritePoint(this XmlWriter writer, string name, PointF Point)
        {
            WriteFloat(writer, name + "_X", Point.X);
            WriteFloat(writer, name + "_Y", Point.Y);
        }
        public static void WriteRectangle(this XmlWriter writer, string name, RectangleF rectangle)
        {
            writer.WriteFloat(name + "_X", rectangle.X);
            writer.WriteFloat(name + "_Y", rectangle.Y);
            writer.WriteFloat(name + "_Width", rectangle.Width);
            writer.WriteFloat(name + "_Height", rectangle.Height);
        }
        public static void WriteColorHexARGB(this XmlWriter writer, string name, Color Color)
        {
            writer.WriteAttributeString(name, Color.ToArgb().ToString("x8"));
        }

        public static Pen GetPen(this XmlReader reader, string name)
        {
            return new Pen(reader.GetColorHexARGB(name + "_color"), reader.GetFloat(name + "_width"));
        }
        public static void WritePen(this XmlWriter writer, string name, Pen Pen)
        {
            if (Pen != null)
            {
                writer.WriteColorHexARGB(name + "_color", Pen.Color);
                writer.WriteFloat(name + "_width", Pen.Width);
            }
        }
        public static Font GetFont(this XmlReader reader, string name)
        {
            string s = reader.GetString(name + "_name");
            if (s.Length == 0)
                return null;
            return new Font(s, reader.GetFloat(name + "_size"), reader.GetEnum<FontStyle>(name + "_style"));
        }
        public static void WriteFont(this XmlWriter writer, string name, Font Font)
        {
            if (Font != null)
            {
                writer.WriteAttribute(name + "_name", Font.Name);
                writer.WriteEnum(name + "_style", Font.Style);
                writer.WriteAttribute(name + "_size", Font.Size.ToString());
            }
        }

        public static void Add(this Control.ControlCollection ControlCollection, params Control[] Controls)
        {
            ControlCollection.AddRange(Controls);
        }
        public static void Add<T>(this ICollection<T> Collection, params T[] Values)
        {
            foreach (var item in Values)
                Collection.Add(item);
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
