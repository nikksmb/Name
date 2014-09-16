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
    protected int x3,y3;
    public TriangleC(int a, int b, int c, int d, int e, int f)
    {
        x1 = a;
        y1 = b;
        x2 = c;
        y2 = d;
        x3 = e;
        y3 = f;
    }
    public override void Draw()
    {
        System.Drawing.Pen myPen;
        myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        System.Drawing.Graphics formGraphics = Form.ActiveForm.CreateGraphics();
        formGraphics.DrawLine(myPen, x1, y1, x2, y2);
        formGraphics.DrawLine(myPen, x2, y2, x3, y3);
        formGraphics.DrawLine(myPen, x1, y1, x3, y3);
        myPen.Dispose();
        formGraphics.Dispose();
    }
}