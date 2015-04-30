using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assistment.Parsing.Testing
{
    public static class Tester
    {
        private static Parser parser = new Parser();

        static Tester()
        {
            parser.setBasis(Typus.Basis);
        }

        public static void runTests(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.spell");
            foreach (var testFile in files)
            {
                string sollFile = testFile.Remove(testFile.Length - 5) + "soll";
                string code = File.ReadAllText(testFile);
                string ergebnis = parser.parse(code).ToString();

                if (!File.Exists(sollFile))
                    File.WriteAllText(sollFile, ergebnis);
                else
                {
                    string soll = File.ReadAllText(sollFile);
                    if (!soll.Equals(ergebnis))
                    {
                        System.Windows.Forms.MessageBox.Show(ergebnis);
                    }
                }
            }
        }
    }
}
