//#define CSTRINGDEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Mathematik;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    /// <summary>
    /// Now here's the real deal.
    /// </summary>
    public class CString : PreText
    {
        public CString()
            : base()
        {
        }
        public CString(PreText PreText)
            : base(PreText)
        {

        }
        public CString(params DrawBox[] DrawBoxes)
            :base()
        {
            this.addRange(DrawBoxes);
        }

        protected override void Assigne(PreText.Line Line)
        {
            Line.ComplexAssignment();
        }
        public override DrawBox clone()
        {
            return new CString(this);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "CString:");
            sb.AppendLine(tabs + "\tbox: " + box);
            foreach (DrawBox item in words)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
