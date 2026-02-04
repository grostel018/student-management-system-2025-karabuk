using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Transparent_Form
{
    public partial class LoginForm : Form
    {
        //create student object

        StudentClass student = new StudentClass();

        public LoginForm()
        {
            InitializeComponent();
        }

        // function to close all the program
        private void label6_Click(object sender, EventArgs e)
        {
            Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();

            foreach (Form f in openForms)
            {
                if (f != this) // ❗ Prevent recursive self-close
                    f.Close();
            }

            this.Close(); // Close AFTER loop
        }

        //function to change the exit button icon's color when the mouse is over it
        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Red;
        }
        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.White;
        }


        //function to handle the login button 

        private void button_login_Click(object sender, EventArgs e)
        {
            //text and input validation
            if (string.IsNullOrWhiteSpace(textBox_usrname.Text) || string.IsNullOrWhiteSpace(textBox_password.Text))
            {
                MessageBox.Show("Need login data", "Wrong Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //mapping the variable with the database
            string uname = textBox_usrname.Text.Trim();
            string pass = textBox_password.Text; // ideally hashed
            string sql = "SELECT userId, username, role FROM `user` WHERE username=@uname AND password=@pass";

            //connecting to the database
            using (MySqlConnection conn = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=studentdb"))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(
                        "Failed to connect to MySQL.\n\n" +
                        "Reason: " + ex.Message,
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return; // stop execution of the initializer
                }

                //actually setting the variables in the database and checking if the username and password exits in the database
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uname", uname);
                    cmd.Parameters.AddWithValue("@pass", pass);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32("userId");
                            string role = reader.GetString("role");

                            // Pass username, role, and ID to MainForm
                            MainForm main = new MainForm(userId, uname, role);
                            this.Hide();
                            main.Show();
                        }
                        else
                        {
                            MessageBox.Show("Your username and password do not exist", "Wrong Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        //function to handle the label link and open the sign up form
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form signup = new signup();
            signup.Show();
            this.Hide();
        }

        //functions to switch textboxes when the user types "enter"
        private void textBox_usrname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox_password.Focus();
                e.SuppressKeyPress = true;
            }
        }
        private void textBox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button_login.PerformClick();
            }
        }
    }
}
