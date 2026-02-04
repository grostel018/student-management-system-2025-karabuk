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
    public partial class ManageScoreForm : Form
    {
        // creation of objects
        CourseClass course = new CourseClass();
        ScoreClass score = new ScoreClass();
        public ManageScoreForm()
        {
            InitializeComponent();
        }

        //on load display the data from the database
        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            //populate the combobox with courses name
            comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
            comboBox_course.DisplayMember = "CourseName";
            comboBox_course.ValueMember = "CourseId";
            // to show score data on datagridview
            showScore();
        }

        //funton for the button to show score
        public void showScore()
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId,student.StdFirstName,student.StdLastName,score.CourseName,score.Score,score.Description FROM student INNER JOIN score ON score.StudentId=student.StdId"));
        }

        // modify the score
        private void button_Update_Click(object sender, EventArgs e)
        {
            //input validation

            if (textBox_stdId.Text == "" || textBox_score.Text == "")
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    int stdId = Convert.ToInt32(textBox_stdId.Text);
                    string cName = comboBox_course.Text;
                    double scor = Convert.ToInt32(textBox_score.Text);
                    string desc = textBox_description.Text;

                    //update the score

                    if (score.updateScore(stdId, cName, scor, desc))
                    {
                        showScore();
                        button_clear.PerformClick();
                        MessageBox.Show("Score Edited Complete", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex) 
                {
                        MessageBox.Show("Enter a valid id", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            showScore();

        }

        //function for delete button
        private void button_delete_Click(object sender, EventArgs e)
        {
            //input validation
            try
            {
                if (textBox_stdId.Text == "")
                {
                    MessageBox.Show("Field Error- we need student id", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //dialog box
                    int id = Convert.ToInt32(textBox_stdId.Text);
                    if (MessageBox.Show("Are you sure you want to remove this score", "Delete Score", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (score.deleteScore(id))
                        {
                            showScore();
                            MessageBox.Show("Score Removed", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button_clear.PerformClick();
                        }
                    }

                }
                showScore();
            }
            catch (Exception ex )
            {
                MessageBox.Show("Enter a valid id", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            
        }

        //clear the fields
        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_stdId.Clear();
            textBox_score.Clear();
            textBox_description.Clear();
            textBox_search.Clear();
        }

        //when you click on a button in datagridview, shows values in the textboxes
        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_stdId.Text = DataGridView_score.CurrentRow.Cells[0].Value.ToString();
            comboBox_course.Text = DataGridView_score.CurrentRow.Cells[3].Value.ToString();
            textBox_score.Text = DataGridView_score.CurrentRow.Cells[4].Value.ToString();
            textBox_description.Text = DataGridView_score.CurrentRow.Cells[5].Value.ToString();
        }

        //search function
        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId, student.StdFirstName, student.StdLastName, score.CourseName, score.Score, score.Description FROM student INNER JOIN score ON score.StudentId = student.StdId WHERE CONCAT(student.StdFirstName, student.StdLastName, score.CourseName)LIKE '%"+textBox_search.Text+"%'"));
            
        }

        private void DataGridView_score_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
