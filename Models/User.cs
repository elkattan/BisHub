using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Insight.Database;
using BisHub.Libs;
using BisHub.Entities;

namespace BisHub.Models
{
    static class Global
    {
        public static StudentRepository studentRepo = new StudentRepository();
        public static InstructorRepository instructorRepo = new InstructorRepository();
    }
    public class UserRepository
    {

        private MySqlConnectionStringBuilder db = new MySqlConnectionStringBuilder(Environment.GetEnvironmentVariable("MYSQL_URL"));


        public User InsertUser(User user)
        {
            string student = "null";
            string instructor = "null";
            if (user.student != null)
            {
                user.student = Global.studentRepo.InsertStudent(user.student);
                student = user.student.id + ""; // Creating New Student and converting his ID to String
            }
            else if (user.instructor != null)
            {
                user.instructor = Global.instructorRepo.InsertInstructor(user.instructor);
                instructor = user.instructor.id + ""; // Creating New Instructor and converting his ID to String
            }
            IList<UInt64> res = db.Connection().QuerySql<UInt64>(
                string.Format("INSERT INTO `user` (username, email, password, create_date, citizen_id, image_url, student_id, instructor_id) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7});SELECT LAST_INSERT_ID();",
                    user.username,
                    user.email,
                    user.getEncryptedPassword(),
                    user.create_date.ToString("yyyy-MM-dd"),
                    user.citizen_id,
                    user.image_url,
                    student,
                    instructor
                )
            );
            // Converting New ID to the user Int ID
            user.id = Convert.ToInt32(res[0]);
            return user;
        }

        public IList<User> GetUsers()
        {
            return db.Connection().QuerySql<User>("SELECT * FROM `user`");
        }

        public User getUserByEmailOrUsername(string usernameOrEmail)
        {
            IList<User> res = db.Connection().QuerySql<User>(string.Format("SELECT * FROM `user` WHERE `username` = '{0}' OR `email` = '{0}'", usernameOrEmail));
            if (res.Count == 0)
            {
                return null;
            }
            else
            {
                return res[0];
            }
        }

        public User GetUserById(int id)
        {
            return db.Connection().SingleSql<User>("SELECT * FROM `user` WHERE id=" + id);
        }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string accessToken { get; set; }
        private string _password { get; set; }
        public string password
        {
            get
            {
                return null;
            }
            set
            {
                _password = value;
            }
        }
        public DateTime create_date { get; set; }
        public string citizen_id { get; set; }
        public string image_url { get; set; }
        public Student student = null;
        public int student_id
        {
            get
            {
                if (student != null) return student.id;
                return 0;
            }
            set
            {
                student = Global.studentRepo.GetStudentById(value);
            }
        }
        public Instructor instructor = null;
        public int instructor_id
        {
            get
            {
                if (instructor != null) return instructor.id;
                return 0;
            }
            set
            {
                instructor = Global.instructorRepo.GetInstructorById(value);
            }
        }

        // Default to USER
        public string role = Role.User;


        public string getEncryptedPassword()
        {
            return Crypto.Encrypt(_password);
        }

        public bool isPasswordValid(string pass)
        {
            string decrypted = Crypto.Decrypt(_password);
            if (pass == decrypted) return true;
            return false;
        }

    }

}
