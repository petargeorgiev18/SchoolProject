using Loggers_SchoolProject;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Abstractions;
using System.Text;

namespace ADO.NET_SchoolProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
            Initial Catalog=master;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
            Console.Write("Enter database name: ");
            string databaseName = Console.ReadLine();
            if (DatabaseExists(connectionString, "SchoolDB"))
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;
                Initial Catalog=SchoolDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
                connection.Open();
                using (connection)
                {
                    Console.WriteLine(Query1(connection));
                    Console.WriteLine(Query2(connection));
                    Console.WriteLine(Query3(connection));
                    Console.WriteLine(Query4(connection));
                    Console.WriteLine(Query5(connection));
                    Console.WriteLine(Query6(connection));
                    Console.Write("Enter student class number: ");
                    int classNumber = int.Parse(Console.ReadLine());
                    Console.Write("Enter student class letter: ");
                    char classLetter = char.Parse(Console.ReadLine());
                    Console.WriteLine(Query7(connection, classNumber, classLetter));
                    Console.Write("Enter student's date of birth in format '(yyyy-mm-dd)': ");
                    DateTime dateTime = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine(Query8(connection,dateTime));
                    Console.Write("Enter student name:");
                    string studentName = Console.ReadLine();
                    Console.WriteLine(Query9(connection,studentName));
                    Console.Write("Enter student name:");
                    string studentName1 = Console.ReadLine();
                    Console.WriteLine("Teachers' names and subjects of this student:");
                    Console.WriteLine(Query10(connection, studentName1));
                    Console.Write("Enter parent email:");
                    string parentEmail = Console.ReadLine();
                    Console.WriteLine(Query11(connection,parentEmail));
                }
            }
            else
            {
                Console.WriteLine("There's no such database.");
                Console.WriteLine("Proccess of creating database...");
                CreateDatabase(connectionString,  "SchoolDB");
                Console.WriteLine("Proccess of creating tables...");
                CreateTables(connectionString);
                Console.WriteLine("Enter the table which we are going to insert to:");
                string tableName = Console.ReadLine();
                SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;
                Initial Catalog=SchoolDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
                connection.Open();
                using (connection)
                {
                    Console.WriteLine(Query1(connection));
                    Console.WriteLine(Query2(connection));
                    Console.WriteLine(Query3(connection));
                    Console.WriteLine(Query4(connection));
                    Console.WriteLine(Query5(connection));
                    Console.WriteLine(Query6(connection));
                    Console.Write("Enter student class number: ");
                    int classNumber = int.Parse(Console.ReadLine());
                    Console.Write("Enter student class letter: ");
                    char classLetter = char.Parse(Console.ReadLine());
                    Console.WriteLine(Query7(connection, classNumber, classLetter));
                    Console.Write("Enter student's date of birth in format '(yyyy-mm-dd)': ");
                    DateTime dateTime = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine(Query8(connection, dateTime));
                    Console.WriteLine("Enter student name:");
                    string studentName = Console.ReadLine();
                    Console.WriteLine("Count of subjects the student is studying: ");
                    Console.WriteLine(Query9(connection, studentName));
                    Console.WriteLine("Enter student name:");
                    string studentName1 = Console.ReadLine();
                    Console.WriteLine("Teachers' names and subjects of this student:");
                    Console.WriteLine(Query10(connection,studentName1));
                    Console.Write("Enter parent email:");
                    string parentEmail = Console.ReadLine();
                    Console.WriteLine(Query11(connection, parentEmail));
                }
            }
        }
        public static bool DatabaseExists(string connectionString, string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM sys.databases WHERE name = @DatabaseName", connection);
                command.Parameters.AddWithValue("@DatabaseName", databaseName);

                int databaseCount = (int)command.ExecuteScalar();
                return databaseCount > 0;
            }
        }
        public static void CreateDatabase(string connectionString, string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
                command.ExecuteNonQuery();
                Console.WriteLine($"Database successfully created.");
            }
        }
        public static void CreateTables(string connectionString)
        {
            string[] createTableQueries = new string[]
            {
               @"CREATE TABLE subjects
                 (
                   id INT PRIMARY KEY IDENTITY,
                   title NVARCHAR(50) NOT NULL,
                   [level] VARCHAR(20) NOT NULL
                 )",
               @"CREATE TABLE teachers
                 (
                   id INT PRIMARY KEY IDENTITY,
                   teacher_code VARCHAR(50) NOT NULL,
                   full_name NVARCHAR(80) NOT NULL,
                   gender CHAR(1) CHECK (gender = 'm' OR gender = 'f'),
                   date_of_birth DATE NOT NULL,
                   email NVARCHAR(50) NOT NULL,
                   phone NVARCHAR(50) NOT NULL,
                   working_days INT NOT NULL
                 )",
               @"CREATE TABLE classrooms
                 (
                   id INT PRIMARY KEY IDENTITY,
                   [floor] INT NOT NULL,
                   capacity INT NOT NULL,
                   [description] NVARCHAR(50) NOT NULL
                 )",
               @"CREATE TABLE parents
                 (
                   id INT PRIMARY KEY IDENTITY,
                   parent_code VARCHAR(50) NOT NULL,
                   full_name NVARCHAR(80) NOT NULL,
                   email NVARCHAR(30) NOT NULL,
                   phone NVARCHAR(12) NOT NULL
                 )",
               @"CREATE TABLE classes
                 (
                   id INT PRIMARY KEY IDENTITY,
                   class_number INT NOT NULL,
                   class_letter NCHAR(1) NOT NULL,
                   class_teacher_id INT REFERENCES teachers(id),
                   classroom_id INT REFERENCES classrooms(id)
                 )",
               @"CREATE TABLE students
                 (
                   id INT PRIMARY KEY IDENTITY,
                   students_code VARCHAR(20) NOT NULL,
                   full_name NVARCHAR(80) NOT NULL,
                   gender CHAR(1) CHECK(gender = 'm' OR gender = 'f'),
                   date_of_birth DATE NOT NULL,
                   email NVARCHAR(30) NOT NULL,
                   phone VARCHAR(12) NOT NULL,
                   class_id INT REFERENCES classes(id),
                   is_active BIT NOT NULL
                 )",
               @"CREATE TABLE teachers_subjects
                 (
                   id INT PRIMARY KEY IDENTITY,
                   teacher_id INT REFERENCES teachers(id),
                   subject_id INT REFERENCES subjects(id)
                 )",
               @"CREATE TABLE classes_subjects
                 (
                   id INT PRIMARY KEY IDENTITY,
                   class_id INT REFERENCES classes(id),
                   subject_id INT REFERENCES subjects(id)
                 )",
               @"CREATE TABLE students_parents
                 (
                   id INT PRIMARY KEY IDENTITY,
                   student_id INT REFERENCES students(id),
                   parent_id INT REFERENCES parents(id)
                 )"
            };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (string query in createTableQueries)
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while creating the table: {ex.Message}");
                    }
                }
            }
        }
        public static void InsertDatabaseInfo(string connectionString, string tableName, ILog logger)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertData;
                while (true)
                {
                    Console.WriteLine("Enter insert data for table " + tableName + " or type 'end' to stop.");
                    insertData = Console.ReadLine();

                    if (insertData.ToLower() == "end") break;

                    switch (tableName.ToLower())
                    {
                        case "subjects":
                            InsertIntoSubjects(connection, insertData, logger);
                            break;

                        case "teachers":
                            InsertIntoTeachers(connection, insertData, logger);
                            break;

                        case "classrooms":
                            InsertIntoClassrooms(connection, insertData, logger);
                            break;

                        case "parents":
                            InsertIntoParents(connection, insertData, logger);
                            break;

                        case "classes":
                            InsertIntoClasses(connection, insertData, logger);
                            break;

                        case "students":
                            InsertIntoStudents(connection, insertData, logger);
                            break;

                        case "teachers_subjects":
                            InsertIntoTeachersSubjects(connection, insertData, logger);
                            break;

                        case "classes_subjects":
                            InsertIntoClassesSubjects(connection, insertData, logger);
                            break;

                        case "students_parents":
                            InsertIntoStudentsParents(connection, insertData, logger);
                            break;

                        default:
                            Console.WriteLine("Unknown table.");
                            break;
                    }
                }
            }
        }
        private static void InsertIntoSubjects(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO subjects (title, [level]) VALUES (@title, @level)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@title", parts[0].Trim());
            command.Parameters.AddWithValue("@level", parts[1].Trim());
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into subjects.");
            string insertedValues = $"Title: {parts[0].Trim()}, Level: {parts[1].Trim()}";
            logger.LogInsertedValues("subjects", insertedValues);
        }
        private static void InsertIntoTeachers(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO teachers (teacher_code, full_name, gender, date_of_birth, email, phone, working_days) VALUES (@teacher_code, @full_name, @gender, @date_of_birth, @email, @phone, @working_days)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@teacher_code", parts[0].Trim());
            command.Parameters.AddWithValue("@full_name", parts[1].Trim());
            command.Parameters.AddWithValue("@gender", parts[2].Trim());
            command.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(parts[3].Trim()));
            command.Parameters.AddWithValue("@email", parts[4].Trim());
            command.Parameters.AddWithValue("@phone", parts[5].Trim());
            command.Parameters.AddWithValue("@working_days", int.Parse(parts[6].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into teachers.");
            string insertedValues = $"TeacherCode: {parts[0].Trim()}, FullName: {parts[1].Trim()}," +
                $" Gender: {parts[2].Trim()}, DateOfBirth: {parts[3].Trim()}," +
                $" Email: {parts[4].Trim()}, Phone: {parts[5].Trim()}, WorkingDays: {parts[6].Trim()}";
            logger.LogInsertedValues("teachers", insertedValues);
        }
        private static void InsertIntoClassrooms(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO classrooms (floor, capacity, description) VALUES (@floor, @capacity, @description)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@floor", int.Parse(parts[0].Trim()));
            command.Parameters.AddWithValue("@capacity", int.Parse(parts[1].Trim()));
            command.Parameters.AddWithValue("@description", parts[2].Trim());
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into classrooms.");
            string insertedValues = $"Floor: {parts[0].Trim()}, Capacity: {parts[1].Trim()}, Description: {parts[2].Trim()}";
            logger.LogInsertedValues("classrooms", insertedValues);
        }
        private static void InsertIntoParents(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO parents (parent_code, full_name, email, phone) VALUES (@parent_code, @full_name, @email, @phone)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@parent_code", parts[0].Trim());
            command.Parameters.AddWithValue("@full_name", parts[1].Trim());
            command.Parameters.AddWithValue("@email", parts[2].Trim());
            command.Parameters.AddWithValue("@phone", parts[3].Trim());
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into parents.");
            string insertedValues = $"ParentCode: {parts[0].Trim()}, FullName: {parts[1].Trim()}," +
                $" Email: {parts[2].Trim()}, Phone: {parts[3].Trim()}";
            logger.LogInsertedValues("parents", insertedValues);
        }
        private static void InsertIntoClasses(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO classes (class_number, class_letter, class_teacher_id, classroom_id) VALUES (@class_number, @class_letter, @class_teacher_id, @classroom_id)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@class_number", int.Parse(parts[0].Trim()));
            command.Parameters.AddWithValue("@class_letter", parts[1].Trim());
            command.Parameters.AddWithValue("@class_teacher_id", int.Parse(parts[2].Trim()));
            command.Parameters.AddWithValue("@classroom_id", int.Parse(parts[3].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into classes.");
            string insertedValues = $"ClassNumber: {parts[0].Trim()}" +
                $", ClassLetter: {parts[1].Trim()}, ClassTeacherId: {parts[2].Trim()}, ClassroomId: {parts[3].Trim()}";
            logger.LogInsertedValues("classes", insertedValues);
        }
        private static void InsertIntoStudents(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO students (students_code, full_name, gender, date_of_birth, email, phone, class_id, is_active) VALUES (@students_code, @full_name, @gender, @date_of_birth, @email, @phone, @class_id, @is_active)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@students_code", parts[0].Trim());
            command.Parameters.AddWithValue("@full_name", parts[1].Trim());
            command.Parameters.AddWithValue("@gender", parts[2].Trim());
            command.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(parts[3].Trim()));
            command.Parameters.AddWithValue("@email", parts[4].Trim());
            command.Parameters.AddWithValue("@phone", parts[5].Trim());
            command.Parameters.AddWithValue("@class_id", int.Parse(parts[6].Trim()));
            command.Parameters.AddWithValue("@is_active", bool.Parse(parts[7].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into students.");
            string insertedValues = $"StudentCode: {parts[0].Trim()}, FullName: {parts[1].Trim()}, " +
                $"Gender: {parts[2].Trim()}, DateOfBirth: {parts[3].Trim()}, Email: {parts[4].Trim()}," +
                $" Phone: {parts[5].Trim()}, ClassId: {parts[6].Trim()}, IsActive: {parts[7].Trim()}";
            logger.LogInsertedValues("students", insertedValues);
        }
        private static void InsertIntoTeachersSubjects(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO teachers_subjects (teacher_id, subject_id) VALUES (@teacher_id, @subject_id)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@teacher_id", int.Parse(parts[0].Trim()));
            command.Parameters.AddWithValue("@subject_id", int.Parse(parts[1].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into teachers_subjects.");
            string insertedValues = $"TeacherId: {parts[0].Trim()}, SubjectId: {parts[1].Trim()}";
            logger.LogInsertedValues("teachers_subjects", insertedValues);
        }
        private static void InsertIntoClassesSubjects(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO classes_subjects (class_id, subject_id) VALUES (@class_id, @subject_id)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@class_id", int.Parse(parts[0].Trim()));
            command.Parameters.AddWithValue("@subject_id", int.Parse(parts[1].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into classes_subjects.");
            string insertedValues = $"ClassId: {parts[0].Trim()}, SubjectId: {parts[1].Trim()}";
            logger.LogInsertedValues("classes_subjects", insertedValues);
        }
        private static void InsertIntoStudentsParents(SqlConnection connection, string data, ILog logger)
        {
            var parts = data.Split(',');
            string query = "INSERT INTO students_parents (student_id, parent_id) VALUES (@student_id, @parent_id)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@student_id", int.Parse(parts[0].Trim()));
            command.Parameters.AddWithValue("@parent_id", int.Parse(parts[1].Trim()));
            command.ExecuteNonQuery();
            Console.WriteLine("Inserted into students_parents.");
            string insertedValues = $"StudentId: {parts[0].Trim()}, ParentId: {parts[1].Trim()}";
            logger.LogInsertedValues("students_parents", insertedValues);
        }
        public static string Query1(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT s.full_name FROM students as s JOIN classes as c WHERE c.class_number = '11' AND c.class_letter = 'B'", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]} {reader[2]} {reader[3]} " +
                        $"{reader[4]} {reader[5]} {reader[6]} {reader[7]}");
                }
            }
            return sb.ToString();
        }
        public static string Query2(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT \r\n\t\tt.full_name,\r\n\t\ts.title\r\n" +
                "FROM teachers as t\r\n\tJOIN teachers_subjects as ts ON ts.teacher_id = t.id\r\n\t" +
                "JOIN subjects as s ON s.id = ts.subject_id\r\n\tGROUP BY s.title, t.full_name;", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]}");
                }
            }
            return sb.ToString();
        }
        public static string Query3(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT \r\nc.class_number, \r\nc.class_letter, \r\n" +
                "t.full_name AS class_teacher\r\nFROM classes AS c\r\nJOIN teachers AS t ON t.id = c.class_teacher_id;\r\n", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]} {reader[2]}");
                }
            }
            return sb.ToString();
        }
        public static string Query4(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT \r\ns.title AS subject, \r\n" +
                "COUNT(ts.teacher_id) AS teacher_count\r\nFROM subjects AS s\r\n" +
                "JOIN teachers_subjects AS ts ON ts.subject_id = s.id\r\nGROUP BY s.title;", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]}");
                }
            }
            return sb.ToString();
        }
        public static string Query5(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT \r\nid, \r\ncapacity\r\nFROM classrooms\r\n" +
                "WHERE capacity > 26\r\nORDER BY [floor] ASC;", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]}");
                }
            }
            return sb.ToString();
        }
        public static string Query6(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand("SELECT \r\nc.class_number, \r\nc.class_letter, \r\n" +
                "STRING_AGG(s.full_name, ', ') AS student_names\r\nFROM students AS s\r\n" +
                "JOIN classes AS c ON c.id = s.class_id\r\nGROUP BY c.class_number, c.class_letter\r\n" +
                "ORDER BY c.class_number ASC\r\n", connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]} {reader[2]}");
                }
            }
            return sb.ToString();
        }
        public static string Query7(SqlConnection connection, int classNumber, char classLetter)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand(@"
                SELECT s.full_name 
                FROM students AS s
                JOIN classes AS c ON c.id = s.class_id
                WHERE c.class_number = @ClassNumber
                AND c.class_letter = @ClassLetter
                ORDER BY s.full_name", connection);
            command.Parameters.AddWithValue("@ClassNumber", classNumber);
            command.Parameters.AddWithValue("@ClassLetter", classLetter);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sb.AppendLine($"{reader[0]}");
                    }
                }
                else
                {
                    Console.WriteLine("No students in this class with the given name");
                }
            }
            return sb.ToString();
        }
        public static string Query8(SqlConnection connection, DateTime dateOfBirth)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand(@"
                SELECT s.full_name 
                FROM students AS s
                WHERE s.date_of_birth = @DateOfBirth", connection);
            command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]}");
                }
            }
            return sb.ToString();
        }
        public static string Query9(SqlConnection connection, string studentName)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand(@"
                SELECT COUNT(DISTINCT cs.subject_id) 
                FROM students AS s
                JOIN classes_subjects AS cs ON cs.class_id = s.class_id
                WHERE s.full_name = @StudentName", connection);
            command.Parameters.AddWithValue("@StudentName", studentName);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]}");
                }
            }
            return sb.ToString();
        }
        public static string Query10(SqlConnection connection, string studentName)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand(@"
                SELECT t.full_name AS TeacherName, s.title AS SubjectTitle
                FROM students AS st
                JOIN classes AS c ON st.class_id = c.id
                JOIN classes_subjects AS cs ON cs.class_id = c.id
                JOIN subjects AS s ON cs.subject_id = s.id
                JOIN teachers_subjects AS ts ON ts.subject_id = s.id
                JOIN teachers AS t ON ts.teacher_id = t.id
                WHERE st.full_name = @StudentName", connection);
            command.Parameters.AddWithValue("@StudentName", studentName);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]}");
                }
            }
            return sb.ToString();
        }
        public static string Query11(SqlConnection connection, string parentEmail)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand command = new SqlCommand(@"
                SELECT c.class_number, c.class_letter
                FROM parents AS p
                JOIN students_parents AS sp ON sp.parent_id = p.id
                JOIN students AS s ON s.id = sp.student_id
                JOIN classes AS c ON c.id = s.class_id
                WHERE p.email = @ParentEmail", connection);
            command.Parameters.AddWithValue("@ParentEmail", parentEmail);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    sb.AppendLine($"{reader[0]} {reader[1]}");
                }
            }
            return sb.ToString();
        }
    }
}