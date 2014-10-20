using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Graphics Gr;
        Type SelectedType;
        private List<ShapeC> ShapesList;
        Dictionary<string, Type> typeNames= new Dictionary<string, Type>();
        List<Point> ListOfPoints = new List<Point>();

 
        public Form1()
        {
            InitializeComponent();
            string[] dlls = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"Shapes", "*.dll");
            for (int i = 0; i < dlls.Length; i++)
            {
                Assembly assembly = Assembly.LoadFile(dlls[i]);
                Type[] type = assembly.GetTypes();
                
                for (int j = 0; j < type.Length;j++ )
                    if (type[j].IsSubclassOf(typeof(ShapeC)) == true)
                    {
                        typeNames.Add(type[j].Name, type[j]);
                        SelectedType = typeNames[type[j].Name];
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = type[j].Name;
                        item.Click += new EventHandler(SelectedShape);
                        shapesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { item });
                    }
            }
        }

        private void SelectedShape(object sender, EventArgs e)
        {
            SelectedType = typeNames[sender.ToString()];
            ListOfPoints.Clear();
        }


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point point = new Point();
                point.X = e.X;
                point.Y = e.Y;
                ListOfPoints.Add(point);
                Type[] args = new Type[1];
                args[0] = typeof(List<Point>);
                object[] par = new object[1];
                par[0] = ListOfPoints;
                ConstructorInfo CI = SelectedType.GetConstructor(args);
                if (CI != null)
                {
                    Gr = Form.ActiveForm.CreateGraphics();
                    ShapeC shape = (ShapeC)CI.Invoke(par);
                    shape.Draw(Gr);
                    Gr.Dispose();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                ListOfPoints.Clear();

            }
        }
    }
}
