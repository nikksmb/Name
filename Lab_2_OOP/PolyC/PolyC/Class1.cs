using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class PolyC:ShapeC
{
    protected List<Point> ListOfPoints;
    public PolyC(List<Point> ListP)
    {
        if (ListP.Count > 1)
        {
            ListOfPoints = new List<Point>();
            ListOfPoints = ListP;
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
            int count = ListOfPoints.Count;
            int i;
            for (i = 0; i < count - 1; i = i + 1)
            {
                gr.DrawLine(Pens.Green, ListOfPoints[i], ListOfPoints[i + 1]);
            }
            gr.DrawLine(Pens.Green, ListOfPoints[0], ListOfPoints[i]);
        }
    }
}

