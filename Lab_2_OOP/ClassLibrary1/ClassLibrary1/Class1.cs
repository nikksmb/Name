using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    public abstract class ShapeC
    {
        protected bool Ready;
        public abstract void Draw(Graphics gr);
    }
