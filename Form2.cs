using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Sql bağlantı kütüphanesi
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using QRCoder;
using ZXing;



namespace Artlite
{
    public partial class Form2 : Form
    {
        private TextBox[] textBoxes; //toplu textbox'a yazım için

        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;

        //Yazıcıdan otomatik yazı çıkarmak için
        //private PrintDocument pd = new PrintDocument();
        
        public Form2()
        {
            InitializeComponent();
            InitializeTextBoxArray();
           

            //BARKOD
            textBox2.KeyPress += textBox2_KeyPress;
            textBox3.KeyPress += textBox3_KeyPress;
            textBox4.KeyPress += textBox4_KeyPress;
            textBox5.KeyPress += textBox5_KeyPress;
            textBox6.KeyPress += textBox6_KeyPress;
            //textBox7.KeyPress += textBox7_KeyPress;


            //"Deneme" adındaki yazıcıyı belirt
            //pd.PrinterSettings.PrinterName = "ZDesigner ZT620-203dpi ZPL";

            //PrintPage olayına bir dinleyici ekleriz.
            //pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);


        }

        private void InitializeTextBoxArray()
        {
            // TextBox dizisini oluştururuz
            textBoxes = new TextBox[] { textBox2, textBox3, textBox4, textBox5 };
        }

        private void ConcatenateText()
        {
            // Tüm önceki TextBox'ların içeriğini birleştirerek 6. TextBox'a yaz
            textBox6.Text = string.Join("", textBoxes.Take(4).Select(tb => tb.Text));
        }


        //Metot oluştururuz. (Her seferinde çağırmamak için)
        void MusteriGetir()
        {
            // Data Source=192.168.0.220;Initial Catalog=artlite;User ID=alparge;Password=***********
            //NOT : Aşağıdaki SQL Connection'ın da Integrated Security'i kaldırırız, güvenliği atlamış oluruz. 

            baglanti = new SqlConnection("Data Source=192.168.0.220; Initial Catalog=artlite; User ID=alparge; Password=a!p@rge?");
            baglanti.Open();
            da = new SqlDataAdapter("Select SOMTOP, SOMBOT, BBFC, TOF, FINAL, DATE from TBLart", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MusteriGetir();
            //textBox1.ReadOnly = true;

            //textBox1.Text = "";
            textBox2.Focus();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            dataGridView1.ClearSelection();

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Datagrid'de veritabanında yer alan veri değerlerini gösteririz
            //textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        }

        // EKLEME KISMI
        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "INSERT INTO TBLart(SOMTOP, SOMBOT, BBFC, TOF, FINAL) VALUES (@SOMTOP, @SOMBOT, @BBFC, @TOF, @FINAL) ";

            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@SOMTOP", textBox2.Text);
            komut.Parameters.AddWithValue("@SOMBOT", textBox3.Text);
            komut.Parameters.AddWithValue("@BBFC", textBox4.Text);
            komut.Parameters.AddWithValue("@TOF", textBox5.Text);
            komut.Parameters.AddWithValue("@FINAL", textBox6.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MusteriGetir();


            dataGridView1.ClearSelection();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

        }

        //Custom DataGrid Yapımı (Data Grid üzerinden de birçok ayarlamalar yapılmıştır)
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Style.BackColor = Color.Black;
            cell.Style.ForeColor = Color.White;
        }

        // SİLME KISMI
        private void button4_Click(object sender, EventArgs e)
        {
            string sorgu = "DELETE FROM TBLart WHERE SOMTOP=@SOMTOP";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@SOMTOP", Convert.ToInt32(textBox2.Text));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MusteriGetir();

        }

        //GÜNCELLE KISMI (ESKİ KISIM-KODLAR DURUYOR)
        //
        //
        //
        // YAZDIRMA KISMI
        private void button3_Click(object sender, EventArgs e)
        {
            //printPreviewDialog1.ShowDialog();


            //string sorgu = "UPDATE TBLart SET ID=@ID , SOMTOP=@SOMTOP , SOMBOT=@SOMBOT , BBFC=@BBFC , TOF=@TOF , FINAL=@FINAL WHERE ID=@ID";
            //komut = new SqlCommand(sorgu, baglanti);

            //komut.Parameters.AddWithValue("@ID", Convert.ToInt32(textBox1.Text));
            //komut.Parameters.AddWithValue("@SOMTOP" , Convert.ToInt32(textBox2.Text));
            //komut.Parameters.AddWithValue("@SOMBOT", Convert.ToInt32(textBox3.Text));
            //komut.Parameters.AddWithValue("@BBFC", Convert.ToInt32(textBox4.Text));
            //komut.Parameters.AddWithValue("@TOF", Convert.ToInt32(textBox5.Text));
            //komut.Parameters.AddWithValue("@FINAL", Convert.ToInt32(textBox6.Text));
            //baglanti.Open();
            //komut.ExecuteNonQuery();
            //baglanti.Close();
            //MusteriGetir();



            //RawPrinterHelper.SendStringToPrinter("ZDesigner ZT620-203dpi ZPL", "1");
            //RawPrinterHelper.SendStringToPrinter("onur", "1");

            //printPreviewDialog1.ShowDialog();

            
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = "ZDesigner ZT620-203dpi ZPL";
            pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            //Yazıcıyı seçmek için bir dialog göster ()KALDIR
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            /*
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                //Yazdırmaya başlat
                pd.Print();
            }
            */
            string metin = textBox6.Text;
            Bitmap qrCode = GenerateQRCode(metin);
            pd.Print();

        }

        private Bitmap GenerateQRCode(string metin)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 300,
                Height = 87
            };

            Bitmap qrCodeImage = new Bitmap(barcodeWriter.Write(metin));

            return qrCodeImage;



            throw new NotImplementedException();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')  //r burada enter karakteridir.
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')  //r burada enter karakteridir.
            {
                e.Handled = true;
                textBox4.Focus();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')  //r burada enter karakteridir.
            {
                e.Handled = true;
                textBox5.Focus();
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')  //r burada enter karakteridir.
            {
                e.Handled = true;
                textBox6.Focus();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')  //r burada enter karakteridir.
            {
                e.Handled = true;
                //textBox6.Focus();
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            ConcatenateText();
        }


        //SERCH - ARAMA KISMI
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable tbl = new DataTable();
            SqlDataAdapter ara = new SqlDataAdapter("SELECT SOMTOP, SOMBOT, BBFC, TOF, FINAL, DATE FROM TBLart WHERE SOMTOP like '%" + textBox8.Text +"%'  " , baglanti);

            ara.Fill(tbl);
            baglanti.Close();
            dataGridView1.DataSource = tbl;


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Yazdırılacak metni al
            string metin = textBox6.Text;
            Bitmap qrCode = GenerateQRCode(metin);


            //Yazdırma İşlemi gerçekleştir //QR CODE
            //e.Graphics.DrawString(metin, new Font("Arial", 12), Brushes.Black, 10, 10);
            e.Graphics.DrawImage(qrCode, new Point(1, 1));

        }
    }
}
