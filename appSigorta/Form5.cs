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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


            NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");

            baglanti.Open();
            string sorgu = "select c_name from table_customer join table_payment on table_customer.c_id=table_payment.c_id join table_insurance on table_payment.payment_id=table_insurance.paymentfk_id where c_tc=@c_tc";
            NpgsqlCommand komut1 = new NpgsqlCommand(sorgu, baglanti);
            komut1.Parameters.AddWithValue("@c_tc", HomepageScreen.texttc);
            NpgsqlDataReader dr1 = komut1.ExecuteReader();

            if (dr1.HasRows)
            {
                MessageBox.Show("Bu Kullanıcıya Ödeme Yapılmış ", " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dr1.Close();
                komut1.ExecuteNonQuery();
            }
            else
            {
                dr1.Close();
                komut1.ExecuteNonQuery();
                HomepageScreen form = new HomepageScreen();
                string sorgu2 = "select*from table_customer join table_payment on table_customer.c_id=table_payment.c_id where c_tc=@c_tc";
                NpgsqlCommand komut3 = new NpgsqlCommand(sorgu2, baglanti);
                komut3.Parameters.AddWithValue("@c_tc", HomepageScreen.texttc);

                NpgsqlDataReader dr = komut3.ExecuteReader();
                dr.Read();

                sorgu = "insert into table_insurance (insurance_reason,insurance_price,paymentfk_id)values(@reason,@price,@p_id)";
                NpgsqlCommand komut2 = new NpgsqlCommand(sorgu, baglanti);
                komut2.Parameters.AddWithValue("@reason", comboBox1.Text);
                komut2.Parameters.AddWithValue("@p_id", Convert.ToInt32(dr["payment_id"].ToString()));
                komut2.Parameters.AddWithValue("@price", (Convert.ToInt32(dr["amount_payment"]) * 3));
                dr.Close();
                komut3.ExecuteNonQuery();
                komut2.ExecuteNonQuery();


                MessageBox.Show("Bu Kullanıcıya Ödeme Yapıldı ", " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
            baglanti.Close();


        }

        private void btnkontrol_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");

            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select c_name,c_tc,insurance_reason,insurance_price,baslangictarihi,bitistarihi from table_customer join table_payment on table_customer.c_id=table_payment.c_id join table_insurance on table_payment.payment_id=table_insurance.paymentfk_id", baglanti);
            
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].HeaderText = "İsim";
            dataGridView1.Columns[1].HeaderText = "TC";
            dataGridView1.Columns[2].HeaderText = "Sebep";
            dataGridView1.Columns[3].HeaderText = "Ücret";
            dataGridView1.Columns[4].HeaderText = "Başlangıç Tarihi";
            dataGridView1.Columns[5].HeaderText = "Bitiş Tarihi";
            baglanti.Close();
        }
    }
         

}
        
    
  
