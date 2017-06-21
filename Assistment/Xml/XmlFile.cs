using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assistment.Xml
{
    public abstract class XmlFile
    {
        public string XmlName { get; set; }
        public string Name { get; set; }

        public XmlFile(string XmlName)
        {
            this.XmlName = XmlName;
        }

        public void Read(XmlReader Reader)
        {
            if (!Reader.Name.Equals(XmlName))
                throw new NotImplementedException("Expected: " + XmlName + "\r\nFound: " + Reader.Name);

            ReadIntern(Reader);
            Reader.NextRelevant();
        }
        public void Read(string File)
        {
            XmlReader Reader = XmlTextReader.Create(File);
            Reader.Next();
            Read(Reader);
            Reader.Close();
        }
        public virtual void ReadIntern(XmlReader Reader)
        {
            this.Name = Reader.getString("Name");
        }
        public SortedDictionary<string, K> ReadCollection<K>(XmlReader Reader, string listName) where K : XmlFile, new()
        {
            return ReadCollection<K>(Reader, listName, () => new K());
        }
        public SortedDictionary<string, K> ReadCollection<K>(XmlReader Reader, string listName, Func<K> Creator) where K : XmlFile
        {
            return ReadCollection<string, K>(Reader, listName, Creator, k => k.Name);
        }
        public void ReadCollection<K>(SortedDictionary<string, K> Cached, XmlReader Reader, string listName, Func<K> Creator) where K : XmlFile
        {
            ReadCollection<string, K>(Cached, Reader, listName, Creator, k => k.Name);
        }
        public K[] ReadArray<K>(XmlReader Reader, string listName, Func<K> Creator) where K : XmlFile
        {
            if (Reader.Name != listName)
                throw new NotImplementedException("Found " + Reader.Name + "\r\nExpected " + listName);
            if (Reader.IsEmptyElement)
            {
                Reader.NextRelevant();
                return new K[]{};
            }
            Reader.NextRelevant();
            List<K> ks = new List<K>();
            while (Reader.Name != listName)
            {
                K k = Creator();
                k.Read(Reader);
                ks.Add(k);
            }
            Reader.NextRelevant();
            return ks.ToArray();
        }
        public SortedDictionary<X, K> ReadCollection<X, K>(XmlReader Reader, string listName, Func<K> Creator, Func<K, X> KeyGenerator) where K : XmlFile
        {
            SortedDictionary<X, K> dict = new SortedDictionary<X, K>();
            ReadCollection(dict, Reader, listName, Creator, KeyGenerator);
            return dict;
        }
        public void ReadCollection<X, K>(SortedDictionary<X, K> Cached, XmlReader Reader, string listName, Func<K> Creator, Func<K, X> KeyGenerator) where K : XmlFile
        {
            if (Reader.Name != listName)
                throw new NotImplementedException("Found " + Reader.Name + "\r\nExpected " + listName);
            if (!Reader.IsEmptyElement)
            {
                Reader.NextRelevant();
                while (Reader.Name != listName)
                {
                    K k = Creator();
                    k.Read(Reader);
                    Cached.Add(KeyGenerator(k), k);
                }
            }
            Reader.NextRelevant();
        }

        public void Write(XmlWriter Writer)
        {
            Writer.WriteStartElement(XmlName);
            WriteIntern(Writer);
            Writer.WriteEndElement();
        }
        public virtual void WriteIntern(XmlWriter Writer)
        {
            Writer.writeAttribute("Name", Name);
        }
        public void WriteCollection(XmlWriter Writer, string listName, IEnumerable<XmlFile> Collection)
        {
            Writer.WriteStartElement(listName);
            foreach (var item in Collection)
                item.Write(Writer);
            Writer.WriteEndElement();
        }
    }
}
