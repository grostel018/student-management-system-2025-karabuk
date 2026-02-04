using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Transparent_Form
{
    class StudentClass
    {
        //object to help connect to the database
        DBconnect connect = new DBconnect();

        //function to Insert new student
        public bool insertStudent(int userId, string fname, string lname, DateTime bdate, string gender, string phone, string address, byte[] img)
        {
            //commands sql
            MySqlCommand command = new MySqlCommand(
                "INSERT INTO `student`(`userId`, `StdFirstName`, `StdLastName`, `Birthdate`, `Gender`, `Phone`, `Address`, `Photo`) " +
                "VALUES(@uid, @fn, @ln, @bd, @gd, @ph, @adr, @img)", connect.getconnection);

            command.Parameters.Add("@uid", MySqlDbType.Int32).Value = userId;
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

            // connecting, executing smartly
            connect.openConnect();
            bool result = command.ExecuteNonQuery() == 1;
            connect.closeConnect();
            return result;
        }

        // Get student list with optional user filters
        public DataTable getStudentlist(int? userId = null)
        {
            //setting the commands
            MySqlCommand command;
            if (userId.HasValue)
            {
                command = new MySqlCommand("SELECT * FROM `student` WHERE `userId`=@uid", connect.getconnection);
                command.Parameters.Add("@uid", MySqlDbType.Int32).Value = userId.Value;
            }
            else
            {
                command = new MySqlCommand("SELECT * FROM `student`", connect.getconnection);
            }

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();

            //execute and handle exceptions in case the commands don't work
            try
            {
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Failed to retrieve data from MySQL.\n\n" +
                    "Reason: " + ex.Message,
                    "MySQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unexpected error while filling the table:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            return table;
        }

        // function to Search students with optional user filter
        public DataTable searchStudent(string searchdata, int? userId = null)
        {
            //the SQL commands
            MySqlCommand command;
            if (userId.HasValue)
            {
                command = new MySqlCommand(
                    "SELECT * FROM `student` WHERE `userId`=@uid AND CONCAT(StdFirstName, StdLastName, Address) LIKE @s",
                    connect.getconnection);
                command.Parameters.Add("@uid", MySqlDbType.Int32).Value = userId.Value;
            }
            else
            {
                command = new MySqlCommand(
                    "SELECT * FROM `student` WHERE CONCAT(StdFirstName, StdLastName, Address) LIKE @s",
                    connect.getconnection);
            }

            command.Parameters.Add("@s", MySqlDbType.VarChar).Value = "%" + searchdata + "%";

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();

            //handling the exceptions
            try
            {
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Failed to retrieve data from MySQL.\n\n" +
                    "Reason: " + ex.Message,
                    "MySQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unexpected error while filling the table:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            return table;
        }

        // Update student with optional user filter
        public bool updateStudent(int id, int? userId, string fname, string lname, DateTime bdate, string gender, string phone, string address, byte[] img)
        {
            // setting the SQL commands
            string sql = "UPDATE `student` SET `StdFirstName`=@fn, `StdLastName`=@ln, `Birthdate`=@bd, `Gender`=@gd, `Phone`=@ph, `Address`=@adr, `Photo`=@img WHERE `StdId`=@id";
            if (userId.HasValue)
                sql += " AND `userId`=@uid";

            MySqlCommand command = new MySqlCommand(sql, connect.getconnection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            if (userId.HasValue)
                command.Parameters.Add("@uid", MySqlDbType.Int32).Value = userId.Value;

            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

            //executing
            connect.openConnect();
            bool result = command.ExecuteNonQuery() == 1;
            connect.closeConnect();
            return result;
        }

        // Delete student with optional user filter
        public bool deleteStudent(int id, int? userId = null)
        {   
            //the SQL commands
            string sql = "DELETE FROM `score` WHERE StudentId = @id; DELETE FROM `student` WHERE `StdId`=@id";
            MySqlCommand command = new MySqlCommand(sql, connect.getconnection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            if (userId.HasValue)
            {
                sql += " AND `userId`=@uid";
                command.CommandText = sql;
                command.Parameters.Add("@uid", MySqlDbType.Int32).Value = userId.Value;
            }
            //executing
            connect.openConnect();
            bool result = command.ExecuteNonQuery() == 1;

            connect.closeConnect();
            return result;
        }

        // Execute a count query
        public string exeCount(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connect.getconnection);
            connect.openConnect();
            string count = command.ExecuteScalar().ToString();
            connect.closeConnect();
            return count;
        }
        //get total students number
        public string totalStudent()
        {
            return exeCount("SELECT COUNT(*) FROM student");
        }
        //get total male students number
        public string maleStudent()
        {
            return exeCount("SELECT COUNT(*) FROM student WHERE `Gender`='Male'");
        }
        //get total female students number
        public string femaleStudent()
        {
            return exeCount("SELECT COUNT(*) FROM student WHERE `Gender`='Female'");
        }

        // Execute any command and get table
        public DataTable getList(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}
