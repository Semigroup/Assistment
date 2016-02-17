using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assistment.Latex
{
    public class LatexWriter : System.IO.StreamWriter
    {

        public LatexWriter(string path, string documentclass, params string[] options)
            : base(path + ".tex")
        {
            WriteBefehl("documentclass", documentclass, options);
        }

        public void UsePackage(string package, params string[] options)
        {
            WriteBefehl("usepackage", package, options);
        }
        /// <summary>
        /// \befehl[options]{argument}
        /// </summary>
        /// <param name="befehl"></param>
        /// <param name="argument"></param>
        /// <param name="options"></param>
        public void WriteBefehl(string befehl, string argument, params string[] options)
        {
            Write("\"");
            Write(befehl);
            if (options.Length > 0)
            {
                Write("[");
                Write(options[0]);
                for (int i = 1; i < options.Length; i++)
                {
                    Write(", ");
                    Write(options[i]);
                }
                Write("]");
            }
            Write("{");
            Write(argument);
            Write("}");
            WriteLine();
        }

        public void Chapter(string name)
        {
            WriteBefehl("chapter", name);
        }
        public void Section(string name)
        {
            WriteBefehl("section", name);
        }
        public void Subsection(string name)
        {
            WriteBefehl("subsection", name);
        }
        public void Subsubsection(string name)
        {
            WriteBefehl("subsubsection", name);
        }
        public void Paragraph(string name)
        {
            WriteBefehl("paragraph", name);
        }

        public void Absatz()
        {
            WriteLine("//");
        }
    }
}
