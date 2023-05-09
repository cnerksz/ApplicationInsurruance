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
    public partial class Form1 : Form
    {
        public static int tıklandı = 0;
        public Form1()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
        NpgsqlCommand komut;
        NpgsqlDataAdapter da;
        public static string currentuser;
        private void refresh()
        {
            baglanti.Open();
            da = new NpgsqlDataAdapter("select c_name,c_tc,c_phone,c_adress,c_sorumlupersonel,baslangictarihi,bitistarihi,amount_payment from table_customer join table_payment on table_customer.c_id=table_payment.c_id ", baglanti);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
           

            baglanti.Close();
            txtname.Text = "";
            txtphone.Text = "";
            txtsk.Text = "";
            txttc.Text = "";
           

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if(txttc.Text.Equals("") || txtname.Text.Equals("") ||txtphone.Text.Equals("") || txtsk.Text.Equals(""))
            {
                
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

            HomepageScreen form2 = new HomepageScreen();
            baglanti.Open();
           currentuser = form2.labelcalisanname.Text;
            

            string sorgu = "INSERT INTO table_customer(c_name,c_tc,c_phone,c_adress,c_sorumlupersonel)VALUES(@c_name,@c_tc,@c_phone,@c_adress,@c_sorumlupersonel)";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@c_name", txtname.Text);
            komut.Parameters.AddWithValue("@c_tc", txttc.Text);
            komut.Parameters.AddWithValue("@c_phone", txtphone.Text);
            komut.Parameters.AddWithValue("@c_adress", txtsk.Text);
            komut.Parameters.AddWithValue("@c_sorumlupersonel", Convert.ToInt32(LoginScreen.currentuserid));
            komut.ExecuteNonQuery();
            baglanti.Close();
            Form3 frm = new Form3();
            frm.label1.Text = txttc.Text;
            frm.btnupdate.Visible = false;

            frm.Show();
            }


        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (txttc.Text.Equals("") || txtname.Text.Equals("") || txtphone.Text.Equals("") || txtsk.Text.Equals(""))
            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
 

            baglanti.Open();

            string sorgu2 = "Select*from table_customer where c_tc=@c_tc";
            string sorgu = "DELETE FROM table_customer WHERE c_tc=@c_tc";

            komut = new NpgsqlCommand(sorgu2, baglanti);

            komut.Parameters.AddWithValue("@c_tc", txttc.Text);
            NpgsqlDataReader dr = komut.ExecuteReader();
            dr.Read();

            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@c_tc", txttc.Text);
            komut.Parameters.AddWithValue("@p1", dr["c_id"]);
            dr.Close();
            komut.ExecuteNonQuery();
            baglanti.Close();
            refresh();
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (txttc.Text.Equals("") || txtname.Text.Equals("") || txtphone.Text.Equals("") || txtsk.Text.Equals(""))
            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

           
            baglanti.Open();
            string sorgu = "UPDATE table_customer SET c_name=@c_name,c_tc=@c_tc,c_phone=@c_phone,c_adress=@c_address WHERE c_id=@c_id";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@c_id", Convert.ToInt32(txtid.Text));
            komut.Parameters.AddWithValue("@c_name", txtname.Text);
            komut.Parameters.AddWithValue("@c_tc", txttc.Text);
            komut.Parameters.AddWithValue("@c_phone", txtphone.Text);
            komut.Parameters.AddWithValue("@c_address", txtsk.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            Form3 form = new Form3();
            form.label1.Text = txttc.Text;
            form.Show();
            form.btnadd.Visible = false;
            tıklandı = 2;
          
        }
        }

        private void btnfind_Click(object sender, EventArgs e)
        {
            if (txttc.Text.Equals(""))
            {
                MessageBox.Show("Lütfen Tc  Giriniz! ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

           
            baglanti.Open();
            da = new NpgsqlDataAdapter("select c_name,c_tc,c_phone,c_adress,c_sorumlupersonel,baslangictarihi,bitistarihi,amount_payment from table_customer join table_payment on table_customer.c_id=table_payment.c_id  WHERE c_tc ='" + txttc.Text+"' ", baglanti);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
          
                baglanti.Close();
        }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HomepageScreen form2 = new HomepageScreen();
            if (LoginScreen.currentuseryetki.Equals("admin"))
            {
                form2.buttoncalisangoster.Visible = true;
                form2.labelcalisanid.Text = LoginScreen.currentuserid;
                form2.labelcalisanname.Text=LoginScreen.currentusername2;
            }
            form2.Show();
            this.Hide();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
         
            baglanti.Open();
            string sorgu = "Select * from table_customer where c_tc=@c_tc";
            komut = new NpgsqlCommand(sorgu, baglanti);


            txtname.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txttc.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            komut.Parameters.AddWithValue("@c_tc", txttc.Text);
            txtphone.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            txtsk.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            NpgsqlDataReader dr = komut.ExecuteReader();
            dr.Read();
            txtid.Text = (dr["c_id"].ToString());
            dr.Close();
            komut.ExecuteNonQuery();
            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
