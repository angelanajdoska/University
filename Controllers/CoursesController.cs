using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.ViewModels;

namespace University.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UniversityContext _context;

        public CoursesController(UniversityContext context)
        {
            _context = context;
        }

  


        // GET: Courses
        public async Task<IActionResult> Index(string stringsearch, int? intsearch)
        {            
                var courses =  _context.Courses                       
                .Include(f => f.FirstTeacher)
                .Include(p => p.SecondTeacher)
            .Include(e => e.Enrollments)
                     .ThenInclude(e => e.Student)
                .AsNoTracking();

     
     ViewData["Filter"]=stringsearch;

    if (!string.IsNullOrEmpty(stringsearch))
    {
        courses = courses.Where(s => s.Title.Contains(stringsearch)
                                || s.Programme.Contains(stringsearch));
    }
    ViewData["Filter"]=intsearch;
    if( intsearch!=null ){
      courses=courses.Where(i => i.Semester==(intsearch));
    }

              return View(await courses.AsNoTracking().ToListAsync());
        }
          public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Student)
                .Include(c => c.FirstTeacher)
                .Include(p => p.SecondTeacher)                               
               .FirstOrDefaultAsync(m => m.CourseID == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

       



         public IActionResult Create()
        {
            PopulateTeachersDropDownList1();
            PopulateTeachersDropDownList2();
          return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID, Title, Credits, Semester, Programme, EducationLevel, FirstTeacherId, SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           PopulateTeachersDropDownList1(course.FirstTeacherId);
           PopulateTeachersDropDownList2(course.SecondTeacherId);

           return View(course);
        }


        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Courses.Where(m=> m.CourseID==id)
                .Include(m=>m.Enrollments).First();
            if (course == null)
            {
                return NotFound();
            }

            ViewData["FirstTeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
      
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViewModel viewModel)
        {
            if (id != viewModel.course.CourseID)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(viewModel.course);
                    await _context.SaveChangesAsync();

                    IEnumerable<long> listStudents = viewModel.selectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollments.Where(s => !listStudents.Contains(s.StudentID) && s.CourseID == id);
                    _context.Enrollments.RemoveRange(toBeRemoved);
                    IEnumerable<long> existStudents = _context.Enrollments.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == id).Select(s => s.StudentID);
                    IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    foreach (int studentId in newStudents)
                        _context.Enrollments.Add(new Enrollment { StudentID = studentId, CourseID = id });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewModel.course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
                }

        private void PopulateTeachersDropDownList1(object selectedTeacher = null)
        {
            var teachersQuery = from d in _context.Teachers
                                   orderby d.FirstName
                                   select d;
            ViewBag.FirstTeacherId = new SelectList(teachersQuery.AsNoTracking(), "TeacherId", "FullName", selectedTeacher);
        }
          private void PopulateTeachersDropDownList2(object selectedTeacher = null)
        {
            var teachersQuery = from d in _context.Teachers
                                   orderby d.FirstName
                                   select d;
            ViewBag.SecondTeacherId = new SelectList(teachersQuery.AsNoTracking(), "TeacherId", "FullName", selectedTeacher);
        }
        
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.FirstTeacher)
                .Include(p => p.SecondTeacher)
                .Include(e => e.Enrollments)
                .ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    

        [HttpPost]
     
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }

           // GET: Courses/Enroll/3
        public async Task<IActionResult> Enroll(int? id)
        {
            var course = _context.Courses.Where(m => m.CourseID == id).Include(m => m.Enrollments).First();

            EnrollmentViewModel vm = new EnrollmentViewModel
            {
                StudentsList = new MultiSelectList(_context.Students.OrderBy(s => s.FirstName), "ID", "FullName"),
                SelectedStudents = course.Enrollments.Select(sa => sa.StudentID)
            };

            ViewData["CourseName"] = _context.Courses.Where(c => c.CourseID == id).Select(c => c.Title).FirstOrDefault();
            ViewData["chosenId"] = id;
            return View(vm);
        }

         // POST: Courses/Enroll/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id, EnrollmentViewModel viewmodel)
        { 
            if (id != viewmodel.NewEnrollment.CourseID)
            {
                return NotFound();
            }

            //Insert (enroll students)
            if (viewmodel.NewEnrollment.FinishDate == null)
            {
                IEnumerable<long> listStudents = viewmodel.SelectedStudents;
                IEnumerable<long> existStudents = _context.Enrollments.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == id).Select(s => s.StudentID);
                IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));

                foreach (int studentId in newStudents)
                    _context.Enrollments.Add(new Enrollment { StudentID = studentId, CourseID = id, Year = viewmodel.NewEnrollment.Year, Semester = viewmodel.NewEnrollment.Semester });

                await _context.SaveChangesAsync();
            }
            else
            {
                //Update enrollments (write off students) 
                var enrollments = _context.Enrollments.Where(e => e.CourseID == id).Include(e => e.Course).Include(e => e.Student);

                foreach (Enrollment e in enrollments) {
                    e.FinishDate = viewmodel.NewEnrollment.FinishDate;
                    _context.Enrollments.Add(e);
                }

                
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }


    }
}