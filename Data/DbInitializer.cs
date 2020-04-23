using University.Models;
using System;
using System.Linq;

namespace University.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UniversityContext context)
        {
           context.Database.EnsureCreated();

            
            if (context.Students.Any())
            {
                return;   
            }
            
            var students = new Student[]
            {
            new Student{StudentId="1000",FirstName="Harry",LastName="Potter", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1001",FirstName="Hermione",LastName="Granger", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1002",FirstName="Ronald",LastName="Weasley", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1003",FirstName="Ginny",LastName="Weasley", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1004",FirstName="Neville",LastName="Longbottom", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1005",FirstName="Luna",LastName="Lovegood", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1006",FirstName="Draco",LastName="Malfoy", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"},
            new Student{StudentId="1007",FirstName="Cedric",LastName="Diggory", EnrollmentDate=DateTime.Parse("2018-09-01"),
             AcquiredCredits=100, CurrentSemestar=4,EducationLevel="High School Diploma"}
            };
            
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            // if(context.Teachers.Any()){
              //  return;
           // }
            var teachers=new Teacher[]
            {
            new Teacher{ FirstName="Severus", LastName="Snape", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="304",HireDate=DateTime.Parse("2003-07-01")},
            new Teacher{ FirstName="Minerva", LastName="McGonagall", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="201",HireDate=DateTime.Parse("2004-08-02")},
            new Teacher{ FirstName="Albus", LastName="Dumbledore", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="804",HireDate=DateTime.Parse("2002-06-05")},
            new Teacher{ FirstName="Filius", LastName="Flitwick", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="312",HireDate=DateTime.Parse("2003-04-23")},
            new Teacher{ FirstName="Pomona", LastName="Sprout", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="901",HireDate=DateTime.Parse("2005-09-23")},
            new Teacher{ FirstName="Horace", LastName="Slughorn", Degree="Doctorate", AcademicRank="Full professor", 
            OfficeNumber="134",HireDate=DateTime.Parse("2004-02-12")},
            new Teacher{ FirstName="Rubeus", LastName="Hagrid", Degree="Doctorate", AcademicRank="Full professor",
             OfficeNumber="784",HireDate=DateTime.Parse("2002-08-16")}
            };
            foreach (Teacher t in teachers){
                context.Teachers.Add(t);
            }
            context.SaveChanges();

           // if(context.Courses.Any()){
             //   return;
            //}

            var courses = new Course[]
            {
            new Course{CourseID=1050,Title="Potions",Credits=3,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Snape").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="McGonagall").TeacherId },
            new Course{CourseID=4022,Title="History of Magic",Credits=3,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Snape").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="McGonagall").TeacherId},
            new Course{CourseID=4041,Title="Defence Againts the Dark Arts",Credits=3,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Dumbledore").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="Flitwick").TeacherId},
            new Course{CourseID=1045,Title="Care of Magical Creatures",Credits=3,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Dumbledore").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="Flitwick").TeacherId},
            new Course{CourseID=3141,Title="Astronomy",Credits=4,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Sprout").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="Slughorn").TeacherId},
            new Course{CourseID=2021,Title="Flying",Credits=3,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Sprout").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="Hagrid").TeacherId},
            new Course{CourseID=2042,Title="Study of Ancient Runes",Credits=4,Semester=4,Programme="New programme", EducationLevel="High School Diploma",
            FirstTeacherId=teachers.Single(s=>s.LastName=="Slughorn").TeacherId, SecondTeacherId=teachers.Single(s=>s.LastName=="Hagrid").TeacherId}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

           

            var enrollments = new Enrollment[]
            {
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Potter").ID,CourseID= courses.Single(c => c.Title == "Potions").CourseID, Semester="4", 
            Grade=10, SeminalUrl="abc", ProjectUrl="ab1",ExamPoints=100, SeminalPoints=100, ProjectPoints=100,AdditionalPoints=5, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Potter").ID,CourseID=courses.Single(c => c.Title == "History of Magic").CourseID,Semester="4",
             Grade=9, SeminalUrl="abd", ProjectUrl="ab2", ExamPoints=80, SeminalPoints=90, ProjectPoints=500,AdditionalPoints=0, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Granger").ID,CourseID=courses.Single(c => c.Title == "History of Magic").CourseID,Semester="4",
             Grade=8, SeminalUrl="abe", ProjectUrl="ab3", ExamPoints=50, SeminalPoints=80, ProjectPoints=100,AdditionalPoints=3, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Granger").ID,CourseID=courses.Single(c => c.Title == "Defence Againts the Dark Arts").CourseID,Semester="4",
             Grade=7, SeminalUrl="abf", ProjectUrl="ab4", ExamPoints=70, SeminalPoints=50, ProjectPoints=50,AdditionalPoints=2, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.FirstName == "Ron").ID,CourseID=courses.Single(c => c.Title == "Defence Againts the Dark Arts").CourseID,Semester="4",
             Grade=6, SeminalUrl="abg", ProjectUrl="ab5", ExamPoints=50, SeminalPoints=50, ProjectPoints=50,AdditionalPoints=5, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.FirstName == "Ron").ID,CourseID=courses.Single(c => c.Title == "Care of Magical Creatures").CourseID,Semester="4",
             Grade=6, SeminalUrl="abh", ProjectUrl="ab6", ExamPoints=40, SeminalPoints=50, ProjectPoints=60,AdditionalPoints=3, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.FirstName == "Giny").ID,CourseID=courses.Single(c => c.Title == "Care of Magical Creatures").CourseID,Semester="4",
             Grade=9, SeminalUrl="abi", ProjectUrl="ab7", ExamPoints=70, SeminalPoints=100, ProjectPoints=60,AdditionalPoints=6, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.FirstName == "Giny").ID,CourseID=courses.Single(c => c.Title == "Astronomy").CourseID,Semester="4",
             Grade=9, SeminalUrl="abj", ProjectUrl="ab8", ExamPoints=80, SeminalPoints=90, ProjectPoints=100,AdditionalPoints=12, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Longbottom").ID,CourseID=courses.Single(c => c.Title == "Astronomy").CourseID,Semester="4",
             Grade=6, SeminalUrl="abk", ProjectUrl="ab9", ExamPoints=50, SeminalPoints=30, ProjectPoints=40,AdditionalPoints=8, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Longbottom").ID,CourseID=courses.Single(c => c.Title == "Flying").CourseID,Semester="4",
             Grade=8, SeminalUrl="abl", ProjectUrl="ab10", ExamPoints=100, SeminalPoints=50, ProjectPoints=100,AdditionalPoints=0, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Lovegood").ID,CourseID=courses.Single(c => c.Title == "Flying").CourseID,Semester="4",
             Grade=7, SeminalUrl="abm", ProjectUrl="ab11", ExamPoints=70, SeminalPoints=70, ProjectPoints=70,AdditionalPoints=10, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Lovegood").ID,CourseID=courses.Single(c => c.Title == "Study of Ancient Runes").CourseID,Semester="4",
             Grade=10, SeminalUrl="abn", ProjectUrl="ab12", ExamPoints=100, SeminalPoints=100, ProjectPoints=100,AdditionalPoints=3, FinishDate=DateTime.Parse("2020-10-01")},
             new Enrollment{ StudentID=students.Single(s => s.LastName == "Malfoy").ID,CourseID=courses.Single(c => c.Title == "Study of Ancient Runes").CourseID,Semester="4",
              Grade=6, SeminalUrl="abo", ProjectUrl="ab13", ExamPoints=50, SeminalPoints=30, ProjectPoints=40,AdditionalPoints=8, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Malfoy").ID,CourseID=courses.Single(c => c.Title == "Defence Againts the Dark Arts").CourseID,Semester="4",
             Grade=8, SeminalUrl="abp", ProjectUrl="ab14", ExamPoints=100, SeminalPoints=50, ProjectPoints=100,AdditionalPoints=0, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Diggory").ID,CourseID=courses.Single(c => c.Title == "Flying").CourseID,Semester="4",
             Grade=7, SeminalUrl="abq", ProjectUrl="ab15", ExamPoints=70, SeminalPoints=70, ProjectPoints=70,AdditionalPoints=10, FinishDate=DateTime.Parse("2020-10-01")},
            new Enrollment{ StudentID=students.Single(s => s.LastName == "Diggory").ID,CourseID=courses.Single(c => c.Title == "Potions").CourseID,Semester="4",
             Grade=10, SeminalUrl="abr", ProjectUrl="ab16", ExamPoints=100, SeminalPoints=100, ProjectPoints=100,AdditionalPoints=3, FinishDate=DateTime.Parse("2020-10-01")}
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();

            
        }
    }
}