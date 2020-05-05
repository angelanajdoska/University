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
    public class EnrollmentsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EnrollmentsController(UniversityContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var UniversityContext = _context.Enrollments.Include(e => e.Course).Include(e => e.Student);
            return View(await UniversityContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title");
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "FullName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,CourseID,StudentID,Semester,Grade,Year,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EnrollmentID,CourseID,StudentID,Semester,Grade,Year,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID))
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
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(long id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
        
         // GET: Enrollments/StudentsbyCourse/5
        public async Task<IActionResult> StudentsbyCourse(int? id, int enrollmentYear)
        { 
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<Enrollment> enrollments = _context.Enrollments.Where(e => e.CourseID == id);
            enrollments = enrollments.Include(e => e.Course).Include(e => e.Student).OrderBy(e=>e.Student.StudentId).OrderBy(e=>e.Student.StudentId);

            if (enrollmentYear != 0)
            {
                enrollments = enrollments.Where(x => x.Year == enrollmentYear);
            }
            else
            {
                //se prikazuvat studentite zapisani vo poslednata godina
                enrollments = enrollments.Where(x => x.Year == DateTime.Now.Year);
            }

            EnrollmentFilter vm = new EnrollmentFilter
            {
                Enrollments = await enrollments.ToListAsync()
            };

            ViewData["CourseName"] = _context.Courses.Where(c => c.CourseID == id).Select(c => c.Title).FirstOrDefault();
            return View(vm);
        }
         // GET: Enrollments/Editstudent/5
        public async Task<IActionResult> Editstudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["ID"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);
            ViewData["StudentName"] = _context.Students.Where(s => s.ID == enrollment.StudentID).Select(s => s.FullName).FirstOrDefault();
            ViewData["CourseName"] = _context.Courses.Where(s => s.CourseID == enrollment.CourseID).Select(s => s.Title).FirstOrDefault();
            return View(enrollment);
        }

        // POST: Enrollments/Editstudent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editstudent(int id, [Bind("EnrollmentID,CourseID, StudentID, Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CourseStudents", new { id = enrollment.CourseID });
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["ID"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

     // GET: Enrollments/EditByStudent/5
        public async Task<IActionResult> EditByStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            } 

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentId"] = new SelectList(_context.Students, "ID", "FullName", enrollment.StudentID);

            EnrollmentView vm = new EnrollmentView
            {
                Id = enrollment.EnrollmentID,
                Semester = enrollment.Semester,
                Year = enrollment.Year,
                Grade = enrollment.Grade,
                ProjectUrl = enrollment.ProjectUrl,
                SeminalPoints = enrollment.SeminalPoints,
                ProjectPoints = enrollment.ProjectPoints,
                AdditionalPoints = enrollment.AdditionalPoints,
                ExamPoints = enrollment.ExamPoints,
                FinishDate = enrollment.FinishDate,
                CourseId = enrollment.CourseID,
                StudentId = enrollment.StudentID
            };

            ViewData["StudentName"] = _context.Students.Where(s => s.ID == enrollment.StudentID).Select(s => s.FullName).FirstOrDefault();
            ViewData["CourseName"] = _context.Courses.Where(s => s.CourseID == enrollment.CourseID).Select(s => s.Title).FirstOrDefault();
            return View(vm);
        }

        // POST: Enrollments/EditByStudent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditByStudent(int id, EnrollmentView vm)
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

                    Enrollment enrollment= new Enrollment
                    {
                        EnrollmentID=vm.Id,
                        Semester= vm.Semester,
                        Year= vm.Year,
                        Grade= vm.Grade,
                        SeminalUrl = uniqueFileName,
                        ProjectUrl = vm.ProjectUrl,
                        SeminalPoints=vm.SeminalPoints,
                        ProjectPoints=vm.ProjectPoints,
                        AdditionalPoints=vm.AdditionalPoints,
                        ExamPoints=vm.ExamPoints,
                        FinishDate=vm.FinishDate,
                        CourseID=vm.CourseId,
                        StudentID=vm.StudentId
                    };

                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = vm.Id });
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseID", "Title", vm.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "ID", "FullName", vm.StudentId);
            return View(vm);
        }

        private string UploadedFile(EnrollmentView model)
        {
            string uniqueFileName = null;

            if (model.SeminalUrl != null)
            {
                string uploadsFolder = Path.Combine( _webHostEnvironment.WebRootPath  , "seminalFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.SeminalUrl.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.SeminalUrl.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


    }
    }
