using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using Assistment.Xml;

namespace CyberPipeline
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
                throw new NotImplementedException();

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
            if (Reader.Name != listName)
                throw new NotImplementedException();
            SortedDictionary<string, K> dict = new SortedDictionary<string, K>();
            if (Reader.IsEmptyElement)
            {
                Reader.NextRelevant();
                return dict;
            }
            Reader.NextRelevant();
            while (Reader.Name != listName)
            {
                K k = new K();
                k.Read(Reader);
                dict.Add(k.Name, k);
            }
            Reader.NextRelevant();
            return dict;
        }
        public SortedDictionary<string, K> ReadCollection<K>(XmlReader Reader, string listName, Func<K> Creator) where K : XmlFile
        {
            if (Reader.Name != listName)
                throw new NotImplementedException();
            SortedDictionary<string, K> dict = new SortedDictionary<string, K>();
            if (Reader.IsEmptyElement)
            {
                Reader.NextRelevant();
                return dict;
            }
            Reader.NextRelevant();
            while (Reader.Name != listName)
            {
                K k = Creator();
                k.Read(Reader);
                dict.Add(k.Name, k);
            }
            Reader.NextRelevant();
            return dict;
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
