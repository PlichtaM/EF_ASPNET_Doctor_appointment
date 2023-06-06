using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return View(appointments);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (IsAppointmentAvailable(appointment.Date))
                {
                    _context.Appointments.Add(appointment); 
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The appointment slot is already taken.");
                }
            }
            return View(appointment);
        }

        [Authorize]
        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.Identity.Name;
            var appointments = await _context.Appointments.Where(a => a.PatientName == userId).ToListAsync(); 
            return View(appointments);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id); 
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment); 
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyAppointments));
        }

        private bool IsAppointmentAvailable(DateTime date)
        {
            return !_context.Appointments.Any(a => a.Date == date); 
        }
    }
}
