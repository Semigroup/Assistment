using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Parsing
{
    public class Parser2
    {
        private IEnumerator<Token> tokenEnumerator;
        private Token token
        {
            get
            {
                return tokenEnumerator.Current;
            }
        }
        private TokenMetaType meta
        {
            get
            {
                return token.metaType;
            }
        }
        private TokenType type
        {
            get
            {
                return token.type;
            }
        }

        private BasisAufruf basis;

        public void setBasis(Typus basisTyp)
        {
            this.basis = new BasisAufruf(basisTyp);
        }
        public Prog parse(string code)
        {
            Tokenizer t = new Tokenizer();
            List<Token> list = t.tokenize(code);
            tokenEnumerator = list.GetEnumerator();
            next();
            return parseProgramm();
        }

        private void next()
        {
            tokenEnumerator.MoveNext();
        }
        private void halt()
        {
            System.Diagnostics.Debugger.Break();
            next();
        }

        public Prog parseProgramm()
        {
            bool fertig = false;
            Reihe reihe = new Reihe();
            reihe.addAusdruck(parseAufruf());

            while (!fertig)
            {
                switch (meta)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.Operation:
                        reihe.addOperator(token);
                        next();
                        reihe.addAusdruck(parseAufruf());
                        break;
                    default:
                        fertig = true;
                        break;
                }
            }
            return reihe.ordne();
        }
        public List<Token> parseVorzeichen()
        {
            bool fertig = false;
            List<Token> vorzeichen = new List<Token>();
            while (!fertig)
            {
                switch (meta)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.Operation:
                        vorzeichen.Add(token);
                        next();
                        break;
                    default:
                        fertig = true;
                        break;
                }
            }
            return vorzeichen;
        }
        public Prog parseAufruf()
        {
            bool fertig = false;
            bool wortErwartet = false;
            List<Token> vorzeichen = parseVorzeichen();
            Prog ausdruck = parseAusdruck();
            while (!fertig)
            {
                switch (type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.Punkt:
                        if (wortErwartet)
                            throw new NotImplementedException();
                        else
                        {
                            wortErwartet = true;
                            next();
                        }
                        break;
                    case TokenType.Wort:
                        if (wortErwartet)
                        {
                            Aufruf aufruf = new Aufruf(ausdruck, token);
                            ausdruck = aufruf;
                            next();
                            wortErwartet = false;
                            if (aufruf.istMethode)
                                aufruf.setArgumente(parseArgumente());
                        }
                        else
                            fertig = true;
                        break;
                    default:
                        fertig = true;
                        break;
                }
            }
            ausdruck.setVorzeichen(vorzeichen);
            return ausdruck;
        }
        public Prog parseAusdruck()
        {
            while (true)
                switch (meta)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.Klammer:
                        return parseKlammer();
                    case TokenMetaType.Ausdruck:
                        switch (token.type)
                        {
                            case TokenType.Zahl:
                            case TokenType.KommaZahl:
                            case TokenType.String:
                                Konstante konstante = new Konstante(token);
                                next();
                                return konstante;
                            case TokenType.Wort:
                                Aufruf aufruf = new Aufruf(basis, token);
                                next();
                                if (aufruf.istMethode)
                                    aufruf.setArgumente(parseArgumente());
                                return aufruf;
                            default:
                                throw new NotImplementedException();
                        }
                    case TokenMetaType.Steuerwort:
                        return parseSteuerwort();
                    case TokenMetaType.Interpunktion:
                        throw new NotImplementedException();
                    case TokenMetaType.EndOfFile:
                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException();
                }
        }
        public Prog parseSteuerwort()
        {
            while (true)
                switch (type)
                {
                    case TokenType.STOP:
                        halt();
                        break;
                    case TokenType.If:
                        next();
                        return new IfProg(parseProgramm(),
                                          parseProgramm(),
                                          parseProgramm());
                    case TokenType.For:
                        next();
                        return new ForProg(parseConstraint(),
                                           parseProgramm());
                    default:
                        throw new NotImplementedException();
                }
        }
        public Prog parseKlammer()
        {
            if (!token.istAufgehendeKlammer()) throw new NotImplementedException();
            TokenType erwarteterSchluss = token.getZugehendeKlammer();
            next();
            ListProg list = new ListProg();
            while (!token.istZugehendeKlammer())
                switch (meta)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.EndOfFile:
                        throw new NotImplementedException();
                    default:
                        list.addProg(parseProgramm());
                        break;
                }
            if (token.type != erwarteterSchluss) throw new NotImplementedException();
            next();
            return list;
        }
        public List<Prog> parseArgumente()
        {
            List<Prog> args = new List<Prog>();
            if (type != TokenType.KlammerAuf) throw new NotImplementedException();
            next();
            while (type != TokenType.KlammerZu)
            {
                switch (meta)
                {
                    case TokenMetaType.STOP:
                        halt();
                        break;
                    case TokenMetaType.Interpunktion:
                        if (type == TokenType.Komma)
                            next();
                        else
                            throw new NotImplementedException();
                        break;
                    case TokenMetaType.EndOfFile:
                        throw new NotImplementedException();
                    default:
                        args.Add(parseProgramm());
                        break;
                }
            }
            next();
            return args;
        }
        public Constraint parseConstraint()
        {
            throw new NotImplementedException();
        }
    }
}
