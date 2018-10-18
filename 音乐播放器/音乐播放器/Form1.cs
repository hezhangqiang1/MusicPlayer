using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace 音乐播放器
{
    public partial class Form1 : Form
    {
        double max, min, bai;
        Thread th1;
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            timer_jc.Enabled = false;
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            double now_value = (trackBar1.Value * 0.1) * 0.1 * max;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = now_value;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer_jc.Enabled = true;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1 .SelectedIndex == -1)
            {
                return;
            }
            axWindowsMediaPlayer1.URL = listBox1.SelectedItem.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }
        private delegate void read_value();
        private void r()
        {
            read_value rv = new 音乐播放器.Form1.read_value(read);
            this.Invoke(rv);
        }
        private void read()
        {
            System.IO.FileStream fs = new System.IO.FileStream("C:\\Users\\hello何同学\\Desktop\\geci.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs, Encoding.Default);
            while(!sr.EndOfStream)
            {
                listBox1.Items.Add(sr.ReadLine());
            }
            sr.Close();
            fs.Close();
            th1.Abort();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string save = "";
            for(int i=0;i<listBox1.Items.Count; i++)
            {
                save += listBox1.Items[i].ToString() + "\r\n"; 
            }
            System.IO.FileStream fs = new System.IO.FileStream("C:\\Users\\hello何同学\\Desktop\\geci.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, Encoding.Default);
            sw.Write(save);
            fs.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th1 = new Thread(new ThreadStart(r));
            th1.IsBackground = true;
            th1.Start();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listBox1 .SelectedIndex == -1)
            {
                return;
            }
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void 清空列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if(MessageBox .Show ("是否清空列表？")==DialogResult.OK)
            {
                listBox1.Items.Clear();
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar2.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "音频文件（*.mp3)|*.mp3";
            if(open.ShowDialog()==DialogResult.OK)

            {
                max = 0;min = 0;bai = 0;trackBar1.Value = 1;
                timer_jc.Enabled = false;
                axWindowsMediaPlayer1.URL = open.FileName;
                listBox1.Items.Add(open.FileName);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                timer_jc.Enabled = true;
            }


        }

        private void timer_jc_Tick(object sender, EventArgs e)
        {
            max = axWindowsMediaPlayer1.currentMedia.duration;
            min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            bai = min / max;
            trackBar1.Value = (int)(bai * 100);
        }
    }
}
