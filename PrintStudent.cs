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
using DGVPrinterHelper;

namespace Transparent_Form
{
    public partial class PrintStudent : Form
    {
        // creation of objects
        StudentClass student = new StudentClass();
        DGVPrinter printer = new DGVPrinter();
        CourseClass course = new CourseClass();

        public PrintStudent()
        {
            InitializeComponent();
        }

        // display student info before printing
        private void PrintStudent_Load(object sender, EventArgs e)
        {
            showData(new MySqlCommand("SELECT * FROM `student`"));
            comboBox_class.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
            comboBox_class.DisplayMember = "CourseName";
            comboBox_class.ValueMember = "CourseId";
        }
        
        // create a function to show the student list in datagridview
        public void showData(MySqlCommand command)
        {
            DataGridView_student.ReadOnly = true;
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridView_student.DataSource = student.getList(command);
            // column 7 is the image column index
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[8];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        // function for selecting different student gender and show the search results
        private void button_search_Click(object sender, EventArgs e)
        {
            //check the radio button
            string selectQuery;
            if (radioButton_all.Checked)
            {
                selectQuery = "SELECT* FROM `student`";
            }
            else if (radioButton_male.Checked)
            {
                selectQuery = "SELECT * FROM `student` WHERE `Gender`='Male'";
            }
            else
            {
                selectQuery = "SELECT * FROM `student` WHERE `Gender`='Female'";
            }
            showData(new MySqlCommand(selectQuery));
        }

        // funtion to print 
        private void button_print_Click(object sender, EventArgs e)
        {
            //We need DGVprinter helper for print pdf file
            printer.Title = "Students list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Karabuk university";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_student);
        }

        // update the page
        private void radioButton_female_CheckedChanged(object sender, EventArgs e)
        {
            button_search.PerformClick();
        }

        private void radioButton_male_CheckedChanged(object sender, EventArgs e)
        {
            button_search.PerformClick();
        }

        private void radioButton_all_CheckedChanged(object sender, EventArgs e)
        {
            button_search.PerformClick();
        }

        private void comboBox_class_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
