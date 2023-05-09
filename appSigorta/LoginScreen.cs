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
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        NpgsqlConnection con = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=SigortaDatabase");
        NpgsqlCommand cmd;
       
        public static String currentuseryetki;
        public static string currentuserid;
        public static string currentusername2;

        private void button1_Click(object sender, EventArgs e)
        {

            con.Open();
            string query = "select * from public.table_personel Where p_tc='" + textBox1.Text + "' and p_password='" + textBox2.Text + "'";
            cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader dr = cmd.ExecuteReader();

            try
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Tc kimlik numarası veya şifre boş olamaz","Giriş Hatası",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

                else if (dr.Read())
                {

                    while (true)
                    {


                        if (dr["p_tc"].ToString().Equals(textBox1.Text) && dr["p_password"].ToString().Equals(textBox2.Text))
                        {
                            currentuseryetki = dr["p_authorization"].ToString();
                            currentuserid = dr["p_id"].ToString();
                            currentusername2 = dr["p_name"].ToString();

                            HomepageScreen homepage = new HomepageScreen();
                            homepage.labelcalisanname.Text = dr["p_name"].ToString();
                            homepage.labelcalisanid.Text = dr["p_id"].ToString();

                            if (dr["p_authorization"].ToString().Equals("postgres"))
                            {

                                homepage.Show();
                                this.Hide();


                                break;
                            }
                            else if (dr["p_authorization"].ToString().Equals("admin"))
                            {


                                homepage.buttoncalisangoster.Visible = true;
                                homepage.Show();
                                this.Hide();

                                break;

                            }


                        }


                    }
                }
                else
                {
                    MessageBox.Show("Tc kimlik numarası veya şifre Yanlış ", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

               

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Kodu : " + ex.Message);
            }
            con.Close();
            dr.Close();
        }
    }
}
