using System;
using System.Drawing;
using System.Windows.Forms;

namespace AD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double w0, wn, Mn, Mk, sk;
        int x0, y0, xm, ym;
        Color gr = Color.Blue;
        Bitmap bmp;
        bool flag1 = false;
        public double StrCon(string txt) => double.Parse(txt.Replace(".", ","));

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Text = "Построение графиков";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            gr = colorDialog1.Color;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                textBox1.Text = "0.37";
                textBox2.Text = "750";
                textBox3.Text = "7.3";
                textBox4.Text = "2.2";
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Text = "2.2";
                textBox2.Text = "750";
                textBox3.Text = "5.3";
                textBox4.Text = "2";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Text = "4";
                textBox2.Text = "750";
                textBox3.Text = "4.7";
                textBox4.Text = "2.5";
            }
            if (comboBox1.SelectedIndex == 3)
            {
                textBox1.Text = "15";
                textBox2.Text = "750";
                textBox3.Text = "2.7";
                textBox4.Text = "2.2";
            }
            if (comboBox1.SelectedIndex == 4)
            {
                textBox1.Text = "45";
                textBox2.Text = "750";
                textBox3.Text = "1.3";
                textBox4.Text = "2.6";
            }
            if (comboBox1.SelectedIndex == 5)
            {
                textBox1.Text = "110";
                textBox2.Text = "750";
                textBox3.Text = "1.3";
                textBox4.Text = "2.1";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bmp.Save(saveFileDialog1.FileName+".png");
                }
                catch
                {
                    MessageBox.Show("Ошибка сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double Pn = StrCon(textBox1.Text) * 1000,
                n0 = StrCon(textBox2.Text),
                sn = StrCon(textBox3.Text) / 100.0,
                lamda = StrCon(textBox4.Text);
                if ((Pn < 0 || Pn > 1000000) || (n0 < 500 || n0 > 10000) || (sn < 0.002 || sn > 0.3) || (lamda < 1 || lamda > 10))
                {
                    if (Pn < 0 || Pn > 1000000) textBox1.BackColor = Color.IndianRed;
                    if (n0 < 500 || n0 > 10000) textBox2.BackColor = Color.IndianRed;
                    if (sn < 0.002 || sn > 0.3) textBox3.BackColor = Color.IndianRed;
                    if (lamda < 1 || lamda > 10) textBox4.BackColor = Color.IndianRed;
                    throw new Exception();
                }
                else
                {
                    textBox1.BackColor = Color.White;
                    textBox2.BackColor = Color.White;
                    textBox3.BackColor = Color.White;
                    textBox4.BackColor = Color.White;
                }
                flag1 = true;
                try
                {
                    bmp = new Bitmap("../../Resources/wM1.jpg");
                    pictureBox1.Image = bmp;

                    w0 = 3.1415 * n0 / 30;
                    wn = w0 * (1 - sn);
                    Mn = Pn / wn;
                    Mk = Mn * lamda;
                    sk = sn * (lamda + Math.Sqrt(lamda * lamda - 1));

                    x0 = 310; y0 = 477; xm = 553; ym = 185;
                    for(double w=(int)-w0; w<2*w0; w+=0.001)
                    {
                        double M = 2 * Mk / ((1 - w / w0) / sk + sk / (1 - w / w0));
                        int x = (int)(x0 + M * (xm - x0) / Mk);
                        int y = (int)(y0 - w * (y0 - ym) / w0);
                        if(y>567 || y<25)continue;
                        bmp.SetPixel(x, y, gr);
                    }

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        int j = 10;
                        for (int i = 0; i < 13; i++)
                        {
                            g.DrawString(((int)(w0 * (45 * j) / (y0 - ym))).ToString(),
                                new Font("Times New Roman", 10), new SolidBrush(Color.Black), 10, 20 + 45 * i);
                            j--;
                        }

                        j = -5;
                        double k = Mk / (xm - x0);
                        for (int i = 0; i < 11; i++)
                        {
                            g.DrawString(Math.Round((k * 54 * j), 1).ToString(),
                                new Font("Times New Roman", 10), new SolidBrush(Color.Black), 28 + 54 * i, 574);
                            j++;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка вычислений!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag1)
            {
                this.Text = "w = " + (int)(w0 * (y0 - e.Y) / (y0 - ym)) +
                    "; M = " + Math.Round((Mk * (e.X - x0) / (xm - x0)), 1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа предназначена для вычисления Механической характеристики электродвигателя. " +
                "Расчет данной характеристики происходит по упрощенной формуле Клосса.\n\n " +
                "На графике можно выделить 5 характерных точек:\n" +
                "1. Момент пуска двигателя: w = 0, M = " + Math.Round((2 * Mk / (1 / sk + sk)),1) +
                "\n2. Точка критического момента: w = " + (int)(w0*(1-sk)) + ", M = "+ Math.Round(Mk,1) +
                "\n3. Точка номинальноно момента: w = " + (int)wn + "; M = " +  Math.Round(Mn, 1) +
                "\n4. Точка идеального холостого хода: w = " + (int)w0 + "; M = 0" +
                "\n5. Точка критического момента в генераторном режиме: w = " + (int)(w0 * (1 + sk)) + ", M = " + Math.Round(-Mk, 1),
                "Информация", MessageBoxButtons.OK);
        }
    }
}
