using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Insight.Database;

namespace BisHub.Models
{
    public class InstructorRepository
    {

        private MySqlConnectionStringBuilder db = new MySqlConnectionStringBuilder(Environment.GetEnvironmentVariable("MYSQL_URL"));

        public Instructor InsertInstructor(Instructor instructor)
        {
            IList<UInt64> res = db.Connection().QuerySql<UInt64>(
                string.Format("INSERT INTO `instructor` (name, birth_date, graduation_date, subject, bio) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');SELECT LAST_INSERT_ID();",
                    instructor.name,
                    instructor.birth_date.ToString("yyyy-MM-dd"),
                    instructor.graduation_date.ToString("yyyy-MM-dd"),
                    instructor.subject,
                    instructor.bio.Replace("'", "\\'")
                )
            );
            // Converting New ID to the instructor Int ID
            instructor.id = Convert.ToInt32(res[0]);
            return instructor;
        }

        public IList<Instructor> GetInstructors()
        {
            return db.Connection().QuerySql<Instructor>("SELECT * FROM `instructor`");
        }

        public Instructor GetInstructorById(int id)
        {
            return db.Connection().SingleSql<Instructor>("SELECT * FROM `instructor` WHERE id=" + id);
        }
    }

    public class Instructor
    {
        public Instructor(string name, string email)
        {
            this.name = name;
            this.email = email;
            this.birth_date = birth_date;
            this.graduation_date = graduation_date;
            this.subject = subject;
            this.bio = bio;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime birth_date { get; set; }
        public DateTime graduation_date { get; set; }
        public string subject { get; set; }
        public string bio { get; set; }
        public IList<Student> students { get; set; }
    }

}
