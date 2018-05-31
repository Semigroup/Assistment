using System.Drawing;

namespace Assistment.Drawing.Graph
{
    public struct Kante
    {
        /// <summary>
        /// endknoten
        /// </summary>
        public Knoten ziel;
        public Pen stift;

        public Kante(Knoten ziel, Pen stift)
        {
            this.ziel = ziel;
            this.stift = stift;
        }
    }
}
