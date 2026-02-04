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
    public partial class CourseForm : Form
        
    {
        // creation of object 
        CourseClass course = new CourseClass();
        public CourseForm()
        {
            InitializeComponent();
        }

        // 
        private void button_add_Click(object sender, EventArgs e)
        {
            //input validation
            if (textBox_Cname.Text == "" || textBox_Chour.Text == "")
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string cName = textBox_Cname.Text;

                int chr;

                if (!int.TryParse(textBox_Chour.Text.Trim(), out chr))
                {
                    MessageBox.Show("Course hour must be a number!",
                                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // stop function here
                }

                string desc = textBox_description.Text;

                // add new course

                if (course.insetCourse(cName, chr, desc))
                {
                    showData();
                    button_clear.PerformClick();
                    MessageBox.Show("New course inserted", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Course not insert", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Cname.Clear();
            textBox_Chour.Clear();
            textBox_description.Clear();
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            showData();
        }
       

        private void showData()
        {   //to show course list on datagridview
            // Use proper SQL
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
        }
    }
}
