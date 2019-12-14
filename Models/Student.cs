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
            Student student = db.Connection().SingleSql<Student>("SELECT * FROM `student` WHERE id=" + id);
            if (student != null)
            {
                student.instructors = db.Connection().QuerySql<int>("SELECT `instructor_id` FROM `instructor_has_student` WHERE `student_id` = " + id);
            }
            return student;
        }

        public Student AddInstructor(int id, int iid)
        {
            db.Connection().ExecuteSql(string.Format("INSERT INTO `instructor_has_student` (`student_id`, `instructor_id`) VALUES ({0}, {1})", id, iid));
            return GetStudentById(id);
        }

        public Student RemoveInstructor(int id, int iid)
        {
            db.Connection().ExecuteSql(string.Format("DELETE FROM `instructor_has_student` WHERE `student_id` = {0} AND `instructor_id` = {1}", id, iid));
            return GetStudentById(id);
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
        public IList<int> instructors { get; set; }

    }

}
