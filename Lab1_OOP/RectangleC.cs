using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    public class RectangleC:ShapeC
    {
       // private int x1, y1, x2, y2;
        public RectangleC(int a, int b, int c, int d)
        {
            x1 = a;
            y1 = b;
            x2 = c;
            y2 = d;
        }
        public override void Draw()
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics = Form.ActiveForm.CreateGraphics();
            formGraphics.DrawRectangle(myPen, x1, y1, x2, y2);
            myPen.Dispose();
            formGraphics.Dispose();
        }
    }
