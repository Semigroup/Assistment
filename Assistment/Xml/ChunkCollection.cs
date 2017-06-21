using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assistment.Xml;


namespace Assistment.Xml
{
    public class ChunkCollection<T> : XmlFile where T : XmlFile, new()
    {
        private SortedDictionary<string, T> Chunks { get; set; }
        public IEnumerable<string> Keys { get { return Chunks.Keys; } }

        public ChunkCollection(string XmlName, string FileName)
            : base(XmlName)
        {
            this.Read(FileName);
        }

        public T this[string Name] { get { return Chunks[Name]; } }
        public override void ReadIntern(XmlReader Reader)
        {
            base.ReadIntern(Reader);
            Chunks = new SortedDictionary<string, T>();
            Reader.NextRelevant();
            while (Reader.Name != XmlName)
            {
                T Chunk = new T();
                Chunk.Read(Reader);
                Chunks.Add(Chunk.Name, Chunk);
            }
        }
    }
}
