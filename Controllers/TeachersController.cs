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
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace University.Controllers
{
    public class TeachersController : Controller
    {
         private readonly UniversityContext _context;
         private readonly IWebHostEnvironment _webHostEnvironment;

        public TeachersController(UniversityContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: teachers
        public async Task<IActionResult> Index(int? id, int? courseID, string search)
        {
            var viewModel = new pom();
            viewModel.Teachers = await _context.Teachers           
                  .Include(c => c.Course1)
                  .Include(d => d.Course2)                
                .ThenInclude(i => i.Enrollments)
                    .ThenInclude(i => i.Student)
                .AsNoTracking()
                .ToListAsync();

    if (id != null)
    {
        ViewData["FirstTeacherId"] = id.Value;
        viewModel.Courses = viewModel.Teachers.Where(
            i => i.TeacherId == id).Single().Course1;
    }
     if (id != null)
    {
        ViewData["SecondTeacherID"] = id.Value;
        viewModel.Courses = viewModel.Teachers.Where(
            i => i.TeacherId == id).Single().Course2;
    }

    if (courseID != null)
    {
        ViewData["CourseID"] = courseID.Value;
        viewModel.Enrollments = viewModel.Courses.Where(
            x => x.CourseID == courseID).Single().Enrollments;
    }
    
         ViewData["CurrentFilter"] = search;
         var teachers = from t in _context.Teachers
                  .Include(c => c.Course1)
                  .Include(d => d.Course2)  
                   select t;
    if (!String.IsNullOrEmpty(search))
    {
        teachers = teachers.Where(s => s.FirstName.Contains(search)
                               || s.LastName.Contains(search)
                               || s.Degree.Contains(search)
                               || s.AcademicRank.Contains(search)
                               );
        viewModel.Teachers=await teachers.AsNoTracking().ToListAsync();
    }

    return View(viewModel);

        }
         // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }
 
                
            var teacher = await _context.Teachers
                 .Include(c => c.Course1)
                  .Include(d => d.Course2)  
                .FirstOrDefaultAsync(m => m.TeacherId == id);
                
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }
          // GET: Teachers/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherForm model, string[] selectedCourses, Teacher teacher)
        {
            if (selectedCourses != null)
            {
                teacher.Course1 = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new Course { FirstTeacherId = teacher.TeacherId, CourseID = int.Parse(course) };
                    teacher.Course1.Add(courseToAdd);
                }
                teacher.Course2 = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new Course { SecondTeacherId = teacher.TeacherId, CourseID = int.Parse(course) };
                    teacher.Course2.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                teacher = new Teacher
                {
                    
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Degree = model.Degree,
                    AcademicRank = model.AcademicRank,
                    OfficeNumber = model.OfficeNumber,
                    HireDate = model.HireDate,
                    ProfilePicture = uniqueFileName,
                    Course1 = model.Courses_first,
                    Course2 = model.Courses_second
                };

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View();
        }
          private string UploadedFile(TeacherForm model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "teacherimages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
               
            if (teacher == null)
            {
                return NotFound();
            }
            
              TeacherForm vm = new TeacherForm
            {
                Id = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Degree = teacher.Degree,
                AcademicRank = teacher.AcademicRank,
                OfficeNumber = teacher.OfficeNumber,
                HireDate = teacher.HireDate,
                Courses_first = teacher.Course1,
                Courses_second = teacher.Course2

            };

            return View(vm);
        }
       [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(int id, TeacherForm vm)
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

                    Teacher teacher = new Teacher
                    {
                        TeacherId = vm.Id,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        ProfilePicture = uniqueFileName,
                        Degree = vm.Degree,
                        AcademicRank = vm.AcademicRank,
                        OfficeNumber = vm.OfficeNumber,
                        HireDate = vm.HireDate,
                        Course1= vm.Courses_first,
                        Course2 = vm.Courses_second
                    };

                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(vm.Id))
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
     
          // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                var teacher = await _context.Teachers.FindAsync(id);

            //delete image file from the folder
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", teacher.ProfilePicture);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }

        
    // GET: Teachers/Courses/2
        public async Task<IActionResult> Courses(int id)
        {
            var courses = _context.Courses.Where(c=>c.FirstTeacherId == id || c.SecondTeacherId == id);
            courses = courses.Include(t=>t.FirstTeacher).Include(t=>t.SecondTeacher);

            ViewData["TeacherId"] = id;
            ViewData["TeacherAcademicRank"] = _context.Teachers.Where(t => t.TeacherId == id).Select(t => t.AcademicRank).FirstOrDefault();
            ViewData["TeacherName"] = _context.Teachers.Where(t => t.TeacherId == id).Select(t => t.FullName).FirstOrDefault();
            return View(courses);
        }
       
    }
}