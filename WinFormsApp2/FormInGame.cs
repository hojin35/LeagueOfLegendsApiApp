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
using System.Drawing.Imaging;
using System.Threading.Tasks;

// TODO : Team Selection
// Me : Red => Enemys : Blue
// Me : Blue => Enemys : Red
namespace WinFormsApp2
{
    
    public partial class FormInGame : Form
    {
        int[] timer = new int[10];
        int[] spellCoolTime = new int[10];
        Thread thread;
        bool timerStop = false;
        Dictionary<string, int> spellCoolTimeTable = new Dictionary<string, int>();
        //Task k;
        public FormInGame(Root cur,Dictionary<string,string> championTable, Dictionary<string, string> spellTable,int EnemyTeam, Dictionary<string, int> _spellCoolTimeTable)
        {
            InitializeComponent();
            
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            PictureBox [,]pictureBoxesSpell = new PictureBox[2,5]
                                                    { { pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10 },
                                                    { pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15 } };
            Label[,] labels = { { label1, label2, label3, label4, label5 }, { label6, label7, label8, label9, label10 } };
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

                spellCoolTime[i] = _spellCoolTimeTable[cur.participants[i + EnemyTeam].spell1Id.ToString()];
                spellCoolTime[i+5] = _spellCoolTimeTable[cur.participants[i + EnemyTeam].spell2Id.ToString()];
            }
            //k = SpellTimer(pictureBoxesSpell);
            thread = new Thread(new ParameterizedThreadStart(SpellTimer));
            thread.Start(pictureBoxesSpell);
        }
        
        public void SpellTimer(object obj)
        {
            PictureBox[,] pictureBoxesSpell = new PictureBox[2, 5];
            pictureBoxesSpell = (PictureBox[,])obj;
            while (!timerStop)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (timer[i] > 0)
                    {
                        timer[i] -= 1;
                        if (timer[i] == 0)
                        {
                            string labelName = "label" + (i + 1);
                            Control con = this.Controls[labelName];
                            con.Invoke(new MethodInvoker(delegate { con.Text = ""; }));
                        }
                        else
                        {
                            string labelName = "label" + (i + 1);
                            Control con = this.Controls[labelName];
                            con.Invoke(new  MethodInvoker(delegate { con.Text = timer[i].ToString();}));
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
        private void pictureBox_Click(object sender,EventArgs e)
        {
            if(sender is PictureBox pic)
            {
                // 여기에 시간 받아서 출력하고 스펠 색 낮추기 
               
                timer[pic.TabIndex - 6]= spellCoolTime[pic.TabIndex - 6];
            }
            
            return;
        }

        private void FormInGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerStop = true;
        }
    }
    public class ImageList
    {
        PictureBox[] pic = new PictureBox[3];
    }
}
