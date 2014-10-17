﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class EllipseC : ShapeC
{
    int Left, Top, Right, Bottom;
    public EllipseC(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    public override void Draw(Graphics gr)
    {
        //System.Drawing.Pen myPen;
        //myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //System.Drawing.Graphics formGraphics = Form.ActiveForm.CreateGraphics();
        //formGraphics.DrawEllipse(myPen, x1, y1, x2, y2);
        //myPen.Dispose();
        //formGraphics.Dispose();

        gr.DrawEllipse(Pens.Green, Left, Top, Right, Bottom);
    }
}