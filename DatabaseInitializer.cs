using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Transparent_Form
{
    // create and  initialize database table and connection to the database
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {   
            //create variables and connect to the mysql database
            string server = "localhost";
            string user = "root";
            string password = "";
            string dbName = "studentdb";

            // STEP 1 — Create database if not exists
            string connStrNoDb = $"Server={server};Uid={user};Pwd={password};";
            using (MySqlConnection conn = new MySqlConnection(connStrNoDb))
            {
                //try connecting and if it doesn't works handles the exception
                try
                {
                    conn.Open();

                    string createDb = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    using (MySqlCommand cmd = new MySqlCommand(createDb, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle common MySQL errors
                    switch (ex.Number)
                    {
                        case 1042: //in case xampp isnt running
                            MessageBox.Show("Database creation failed: Access denied.\n" +
                                            "MAKE sure xampp, apache and mysql are running .",
                                            "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case 1044: // Access denied
                        case 1045: // Invalid username/password
                            MessageBox.Show("Database creation failed: Access denied.\n" +
                                            "Make sure your MySQL user has CREATE DATABASE permissions.",
                                            "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case 1046: // No database selected
                        case 1049: // Unknown database
                            MessageBox.Show("Connection successful, but the database could not be created.\n" +
                                            "Trying to recreate...",
                                            "MySQL Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            TryFixDatabaseCreation(connStrNoDb, dbName);
                            break;

                        default:
                            MessageBox.Show($"MySQL Error {ex.Number}: {ex.Message}",
                                             "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            

            



        // STEP 2 — Connect to the studentdb
        string connStr = $"Server={server};Database={dbName};Uid={user};Pwd={password};";
            using (MySqlConnection conn = new MySqlConnection(connStr))
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


                // ==============================
                // USER TABLE (WITH ROLE)
                // ==============================
                string createUserTable = @"
                CREATE TABLE IF NOT EXISTS user (
                    userId INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    username VARCHAR(50) NOT NULL UNIQUE,
                    password VARCHAR(255) NOT NULL,
                    role VARCHAR(20) NOT NULL DEFAULT 'user'
                )";
                new MySqlCommand(createUserTable, conn).ExecuteNonQuery();

                // Insert admin only if not exists
                string checkAdmin = "SELECT COUNT(*) FROM user WHERE username = 'admin'";
                long exists = (long)new MySqlCommand(checkAdmin, conn).ExecuteScalar();

                if (exists == 0)
                {
                    // Admin has full access
                    string insertAdmin = "INSERT INTO user (username, password, role) VALUES ('admin', 'admin', 'admin')";
                    new MySqlCommand(insertAdmin, conn).ExecuteNonQuery();
                }

                // ==============================
                // STUDENT TABLE linked to userId
                // ==============================
                string createStudentTable = @"
                CREATE TABLE IF NOT EXISTS student (
                    StdId INT(10) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    userId INT(11) NOT NULL,
                    StdFirstName VARCHAR(50) NOT NULL,
                    StdLastName VARCHAR(50) NOT NULL,
                    birthdate DATETIME NOT NULL,
                    Gender VARCHAR(10) NOT NULL,
                    Phone VARCHAR(15) NOT NULL,
                    Address TEXT NOT NULL,
                    Photo LONGBLOB NOT NULL,
                    FOREIGN KEY (userId) REFERENCES user(userId)
                )";
                new MySqlCommand(createStudentTable, conn).ExecuteNonQuery();

                // ==============================
                // COURSE TABLE
                // ==============================
                string createCourseTable = @"
                CREATE TABLE IF NOT EXISTS course (
                    CourseId INT(10) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    CourseName VARCHAR(50) NOT NULL,
                    CourseHour INT(5) NOT NULL,
                    Description TEXT NOT NULL
                )";
                new MySqlCommand(createCourseTable, conn).ExecuteNonQuery();

                // ==============================
                // SCORE TABLE (linked to student)
                // ==============================
                string createScoreTable = @"
                CREATE TABLE IF NOT EXISTS score (
                    ScoreId INT(10) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    StudentId INT(10) NOT NULL,
                    CourseName VARCHAR(50) NOT NULL,
                    Score DOUBLE NOT NULL,
                    Description TEXT NOT NULL,
                    FOREIGN KEY (StudentId) REFERENCES student(StdId)
                )";
                new MySqlCommand(createScoreTable, conn).ExecuteNonQuery();
            }
        }
        // if there is an error fix the database
        private static void TryFixDatabaseCreation(string connStrNoDb, string dbName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStrNoDb))
                {
                    conn.Open();

                    string sql = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    new MySqlCommand(sql, conn).ExecuteNonQuery();

                    MessageBox.Show("Database recreated successfully!",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database creation retry failed:\n" + ex.Message,
                                "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


