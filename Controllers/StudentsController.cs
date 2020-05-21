using University.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using University.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace University.Controllers
{
    public class StudentsController : Controller
    {
          private readonly UniversityContext _context;
          private readonly IWebHostEnvironment _webHostEnvironment;
          private UserManager<AppUser> userManager;

        public StudentsController(UniversityContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> usrMgr)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            userManager = usrMgr;
        }

         [Authorize(Roles = "Admin")]
      public async Task<IActionResult> Index(string searchString)
    {
         ViewData["CurrentFilter"] = searchString;
         var students = from s in _context.Students
                   select s;
    if (!String.IsNullOrEmpty(searchString))
    {
        students = students.Where(s => s.FirstName.Contains(searchString)
                               || s.LastName.Contains(searchString)
                               || s.StudentId.Contains(searchString)
                               );
    }
 
        return View(await students.AsNoTracking().ToListAsync());
    }

     [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(Int64? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var student = await _context.Students
        .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.ID == id);

    if (student == null)
    {
        return NotFound();
    }

    return View(student);
}
 [Authorize(Roles = "Admin")]
  public IActionResult Create()
        {
            return View();
        }
        
[HttpPost]
[ValidateAntiForgeryToken]
 [Authorize(Roles = "Admin")]
   public async Task<IActionResult> Create(StudentForm model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Student student = new Student
                {
                    StudentId = model.Index,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EnrollmentDate = model.EnrollmentDate,
                    AcquiredCredits = model.AcquiredCredits,
                    CurrentSemestar = model.CurrentSemestar,
                    ProfilePicture = uniqueFileName,
                    EducationLevel = model.EducationLevel,
                    Enrollments= model.Courses
                };

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
          private string UploadedFile(StudentForm model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "studentimages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
  // GET: Students/Edit/5
   [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Int64? id)
         {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            StudentForm vm = new StudentForm
            {
                Id = student.ID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Index = student.StudentId,
                EnrollmentDate = student.EnrollmentDate,
                AcquiredCredits = student.AcquiredCredits,
                CurrentSemestar = student.CurrentSemestar,
                EducationLevel = student.EducationLevel,
                Courses = student.Enrollments
            };

            return View(vm);
        }

        // POST: Students/Edit/5
    
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, StudentForm vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(vm);

                    Student student = new Student
                    {
                        ID = vm.Id,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        ProfilePicture = uniqueFileName,
                        EnrollmentDate = vm.EnrollmentDate,
                        CurrentSemestar = vm.CurrentSemestar,
                        AcquiredCredits = vm.AcquiredCredits,
                        StudentId = vm.Index,
                        EducationLevel = vm.EducationLevel,
                        Enrollments = vm.Courses
                    };

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(vm.Id))
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
            return View(vm);
        }

 // GET: Students/Delete/5
  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Int64? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
         [Authorize(Roles = "Admin")]
         public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
           
            //delete image file from the folder
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", student.ProfilePicture);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

        [Authorize(Roles = "Student")]
       public async Task<IActionResult> MyCourses(long? id)
        {
            IQueryable<Course> courses = _context.Courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollments.AsQueryable();
            enrollments = enrollments.Where(s => s.StudentID==id); //se zemaat onie zapisi kaj koi studentId == id-to od url-to
            IEnumerable<int> enrollmentsIDS = enrollments.Select(e => e.CourseID).Distinct(); //se zemaat distinct IDs na courses od prethodno najdenite zapisi

            courses = courses.Where(s => enrollmentsIDS.Contains(s.CourseID));  //filtriranje na students spored id

            courses = courses.Include(c => c.Enrollments).ThenInclude(c => c.Student);

            ViewData["StudentName"] = _context.Students.Where(t => t.ID == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["studentId"] = id;
            return View(courses);
        }

    }
    

}

  