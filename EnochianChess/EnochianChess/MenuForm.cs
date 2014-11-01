﻿using System;
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
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.ButtonGreen;
            (sender as Button).BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm();
            gameForm.Owner = this;
            gameForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MenuForm.ActiveForm.Close();
        }

    }
}
