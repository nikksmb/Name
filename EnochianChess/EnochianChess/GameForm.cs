using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnochianChess
{
    public partial class GameForm : Form
    {
        static Bitmap[,] chessmen = new Bitmap[4,9] { {Properties.Resources.YellowKing, Properties.Resources.YellowQueen,
        Properties.Resources.YellowBishop, Properties.Resources.YellowKnight, Properties.Resources.YellowTower,
        Properties.Resources.YellowPawnQueen, Properties.Resources.YellowPawnBishop,Properties.Resources.YellowPawnKnight,
        Properties.Resources.YellowPawnTower},{Properties.Resources.RedKing, Properties.Resources.RedQueen,
        Properties.Resources.RedBishop, Properties.Resources.RedKnight, Properties.Resources.RedTower,
        Properties.Resources.RedPawnQueen, Properties.Resources.RedPawnBishop,Properties.Resources.RedPawnKnight,
        Properties.Resources.RedPawnTower},{Properties.Resources.BlueKing, Properties.Resources.BlueQueen,
        Properties.Resources.BlueBishop, Properties.Resources.BlueKnight, Properties.Resources.BlueTower,
        Properties.Resources.BluePawnQueen, Properties.Resources.BluePawnBishop,Properties.Resources.BluePawnKnight,
        Properties.Resources.BluePawnTower},{Properties.Resources.BlackKing, Properties.Resources.BlackQueen,
        Properties.Resources.BlackBishop, Properties.Resources.BlackKnight, Properties.Resources.BlackTower,
        Properties.Resources.BlackPawnQueen, Properties.Resources.BlackPawnBishop,Properties.Resources.BlackPawnKnight,
        Properties.Resources.BlackPawnTower}};


        static int arrangement = 0;

        static Label[] labelGroup;
        static Button[] buttonGroup;

        static int[, , ,] positionOfChessmen = new int[8, 4, 9, 2] {
        /*Air of Fire and of Earth*/
        {
            //Yellow
        {{1,0},{1,4},{1,1},{1,3},{1,2},{2,4},{2,1},{2,3},{2,2}},
            //Red
        {{8,9},{8,5},{8,8},{8,6},{8,7},{7,5},{7,8},{7,6},{7,7}},
            //Blue
        {{0,8},{4,8},{1,8},{3,8},{2,8},{4,7},{1,7},{3,7},{2,7}},
            //Black
        {{9,1},{5,1},{8,1},{6,1},{7,1},{5,2},{8,2},{6,2},{7,2}}
        },

        /*Air of Air and of Water*/
        {
            //Yellow
        {{1,0},{1,2},{1,1},{1,3},{1,4},{2,2},{2,1},{2,3},{2,4}},
            //Red
        {{8,9},{8,7},{8,8},{8,6},{8,5},{7,7},{7,8},{7,6},{7,5}},
            //Blue
        {{0,8},{2,8},{1,8},{3,8},{4,8},{2,7},{1,7},{3,7},{4,7}},
            //Black
        {{9,1},{7,1},{8,1},{6,1},{5,1},{7,2},{8,2},{6,2},{5,2}}
        },

        /*Fire of Air and of Water*/
        {
            //Yellow
        {{1,0},{1,4},{1,3},{1,1},{1,2},{2,4},{2,3},{2,1},{2,2}},
            //Red
        {{8,9},{8,5},{8,6},{8,8},{8,7},{7,5},{7,6},{7,8},{7,7}},
            //Blue
        {{0,8},{4,8},{3,8},{1,8},{2,8},{4,7},{3,7},{1,7},{2,7}},
            //Black
        {{9,1},{5,1},{6,1},{8,1},{7,1},{5,2},{6,2},{8,2},{7,2}}
        },
        
        /*Water of Air and of Water*/
        {
            //Yellow
        {{1,0},{1,1},{1,2},{1,4},{1,3},{2,1},{2,2},{2,4},{2,3}},
            //Red
        {{8,9},{8,8},{8,7},{8,5},{8,6},{7,8},{7,7},{7,5},{7,6}},
            //Blue
        {{0,8},{1,8},{2,8},{4,8},{3,8},{1,7},{2,7},{4,7},{3,7}},
            //Black
        {{9,1},{8,1},{7,1},{5,1},{6,1},{8,2},{7,2},{5,2},{6,2}}
        },

        /*Earth of Fire and of Earth*/
        {
            //Yellow
        {{1,0},{1,3},{1,2},{1,4},{1,1},{2,3},{2,2},{2,4},{2,1}},
            //Red
        {{8,9},{8,6},{8,7},{8,5},{8,8},{7,6},{7,7},{7,5},{7,8}},
            //Blue
        {{0,8},{3,8},{2,8},{4,8},{1,8},{3,7},{2,7},{4,7},{1,7}},
            //Black
        {{9,1},{6,1},{7,1},{5,1},{8,1},{6,2},{7,2},{5,2},{8,2}}
        },

        /*Earth of Air and of Water*/
        {
            //Yellow
        {{1,0},{1,3},{1,4},{1,2},{1,1},{2,3},{2,4},{2,2},{2,1}},
            //Red
        {{8,9},{8,6},{8,5},{8,7},{8,8},{7,6},{7,5},{7,7},{7,8}},
            //Blue
        {{0,8},{3,8},{4,8},{2,8},{1,8},{3,7},{4,7},{2,7},{1,7}},
            //Black
        {{9,1},{6,1},{5,1},{7,1},{8,1},{6,2},{5,2},{7,2},{8,2}}
        },

        /*Fire of Fire and of Earth*/
        {
            //Yellow
        {{1,0},{1,2},{1,3},{1,1},{1,4},{2,2},{2,3},{2,1},{2,4}},
            //Red
        {{8,9},{8,7},{8,6},{8,8},{8,5},{7,7},{7,6},{7,8},{7,5}},
            //Blue
        {{0,8},{2,8},{3,8},{1,8},{4,8},{2,7},{3,7},{1,7},{4,7}},
            //Black
        {{9,1},{7,1},{6,1},{8,1},{5,1},{7,2},{6,2},{8,2},{5,2}}
        },

        /*Water of Fire and of Earth*/
        {
            //Yellow
        {{1,0},{1,1},{1,4},{1,2},{1,3},{2,1},{2,4},{2,2},{2,3}},
            //Red
        {{8,9},{8,8},{8,5},{8,7},{8,6},{7,8},{7,5},{7,7},{7,6}},
            //Blue
        {{0,8},{1,8},{4,8},{2,8},{3,8},{1,7},{4,7},{2,7},{3,7}},
            //Black
        {{9,1},{8,1},{5,1},{7,1},{6,1},{8,2},{5,2},{7,2},{6,2}}
        },
        
        };

        //CHESSMAN
        private class Chessman
        {
            private Bitmap imageOfChessman;
            int hor, ver;//position
            //there are 8 arrangements
            //NOW IT'S JUST DRAWING,NOTHING MORE! NEXT TIME ANOTHER CLASSES WILL COME!
            //it will be abstract class

            public Chessman(int numberOfChessman, int numberOfColor, int numberOfArrangement )/*
            numbers of chessmen are: 0 - King, 1 - Queen, 2 - Bishop, 3 - Knight, 4 - Tower,
            5 - Pawn of Queen, 6 - Pawn of Bishop, 7 - Pawn of Knight, 8 - Pawn of Tower
            numbers of colors are: 0 - Yellow, 1 - Red, 2 - Blue, 3 - Black
            numbers of arrangements: soon*/
            {
                imageOfChessman = chessmen[numberOfColor,numberOfChessman];
                imageOfChessman.MakeTransparent(Color.White);
                ver = positionOfChessmen[numberOfArrangement, numberOfColor, numberOfChessman, 0];
                hor = positionOfChessmen[numberOfArrangement, numberOfColor, numberOfChessman, 1];
            }

            public Bitmap drawChessmanOnChessboard(Bitmap chessboardBitmap, int x, int y, int sizeOfChessboard)
            {
                Bitmap chessboardSecondBitmap = new Bitmap(chessboardBitmap);
                Graphics chessboardGraphics = Graphics.FromImage(chessboardSecondBitmap);
                //
                int cellSize = sizeOfChessboard / 10;
                chessboardGraphics.DrawImage(imageOfChessman, x + cellSize * hor + cellSize / 6, y + cellSize * ver + cellSize / 10, 2 * cellSize / 3, 4 * cellSize / 5);
                chessboardGraphics.Dispose();
                return chessboardSecondBitmap;
            }

            public Bitmap drawChessmanOnBitmap(Bitmap chessmanBitmap, int x, int y, int width, int height)
            {
                Bitmap chessman = new Bitmap(chessmanBitmap);
                Graphics chessmanGraphics = Graphics.FromImage(chessman);
                chessmanGraphics.DrawImage(imageOfChessman, x, y, width, height);
                chessmanGraphics.Dispose();
                return chessman;
            }

            //useful for help menu
            public void drawChessmanOnBitmap(PictureBox picturebox, int x, int y, int width, int height)
            {
                Bitmap chessman = new Bitmap(picturebox.Width, picturebox.Height);
                Graphics chessmanGraphics = Graphics.FromImage(chessman);
                chessmanGraphics.DrawImage(imageOfChessman, x, y, width, height);
                chessmanGraphics.Dispose();
                picturebox.Image = chessman;
            }


        }

        //CHESSBOARD
        public class Chessboard
        {
            //Game constants
            const int COUNT_OF_PLAYERS = 4;
            const int COUNT_OF_PLAYER_CHESSMEN = 9;
            const int COUNT_OF_CHESSMEN = COUNT_OF_PLAYERS * COUNT_OF_PLAYER_CHESSMEN;
            const int WIDTH_OF_CHESSBOARD = 8;

            private Chessman[] currentState;
            private static Bitmap chessboardBitmap;
            private int x, y, width, height;
            private int sizeOfCell, sizeOfChessboard;
            private int numberOfArrangement;

            public Chessboard(int X, int Y, int Width, int Height, int arrangementNumber)
            {
                x = X;
                y = Y;
                width = Width;
                height = Height;
                sizeOfCell = Math.Min(Width, Height) / 10;
                sizeOfChessboard = Math.Min(width - x, height - y);
                numberOfArrangement = arrangementNumber;
                onceMakeFirstArrangement();
            }

            private void onceMakeFirstArrangement()
            //There are 8 variants of different first positions of chessmen. Possibly this class should
            //get users options from record
            {
                int k = 0;
                currentState = new Chessman[COUNT_OF_CHESSMEN];
                for (int i = 0; i < COUNT_OF_PLAYERS; i++)
                {
                    for (int j = 0; j < COUNT_OF_PLAYER_CHESSMEN; j++)
                    {
                        currentState[k] = new Chessman(j, i, arrangement);
                        k++;
                    }
                }
            }

            public void resizeChessboard(int X, int Y, int Width, int Height)
            {
                x = X;
                x = X;
                y = Y;
                width = Width;
                height = Height;
                sizeOfCell = Math.Min(Width,Height) / 10;
                sizeOfChessboard = Math.Min(width - x,height-y);
            }

            public void drawChessmen()
            {
                for (int i = 0; i < COUNT_OF_CHESSMEN; i++)
                {
                    chessboardBitmap = currentState[i].drawChessmanOnChessboard(chessboardBitmap, x, y, sizeOfChessboard);
                }
            }

            public Bitmap showChessboard()
            {
                drawChessboard();
                drawChessmen();
                return chessboardBitmap;
            }

            public void showChessboard(PictureBox picturebox)
            {
                drawChessboard();
                drawChessmen();
                picturebox.Image = chessboardBitmap;
            }

            private void drawChessboard()
            {
                Color colorOfDarkCell = Color.FromArgb(0, 152, 70);
                Color colorOfLightCell = Color.FromArgb(255, 255, 255);

                if (chessboardBitmap!=null)
                {
                    chessboardBitmap.Dispose();
                }

                chessboardBitmap = new Bitmap(width,height);
                Graphics chessboardGraphics = Graphics.FromImage(chessboardBitmap);

                //creating pen
                Pen chessboardPen = new Pen(Color.Black, 2);

                //creating brushes
                Brush chessboardBackground = new SolidBrush(Color.LightSkyBlue);
                Brush chessboardLightBrush = new SolidBrush(colorOfLightCell);
                Brush chessboardDarkBrush = new SolidBrush(colorOfDarkCell);

                chessboardGraphics.FillRectangle(chessboardBackground, x, y, width, height);

                //drawing chessboard
                for (int i = 1; i <= WIDTH_OF_CHESSBOARD; i++)
                {
                    for (int j = 1; j <= WIDTH_OF_CHESSBOARD; j++)
                    {
                        //Corner cells have special shape, they can't pass here
                        if (!(((i == 1) || (i == WIDTH_OF_CHESSBOARD)) && ((j == 1) || (j == WIDTH_OF_CHESSBOARD))))
                        {
                            if ((i + j) % 2 == 0)
                            {
                                chessboardGraphics.FillRectangle(chessboardLightBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                            }
                            else
                            {
                                chessboardGraphics.FillRectangle(chessboardDarkBrush, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                            }
                        }
                        chessboardGraphics.DrawRectangle(chessboardPen, x + i * sizeOfCell, y + j * sizeOfCell, sizeOfCell, sizeOfCell);
                    }
                }
            
                //Drawing special corners, they are doubled, so some rectangle parametrs are doubled
                chessboardGraphics.FillRectangle(chessboardLightBrush, x + 0 * sizeOfCell, y + 1 * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.DrawRectangle(chessboardPen, x + 0 * sizeOfCell, y + 1 * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.FillRectangle(chessboardLightBrush, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.DrawRectangle(chessboardPen, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell * 2, sizeOfCell);
                chessboardGraphics.FillRectangle(chessboardDarkBrush, x + 1 * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.DrawRectangle(chessboardPen, x + 1 * sizeOfCell, y + WIDTH_OF_CHESSBOARD * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.FillRectangle(chessboardDarkBrush, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + 0 * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.DrawRectangle(chessboardPen, x + WIDTH_OF_CHESSBOARD * sizeOfCell, y + 0 * sizeOfCell, sizeOfCell, sizeOfCell * 2);
                chessboardGraphics.Dispose();
            }
        }

        public GameForm()
        {

            InitializeComponent();


            //creating array of objects for resizing
            labelGroup = new Label[5] {label1, label2, label3, label4, label5 };
            buttonGroup = new Button[2] { button1, button2 };

            tableLayoutPanel1.SizeChanged += GameForm_ResizeEnd;


            //Bitmap btm = Properties.Resources.YellowKing;
            //btm.MakeTransparent(Color.White);
            //pictureBox1.BackColor = Color.Green;
            //pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            //pictureBox1.BackgroundImage = btm;
            
        }

        static int turn=0;
        static Bitmap buttonBackground;

        public void nextTurn()
        {
            switch (turn)
            {
                case 0:
                    label1.Text="Ходит красный игрок";
                    label1.ForeColor = Color.FromArgb(192, 0, 0);
                    label4.ForeColor = Color.FromArgb(192, 0, 0);
                    button1.BackColor = Color.FromArgb(192, 0, 0);
                    button2.BackColor = Color.FromArgb(192, 0, 0);
                    buttonBackground = Properties.Resources.ButtonRed;
                    turn += 1;
                    break;
                case 1:
                    label1.Text = "Ходит черный игрок";
                    label1.ForeColor = Color.FromArgb(157, 157, 158);
                    label4.ForeColor = Color.FromArgb(157, 157, 158);
                    button1.BackColor = Color.FromArgb(157, 157, 158);
                    button2.BackColor = Color.FromArgb(157, 157, 158);
                    buttonBackground = Properties.Resources.ButtonGray;
                    turn += 1;
                    break;
                case 2:
                    label1.Text = "Ходит желтый игрок";
                    label1.ForeColor = Color.FromArgb(255, 237, 0);
                    label4.ForeColor = Color.FromArgb(255, 237, 0);
                    button1.BackColor = Color.FromArgb(255, 237, 0);
                    button2.BackColor = Color.FromArgb(255, 237, 0);
                    buttonBackground = Properties.Resources.ButtonYellow;
                    turn += 1;
                    break;
                case 3:
                    label1.Text = "Ходит синий игрок";
                    label1.ForeColor = Color.FromArgb(0, 160, 227);
                    label4.ForeColor = Color.FromArgb(0, 160, 227);
                    button1.BackColor = Color.FromArgb(0, 160, 227);
                    button2.BackColor = Color.FromArgb(0, 160, 227);
                    buttonBackground = Properties.Resources.ButtonBlue;
                    turn = 0;
                    break;
            }
        }

        private void initializePictureBox(PictureBox picturebox)
        {
           // Graphics graphics = picturebox.CreateGraphics();
            Graphics graphics;
            Bitmap bitmap;
            if (picturebox.Image != null)
            {
                graphics = Graphics.FromImage(picturebox.Image);
                graphics.FillRectangle(Brushes.White, 0, 0, picturebox.Width, picturebox.Height);
                graphics.Dispose();
            }
            else
            {
                bitmap = new Bitmap(picturebox.Width,picturebox.Height);
                graphics = Graphics.FromImage(bitmap);
                graphics.FillRectangle(Brushes.White, 0, 0, picturebox.Width, picturebox.Height);
                graphics.Dispose();
                picturebox.Image = bitmap;
            }
        }

        private void showStockReconArrangement()
        {
            //only chessboard
          //  Bitmap chessboardBuffer = showChessboard(pictureBox1.Width, 0, 0, pictureBox1.Width, pictureBox1.Height);
        //    pictureBox1.Image = chessboardBuffer;
            //now creating chessmen


            //int k = 0;
            //Chessman[] units = new Chessman[36];
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 9; j++)
            //    {
            //        units[k] = new Chessman(j,i,1);
            //        units[k].draw(chessboardBuffer, 0, 0, pictureBox1.Width);
            //        k++;
            //    }
            //}
            //pictureBox1.Image = chessboardBuffer;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = buttonBackground;
            (sender as Button).BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = null;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        Chessboard chessboard;

        private void новаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //if (pictureBox1.Image != null)
                initializePictureBox(pictureBox1);
            
            chessboard = new Chessboard(0, 0, pictureBox1.Width, pictureBox1.Height, arrangement);
            pictureBox1.Image = chessboard.showChessboard();
            //showStockReconArrangement();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //int x = e.X;
            //int y = e.Y;
            //if (x>
        }

        private void mouseMovementsOnChessboard(object sender, MouseEventArgs mouseEventArgs)
        {
            int x = mouseEventArgs.X;
            int y = mouseEventArgs.Y;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            nextTurn();
        }


        private void label4_Click(object sender, EventArgs e)
        {
            arrangement += 1;
            if (arrangement == 8)
            {
                arrangement = 0;
            }
            switch (arrangement)
            {
                case 0:
                    label4.Text = "Air of Fire and of Earth";
                    break;
                case 1:
                    label4.Text = "Air of Air and of Water";
                    break;
                case 2:
                    label4.Text = "Fire of Air and of Water";
                    break;
                case 3:
                    label4.Text = "Water of Air and of Water";
                    break;
                case 4:
                    label4.Text = "Earth of Fire and of Earth";
                    break;
                case 5:
                    label4.Text = "Earth of Air and of Water";
                    break;
                case 6:
                    label4.Text = "Fire of Fire and of Earth";
                    break;
                case 7:
                    label4.Text = "Water of Fire and of Earth";
                    break;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initializePictureBox(pictureBox1);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GameForm_ResizeEnd(object sender, EventArgs e)
        {
            foreach (Label label in labelGroup)
            {
                label.Font = new Font(label.Font.Name, Math.Min(label.Height / 3, label.Width / 3), FontStyle.Regular);
            }

            foreach (Button button in buttonGroup)
            {
                button.Font = new Font(button.Font.Name, Math.Min(button.Height / 6, button.Width / 6), FontStyle.Regular);
            }
            tableLayoutPanel1.Width = 9 * GameForm.ActiveForm.Width / 10;
            tableLayoutPanel1.Height = 9 * GameForm.ActiveForm.Height / 10 - menuStrip1.Height * 2;
            if (chessboard != null)
            {
                chessboard.resizeChessboard(0, 0, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = chessboard.showChessboard();
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
    }
}
