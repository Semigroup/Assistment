using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assistment.Parsing
{
    public abstract class ArithmetischerAusdruck
    {
        protected List<Token> vorzeichen;

        public ArithmetischerAusdruck(List<Token> vorzeichen)
        {
            setVorzeichen(vorzeichen);
        }

        public void setVorzeichen(List<Token> vorzeichen)
        {
            this.vorzeichen = new List<Token>();
            if (vorzeichen != null)
                foreach (var item in vorzeichen)
                {
                    switch (item.type)
                    {
                        case TokenType.Stern:
                        case TokenType.Plus:
                            break;
                        case TokenType.Hut:
                        case TokenType.Minus:
                        case TokenType.Slash:
                        case TokenType.Nicht:
                            this.vorzeichen.Add(item);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(vorzeichen.Count);
            foreach (var item in vorzeichen)
                sb.Append(item.text);
            return sb.ToString();
        }
    }
    public class Reihe : ArithmetischerAusdruck
    {
        public List<Token> operatoren = new List<Token>();
        public List<ArithmetischerAusdruck> ausdrucke = new List<ArithmetischerAusdruck>();

        public Reihe(List<Token> vorzeichen)
            : base(vorzeichen)
        {

        }

        public void addKlammer(ArithmetischerAusdruck klammer)
        {
            this.ausdrucke.Add(klammer);
        }
        public void addAusdruck(List<Token> vorzeichen, Token token)
        {
            ausdrucke.Add(new Ausdruck(vorzeichen, token));
        }
        public void addOperator(Token token)
        {
            operatoren.Add(token);
        }

        public ArithmetischerAusdruck ordne()
        {
            List<Token> operatoren = new List<Token>(this.operatoren);
            List<ArithmetischerAusdruck> ausdrucke = new List<ArithmetischerAusdruck>(this.ausdrucke);
            for (int i = 0; i < Token.getPrecedence(TokenType.Max); i++)
            {
                List<int> ops = new List<int>();
                int j = 0;
                bool links = false;
                foreach (var op in operatoren)
                {
                    if (op.precedence == i)
                    {
                        ops.Add(j);
                        links = links || op.istLinksGeklammert();
                    }
                    j++;
                }
                if (links)
                    j = 0;
                else
                    ops.Reverse();
                foreach (var index in ops)
                {
                    int k = links ? index - j++ : index;
                    Operation op = new Operation(null, operatoren[k], ausdrucke[k], ausdrucke[k + 1]);
                    ausdrucke[k] = op;
                    ausdrucke.RemoveAt(k + 1);
                    operatoren.RemoveAt(k);
                }
            }

            ausdrucke[0].setVorzeichen(vorzeichen);
            return ausdrucke[0];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator<ArithmetischerAusdruck> en = ausdrucke.GetEnumerator();
            sb.Append(base.ToString());
            sb.Append("[");
            en.MoveNext();
            sb.Append(en.Current.ToString());
            foreach (var item in operatoren)
            {
                sb.Append(" " + item.text + " ");
                en.MoveNext();
                sb.Append(en.Current.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
    public class Operation : ArithmetischerAusdruck
    {
        public Token operation;
        public ArithmetischerAusdruck operand1, operand2;

        public Operation(List<Token> vorzeichen, Token operation, ArithmetischerAusdruck operand1, ArithmetischerAusdruck operand2)
            : base(vorzeichen)
        {
            this.operation = operation;
            this.operand1 = operand1;
            this.operand2 = operand2;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + operand1.ToString() + " " + operation.text + " " + operand2.ToString() + ")";
        }
    }
    public class Ausdruck : ArithmetischerAusdruck
    {
        Token token;
        public Ausdruck(List<Token> vorzeichen, Token token)
            : base(vorzeichen)
        {
            this.token = token;
        }
        public override string ToString()
        {
            return base.ToString() + token.text;
        }
    }

    public class Parser
    {

        public string code;
        private IEnumerator<Token> token;

        public void parse(string code)
        {
            this.code = code;
            Tokenizer t = new Tokenizer();
            token = t.tokenize(code).GetEnumerator();
            token.MoveNext();

            ArithmetischerAusdruck a = parseReihe(null).ordne();//.ordne();

            System.Windows.Forms.MessageBox.Show(a.ToString());
        }

        public ArithmetischerAusdruck parseKlammer(List<Token> vorzeichen)
        {
            if (token.Current.type != TokenType.KlammerAuf) throw new Exception();
            token.MoveNext();
            ArithmetischerAusdruck a = parseReihe(vorzeichen).ordne();
            if (token.Current.type != TokenType.KlammerZu) throw new Exception();
            token.MoveNext();
            return a;
        }
        public Reihe parseReihe(List<Token> vorzeichen)
        {
            List<Token> subVorzeichen = new List<Token>();
            bool operatorErwartet = false;
            Reihe r = new Reihe(vorzeichen);
            do
            {
                switch (token.Current.type)
                {
                    case TokenType.Wort:
                    case TokenType.String:
                    case TokenType.Zahl:
                    case TokenType.KommaZahl:
                        if (operatorErwartet)
                            throw new NotImplementedException();
                        r.addAusdruck(subVorzeichen, token.Current);
                        subVorzeichen.Clear();
                        operatorErwartet = true;
                        break;

                    case TokenType.KlammerAuf:
                        if (operatorErwartet)
                            throw new NotImplementedException();
                        r.ausdrucke.Add(parseKlammer(subVorzeichen));
                        subVorzeichen.Clear();
                        operatorErwartet = true;
                        break;

                    case TokenType.Gleich:
                    case TokenType.HutGleich:
                    case TokenType.MinusGleich:
                    case TokenType.OderGleich:
                    case TokenType.OderOderGleich:
                    case TokenType.PlusGleich:
                    case TokenType.ProzentGleich:
                    case TokenType.SlashGleich:
                    case TokenType.SternGleich:
                    case TokenType.UndGleich:
                    case TokenType.UndUndGleich:

                    case TokenType.Punkt:
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
                        if (operatorErwartet)
                        {
                            r.addOperator(token.Current);
                            operatorErwartet = false;
                        }
                        else
                            subVorzeichen.Add(token.Current);
                        break;

                    case TokenType.KlammerZu:
                        if (subVorzeichen.Count > 0)
                            throw new NotImplementedException();
                        return r;

                    default:
                        throw new NotImplementedException();
                }
            } while (token.MoveNext());
            if (subVorzeichen.Count > 0)
                throw new NotImplementedException();
            return r;
        }
    }

    public enum TokenType
    {
        //ZeilenEnde,
        Wort,
        String,

        KlammerAuf,
        KlammerZu,
        SchweifAuf,
        SchweifZu,
        EckAuf,
        EckZu,

        Zahl,
        KommaZahl,
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
        //NichtGleich,

        GleichGleich,
        NichtGleich,

        Max,
    }

    public struct Token
    {
        public TokenType type;
        public string text;
        public int precedence;
        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
            this.precedence = getPrecedence(type);
        }

        public override string ToString()
        {
            return type.ToString() + ": " + text;
        }

        public bool istLinksGeklammert()
        {
            return linksGeklammert(type);
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

        private static Regex xString = new Regex("\".*\"", RegexOptions.Singleline);
        private static Regex xWort = new Regex("[A-Za-z][A-Za-z0-9]*");
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
                //case Token.ZeilenEnde:
                //    return xZeilenende;
                case TokenType.String:
                    return xString;
                case TokenType.Wort:
                    return xWort;
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
                //case TokenType.GleichNicht:
                //    return xGleichNicht;
                default:
                    break;
            }
            throw new NotImplementedException();
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
            {
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
            }
            add(a, b);
        }

        public List<Token> tokenize(string code)
        {
            this.code = code;
            this.clear();
            List<Token> l = new List<Token>();

            int p;

            foreach (string wort in satz)
            {
                p = 0;
                while (p < wort.Length)
                {
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
            }

            return l;
        }
    }
}
