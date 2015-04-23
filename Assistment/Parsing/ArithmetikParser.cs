using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assistment.Parsing
{
    public struct Objekt
    {
        Typus typ;
        object objekt;
        string bezeichner;
    }

    public class Generika
    {
    }
    public class Typus
    {
        public string name { get; private set; }
        public bool generisch;
        public Generika schema;
        public Typus generus;
        public SortedDictionary<string, Methode> methoden = new SortedDictionary<string, Methode>();

        public Typus(string name)
        {
            this.name = name;
            this.generisch = false;
        }

        public static Typus Void = new Typus("void");
        public static Typus Any = new Typus("any");
        public static Typus Ganzzahl = new Typus("int");
        public static Typus Fliesskommazahl = new Typus("float");
        public static Typus Basis = new Typus("basis");
    }

    public abstract class Methode
    {
        public Typus aufruferTyp;
        public string bezeichner;
        public Typus ruckgabeTyp;
        public Typus[] eingabeTypen;
        /// <summary>
        /// Anzahl der Eingabe Werte
        /// </summary>
        public int stelligkeit;

        public abstract Objekt execute(Objekt aufrufer, List<Objekt> eingabe);
    }
    public abstract class Operator : Methode
    {
        TokenType token;
    }
    public abstract class Vorzeichen : Methode
    {
        TokenType token;
    }
    public abstract class Konversator : Methode
    {
    }

    public abstract class Prog
    {
        protected List<Token> vorzeichen;

        public Prog(List<Token> vorzeichen)
        {
            this.vorzeichen = vorzeichen;
        }

        public void setVorzeichen(List<Token> vorzeichen)
        {
            this.vorzeichen = vorzeichen;
        }

        public abstract Typus getReturnType();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(vorzeichen.Count);
            foreach (var item in vorzeichen)
                sb.Append(item.text);
            return sb.ToString();
        }
    }
    public class LeerProg : Prog
    {
        public LeerProg()
            : base(null)
        {

        }
        public override Typus getReturnType()
        {
            return Typus.Void;
        }
    }
    public class Reihe : Prog
    {
        public List<Token> operatoren = new List<Token>();
        public List<Prog> ausdrucke = new List<Prog>();

        public Reihe(List<Token> vorzeichen)
            : base(vorzeichen)
        {

        }

        public void addKlammer(Prog klammer)
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

        public Prog ordne()
        {
            List<Token> operatoren = new List<Token>(this.operatoren);
            List<Prog> ausdrucke = new List<Prog>(this.ausdrucke);
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
                    Operation op = new Operation(new List<Token>(), operatoren[k], ausdrucke[k], ausdrucke[k + 1]);
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
            IEnumerator<Prog> en = ausdrucke.GetEnumerator();
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

        public override Typus getReturnType()
        {
            return ordne().getReturnType();
        }
    }
    public class Operation : Prog
    {
        public Token operation;
        public Prog operand1, operand2;

        public Operation(List<Token> vorzeichen, Token operation, Prog operand1, Prog operand2)
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
        public override Typus getReturnType()
        {
            return operand1.getReturnType();
        }
    }
    public class Bezeichnerkette : Prog
    {
        public Bezeichnerkette(List<Token> vorzeichen)
            : base(vorzeichen)
        {

        }
        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class Ausdruck : Prog
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

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class ListProg : Prog
    {
        List<Prog> progs;
        public ListProg(List<Token> vorzeichen, List<Prog> progs)
            : base(vorzeichen)
        {
            this.progs = progs;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("{");
            foreach (var item in progs)
            {
                sb.AppendLine();
                sb.Append(item.ToString());
            }
            sb.Append("}");
            return sb.ToString();
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class IfProg : Prog
    {
        Prog ifProg;
        Prog thenProg;
        Prog elseProg;

        public IfProg(List<Token> vorzeichen, Prog ifProg, Prog thenProg, Prog elseProg)
            : base(vorzeichen)
        {
            this.ifProg = ifProg;
            this.thenProg = thenProg;
            this.elseProg = elseProg;
        }
        public IfProg(List<Token> vorzeichen, Prog ifProg, Prog thenProg)
            : base(vorzeichen)
        {
            this.ifProg = ifProg;
            this.thenProg = thenProg;
            this.elseProg = new LeerProg();
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class ForProg : Prog
    {
        Constraint forCons;
        Prog doProg;

        public ForProg(List<Token> vorzeichen, Constraint forCons, Prog doProg)
            : base(vorzeichen)
        {
            this.forCons = forCons;
            this.doProg = doProg;
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class WhileProg : Prog
    {
        Prog whileProg;
        Prog doProg;

        public WhileProg(List<Token> vorzeichen, Prog whileProg, Prog doProg)
            : base(vorzeichen)
        {
            this.whileProg = whileProg;
            this.doProg = doProg;
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Constraint
    {
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

            Prog a = parseProg(new List<Token>());

            System.Windows.Forms.MessageBox.Show(a.ToString());
        }

        public Prog parseProg(List<Token> vorzeichen)
        {
            List<Token> subVorzeichen = new List<Token>();
            switch (token.Current.type)
            {
                case TokenType.STOP:
                    halt();
                    break;
                case TokenType.KlammerAuf:
                    vorzeichen.AddRange(subVorzeichen);
                    return parseKlammer(subVorzeichen);
                case TokenType.Stern:
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Slash:
                case TokenType.Nicht:
                    subVorzeichen.Add(token.Current);
                    break;
                case TokenType.If:
                    vorzeichen.AddRange(subVorzeichen);
                    token.MoveNext();
                    Prog ifProg = parseKlammer(new List<Token>());
                    Prog thenProg = parseKlammer(new List<Token>());
                    if (token.Current.type == TokenType.Else)
                    {
                        token.MoveNext();
                        Prog elseProg = parseKlammer(new List<Token>());
                        return new IfProg(vorzeichen, ifProg, thenProg, elseProg);
                    }
                    else
                        return new IfProg(vorzeichen, ifProg, thenProg);
                case TokenType.For:
                    vorzeichen.AddRange(subVorzeichen);
                    token.MoveNext();
                    return new ForProg(vorzeichen, parseConstraint(), parseKlammer(new List<Token>()));
                case TokenType.While:
                    vorzeichen.AddRange(subVorzeichen);
                    token.MoveNext();
                    return new WhileProg(vorzeichen, parseKlammer(new List<Token>()), parseKlammer(new List<Token>()));
                case TokenType.Return:
                    token.MoveNext();
                    return parseReihe(vorzeichen, subVorzeichen);
                default:
                    return parseReihe(vorzeichen, subVorzeichen);
            }
            throw new NotImplementedException();
        }

        public void halt()
        {
            System.Diagnostics.Debugger.Break();
            token.MoveNext();
        }

        public Constraint parseConstraint()
        {
            throw new NotImplementedException();
        }
        public Prog parseKlammer(List<Token> vorzeichen)
        {
            if (token.Current.type != TokenType.KlammerAuf) throw new Exception();
            token.MoveNext();
            List<Prog> progs = new List<Prog>();
            while (token.Current.type != TokenType.KlammerZu)
                progs.Add(parseProg(new List<Token>()));
            token.MoveNext();
            return new ListProg(vorzeichen, progs);
        }
        public Prog parseReihe(List<Token> vorzeichen, List<Token> subVorzeichen)
        {
            bool operatorErwartet = false;
            Reihe r = new Reihe(vorzeichen);
            do
            {
                switch (token.Current.type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.Wort:
                    case TokenType.String:
                    case TokenType.Zahl:
                    case TokenType.KommaZahl:
                        if (operatorErwartet)
                            return r.ordne();
                        r.addAusdruck(subVorzeichen, token.Current);
                        subVorzeichen = new List<Token>();
                        operatorErwartet = true;
                        break;

                    case TokenType.KlammerAuf:
                        if (operatorErwartet)
                            throw new NotImplementedException();
                        r.ausdrucke.Add(parseKlammer(subVorzeichen));
                        subVorzeichen = new List<Token>();
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
                        return r.ordne();

                    default:
                        throw new NotImplementedException();
                }
            } while (token.MoveNext());
            if (subVorzeichen.Count > 0)
                throw new NotImplementedException();
            return r.ordne();
        }
    }

    public enum TokenType
    {
        /// <summary>
        /// Teilt dem Parser mit an dieser Stelle innezuhalten
        /// </summary>
        STOP,

        //ZeilenEnde,
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

        If,
        Else,
        For,
        While,
        Return,
        Wort,

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
                //case Token.ZeilenEnde:
                //    return xZeilenende;
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
                case TokenType.Else:
                    return xElse;
                case TokenType.For:
                    return xFor;
                case TokenType.While:
                    return xWhile;
                case TokenType.Return:
                    return xReturn;
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
