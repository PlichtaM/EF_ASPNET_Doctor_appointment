using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly WebApplication2Context _context;

        public AppointmentController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: /Appointment/Calendar
        public async Task<IActionResult> Calendar()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return View(appointments);
        }

        // GET: /Appointment/BookAppointment/5
        public async Task<IActionResult> BookAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: /Appointment/BookAppointment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var loggedInUserId = HttpContext.Session.GetInt32("UserId");
                if (loggedInUserId != null)
                {
                    appointment.UserId = loggedInUserId.Value;

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(appointment);
        }

        // GET: /Appointment/CancelAppointment/5
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: /Appointment/CancelAppointment/5
        [HttpPost, ActionName("CancelAppointment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointmentConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
