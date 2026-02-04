using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Transparent_Form
{
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        // object to connect the database with path
        MySqlConnection conn = new MySqlConnection("datasource = localhost; port=3306;username=root;password=;database=studentdb");


        // function for signUp button 
        private void button1_Click(object sender, EventArgs e)
        {
            
            string username = textBox_usrname.Text.Trim();
            string password = textBox_password.Text;

            // ----------------------------
            // 1. BASIC VALIDATION
            // ----------------------------

            if (username == "" || password == "" || textBox1.Text == "")
            {
                MessageBox.Show("All fields are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Username must contain letters
            if (username.All(char.IsDigit))
            {
                MessageBox.Show("Username cannot be only numbers!", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Username cannot contain spaces or special characters
            if (!username.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("Username can only contain letters and digits!", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Password length
            if (password.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters!", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm Password
            if (password != textBox1.Text)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ----------------------------
            // 2. DATABASE LOGIC (SAFE)
            // ----------------------------
            //connecting
            using (MySqlConnection conn = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=studentdb"))
            {
                // exception handling

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


                // CHECK IF USERNAME EXISTS
                string checkSql = "SELECT COUNT(*) FROM user WHERE username = @username";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@username", username);

                long exists = (long)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    MessageBox.Show("This username is already taken!", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // INSERT USER (SAFE)
                string insertSql = "INSERT INTO user (username, password) VALUES (@username, @password)";
                MySqlCommand insertCmd = new MySqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@password", password);

                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



            this.Close();

        }



        //logic and functions to automaticly switch textboxes when the user types "enter"
        private void textBox_usrname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            textBox_password.Focus();
            e.SuppressKeyPress = true;
            }
        }


    
        //to switcth textboxes when the user types enter
        private void textBox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            textBox1.Focus();
            e.SuppressKeyPress = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            button1.PerformClick();
            e.SuppressKeyPress = true;
            }
        }

        //function to close the signup form and open the signin form
        private void signup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form LoginForm = new LoginForm();
            LoginForm.Show();
            this.Hide();
        }

        private void signup_Load(object sender, EventArgs e)
        {

        }
    }
}
