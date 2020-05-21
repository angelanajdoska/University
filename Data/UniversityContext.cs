using University.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace University.Data
{
    public class UniversityContext : IdentityDbContext<AppUser>
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
         public DbSet<AppUser> AppUser { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);                            
          
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Course>()
                    .HasOne(m => m.FirstTeacher)
                    .WithMany(t => t.Course1)
                    .HasForeignKey(m => m.FirstTeacherId).OnDelete(DeleteBehavior.NoAction);
                   // .WillCascadeOnDelete(false);

                     modelBuilder.Entity<Course>()
                    .HasOne(m => m.SecondTeacher)
                    .WithMany(t => t.Course2)
                    .HasForeignKey(m => m.SecondTeacherId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrollment>()
            .HasOne(m => m.Course)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(m => m.CourseID).OnDelete(DeleteBehavior.NoAction);     
            
              modelBuilder.Entity<Enrollment>()
            .HasOne(m => m.Student)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(m => m.StudentID).OnDelete(DeleteBehavior.NoAction);
                    
                     

           // modelBuilder.Entity<Course>()
            //    .Has(c => new { c.FirstTeacherId, c.SecondTeacherId});
        }
    }
}