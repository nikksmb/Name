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
        public LineC(Point A, Point B)
        {
            First = A;
            Second = B;
        }
        public override void  Draw(Graphics gr)
        {
         /*   System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);*/
            gr.DrawLine(Pens.Green, First, Second);
        }
    }

