using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appSigorta
{
    public partial class HomepageScreen : Form
    {
        public HomepageScreen()
        {
            InitializeComponent();
        }
        NpgsqlDataAdapter da;
        NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
        public static string texttc; 
        private void button2_Click(object sender, EventArgs e)
        {
          

         
            Form1 frm = new Form1();
            NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
            baglanti.Open();
            da = new NpgsqlDataAdapter("select c_name,c_tc,c_phone,c_adress,c_sorumlupersonel,baslangictarihi,bitistarihi,amount_payment from table_customer join table_payment on table_customer.c_id=table_payment.c_id", baglanti);
            DataTable table = new DataTable();
            da.Fill(table);
            frm.dataGridView1.DataSource = table;
            frm.dataGridView1.Columns[0].HeaderText = "İsim";
            frm.dataGridView1.Columns[1].HeaderText = "TC";
            frm.dataGridView1.Columns[2].HeaderText = "Telefon";
            frm.dataGridView1.Columns[3].HeaderText = "Adres";
            frm.dataGridView1.Columns[4].HeaderText = "Sorumlu Personel";
            frm.dataGridView1.Columns[5].HeaderText = "Başlangıç Tarihi";
            frm.dataGridView1.Columns[6].HeaderText = "Bitiş Tarihi";
            frm.dataGridView1.Columns[7].HeaderText = "Sigorta Tutarı";


            this.Hide();
            frm.Show();
            baglanti.Close();
            
        }

        private void buttoncalisangoster_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
            calisanlargoster frmcalisan = new calisanlargoster();
            baglanti.Open();
            da = new NpgsqlDataAdapter("SELECT*FROM table_personel", baglanti);
            DataTable table = new DataTable();
            da.Fill(table);
            frmcalisan.dataGridView2.DataSource = table;
            frmcalisan.dataGridView2.Columns[0].HeaderText = "Çalışan ID";
            frmcalisan.dataGridView2.Columns[1].HeaderText = "İsim";
            frmcalisan.dataGridView2.Columns[2].HeaderText = "TC";
            frmcalisan.dataGridView2.Columns[3].HeaderText = "Telefon";
            frmcalisan.dataGridView2.Columns[4].HeaderText = "Email";
            frmcalisan.dataGridView2.Columns[5].HeaderText = "Doğum Tarihi";
            frmcalisan.dataGridView2.Columns[6].HeaderText = "Şifre";
            frmcalisan.dataGridView2.Columns[7].HeaderText = "Çalışan Yetkisi";

            frmcalisan.Show();
            this.Hide();

            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxtc.Text == "")
            {
                
                MessageBox.Show("Lütfen Tc kimlik numarası giriniz", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
              
                baglanti.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select*from table_customer where c_tc=@p1", baglanti);
                cmd.Parameters.AddWithValue("@p1", textBoxtc.Text);
                NpgsqlDataReader dr = cmd.ExecuteReader();


                Form1 frm = new Form1();

                if (!dr.Read())
                {
                    frm.Show();
                    baglanti.Close();
                    MessageBox.Show("Müşteri bulunamadı!", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dr.Close();
                    da = new NpgsqlDataAdapter("SELECT*FROM table_customer WHERE c_tc like '%" + textBoxtc.Text + "%' ", baglanti);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    frm.dataGridView1.DataSource = table;
                    this.Hide();
                    frm.Show();
                    baglanti.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxtc.Text == "")
            {
                MessageBox.Show("Lütfen Tc kimlik numarası giriniz", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                texttc = textBoxtc.Text;
                baglanti.Open();
                Form5 form = new Form5();
                NpgsqlCommand komut = new NpgsqlCommand("select*from table_customer join table_payment on table_customer.c_id=table_payment.c_id where c_tc=@c_tc", baglanti);
                komut.Parameters.AddWithValue("@c_tc", textBoxtc.Text);
                NpgsqlDataReader dr = komut.ExecuteReader();

                if (!dr.Read())
                {

                    
                    MessageBox.Show("Müşteri bulunamadı!", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    form.amount.Text = (Convert.ToInt32(dr["amount_payment"]) * 2.5).ToString();
                    form.Show();


                }
                dr.Close();
                baglanti.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult cevap;
            cevap = MessageBox.Show("Çıkmak istiyor musunuz?","Çıkıs",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if(cevap == DialogResult.Yes)
            {
                LoginScreen log = new LoginScreen();
                log.Show();
                this.Hide();
            }
          
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void HomepageScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
