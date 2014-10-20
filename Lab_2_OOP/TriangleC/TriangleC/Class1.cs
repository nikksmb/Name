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
    public TriangleC(List<Point> ListP)
    {
        if (ListP.Count > 2)
        {
            A = ListP[0];
            B = ListP[1];
            C = ListP[2];
            Ready = true;
        }
        else
        {
            Ready = false;
        }
    }
    public override void Draw(Graphics gr)
    {
        if (Ready)
        {
            gr.DrawLine(Pens.Green, A, B);
            gr.DrawLine(Pens.Green, B, C);
            gr.DrawLine(Pens.Green, C, A);
        }
    }
}