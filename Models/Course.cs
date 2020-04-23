using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;


namespace University.Models
{
    public class Course
    {
       
      
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int CourseID { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public int Credits { get; set; }
        public int Semester {get; set;}
        [StringLength(100)]
        public string Programme {get; set;}
        [Display(Name = "Ecudation Level")]
        [StringLength(25)]
        public string EducationLevel {get; set;}
        public int FirstTeacherId {get; set;}
        public int SecondTeacherId {get; set;}
        
        [ForeignKey("FirstTeacherId")]
        [Display(Name = "First Teacher")]
        public Teacher FirstTeacher {get; set;}
        
        [ForeignKey("SecondTeacherId")]
        [Display(Name = "Second Teacher")]
        public Teacher SecondTeacher {get; set;}
        
        
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}