using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace WinFormsApp2
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string targetURL = "https://kr.api.riotgames.com/";

        static string byName = "lol/summoner/v4/summoners/by-name/";
        static string getGameId = "lol/spectator/v4/active-games/by-summoner/";
        string userName = "";
        const string api_key = "your APi Key Input";
        const string getChampionId = "http://ddragon.leagueoflegends.com/cdn/12.8.1/data/ko_KR/champion.json";
        const string getSellId = "https://ddragon.leagueoflegends.com/cdn/12.8.1/data/ko_KR/summoner.json";
        Dictionary<string, string> championTable = new Dictionary<string, string>();
        Dictionary<string, string>spellTable = new Dictionary<string, string>();
        Dictionary<string, int>spellCoolTimeTable = new Dictionary<string, int>();

        User u = new User();
        Task k;
        Task k2;
        private void Form1_Load(object sender, EventArgs e)
        {
            k = GetChampionId();
            k2 = GetSpellId();
        }
        private async Task GetSpellId()
        {
            string result = getResults(getSellId);
            var list = JObject.Parse(result);
            await Task.Run(() =>
           {
               foreach (var index in list["data"])
               {
                   spellTable[index.First["key"].ToString()] = index.First["id"].ToString();
                   spellCoolTimeTable[index.First["key"].ToString()] = int.Parse(index.First["cooldownBurn"].ToString());

                   
                   DownloadSpell(index.First["id"].ToString());
               }
           });

        }
        
        private async Task GetChampionId()
        {
            string result = getResults(getChampionId);
            var list = JObject.Parse(result);
            await Task.Run(() =>
            {
                foreach (var index in list["data"])
                {
                    // 챔피언 이름 & 아이디 저장
                    var championList = index.First["key"].ToString();
                    championTable[index.First["key"].ToString()] = index.First["id"].ToString();
                    Download(index.First["id"].ToString());
                }
            });

        }
        private void Download(string championName)
        {
            FileInfo fi = new FileInfo(Application.StartupPath + @$"Images\Champions\{championName}.png");
            string url = string.Format(@"http://ddragon.leagueoflegends.com/cdn/12.8.1/img/champion/{0}.png", championName);
            if (fi.Exists == true)
            {
                return;
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), fi.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void DownloadSpell(string spellName)
        {
            FileInfo fi = new FileInfo(Application.StartupPath + @$"Images\Spells\{spellName}.png");
            string url = string.Format(@"http://ddragon.leagueoflegends.com/cdn/12.8.1/img/spell/{0}.png", spellName);
            if (fi.Exists == true)
            {
                return;
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), fi.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            userName = textBoxNameInput.Text;
            string url = string.Format(@"{0}{1}{2}?api_key={3}", targetURL, byName, userName, api_key);
            string result = getResults(url);

            if (result == "")
            {
                label1.Text = "존재하지 않는 사용자입니다";
                return;
            }
            var a = JObject.Parse(result);

            label1.Text = a["id"].ToString();
            u.Id = a["id"].ToString();
            u.UserName = userName;
           
        }


        private void button2_Click(object sender, EventArgs e)
        {
            while(!(k.IsCompleted && k2.IsCompleted))
            {
                label1.Text = "이미지 로딩중입니다 기다려 주세요!";
            }

            if (u.Id != null)
            {
                string url = string.Format(@"{0}{1}{2}?api_key={3}", targetURL, getGameId, u.Id, api_key);
                string result = getResults(url);
                if (result == "")
                {
                    label1.Text = "현재 게임중이 아닙니다.";
                    return;
                }


                Root cur = JsonSerializer.Deserialize<Root>(result);

                for (int i = 0; i < 5; i++)
                {
                    if (u.UserName == cur.participants[i].summonerName)
                    {
                        u.EnemyTeam = u.Red;
                    }
                    listBox1.Items.Add(cur.participants[i].summonerName + "   " + championTable[cur.participants[i].championId.ToString()]);
                }
                for (int i = 5; i < 10; i++)
                {
                    if (u.UserName == cur.participants[i].summonerName)
                    {
                        u.EnemyTeam = u.Blue;
                    }
                    listBox2.Items.Add(cur.participants[i].summonerName + "   " + championTable[cur.participants[i].championId.ToString()]);
                }

                FormInGame form = new FormInGame(cur, championTable, spellTable, u.EnemyTeam, spellCoolTimeTable);
                form.Show();


            }
        }
        public static string getResults(string url)
        {
            string result = string.Empty;

            try
            {
                WebClient client = new WebClient();
                using (Stream data = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        string s = reader.ReadToEnd();
                        result = s;

                        reader.Close();
                        data.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
    class User
    {
        public int Blue
        {
            get { return 0; }
        }
        public int Red
        {
            get { return 5; }
        }
        public string Id
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
        public int EnemyTeam
        {
            get; set;
        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Perks
    {
        public List<int> perkIds { get; set; }
        public int perkStyle { get; set; }
        public int perkSubStyle { get; set; }
    }

    public class Participant
    {
        public int teamId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int championId { get; set; }
        public int profileIconId { get; set; }
        public string summonerName { get; set; }
        public bool bot { get; set; }
        public string summonerId { get; set; }
        public List<object> gameCustomizationObjects { get; set; }
        public Perks perks { get; set; }
    }

    public class Observers
    {
        public string encryptionKey { get; set; }
    }

    public class BannedChampion
    {
        public int championId { get; set; }
        public int teamId { get; set; }
        public int pickTurn { get; set; }
    }

    public class Root
    {
        public long gameId { get; set; }
        public int mapId { get; set; }
        public string gameMode { get; set; }
        public string gameType { get; set; }
        public int gameQueueConfigId { get; set; }
        public List<Participant> participants { get; set; }
        public Observers observers { get; set; }
        public string platformId { get; set; }
        public List<BannedChampion> bannedChampions { get; set; }
        public long gameStartTime { get; set; }
        public int gameLength { get; set; }
    }


}
