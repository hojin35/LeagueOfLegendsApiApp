using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;


// TODO : Team Selection
// Me : Red => Enemys : Blue
// Me : Blue => Enemys : Red
namespace WinFormsApp2
{
    
    public partial class FormInGame : Form
    {
        int[] timer = new int[10];
        
        public FormInGame(Root cur,Dictionary<string,string> championTable, Dictionary<string, string> spellTable,int EnemyTeam)
        {
            InitializeComponent();
            PictureBox[] pictureBoxes       = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            PictureBox [,]pictureBoxesSpell = new PictureBox[2,5]
                                                    { { pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10 },
                                                    { pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15 } };         
            for (int i=0;i<5;i++)
            {
                string di = new DirectoryInfo(Application.StartupPath +
                @$"Images\Champions\{championTable[cur.participants[i + EnemyTeam].championId.ToString()]}.png").ToString();
                pictureBoxes[i].Load(di);
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;

                string diSpell1 = new DirectoryInfo(Application.StartupPath +
                    @$"Images\Spells\{spellTable[cur.participants[i + EnemyTeam].spell1Id.ToString()]}.png").ToString();
                pictureBoxesSpell[0,i].Load(diSpell1);
                pictureBoxesSpell[0,i].SizeMode = PictureBoxSizeMode.StretchImage;
                string diSpell2 = new DirectoryInfo(Application.StartupPath +
                    @$"Images\Spells\{spellTable[cur.participants[i + EnemyTeam].spell2Id.ToString()]}.png").ToString();
                pictureBoxesSpell[1,i].Load(diSpell2);
                pictureBoxesSpell[1,i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxesSpell[0,i].Click += pictureBox_Click;
                pictureBoxesSpell[1,i].Click += pictureBox_Click;
                
            }
            //Thread thread = new Thread(SpellTimer);
            //thread.Start(pictureBoxesSpell);
        }
        private void pictureBox_Click(object sender,EventArgs e)
        {
            if(sender is PictureBox pic)
            {
                // 여기에 시간 받아서 출력하고 스펠 색 낮추기 
               timer[pic.TabIndex - 6]+=10;
                pic.Visible = false;
            }
            
            return;
        }
        private void SpellTimer(object obj)
        {
            PictureBox[,] pictureBoxesSpell = new PictureBox[2, 5];
            pictureBoxesSpell = (PictureBox[,])obj;
            while(true)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (timer[i] > 0)
                    {
                        timer[i] -= 1;
                        if (timer[i] == 0)
                        {
                            pictureBoxesSpell[i / 5, i % 5].Visible = true;
                        }
                        else
                        {

                        }
                    }
                }
                Thread.Sleep(1000);
            }
           
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
    }
    public class ImageList
    {
        PictureBox[] pic = new PictureBox[3];
    }
}
