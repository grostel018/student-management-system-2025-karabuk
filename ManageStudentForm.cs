using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Transparent_Form
{
    public partial class ManageStudentForm : Form
    {
        //usefull objects and variables creation

        StudentClass student = new StudentClass();
        private int LoggedUserId;
        private string LoggedUserRole;

        //constructor and initialising
        public ManageStudentForm(int userId, string role)
        {
            InitializeComponent();
            LoggedUserId = userId;
            LoggedUserRole = role;
        }

        //show table on load 
        private void ManageStudentForm_Load(object sender, EventArgs e)
        {
            showTable();
        }

        // To show student list in DatagridView
        public void showTable()
        {
            DataTable table = student.getStudentlist(LoggedUserRole == "admin" ? (int?)null : LoggedUserId);
            DataGridView_student.DataSource = table;

            //to resize the images in the profile pics
            if (DataGridView_student.Columns.Count > 8 && DataGridView_student.Columns[8] is DataGridViewImageColumn imageColumn)
            {
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }

        // Display student data from student to textboxes
        private void DataGridView_student_Click(object sender, EventArgs e)
        {
            if (DataGridView_student.CurrentRow == null) return;

            //filling the textboxes with the values from the student selected in datagridview
            textBox_id.Text = DataGridView_student.CurrentRow.Cells["StdId"].Value.ToString();
            textBox_Fname.Text = DataGridView_student.CurrentRow.Cells["StdFirstName"].Value.ToString();
            textBox_Lname.Text = DataGridView_student.CurrentRow.Cells["StdLastName"].Value.ToString();
            dateTimePicker1.Value = (DateTime)DataGridView_student.CurrentRow.Cells["Birthdate"].Value;
            radioButton_male.Checked = DataGridView_student.CurrentRow.Cells["Gender"].Value.ToString() == "Male";
            textBox_phone.Text = DataGridView_student.CurrentRow.Cells["Phone"].Value.ToString();
            textBox_address.Text = DataGridView_student.CurrentRow.Cells["Address"].Value.ToString();

            //filling the image for the userpicture
            byte[] img = (byte[])DataGridView_student.CurrentRow.Cells["Photo"].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox_student.Image = Image.FromStream(ms);
        }

        //button to empty the entered fields
        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pictureBox_student.Image = null;
        }

        //button to upload the entered values to the database
        private void button_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog
            {
                Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif"
            };

            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox_student.Image = Image.FromFile(opf.FileName);
            }
        }

        //button to search a student
        private void button_search_Click(object sender, EventArgs e)
        {
            string searchText = textBox_search.Text;
            DataTable table = student.searchStudent(searchText, LoggedUserRole == "admin" ? (int?)null : LoggedUserId);
            DataGridView_student.DataSource = table;

            if (DataGridView_student.Columns.Count > 8 && DataGridView_student.Columns[8] is DataGridViewImageColumn imageColumn)
            {
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }

        // Verify required fields
        bool verify()
        {
            return !(string.IsNullOrWhiteSpace(textBox_Fname.Text) ||
                     string.IsNullOrWhiteSpace(textBox_Lname.Text) ||
                     string.IsNullOrWhiteSpace(textBox_phone.Text) ||
                     string.IsNullOrWhiteSpace(textBox_address.Text) ||
                     pictureBox_student.Image == null);
        }

        //update button
        private void button_update_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox_id.Text, out int id))
            { 
            MessageBox.Show("Enter a valid id", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
            }

            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;

            //input verification and exception handling

            int age = DateTime.Now.Year - bdate.Year;
            if (age < 10 || age > 100)
            {
                MessageBox.Show("The student age must be between 10 and 100", "Invalid Birthdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!verify())
            {
                MessageBox.Show("Empty Field", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //exception handling
            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox_student.Image.Save(ms, pictureBox_student.Image.RawFormat);
                byte[] img = ms.ToArray();

                if (student.updateStudent(id,
                                          LoggedUserRole == "admin" ? (int?)null : LoggedUserId,
                                          fname, lname, bdate, gender, phone, address, img))
                {
                    showTable();
                    MessageBox.Show("Student data updated", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            showTable();
        }

        //to refresh the table everytime the user change something in the textbox
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            // Example: Refresh the student table based on search text
            showTable();
        }


        //to delete a student
        private void button_delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox_id.Text, out int id))
            {
                MessageBox.Show("Enter a valid id", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                if (MessageBox.Show("Are you sure you want to remove this student?", "Remove Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (student.deleteStudent(id, LoggedUserRole == "admin" ? (int?)null : LoggedUserId))
                {
                    showTable();
                    MessageBox.Show("Student Removed", "Remove student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
            }
            showTable();
        }

        private void pictureBox_student_Click(object sender, EventArgs e)
        {

        }
    }
}
