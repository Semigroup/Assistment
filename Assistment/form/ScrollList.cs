using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Extensions;

namespace Assistment.form
{
    public class ScrollList : ScrollBox
    {
        public ControlList ControlList { get { return Control as ControlList; } }

        public ScrollList()
            : base(new ControlList())
        {

        }

        public void AddControl(IEnumerable<Control> Controls)
        {
            foreach (var item in Controls)
                ControlList.Add(item);

            ControlList.Setup();
        }
        public void AddControl(params Control[] Controls)
        {
            AddControl((IEnumerable<Control>)Controls);
        }
        public void SetUp()
        {
            ControlList.Setup();
        }
    }
}
