using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
   
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [StringLength(10)]
        public string Semester {get; set;}
        public int Year { get; set; }
        public int Grade { get; set; }
        [StringLength(255)]
       
        public string SeminalUrl {get; set;}
        
        [StringLength(255)]
        public string ProjectUrl {get; set;}
       
        public int ExamPoints { get; set; }
        
        public int SeminalPoints { get; set; }
        
        public int ProjectPoints { get; set; }
       
        public int AdditionalPoints { get; set; }
        
        public DateTime FinishDate { get; set; }
      

       [ForeignKey("CourseID")]
        public Course Course { get; set; }

        [ForeignKey("StudentID")]
        public Student Student { get; set; }
    }
}