using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<ShapeC> ListOfShapes = new List<ShapeC>();
        Point A, B, C;
        private void shapeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    Point A, B;
            A.X = 100;
            A.Y = 100;
            B.X = 130;
            B.Y = 200;
            ShapeC line = new LineC(A,B);
            Graphics Gr = Form.ActiveForm.CreateGraphics();
            line.Draw(Gr);
            Gr.Dispose();
        }

        private void rectangleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int top, bottom, left, right;
            top = 100;
            left = 150;
            right = 50;
            bottom = 30;
            ShapeC rectangle = new RectangleC(left,top,right,bottom);
            Graphics Gr = CreateGraphics();
            rectangle.Draw(Gr);
            Gr.Dispose();
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   Point A, B, C;
            A.X = 100;
            A.Y = 100;
            B.X = 110;
            B.Y = 220;
            C.X = 50;
            C.Y = 50;
            ShapeC triangle = new TriangleC(A, B, C);
            Graphics Gr = Form.ActiveForm.CreateGraphics();
            triangle.Draw(Gr);
            Gr.Dispose();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int top, bottom, left, right;
            top = 100;
            left = 100;
            right = 60;
            bottom = 40;
            ShapeC ellipse = new EllipseC(left,top,right,bottom);
            Graphics Gr = Form.ActiveForm.CreateGraphics();
            ellipse.Draw(Gr);
            Gr.Dispose();
        }

        private void drawAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd= new Random();
         //   Point A, B, C;
            A.X = rnd.Next(100);
            A.Y = rnd.Next(100);
            B.X = rnd.Next(150);
            B.Y = rnd.Next(150);
            C.X = rnd.Next(250);
            C.Y = rnd.Next(250);
            ShapeC ellipse = new EllipseC(rnd.Next(50), rnd.Next(50), rnd.Next(150), rnd.Next(150));
            ShapeC rectangle = new RectangleC(rnd.Next(50), rnd.Next(50), rnd.Next(350), rnd.Next(300));
            ShapeC triangle = new TriangleC(A,B,C);
            A.X = rnd.Next(150);
            A.Y = rnd.Next(150);
            B.X = rnd.Next(200);
            B.Y = rnd.Next(200);
            ShapeC line = new LineC(A,B);
            ListOfShapes.Add(ellipse);
            ListOfShapes.Add(rectangle);
            ListOfShapes.Add(triangle);
            ListOfShapes.Add(line);
            Graphics Gr = Form.ActiveForm.CreateGraphics();
            foreach (ShapeC Shape in ListOfShapes)
            {
                Shape.Draw(Gr);
            }
            Gr.Dispose();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.Clear(Color.White);
            formGraphics.Dispose();
            ListOfShapes.Clear();
         /*   foreach (ShapeC Shape in ListOfShapes)
            {
                
            }*/
        }

        private void polygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> ListP = new List<int>();
            ListP.Add(110);
            ListP.Add(110);
            ListP.Add(210);
            ListP.Add(110);
            ListP.Add(410);
            ListP.Add(490);
            ListP.Add(200);
            ListP.Add(209);
            ListP.Add(290);
            ListP.Add(200);
            ShapeC Poly=new PolyC(ListP);
            Graphics Gr = Form.ActiveForm.CreateGraphics();
            Poly.Draw(Gr);
            Gr.Dispose();
        }
    }
}
