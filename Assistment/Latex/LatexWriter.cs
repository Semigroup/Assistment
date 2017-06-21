using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Assistment.Latex
{
    public class LatexWriter : System.IO.StreamWriter
    {
        /// <summary>
        /// Chapter, 0
        /// <para>Section, 1</para>
        /// <para>Subsection, 2</para>
        /// <para>Subsubsection, 3</para>
        /// <para>Paragraph, 4</para>
        /// </summary>
        public int ContentDepth = 0;
        private Stack<string> Environment = new Stack<string>();
        private Regex NewLineRegex = new Regex("\r?\n");

        public LatexWriter(string Path)
            : base(Path)
        {

        }

        public void Init(string documentclass, params string[] options)
        {
            WriteBefehl("documentclass", documentclass, options);
        }

        public void MakeTitle()
        {
            WriteBefehl("maketitle");
        }
        public void MakeTableOfContents()
        {
            WriteBefehl("tableofcontents");
        }
        public void SetTitlepage(string title, string author, string date)
        {
            WriteBefehl("title", title);
            WriteBefehl("author", author);
            WriteBefehl("date", date);
        }
        public void RenewCommand(string command,  string makro)
        {
            RenewCommand(command, 0, makro);
        }
        public void RenewCommand(string command, int args, string makro)
        {
            if (args > 0)
                WriteLine(@"\renewcommand{" + command+"}[" +args+"]{" +makro+"}");
            else
                WriteLine(@"\renewcommand{" + command + "}{" + makro + "}");
        }

        public void BeginDocument()
        {
            BeginEnvironment("document");
        }
        public void BeginEnvironment(string environment, params string[] options)
        {
            WriteBefehl("begin", environment, options);
            Environment.Push(environment);
        }
        public void EndEnvironment()
        {
            WriteBefehl("end", Environment.Pop());
        }

        public override void Close()
        {
            for (int i = 0; i < Environment.Count; i++)
                EndEnvironment();
            base.Close();
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
        public void WriteBefehl(string befehl)
        {
            WriteBefehl(befehl, null);
        }
        public void WriteBefehl(string befehl, string argument, params string[] options)
        {
            Write("\\");
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
            if (argument != null)
            {
                Write("{");
                Write(argument);
                Write("}");
            }
            WriteLine();
        }

        public void ClearDouplePage()
        {
            WriteBefehl("cleardoublepage");
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

        public void NextContent(string TitleName)
        {
            switch (ContentDepth)
            {
                case 0:
                    Chapter(TitleName);
                    break;
                case 1:
                    Section(TitleName);
                    break;
                case 2:
                    Subsection(TitleName);
                    break;
                case 3:
                    Subsubsection(TitleName);
                    break;
                case 4:
                    Paragraph(TitleName);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void PushContent(string TitleName)
        {
            ContentDepth++;
            NextContent(TitleName);
        }
        public void PopContent(string TitleName)
        {
            ContentDepth--;
            NextContent(TitleName);
        }

        public void WriteText(string text)
        {
            Write(NewLineRegex.Replace(text, "\\\\\r\n"));
        }

        public void Absatz()
        {
            WriteLine("//");
        }
    }
}
