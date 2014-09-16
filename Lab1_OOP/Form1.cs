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
        private void shapeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShapeC line = new LineC(100,100,130,200);
            line.Draw();
        }

        private void rectangleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShapeC rectangle = new RectangleC(100, 100, 50, 50);
            rectangle.Draw();
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShapeC triangle = new TriangleC(100, 100, 110, 220, 50, 50);
            triangle.Draw();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShapeC ellipse = new EllipseC(100, 100, 60, 40);
            ellipse.Draw();
            
        }

        private void drawAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd= new Random();
            ShapeC ellipse = new EllipseC(rnd.Next(50), rnd.Next(50), rnd.Next(150), rnd.Next(150));
            ShapeC rectangle = new RectangleC(rnd.Next(50), rnd.Next(50), rnd.Next(350), rnd.Next(150));
            ShapeC triangle = new TriangleC(rnd.Next(50), rnd.Next(50), rnd.Next(150), rnd.Next(150), rnd.Next(200), rnd.Next(200));
            ShapeC line = new LineC(rnd.Next(50), rnd.Next(50), rnd.Next(150), rnd.Next(350));
            ListOfShapes.Add(ellipse);
            ListOfShapes.Add(rectangle);
            ListOfShapes.Add(triangle);
            ListOfShapes.Add(line);
            foreach (ShapeC Shape in ListOfShapes)
            {
                Shape.Draw();
            }
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
            Poly.Draw();
        }
    }
}
