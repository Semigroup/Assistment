using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assistment.Parsing
{
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
            List<Token> list = t.tokenize(code);
            token = list.GetEnumerator();
            token.MoveNext();
            return parseProg(new List<Token>());
        }

        public bool halt()
        {
            System.Diagnostics.Debugger.Break();
            return token.MoveNext();
        }

        public Prog parseProg(List<Token> vorzeichen)
        {
            List<Token> subVorzeichen = new List<Token>();
            bool fertig = false;
            while (!fertig)
            {
                switch (token.Current.metaType)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.EndOfFile:
                        throw new NotImplementedException();
                    case TokenMetaType.Klammer:
                        subVorzeichen.AddRange(vorzeichen);
                        return parseKlammer(subVorzeichen);
                    case TokenMetaType.Operation:
                        subVorzeichen.Add(token.Current);
                        token.MoveNext();
                        break;
                    case TokenMetaType.Steuerwort:
                        subVorzeichen.AddRange(vorzeichen);
                        switch (token.Current.type)
                        {
                            case TokenType.If:
                                token.MoveNext();
                                return new IfProg(subVorzeichen, parseProg(new List<Token>()), parseProg(new List<Token>()), parseProg(new List<Token>()));
                            case TokenType.For:
                                token.MoveNext();
                                return new ForProg(subVorzeichen, parseConstraint(), parseProg(new List<Token>()));
                            default:
                                throw new NotImplementedException();
                        }
                    case TokenMetaType.Interpunktion:
                        throw new NotImplementedException();
                    default:
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
                if (token.Current.type == TokenType.EndOfFile)
                    throw new NotImplementedException();
                else if (token.Current.type == TokenType.Komma)
                    throw new NotImplementedException();
                else if (token.Current.type == TokenType.Semikolon)
                    throw new NotImplementedException();
                else
                    progs.Add(parseProg(new List<Token>()));

            token.MoveNext();
            return new ListProg(vorzeichen, progs);
        }
        public Prog parseAusdruck(List<Token> vorzeichen);
        public Prog parseReihe(List<Token> vorzeichen, List<Token> subVorzeichen)
        {
            bool operatorErwartet = false;
            bool ausdruckErwartet = true;
            bool fertig = false;
            Reihe r = new Reihe(vorzeichen);
            while (!fertig)
            {
                switch (token.Current.metaType)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.Klammer:
                        break;
                    case TokenMetaType.Operation:
                        break;
                    case TokenMetaType.Ausdruck:
                        if (ausdruckErwartet)
                            r.addAusdruck(parseAusdruck(subVorzeichen));
                        else
                            fertig = true;
                        break;
                    case TokenMetaType.Steuerwort:
                        if (ausdruckErwartet)
                            r.addAusdruck(parseProg(subVorzeichen));
                        else
                            fertig = true;
                        break;
                    case TokenMetaType.Interpunktion:
                        break;
                    case TokenMetaType.EndOfFile:
                        if (ausdruckErwartet)
                            throw new NotImplementedException();
                        else
                            fertig = true;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return r.ordne();

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
                            //fertig = !token.MoveNext();
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
}
