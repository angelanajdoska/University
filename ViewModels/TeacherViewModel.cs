using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.ViewModels
{
    public class TeacherViewModel
    {
         public IList<Teacher> Teachers;
        public SelectList Degrees;
        public SelectList AcademicRanks;
        public string FirstNameString;
        public string LastNameString;
        public string DegreeString;
        public string AcademicRankString;
    }
}