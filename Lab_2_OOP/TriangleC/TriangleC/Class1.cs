using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class TriangleC : ShapeC
{
    protected Point A, B, C;
    public TriangleC(Point a,Point b,Point c)
    {
        A = a;
        B = b;
        C = c;
    }
    public override void Draw(Graphics gr)
    {
        //System.Drawing.Pen myPen;
        //myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //System.Drawing.Graphics formGraphics = Form.ActiveForm.CreateGraphics();
        gr.DrawLine(Pens.Green, A, B);
        gr.DrawLine(Pens.Green, B, C);
        gr.DrawLine(Pens.Green, C, A);
        //myPen.Dispose();
        //formGraphics.Dispose();
    }
}