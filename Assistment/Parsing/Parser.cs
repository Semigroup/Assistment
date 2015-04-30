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
        public Prog parseAusdruck(List<Token> vorzeichen)
        {
            if (token.Current.metaType != TokenMetaType.Ausdruck) throw new NotImplementedException();

            switch (token.Current.type)
            {
                case TokenType.Zahl:
                case TokenType.KommaZahl:
                case TokenType.String:
                    Konstante konst = new Konstante(vorzeichen, token.Current);
                    token.MoveNext();
                    return konst;
                case TokenType.Wort:
                    return parseBezeichner(vorzeichen);
                default:
                    throw new NotImplementedException();
            }
        }
        public Prog parseReihe(List<Token> vorzeichen, List<Token> subVorzeichen)
        {
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
                        if (ausdruckErwartet)
                        {
                            r.addAusdruck(parseKlammer(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            ausdruckErwartet = false;
                        }
                        else
                            fertig = true;
                        break;
                    case TokenMetaType.Operation:
                        if (ausdruckErwartet)
                            subVorzeichen.Add(token.Current);
                        else
                        {
                            r.addOperator(token.Current);
                            ausdruckErwartet = true;
                        }
                        token.MoveNext();
                        break;
                    case TokenMetaType.Ausdruck:
                        if (ausdruckErwartet)
                        {
                            r.addAusdruck(parseAusdruck(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            ausdruckErwartet = false;
                        }
                        else
                            fertig = true;
                        break;
                    case TokenMetaType.Steuerwort:
                        if (ausdruckErwartet)
                        {
                            r.addAusdruck(parseProg(subVorzeichen));
                            subVorzeichen = new List<Token>();
                            ausdruckErwartet = false;
                        }
                        else
                            fertig = true;
                        break;
                    case TokenMetaType.Interpunktion:
                        if (ausdruckErwartet)
                            throw new NotImplementedException();
                        else
                            fertig = true;
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
            }
            if (ausdruckErwartet)
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
