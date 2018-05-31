using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometries.Typing
{
    public class LetterBox
    {
        public enum Style
        {
            Hard,
            Fitting,
            Approx,
        }

        public PointF Offset { get; set; }

        public float UpperHeight { get; set; }
        public float MiddleHeight { get; set; }
        public float LowerHeight { get; set; }
        public float TotalHeight => UpperHeight + MiddleHeight + LowerHeight;

        public float Width { get; set; }
        public float InterimWidth { get; set; }

        public Style CornerStyle { get; set; }

        public OrientierbarerWeg[] GetCurves(Letter letter)
        {
            PointF[][] transformed = letter.Segments.Map(x => x.Transform(this)).ToArray();
            switch (CornerStyle)
            {
                case Style.Hard:
                    return transformed.Map(ps => OrientierbarerWeg.HartPolygon(ps)).ToArray();
                case Style.Fitting:
                    return transformed.Map(ps => OrientierbarerWeg.PassPolygon(ps)).ToArray();
                case Style.Approx:
                    return transformed.Map(ps => OrientierbarerWeg.ApproxPolygon(ps)).ToArray();
                default:
                    throw new NotImplementedException();
            }
        }

        public static LetterBox GetGoldenCut(float Width)
        {
            return new LetterBox()
            {
                UpperHeight = 0.618f * Width,
                MiddleHeight = Width,
                LowerHeight = 0.618f * Width,
                 Width = Width,
                 InterimWidth = 0.2f * Width,
                 CornerStyle = Style.Hard,
            };
        }
    }
}
