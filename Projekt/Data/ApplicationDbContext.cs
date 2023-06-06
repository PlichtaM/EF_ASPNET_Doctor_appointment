using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Models;
using System;

namespace Projekt.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Przykładowe lekarze
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, Name = "Dr. Smith", Specialization = "Pediatrics" },
                new Doctor { Id = 2, Name = "Dr. Johnson", Specialization = "Dermatology" }
            );

            // Przykładowy użytkownik
            var appUser = new User
            {
                Id = "1",
                Email = "abc@abc.pl",
                EmailConfirmed = true,
                UserName = "abc@abc.pl",
                NormalizedUserName = "ABC@ABC.PL"
            };

            //set user password
            PasswordHasher<User> ph = new PasswordHasher<User>();
            appUser.PasswordHash = ph.HashPassword(appUser, "Haslo1!");

            //seed user
            modelBuilder.Entity<User>().HasData(appUser);



            // Przykładowe spotkania
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Id = 1, Date = DateTime.Now.AddDays(1), Specialization = "Pediatrics", PatientName = "John Doe", Notes = "Brak uwag", DoctorId = 1 },
                new Appointment { Id = 2, Date = DateTime.Now.AddDays(2), Specialization = "Dermatology", PatientName = "Jane Smith", Notes = "Brak uwag", DoctorId = 2 }
            );
        }
    }
}
