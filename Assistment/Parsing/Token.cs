using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assistment.Parsing
{
    public enum TokenType
    {
        /// <summary>
        /// Teilt dem Parser mit an dieser Stelle innezuhalten
        /// </summary>
        STOP,

        Zahl,
        KommaZahl,
        String,

        If,
        For,
        Wort,

        KlammerAuf,
        KlammerZu,
        SchweifAuf,
        SchweifZu,
        EckAuf,
        EckZu,

        Komma,
        Punkt,
        Semikolon,

        Stern,
        Hut,
        Plus,
        Minus,
        Slash,
        Prozent,
        Und,
        UndUnd,
        Oder,
        OderOder,
        Nicht,

        WennDann,
        DannWenn,

        Kleiner,
        Größer,
        KleinerGleich,
        GrößerGleich,

        Gleich,
        HutGleich,
        SternGleich,
        SlashGleich,
        PlusGleich,
        MinusGleich,
        ProzentGleich,
        UndGleich,
        UndUndGleich,
        OderGleich,
        OderOderGleich,

        GleichGleich,
        NichtGleich,

        Max,
        EndOfFile
    }
    public enum TokenMetaType
    {
        STOP,
        Klammer,
        Operation,
        Ausdruck,
        Steuerwort,
        Interpunktion,
        EndOfFile
    }

    public struct Token
    {
        public TokenType type;
        public TokenMetaType metaType;
        public string text;
        public int precedence;
        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
            this.metaType = getMetaType(type);
            this.precedence = getPrecedence(type);
        }

        public override string ToString()
        {
            return type.ToString() + ": " + text;
        }

        /// <summary>
        /// true iff this == KlammerAuf | EckAuf | SchweifAuf
        /// </summary>
        /// <returns></returns>
        public bool istAufgehendeKlammer()
        {
            return this.type == TokenType.KlammerAuf
                || this.type == TokenType.EckAuf
                || this.type == TokenType.SchweifAuf;
        }
        /// <summary>
        /// true iff this == KlammerZu | EckZu | SchweifZu
        /// </summary>
        /// <returns></returns>
        public bool istZugehendeKlammer()
        {
            return this.type == TokenType.KlammerZu
                || this.type == TokenType.EckZu
                || this.type == TokenType.SchweifZu;
        }
        /// <summary>
        /// liefert den Type der zugehenden Klammer zu dieser aufgehenden Klammer
        /// </summary>
        /// <returns></returns>
        public TokenType getZugehendeKlammer()
        {
            switch (type)
            {
                case TokenType.KlammerAuf:
                    return TokenType.KlammerZu;
                case TokenType.SchweifAuf:
                    return TokenType.SchweifZu;
                case TokenType.EckAuf:
                    return TokenType.EckZu;
                default:
                    throw new NotImplementedException();
            }
        }

        public bool istLinksGeklammert()
        {
            return linksGeklammert(type);
        }

        public static TokenMetaType getMetaType(TokenType type)
        {
            switch (type)
            {
                case TokenType.STOP:
                    return TokenMetaType.STOP;

                case TokenType.Zahl:
                case TokenType.KommaZahl:
                case TokenType.String:
                case TokenType.Wort:
                    return TokenMetaType.Ausdruck;

                case TokenType.If:
                case TokenType.For:
                    return TokenMetaType.Steuerwort;

                case TokenType.EckAuf:
                case TokenType.EckZu:
                case TokenType.SchweifAuf:
                case TokenType.SchweifZu:
                case TokenType.KlammerAuf:
                case TokenType.KlammerZu:
                    return TokenMetaType.Klammer;

                case TokenType.Komma:
                case TokenType.Punkt:
                case TokenType.Semikolon:
                    return TokenMetaType.Interpunktion;

                case TokenType.Stern:
                case TokenType.Hut:
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Slash:
                case TokenType.Prozent:
                case TokenType.Und:
                case TokenType.UndUnd:
                case TokenType.Oder:
                case TokenType.OderOder:
                case TokenType.Nicht:
                case TokenType.WennDann:
                case TokenType.DannWenn:
                case TokenType.Kleiner:
                case TokenType.Größer:
                case TokenType.KleinerGleich:
                case TokenType.GrößerGleich:
                case TokenType.Gleich:
                case TokenType.HutGleich:
                case TokenType.SternGleich:
                case TokenType.SlashGleich:
                case TokenType.PlusGleich:
                case TokenType.MinusGleich:
                case TokenType.ProzentGleich:
                case TokenType.UndGleich:
                case TokenType.UndUndGleich:
                case TokenType.OderGleich:
                case TokenType.OderOderGleich:
                case TokenType.GleichGleich:
                case TokenType.NichtGleich:
                    return TokenMetaType.Operation;

                case TokenType.EndOfFile:
                    return TokenMetaType.EndOfFile;
                default:
                    throw new NotImplementedException();
            }
        }
        public static int getPrecedence(TokenType operation)
        {
            switch (operation)
            {
                case TokenType.Hut:
                    return 0;
                case TokenType.Slash:
                case TokenType.Stern:
                    return 1;
                case TokenType.Minus:
                case TokenType.Plus:
                    return 2;
                case TokenType.Prozent:
                    return 3;
                case TokenType.Und:
                case TokenType.UndUnd:
                    return 4;
                case TokenType.Oder:
                case TokenType.OderOder:
                    return 5;
                case TokenType.WennDann:
                case TokenType.DannWenn:
                    return 6;
                case TokenType.Kleiner:
                case TokenType.KleinerGleich:
                case TokenType.Größer:
                case TokenType.GrößerGleich:
                    return 7;
                case TokenType.GleichGleich:
                case TokenType.NichtGleich:
                    return 8;
                case TokenType.Gleich:
                case TokenType.HutGleich:
                case TokenType.PlusGleich:
                case TokenType.MinusGleich:
                case TokenType.SternGleich:
                case TokenType.SlashGleich:
                case TokenType.ProzentGleich:
                case TokenType.OderGleich:
                case TokenType.OderOderGleich:
                case TokenType.UndGleich:
                case TokenType.UndUndGleich:
                    return 9;
                case TokenType.Max:
                    return 10;
                default:
                    return -1;
            }
        }
        public static bool linksGeklammert(TokenType operation)
        {
            switch (operation)
            {
                case TokenType.Stern:
                case TokenType.Hut:
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Slash:
                case TokenType.Prozent:
                case TokenType.Und:
                case TokenType.UndUnd:
                case TokenType.Oder:
                case TokenType.OderOder:
                case TokenType.Nicht:
                case TokenType.WennDann:
                case TokenType.DannWenn:
                case TokenType.Kleiner:
                case TokenType.Größer:
                case TokenType.KleinerGleich:
                case TokenType.GrößerGleich:
                case TokenType.GleichGleich:
                case TokenType.NichtGleich:
                    return true;

                case TokenType.Gleich:
                case TokenType.HutGleich:
                case TokenType.SternGleich:
                case TokenType.SlashGleich:
                case TokenType.PlusGleich:
                case TokenType.MinusGleich:
                case TokenType.ProzentGleich:
                case TokenType.UndGleich:
                case TokenType.UndUndGleich:
                case TokenType.OderGleich:
                case TokenType.OderOderGleich:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class Tokenizer
    {
        private static Regex xZeilenende = new Regex("$");

        private static Regex xSTOP = new Regex("#");
        private static Regex xString = new Regex("\".*\"", RegexOptions.Singleline);
        private static Regex xKlammerAuf = new Regex(@"\(");
        private static Regex xKlammerZu = new Regex(@"\)");
        private static Regex xSchweifAuf = new Regex(@"{");
        private static Regex xSchweifZu = new Regex(@"}");
        private static Regex xEckAuf = new Regex(@"\[");
        private static Regex xEckZu = new Regex(@"\]");

        private static Regex xZahl = new Regex(@"\d+");
        private static Regex xKommaZahl = new Regex(@"\d*\.\d+");
        private static Regex xKomma = new Regex(@",");
        private static Regex xPunkt = new Regex(@"\.(?!\d)");
        private static Regex xSemikolon = new Regex(@";");

        private static Regex xStern = new Regex(@"\*(?!=)");
        private static Regex xHut = new Regex(@"\^(?!=)");
        private static Regex xPlus = new Regex(@"\+(?!=)");
        private static Regex xMinus = new Regex(@"-(?!=)");
        private static Regex xSlash = new Regex(@"/(?!=)");
        private static Regex xProzent = new Regex(@"%(?!=)");
        private static Regex xUnd = new Regex(@"&(?![&=])");
        private static Regex xUndUnd = new Regex(@"&&(?![=])");
        private static Regex xOder = new Regex(@"\|(?![\|=])");
        private static Regex xOderOder = new Regex(@"\|\|(?!=)");
        private static Regex xNicht = new Regex(@"!(?!=)");

        private static Regex xWennDann = new Regex(@"=>");
        private static Regex xDannWenn = new Regex(@"=<");

        private static Regex xKleiner = new Regex(@"<(?!=)");
        private static Regex xKleinerGleich = new Regex(@"<=");
        private static Regex xGrosser = new Regex(@">(?!=)");
        private static Regex xGrosserGleich = new Regex(@">=");

        private static Regex xIf = new Regex(@"(?<![a-zA-Z])if(?![a-zA-Z])");
        private static Regex xElse = new Regex(@"(?<![a-zA-Z])else(?![a-zA-Z])");
        private static Regex xFor = new Regex(@"(?<![a-zA-Z])for(?![a-zA-Z])");
        private static Regex xWhile = new Regex(@"(?<![a-zA-Z])while(?![a-zA-Z])");
        private static Regex xReturn = new Regex(@"(?<![a-zA-Z])return(?![a-zA-Z])");
        private static Regex xWort = new Regex("[A-Za-z][A-Za-z0-9]*");

        private static Regex xGleich = new Regex(@"(?<![=\*/\^\+\-%&\|<>])=(?![=<>])");
        private static Regex xGleichGleich = new Regex(@"==");
        private static Regex xNichtGleich = new Regex(@"!=");
        //private static Regex xGleichNicht = new Regex(@"=!");

        private static Regex xSternGleich = new Regex(@"\*=");
        private static Regex xSlashGleich = new Regex(@"/=");
        private static Regex xHutGleich = new Regex(@"\^=");
        private static Regex xPlusGleich = new Regex(@"\+=");
        private static Regex xMinusGleich = new Regex(@"-=");
        private static Regex xProzentGleich = new Regex(@"%=");
        private static Regex xUndGleich = new Regex(@"(?<!&)&=");
        private static Regex xOderGleich = new Regex(@"(?<!\|)\|=");
        private static Regex xUndUndGleich = new Regex(@"&&=");
        private static Regex xOderOderGleich = new Regex(@"\|\|=");


        private string code;
        private List<string> satz = new List<string>();

        public Regex getRegex(TokenType token)
        {
            switch (token)
            {
                case TokenType.STOP:
                    return xSTOP;

                case TokenType.String:
                    return xString;
                case TokenType.KlammerAuf:
                    return xKlammerAuf;
                case TokenType.KlammerZu:
                    return xKlammerZu;
                case TokenType.EckAuf:
                    return xEckAuf;
                case TokenType.EckZu:
                    return xEckZu;
                case TokenType.SchweifAuf:
                    return xSchweifAuf;
                case TokenType.SchweifZu:
                    return xSchweifZu;

                case TokenType.Zahl:
                    return xZahl;
                case TokenType.KommaZahl:
                    return xKommaZahl;
                case TokenType.Komma:
                    return xKomma;
                case TokenType.Punkt:
                    return xPunkt;
                case TokenType.Semikolon:
                    return xSemikolon;
                case TokenType.Stern:
                    return xStern;
                case TokenType.Hut:
                    return xHut;
                case TokenType.Plus:
                    return xPlus;
                case TokenType.Minus:
                    return xMinus;
                case TokenType.Slash:
                    return xSlash;
                case TokenType.Prozent:
                    return xProzent;
                case TokenType.Und:
                    return xUnd;
                case TokenType.UndUnd:
                    return xUndUnd;
                case TokenType.Oder:
                    return xOder;
                case TokenType.OderOder:
                    return xOderOder;
                case TokenType.Nicht:
                    return xNicht;

                case TokenType.WennDann:
                    return xWennDann;
                case TokenType.DannWenn:
                    return xDannWenn;
                case TokenType.Kleiner:
                    return xKleiner;
                case TokenType.Größer:
                    return xGrosser;
                case TokenType.KleinerGleich:
                    return xKleinerGleich;
                case TokenType.GrößerGleich:
                    return xGrosserGleich;

                case TokenType.If:
                    return xIf;
                case TokenType.For:
                    return xFor;
                case TokenType.Wort:
                    return xWort;

                case TokenType.SlashGleich:
                    return xSlashGleich;
                case TokenType.SternGleich:
                    return xSternGleich;
                case TokenType.HutGleich:
                    return xHutGleich;
                case TokenType.PlusGleich:
                    return xPlusGleich;
                case TokenType.MinusGleich:
                    return xMinusGleich;
                case TokenType.ProzentGleich:
                    return xProzentGleich;
                case TokenType.UndGleich:
                    return xUndGleich;
                case TokenType.UndUndGleich:
                    return xUndUndGleich;
                case TokenType.OderGleich:
                    return xOderGleich;
                case TokenType.OderOderGleich:
                    return xOderOderGleich;
                case TokenType.Gleich:
                    return xGleich;
                case TokenType.GleichGleich:
                    return xGleichGleich;
                case TokenType.NichtGleich:
                    return xNichtGleich;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// added code[a]...code[b - 1]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void add(int a, int b)
        {
            int c = b - a;
            if (c > 0)
                satz.Add(code.Substring(a, c));
        }
        public void clear()
        {
            satz.Clear();
            int a = 0;
            int b = 0;
            while (b < code.Length)
                switch (code[b])
                {
                    case '\n':
                    case '\r':
                    case '\t':
                    case ' ':
                        add(a, b);
                        a = b = b + 1;
                        break;

                    case '"':
                        add(a, b);
                        a = b;
                        b = code.IndexOf('"', a + 1);
                        while ((b >= 0) && code[b - 1] == '\\')
                            b = code.IndexOf('"', b + 1);
                        b++;
                        if (b <= 0)
                            b = code.Length;
                        add(a, b);
                        a = b = b + 1;
                        break;
                    default:
                        b++;
                        break;
                }
            add(a, b);
        }
        public List<Token> tokenize(string code)
        {
            this.code = code;
            this.clear();
            List<Token> l = new List<Token>();
            foreach (string wort in satz)
            {
                int p = 0;
                while (p < wort.Length)
                    for (int i = 0; i < (int)TokenType.Max; i++)
                    {
                        TokenType t = (TokenType)i;
                        Match m = getRegex(t).Match(wort, p);
                        if (m.Success && m.Index == p)
                        {
                            l.Add(new Token(t, m.Value));
                            p += m.Value.Length;
                            break;
                        }
                    }
            }
            l.Add(new Token(TokenType.EndOfFile, ""));
            return l;
        }
    }
}
