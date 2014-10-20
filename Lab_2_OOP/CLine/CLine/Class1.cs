using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

    public class LineC:ShapeC
    {
        Point First, Second;

        public LineC(List<Point> ListP)
        {
            if (ListP.Count > 1)
            {
                First = ListP[0];
                Second = ListP[1];
                Ready = true;
            }
            else
            {
                Ready = false;
            }
        }
        public override void  Draw(Graphics gr)
        {
            if (Ready)
            {
                gr.DrawLine(Pens.Green, First, Second);
            }
        }
    }

