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
    protected List<int> ListOfPoints;
    public PolyC(List<int> ListP)
    {
        ListOfPoints = new List<int>();
        ListOfPoints = ListP;
    }
    public override void Draw()
    {
        int count=ListOfPoints.Count;
        int i; 
        System.Drawing.Pen myPen;
        myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        System.Drawing.Graphics formGraphics = Form.ActiveForm.CreateGraphics();
        for (i=0;i<count-3;i=i+2)
        {
                formGraphics.DrawLine(myPen, ListOfPoints[i], ListOfPoints[i+1], ListOfPoints[i+2], ListOfPoints[i+3]);
        }
        formGraphics.DrawLine(myPen, ListOfPoints[0], ListOfPoints[1], ListOfPoints[i + 0], ListOfPoints[i + 1]);
        myPen.Dispose();
        formGraphics.Dispose();
    }
}

