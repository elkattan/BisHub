using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Insight.Database;

namespace BisHub.Models
{
    public class StudentRepository
    {

        private MySqlConnectionStringBuilder db = new MySqlConnectionStringBuilder(Environment.GetEnvironmentVariable("MYSQL_URL"));

        public Student InsertStudent(Student student)
        {
            IList<UInt64> res = db.Connection().QuerySql<UInt64>(
                string.Format("INSERT INTO `student` (name, gpa, status, level) VALUES ('{0}', {1}, '{2}', {3});SELECT LAST_INSERT_ID();",
                    student.name,
                    student.gpa,
                    student.status,
                    student.level
                )
            );
            // Converting New ID to the student Int ID
            student.id = Convert.ToInt32(res[0]);
            return student;
        }

        public IList<Student> GetStudents()
        {
            return db.Connection().QuerySql<Student>("SELECT * FROM `student`");
        }

        public Student GetStudentById(int id)
        {
            return db.Connection().SingleSql<Student>("SELECT * FROM `student` WHERE id=" + id);
        }
    }

    public class Student
    {
        public Student(string name, string status, double gpa, int level)
        {
            this.name = name;
            this.status = status;
            this.gpa = gpa;
            this.level = level;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public double gpa { get; set; }
        public int level { get; set; }
        public IList<Instructor> instructors { get; set; }

    }

}
