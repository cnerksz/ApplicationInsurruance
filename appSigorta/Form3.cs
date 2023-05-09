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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
        NpgsqlCommand komut;


        private void btnadd_Click(object sender, EventArgs e)
        {





                baglanti.Open();
                Form1 form = new Form1();


                string sorgu2 = "select*from table_customer where c_tc=@p1";
                NpgsqlCommand komut1 = new NpgsqlCommand(sorgu2, baglanti);
                komut1.Parameters.AddWithValue("@p1", label1.Text);
                NpgsqlDataReader dr = komut1.ExecuteReader();

                dr.Read();
                string sorgu = "INSERT INTO table_payment(c_id,month,type,amount_payment,baslangictarihi,bitistarihi)VALUES(@c_id,@month,@type,@amount,@bas,@bit)";
                komut = new NpgsqlCommand(sorgu, baglanti);

                komut.Parameters.AddWithValue("@c_id", Convert.ToInt32(dr["c_id"]));
                dr.Close();
                komut.Parameters.AddWithValue("@type", type.Text);
                komut.Parameters.AddWithValue("@month", month.Text);
                komut.Parameters.AddWithValue("@amount", Convert.ToInt32(amount.Text));
                DateTime dtba = DateTime.Now;
                DateTime dtbi = DateTime.Now.AddMonths(+Convert.ToInt32(month.Text));
                komut.Parameters.AddWithValue("@bas", dtba);
                komut.Parameters.AddWithValue("@bit", dtbi);

                komut.ExecuteNonQuery();

                baglanti.Close();
                MessageBox.Show("Müşteri Bilgileri Eklendi!", " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Hide();
            
    }

        public void hesapla(object sender, EventArgs e)
        {
            int hesap;
            if (month.Text != "")
            {
                if (type.Text == "Hayat")
                {
                    hesap = (Convert.ToInt32(month.Text)) * 120;
                }
                else if (type.Text == "Ev")
                {
                    hesap = (Convert.ToInt32(month.Text)) * 35;
                }
                else if (type.Text == "Araba")
                {
                    hesap = (Convert.ToInt32(month.Text)) * 150;
                }
                else
                {
                    hesap = 0;
                }
                Form1 form1 = new Form1();

                if (Form1.tıklandı == 0)
                    amount.Text = hesap.ToString();
                else
                    amount.Text = (hesap - hesap * 0.20).ToString();
            }

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            
                baglanti.Open();

                string sorgu2 = "select*from table_customer where c_tc=@p1";
                NpgsqlCommand komut1 = new NpgsqlCommand(sorgu2, baglanti);
                komut1.Parameters.AddWithValue("@p1", label1.Text);
                NpgsqlDataReader dr = komut1.ExecuteReader();
                dr.Read();
                string sorgu = "UPDATE table_payment SET month=@month,type=@type,amount_payment=@amount WHERE c_id=@c_id";
                komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@c_id", Convert.ToInt32(dr["c_id"]));
                dr.Close();
                komut.Parameters.AddWithValue("@month", month.Text);
                komut.Parameters.AddWithValue("@type", type.Text);
                komut.Parameters.AddWithValue("@amount", Convert.ToInt32(amount.Text));
                komut.ExecuteNonQuery();
                baglanti.Close();
            MessageBox.Show("Müşteri Bilgileri Güncellendi!", " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
            }
        
    }
}
