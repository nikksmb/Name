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
            Bitmap bitmap = Properties.Resources.ButtonGreen;
            (sender as Button).BackgroundImage = bitmap;
            (sender as Button).BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Bitmap bitmap;
            bitmap = (Bitmap)(sender as Button).BackgroundImage;
            (sender as Button).BackgroundImage = null;
            bitmap.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm();
            gameForm.Owner = this;
            gameForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MenuForm.ActiveForm.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Owner = this;
            settingsForm.Show();
            this.Hide();
        }

    }
}
