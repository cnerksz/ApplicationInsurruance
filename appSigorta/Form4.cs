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
    public partial class calisanlargoster : Form
    {
        public calisanlargoster()
        {
            InitializeComponent();
        }

        NpgsqlConnection con = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
        NpgsqlCommand cmd;
        NpgsqlDataAdapter dataAdapter;
        private void calisangetir()
        {
            string query = "SELECT * FROM table_personel";
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            HomepageScreen form2 = new HomepageScreen();
            if (LoginScreen.currentuseryetki.Equals("admin"))
            {
                form2.buttoncalisangoster.Visible = true;
                form2.labelcalisanid.Text = LoginScreen.currentuserid;
                form2.labelcalisanname.Text = LoginScreen.currentusername2;

            }
            form2.Show();
            this.Hide();
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxname.Text.Equals("") || textBoxemail.Text.Equals("") || textBoxpassword.Text.Equals("") || textBoxphone.Text.Equals("") || textBoxtc.Equals(""))

            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                con.Open();

                string sorgu = "INSERT INTO table_personel(p_name,p_tc,p_phone,p_email,p_birthday,p_password)VALUES(@p_name,@p_tc,@p_phone,@p_email,@p_birthday,@p_password)";


                cmd = new NpgsqlCommand(sorgu, con);
                cmd.Parameters.AddWithValue("@p_name", textBoxname.Text);
                cmd.Parameters.AddWithValue("@p_tc", textBoxtc.Text);
                cmd.Parameters.AddWithValue("@p_phone", textBoxphone.Text);
                cmd.Parameters.AddWithValue("@p_email", textBoxemail.Text);
                cmd.Parameters.AddWithValue("@p_birthday", Convert.ToDateTime(dateTimePicker1.Text));
                cmd.Parameters.AddWithValue("@p_password", textBoxpassword.Text);

                cmd.ExecuteNonQuery();

                calisangetir();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxtc.Equals(""))

            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
            string sorgu = "DELETE FROM table_personel WHERE p_id=@p_id";
            cmd = new NpgsqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@p_id", Convert.ToInt32(textBoxid.Text));
            cmd.ExecuteNonQuery();
            calisangetir();
            con.Close();
        }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxname.Text.Equals("") || textBoxemail.Text.Equals("") || textBoxpassword.Text.Equals("") || textBoxphone.Text.Equals("") || textBoxtc.Equals(""))

            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                con.Open();
                string sorgu = "UPDATE table_personel SET p_name=@p_name, p_tc=@p_tc,p_phone=@p_phone, p_email=@p_email,p_birthday=@p_birthday,p_password=@p_password WHERE p_id=@p_id";
                cmd = new NpgsqlCommand(sorgu, con);
                cmd.Parameters.AddWithValue("@p_id", Convert.ToInt32(textBoxid.Text));
                cmd.Parameters.AddWithValue("@p_name", textBoxname.Text);
                cmd.Parameters.AddWithValue("@p_tc", textBoxtc.Text);
                cmd.Parameters.AddWithValue("@p_phone", textBoxphone.Text);
                cmd.Parameters.AddWithValue("@p_email", textBoxemail.Text);
                cmd.Parameters.AddWithValue("@p_birthday", Convert.ToDateTime(dateTimePicker1.Text));
                cmd.Parameters.AddWithValue("@p_password", textBoxpassword.Text);

                cmd.ExecuteNonQuery();
                con.Close();
                calisangetir();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBoxtc.Equals(""))

            {
                MessageBox.Show("Lütfen eksik bilgileri doldurunuz ", " Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                dataAdapter = new NpgsqlDataAdapter("SELECT*FROM table_personel WHERE p_tc='"+textBoxtc.Text+"'", con);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                dataGridView2.DataSource = table;
                dataGridView2.Columns[0].HeaderText = "Çalışan ID";
                dataGridView2.Columns[1].HeaderText = "İsim";
                dataGridView2.Columns[2].HeaderText = "TC";
                dataGridView2.Columns[3].HeaderText = "Telefon";
                dataGridView2.Columns[4].HeaderText = "Email";
                dataGridView2.Columns[5].HeaderText = "Doğum Tarihi";
                dataGridView2.Columns[6].HeaderText = "Şifre";
                dataGridView2.Columns[7].HeaderText = "Çalışan Yetkisi";
                
                
                
                con.Close();
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxid.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            textBoxname.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textBoxtc.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            textBoxphone.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            textBoxemail.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            dateTimePicker1.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            textBoxpassword.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            calisangetir();
        }
    }
}
