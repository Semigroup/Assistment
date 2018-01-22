﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Texts;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Assistment.Forms
{
    public abstract class FormBox : DrawBox
    {
        public FormContext context { get; private set; }

        public virtual void setContext(FormContext context)
        {
            this.context = context;
        }

        public bool check()
        {
            return Check(context.position);
        }
        public void click()
        {
            this.click(context.position);
        }
        public void move()
        {
            this.move(context.position);
        }
        public void release()
        {
            this.release(context.position);
        }

        public virtual void click(PointF point)
        {
            if (Check(point))
                context.activate(this);
            else
                context.deactivate(this);
        }
        public virtual void move(PointF point)
        {
            if (Check(point))
                context.activate(this);
            else
                context.deactivate(this);
        }
        public virtual void release(PointF point)
        {
            if (Check(point))
                context.activate(this);
            else
                context.deactivate(this);
        }

        public virtual void draw()
        {
            Draw(context);
        }
        public virtual void drawAgain()
        {
            context.clear(Box);
            draw();
        }

        public abstract FormBox flone();
        public override DrawBox Clone()
        {
            return flone();
        }

        public FormContext show()
        {
            FormBox fb = this.flone();
            FormContext con = new FormContext();
            con.setMain(fb);
            con.setup();
            con.open();
            return con;
        }
        public FormContext showDialog()
        {
            FormBox fb = this.flone();
            FormContext con = new FormContext();
            con.setMain(fb);
            con.setup();
            con.openDialog();
            return con;
        }
    }
}
