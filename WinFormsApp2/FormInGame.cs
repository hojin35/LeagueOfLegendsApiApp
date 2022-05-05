using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace WinFormsApp2
{
    public partial class FormInGame : Form
    {
        public FormInGame(Root cur,Dictionary<string,string> championTable)
        {
            InitializeComponent();
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            for(int i=0;i<5;i++)
            {
                string di = new DirectoryInfo(Application.StartupPath +
                @$"Images\{championTable[cur.participants[i].championId.ToString()]}.png").ToString();
                pictureBoxes[i].Load(di);
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

}
