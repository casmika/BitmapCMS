using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
namespace BitmapCMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Color RGB = Color.FromArgb(250, 100, 100);
        Color Range = Color.FromArgb(10, 10, 10);

        private void Form1_Load(object sender, EventArgs e)
        {
            get_listCam();

            Bitmap temp = new Bitmap(228, 28);
            CMSImage.SetColor(temp, RGB);
            pictureBox3.Image = temp;

            numericUpDown1.Value = RGB.R;
            numericUpDown2.Value = RGB.G;
            numericUpDown3.Value = RGB.B;

            numericUpDown6.Value = Range.R;
            numericUpDown5.Value = Range.G;
            numericUpDown4.Value = Range.B; 
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == "- Refresh -")
            {
                get_listCam();
            }
        }
        private FilterInfoCollection webcam = null;
        private VideoCaptureDevice cam = null;

        private void get_listCam()
        {
            try
            {
                toolStripComboBox1.Items.Clear();
                webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo Vid in webcam)
                {
                    toolStripComboBox1.Items.Add(Vid.Name);
                }
                toolStripComboBox1.Items.Add("- Refresh -");
                toolStripComboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Error, Please refresh the Video Device List!");
            }

        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripButton1.Text == "Start")
                {
                    toolStripButton1.Text = "Close";
                    cam = new VideoCaptureDevice(webcam[toolStripComboBox1.SelectedIndex].MonikerString);
                    cam.NewFrame += Cam_NewFrame;
                    cam.Start();
                }
                else
                {
                    if (cam.IsRunning)
                    {
                        cam.Stop();
                    }
                    toolStripButton1.Text = "Start";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error! Can not start the device.");
            }
        }

        Image bit = null;
        
        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap temp;
                temp = (Bitmap)eventArgs.Frame.Clone();
                bit = CMSImage.Resize(temp, 371, 261);
                pictureBox1.Image = bit;

            }
            catch (Exception)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cam != null)
                if (cam.IsRunning)
                {
                    cam.Stop();
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    timer1.Enabled = true;
                    timer1.Interval = 50;
                    timer1.Start();
                    DateTime dt = DateTime.Now;
                    ptime = dt.ToBinary();
                }
                else
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                }
            }
            catch (Exception)
            {

            }

        }
        long time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (bit != null)
                {
                    DateTime dt = DateTime.Now;
                    time = dt.ToBinary();
                    Bitmap pros = (Bitmap)bit;
                    if (CMSImage.FireDetection(pros, RGB, Range, 100))
                    {
                        textBox3.Text = CMSImage.xc.ToString("F2");
                        textBox4.Text = CMSImage.yc.ToString("F2");
                        label7.Text = "Detected";
                        button2.Text = "Save";
                        textBox1.Text += (Convert.ToDouble(time - ptime) / 10000000.0).ToString("F1") + " s, Obj at " + textBox3.Text + ", " + textBox4.Text + " and  A = " + CMSImage.Ac.ToString("F2") + "\r\n";
                    }
                    else
                    {
                        label7.Text = "Not Detected";
                        // textBox1.Text = ((time - ptime) / 10000).ToString("F0") + " No Object \r\n";
                    }
                    pictureBox2.Image = pros;
                }
            }
            catch (Exception)
            {

            }

        }
        
        void changeRGB()
        {
            try
            {
                RGB = Color.FromArgb(Convert.ToByte(numericUpDown1.Value), Convert.ToByte(numericUpDown2.Value), Convert.ToByte(numericUpDown3.Value));
                Bitmap temp = (Bitmap)pictureBox3.Image;
                CMSImage.SetColor(temp, RGB);
                pictureBox3.Image = temp;
            }
            catch (Exception)
            {


            }

        }

        private void numericUpDown1_MouseDown(object sender, MouseEventArgs e)
        {
            changeRGB();
        }

        private void numericUpDown2_MouseDown(object sender, MouseEventArgs e)
        {
            changeRGB();
        }

        private void numericUpDown3_MouseDown(object sender, MouseEventArgs e)
        {
            changeRGB();
        }

        private void numericUpDown6_MouseDown(object sender, MouseEventArgs e)
        {
            Range = Color.FromArgb(Convert.ToByte(numericUpDown6.Value), Convert.ToByte(numericUpDown5.Value), Convert.ToByte(numericUpDown4.Value));
        }

        private void numericUpDown5_MouseDown(object sender, MouseEventArgs e)
        {
            Range = Color.FromArgb(Convert.ToByte(numericUpDown6.Value), Convert.ToByte(numericUpDown5.Value), Convert.ToByte(numericUpDown4.Value));
        }

        private void numericUpDown4_MouseDown(object sender, MouseEventArgs e)
        {
            Range = Color.FromArgb(Convert.ToByte(numericUpDown6.Value), Convert.ToByte(numericUpDown5.Value), Convert.ToByte(numericUpDown4.Value));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (bit != null && checkBox2.Checked)
                {
                    Bitmap pros = (Bitmap)bit;
                    Color Sel = CMSImage.cekRGB(pros, e.X, e.Y, 5);
                    Bitmap temp = (Bitmap)pictureBox3.Image;
                    CMSImage.SetColor(temp, Sel);
                    pictureBox3.Image = temp;
                    RGB = Sel;
                    numericUpDown1.Value = RGB.R;
                    numericUpDown2.Value = RGB.G;
                    numericUpDown3.Value = RGB.B;
                }
            }
            catch (Exception)
            {

            }

        }
        long ptime = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = "";
                DateTime dt = DateTime.Now;
                ptime = dt.ToBinary();
            }
            catch (Exception)
            {

            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Data|*.txt";
                sfd.Title = "Save File As ...";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = sfd.FileName;
                }
            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    StreamWriter sw = new StreamWriter(textBox2.Text);
                    sw.Write(textBox1.Text);
                    sw.Close();
                    button2.Text = "Saved";
                }
                else {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Data|*.txt";
                    sfd.Title = "Save File As ...";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        textBox2.Text = sfd.FileName;
                        StreamWriter sw = new StreamWriter(sfd.FileName);
                        sw.Write(textBox1.Text);
                        sw.Close();
                        button2.Text = "Saved";
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
