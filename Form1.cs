using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artlite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // GİRİŞ EKRAN ÜYELİK BİLGİLERİ
        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "admin")
            {
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Lütfen Kullanıcı Adınızı Doğru Giriniz", "Uyarı", MessageBoxButtons.OK);
            }
        }
        //

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.ForeColor = Color.Blue;
            //this.BackColor = Color.Yellow;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
