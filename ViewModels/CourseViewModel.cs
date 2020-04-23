using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.ViewModels
{
    public class CourseViewModel
    {
           public IList<Course> Courses;
        public SelectList Programmes;
        public SelectList Semestars;
        public string TitleString;
        public string ProgrammeString;
        public int SemestarInt;
    }
}