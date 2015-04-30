using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Parsing
{
    public class Reihe
    {
        public List<Token> operatoren = new List<Token>();
        public List<Prog> ausdrucke = new List<Prog>();
        List<Token> vorzeichen;

        public Reihe(List<Token> vorzeichen)
        {
            this.vorzeichen = vorzeichen;
        }
        public Reihe()
        {
            vorzeichen = new List<Token>();
        }

        public void addAusdruck(Prog prog)
        {
            ausdrucke.Add(prog);
        }
        //public void addAusdruck(List<Token> vorzeichen, Token token)
        //{
        //    ausdrucke.Add(new Ausdruck(vorzeichen, token));
        //}
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

        public void IntoFormat(StringBuilder format, string einschub)
        {
            IEnumerator<Prog> en = ausdrucke.GetEnumerator();
            //base.IntoFormat(format, einschub); Hier kämen die Vorzeichen ins Spiel
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

        public Typus getReturnType()
        {
            return ordne().getReturnType();
        }
    }
    public abstract class Prog
    {
        protected List<Token> vorzeichen;

        public Prog()
        {

        }

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
        Prog basis;
        Token aufruf;
        public bool istMethode;
        List<Prog> argumente;

        public Aufruf(Prog basis, Token aufruf)
            : base(new List<Token>())
        {
            this.basis = basis;
            this.aufruf = aufruf;
            if (basis != null)
                this.istMethode = basis.getReturnType().hatMethode(aufruf.text);
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
            if (!basis.getReturnType().istHaupt)
            {
                basis.IntoFormat(format, einschub);
                format.Append(".");
            }
            format.Append(aufruf.text);
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
            : base(null, new Token())
        {
            this.basisTyp = basisTyp;
        }

        public override void IntoFormat(StringBuilder format, string einschub)
        {
        }

        public override Typus getReturnType()
        {
            return basisTyp;
        }
    }
    public class Konstante : Prog
    {
        Token token;
        public Konstante(List<Token> vorzeichen, Token token)
            : base(vorzeichen)
        {
            this.token = token;
        }
        public Konstante(Token token)
            : base(new List<Token>())
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
        List<Prog> progs = new List<Prog>();
        public ListProg(List<Token> vorzeichen, List<Prog> progs)
            : base(vorzeichen)
        {
            this.progs = progs;
        }

        public ListProg()
            : base(new List<Token>())
        {

        }
        public void addProg(Prog prog)
        {
            progs.Add(prog);
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

        public IfProg(Prog ifProg, Prog thenProg, Prog elseProg)
        {
            this.ifProg = ifProg;
            this.thenProg = thenProg;
            this.elseProg = elseProg;
        }
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
        public ForProg(Constraint forCons, Prog doProg)
        {
            this.forCons = forCons;
            this.doProg = doProg;
        }

        public override Typus getReturnType()
        {
            throw new NotImplementedException();
        }
    }
}
