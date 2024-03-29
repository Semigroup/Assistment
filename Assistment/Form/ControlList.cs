﻿//#define CONTROLDEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Assistment.Extensions;

namespace Assistment.form
{
    public class ControlList : UserControl, ICollection<Control>
    {
        public List<Control> Liste { get; private set; }
        public float Align { get; set; }

        public ControlList()
        {
            Liste = new List<Control>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
#if CONTROLDEBUG
            e.Graphics.FillRectangle(Brushes.Blue, 0, 0, this.Width, this.Height);
#endif
        }

        public void Setup()
        {
            //MessageBox.Show(DebugInfo());
            Width = 0;
            foreach (var item in Liste)
                if (item.Visible)
                    Width = Math.Max(item.Width, Width);

            int h = 0;
            int d = 10;

            foreach (var item in Liste)
                if (item.Visible)
                {
                    item.Location = new Point((int)((this.Width - item.Width) * Align), d + h);
                    h = item.Bottom;
                }
            Height = h + d;
            //MessageBox.Show(Width + ", " + Height);
            //this.OnResize(new EventArgs());
        }

        public void Add(Control item)
        {
            Liste.Add(item);
            Controls.Add(item);
        }
        public void Clear()
        {
            foreach (var item in Liste)
                Controls.Remove(item);

            Liste.Clear();
        }
        public void CopyTo(Control[] array, int arrayIndex)
        {
            Liste.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return Liste.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(Control item)
        {
            Controls.Remove(item);
            return Liste.Remove(item);
        }
        public IEnumerator<Control> GetEnumerator()
        {
            return Liste.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string DebugInfo()
        {
            string s = "";
            s += Visible ? "Visible: " : "Not: ";
            s += this.Bounds;
            s += "\r\n\r\n";
            foreach (var item in Liste)
            {
                s += item.Visible ? "Visible: " : "Not: ";
                s += item.Bounds;
                s += "\r\n";
            }
            return s;
        }
    }
}
