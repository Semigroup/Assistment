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
        public SortedDictionary<string, List<Methode>> methoden = new SortedDictionary<string, List<Methode>>();
        public SortedDictionary<Typus, Konversator> konversionen = new SortedDictionary<Typus, Konversator>();
        public SortedDictionary<string, Feld> felder = new SortedDictionary<string, Feld>();

        public Typus(string name)
        {
            this.name = name;
            this.generisch = false;
        }

        public void addFeld(string bezeichner, Typus typ, bool beschreibbar)
        {
            felder.Add(bezeichner, new Feld(this, bezeichner, typ, beschreibbar));
        }

        public bool hatFeld(string bezeichner, out Feld feld)
        {
            return felder.TryGetValue(bezeichner, out feld);
        }
        public bool getMethode(Signatur signatur, out Methode methode)
        {
            List<Methode> methoden;
            if (this.methoden.TryGetValue(signatur.bezeichner, out methoden))
                foreach (Methode item in methoden)
                    if (signatur.konvertierbarZu(item.signatur))
                    {
                        methode = item;
                        return true;
                    }

            methode = null;
            return false;
        }
        public bool hatMethode(string bezeichner)
        {
            return methoden.ContainsKey(bezeichner);
        }
        /// <summary>
        /// gibt an, ob dieser Typ spezieller ist als allgemeinerTyp
        /// </summary>
        /// <param name="allgemeinerTyp"></param>
        /// <returns></returns>
        public bool ist(Typus allgemeinerTyp)
        {
            if (this == allgemeinerTyp)
                return true;
            return konversionen.ContainsKey(allgemeinerTyp);
        }


        public static Typus Void = new Typus("void");
        public static Typus Any = new Typus("any");
        public static Typus Ganzzahl = new Typus("int");
        public static Typus Fliesskommazahl = new Typus("float");
        public static Typus Basis = new Typus("basis");
    }
    public struct Signatur
    {
        public string bezeichner;
        public Typus[] eingabeTypen;
        public int stelligkeit;

        /// <summary>
        /// gibt an, ob diese Signatur spezieller ist als die gegebene
        /// <para>ignoriert die bezeichner</para>
        /// </summary>
        /// <param name="signatur"></param>
        /// <returns></returns>
        public bool konvertierbarZu(Signatur signatur)
        {
            if (stelligkeit == signatur.stelligkeit)
            {
                for (int i = 0; i < stelligkeit; i++)
                    if (!eingabeTypen[i].ist(signatur.eingabeTypen[i]))
                        return false;
                return true;
            }
            else return false;
        }
    }
    public abstract class Methode
    {
        public Signatur signatur;
        public Typus aufruferTyp;
        public Typus ruckgabeTyp;
        /// <summary>
        /// Anzahl der Eingabe Werte
        /// </summary>

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
    public class Feld
    {
        public Typus aufruferTyp;
        public string bezeichner;
        public Typus feldTyp;
        public bool beschreibbar;

        public Feld(Typus aufruferTyp, string bezeichner, Typus feldTyp, bool beschreibbar)
        {

        }

        public bool hatFeld(string bezeichner)
        {
            return feldTyp.felder.ContainsKey(bezeichner);
        }
        public Feld getFeld(string bezeichner)
        {
            Feld f;
            if (feldTyp.hatFeld(bezeichner, out f))
                return f;
            else
                throw new NotImplementedException();
        }
        public bool hatMethode(string bezeichner)
        {
            return feldTyp.methoden.ContainsKey(bezeichner);
        }
        public Methode getMethode(Signatur signatur)
        {
            Methode m;
            if (feldTyp.getMethode(signatur, out m))
                return m;
            else
                throw new NotImplementedException();
        }
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
            StringBuilder sb = new StringBuilder();
            this.IntoFormat(sb, "");
            return sb.ToString();
        }
        public virtual void IntoFormat(StringBuilder format, string einschub)
        {
            foreach (var item in vorzeichen)
                format.Append(item.text);
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
        public void addAusdruck(Prog prog)
        {
            ausdrucke.Add(prog);
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

        public override void IntoFormat(StringBuilder format, string einschub)
        {
            IEnumerator<Prog> en = ausdrucke.GetEnumerator();
            base.IntoFormat(format, einschub);
            format.Append("[");
            en.MoveNext();
            format.Append(en.Current.ToString());
            foreach (var item in operatoren)
            {
                format.Append(" " + item.text + " ");
                en.MoveNext();
                en.Current.IntoFormat(format, einschub);
            }
            format.Append("]");
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

        public override Typus getReturnType()
        {
            return operand1.getReturnType();
        }
        public override void IntoFormat(StringBuilder format, string einschub)
        {
            base.IntoFormat(format, einschub);
            format.Append("(");
            operand1.IntoFormat(format, einschub);
            format.Append(" " + operation.text + " ");
            operand2.IntoFormat(format, einschub);
            format.Append(")");
        }
    }
    public class Aufruf : Prog
    {
        Aufruf basis;
        string aufruf;
        public bool istMethode;
        public bool istHaupt = false;
        List<Prog> argumente;

        public Aufruf(Aufruf basis, string aufruf)
            : base(new List<Token>())
        {
            this.basis = basis;
            this.aufruf = aufruf;
            if (basis != null)
                this.istMethode = basis.getReturnType().hatMethode(aufruf);
            else
                this.istMethode = false;
        }

        public void setArgumente(List<Prog> argumente)
        {
            this.argumente = argumente;
        }

        public override void IntoFormat(StringBuilder format, string einschub)
        {
            base.IntoFormat(format, einschub);
            if (!basis.istHaupt)
            {
                basis.IntoFormat(format, einschub);
                format.Append(".");
            }
            format.Append(aufruf);
            if (istMethode)
            {
                format.Append("(");
                foreach (var item in argumente)
                    item.IntoFormat(format, einschub);
                format.Append(")");
            }
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
    public class BasisAufruf : Aufruf
    {
        Typus basisTyp;
        public BasisAufruf(Typus basisTyp)
            : base(null, basisTyp.name)
        {
            this.basisTyp = basisTyp;
            this.istHaupt = true;
        }

        public override void IntoFormat(StringBuilder format, string einschub)
        {
        }

        public override Typus getReturnType()
        {
            return basisTyp;
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
        public override void IntoFormat(StringBuilder format, string einschub)
        {
            base.IntoFormat(format, einschub);
            format.Append(token.text);
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

        public override void IntoFormat(StringBuilder format, string einschub)
        {
            base.IntoFormat(format, einschub);
            if (this.progs.Count == 0)
                format.Append("()");
            else if (this.progs.Count == 1)
                progs[0].IntoFormat(format, einschub);
            else
            {
                base.IntoFormat(format, einschub);
                format.Append("(");
                string extraEinschub = einschub + "\t";
                foreach (var item in progs)
                {
                    format.AppendLine();
                    format.Append(extraEinschub);
                    item.IntoFormat(format, extraEinschub);
                }
                format.AppendLine();
                format.Append(einschub);
                format.Append(")");
            }
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

        public override void IntoFormat(StringBuilder format, string einschub)
        {
            //string extraEinschub = einschub + "\t";
            base.IntoFormat(format, einschub);
            format.Append("if ");
            ifProg.IntoFormat(format, einschub);
            format.Append(" ");
            //format.AppendLine();
            //format.Append(einschub);
            thenProg.IntoFormat(format, einschub);
            format.Append(" ");
            //format.AppendLine();
            //format.Append(einschub);
            //format.Append(" else ");
            //format.AppendLine();
            //format.Append(einschub);
            elseProg.IntoFormat(format, einschub);
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

    public abstract class Constraint
    {
    }

    public class Parser
    {
        public string code;
        private IEnumerator<Token> token;
        public Aufruf basis;

        public void setBasis(Typus basisTyp)
        {
            this.basis = new BasisAufruf(basisTyp);
            basisTyp.addFeld("a", Typus.Ganzzahl, true);
            basisTyp.addFeld("b", Typus.Ganzzahl, true);
            basisTyp.addFeld("c", Typus.Ganzzahl, true);
            basisTyp.addFeld("d", Typus.Ganzzahl, true);
            basisTyp.addFeld("e", Typus.Ganzzahl, true);
        }

        public Prog parse(string code)
        {
            this.code = code;
            Tokenizer t = new Tokenizer();
            token = t.tokenize(code).GetEnumerator();
            token.MoveNext();
            return parseProg(new List<Token>());
        }

        public void halt()
        {
            System.Diagnostics.Debugger.Break();
            token.MoveNext();
        }

        public Prog parseProg(List<Token> vorzeichen)
        {
            List<Token> subVorzeichen = new List<Token>();
            bool fertig = false;
            while (!fertig)
            {
                switch (token.Current.type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.KlammerAuf:
                        subVorzeichen.AddRange(vorzeichen);
                        return parseKlammer(subVorzeichen);
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
                        subVorzeichen.Add(token.Current);
                        token.MoveNext();
                        break;
                    case TokenType.If:
                        subVorzeichen.AddRange(vorzeichen);
                        token.MoveNext();
                        //Prog ifProg = parseKlammer(new List<Token>());
                        //Prog thenProg = parseKlammer(new List<Token>());
                        //Prog elseProg = parseKlammer(new List<Token>());
                        return new IfProg(subVorzeichen, parseProg(new List<Token>()), parseProg(new List<Token>()), parseProg(new List<Token>()));
                    case TokenType.For:
                        subVorzeichen.AddRange(vorzeichen);
                        token.MoveNext();
                        return new ForProg(subVorzeichen, parseConstraint(), parseKlammer(new List<Token>()));
                    default:
                        string s = token.Current.text;
                        return parseReihe(vorzeichen, subVorzeichen);
                }
            }
            throw new NotImplementedException();
        }
        public Prog parseKlammer(List<Token> vorzeichen)
        {
            if (token.Current.type != TokenType.KlammerAuf) throw new Exception();
            token.MoveNext();
            List<Prog> progs = new List<Prog>();
            while (token.Current.type != TokenType.KlammerZu)
                if (token.Current.text == null)
                    return new ListProg(vorzeichen, progs);
                else
                    progs.Add(parseProg(new List<Token>()));
            token.MoveNext();
            return new ListProg(vorzeichen, progs);
        }
        public Prog parseReihe(List<Token> vorzeichen, List<Token> subVorzeichen)
        {
            bool operatorErwartet = false;
            bool fertig = false;
            Reihe r = new Reihe(vorzeichen);
            while (!fertig)
            {
                switch (token.Current.type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.Wort:
                        if (operatorErwartet)
                            fertig = true;
                        else
                        {
                            r.addAusdruck(parseBezeichner(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            operatorErwartet = true;
                        }
                        break;
                    case TokenType.String:
                    case TokenType.Zahl:
                    case TokenType.KommaZahl:
                        if (operatorErwartet)
                            fertig = true;
                        else
                        {
                            r.addAusdruck(subVorzeichen, token.Current);
                            subVorzeichen = new List<Token>();
                            operatorErwartet = true;
                            fertig = !token.MoveNext();
                        }
                        break;

                    case TokenType.KlammerAuf:
                        if (operatorErwartet)
                            fertig = true;
                        else
                        {
                            r.ausdrucke.Add(parseKlammer(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            operatorErwartet = true;
                        }
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
                        fertig = !token.MoveNext();
                        break;
                    case TokenType.KlammerZu:
                        fertig = true;
                        break;
                    default:
                        if (operatorErwartet)
                            fertig = true;
                        else
                        {
                            r.addAusdruck(parseProg(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            operatorErwartet = true;
                            fertig = !token.MoveNext();
                        }
                        break;
                }
            }
            if (subVorzeichen.Count > 0)
                throw new NotImplementedException();
            if (!operatorErwartet)
                throw new NotImplementedException();
            return r.ordne();
        }
        public Prog parseBezeichner(List<Token> vorzeichen)
        {
            Aufruf upper = basis;
            bool wortErwartet = true;
            bool fertig = false;
            while (!fertig)
            {
                switch (token.Current.type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.Wort:
                        if (wortErwartet)
                        {
                            upper = new Aufruf(upper, token.Current.text);
                            wortErwartet = false;
                        }
                        else
                            fertig = true;
                        break;
                    case TokenType.KlammerAuf:
                        if (wortErwartet)
                            throw new NotImplementedException();
                        if (upper.istMethode)
                            upper.setArgumente(parseArgumente());
                        else
                            fertig = true;
                        break;
                    case TokenType.Punkt:
                        if (wortErwartet)
                            throw new NotImplementedException();
                        wortErwartet = true;
                        break;
                    default:
                        if (wortErwartet)
                            throw new NotImplementedException();
                        else
                            fertig = true;
                        break;
                }
                if (!fertig)
                    fertig = !token.MoveNext();
            }
            if (upper == basis)
                throw new NotImplementedException();
            upper.setVorzeichen(vorzeichen);
            return upper;
        }
        public List<Prog> parseArgumente()
        {
            if (token.Current.type != TokenType.KlammerAuf) throw new NotImplementedException();
            List<Prog> args = new List<Prog>();
            while (token.Current.type != TokenType.KlammerZu)
            {
                switch (token.Current.type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.Komma:
                        token.MoveNext();
                        parseProg(new List<Token>());
                        break;
                }
            }
            token.MoveNext();

            return args;
        }
        public Constraint parseConstraint()
        {
            throw new NotImplementedException();
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
        //Else,
        For,
        //While,
        //Return,
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
                //case TokenType.Else:
                //    return xElse;
                case TokenType.For:
                    return xFor;
                //case TokenType.While:
                //    return xWhile;
                //case TokenType.Return:
                //    return xReturn;
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

            int p;

            foreach (string wort in satz)
            {
                p = 0;
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

            return l;
        }
    }
}
