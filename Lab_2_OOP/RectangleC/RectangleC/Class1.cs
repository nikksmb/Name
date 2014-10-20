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
        protected int Left, Top, Right, Bottom;
        public RectangleC(List<Point> ListP)
        {
            if (ListP.Count > 1)
            {
                if (ListP[0].X > ListP[1].X)
                {
                    Left = ListP[1].X;
                    Right = ListP[0].X - Left;
                }
                else
                {
                    Left = ListP[0].X;
                    Right = ListP[1].X - Left;
                }
                if (ListP[0].Y > ListP[1].Y)
                {
                    Top = ListP[1].Y;
                    Bottom = ListP[0].Y - Top;
                }
                else
                {
                    Top = ListP[0].Y;
                    Bottom = ListP[1].Y - Top;
                }
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
                gr.DrawRectangle(Pens.Green, Left, Top, Right, Bottom);
            }
        }
    }
