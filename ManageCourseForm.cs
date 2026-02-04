using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transparent_Form
{
    public partial class ManageCourseForm : Form
    {
        CourseClass course = new CourseClass(); //creation of an object from courseclass
        public ManageCourseForm()
        {
            InitializeComponent();
        }

        //display the courses when loading the form
        private void ManageCourseForm_Load(object sender, EventArgs e)
        {
            showData();

        }
        // Show data of the course 
        private void showData()
        {
            //to show course list on datagridview
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
        }

        //to clear textboxes

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Cname.Clear();
            textBox_Chour.Clear();
            textBox_description.Clear();
        }
        
        // update the course information
        private void button_Update_Click(object sender, EventArgs e)
        {
            //input validation
            if (textBox_Cname.Text == "" || textBox_Chour.Text == ""|| textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!int.TryParse(textBox_id.Text, out int id))
                {
                    MessageBox.Show("Enter a valid input or id", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                    string cName = textBox_Cname.Text;
                int chr;
                if (!int.TryParse(textBox_Chour.Text.Trim(), out chr))
                {
                    MessageBox.Show("Course hour must be a number!",
                                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // stop function here
                }
                string desc = textBox_description.Text;

                // actual course update

                if (course.updateCourse(id, cName, chr, desc))
                {
                    showData();
                    button_clear.PerformClick();
                    MessageBox.Show("course update successfuly", "Update Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Enter a valid course id", "Modify Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            showData();
        }

        // delete couse button
        private void button_delete_Click(object sender, EventArgs e)
        {
           
                // input validation
                if (textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Course Id", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // exception handling 
                try
                {
                    int id = Convert.ToInt32(textBox_id.Text);
                    if (course.deleteCourse(id))
                    {
                        showData();
                        button_clear.PerformClick();
                        MessageBox.Show("course Deleted", "Removed Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show("Enter a valid input or id", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // display course from the database
        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_course.CurrentRow.Cells[0].Value.ToString();
            textBox_Cname.Text = DataGridView_course.CurrentRow.Cells[1].Value.ToString();
            textBox_Chour.Text = DataGridView_course.CurrentRow.Cells[2].Value.ToString();
            textBox_description.Text = DataGridView_course.CurrentRow.Cells[3].Value.ToString();
        }

        // To search
        private void button_search_Click(object sender, EventArgs e)
        {
            //To Search course and show on datagridview
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course` WHERE CONCAT(`CourseName`)LIKE '%"+textBox_search.Text+"%'"));
            textBox_search.Clear();
        }
    }
}
