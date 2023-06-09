using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projekt.Data;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        // GET: Appointment/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Specializations = _context.Doctors.Select(d => d.Specialization).Distinct().ToList();
            ViewBag.Doctors = new List<SelectListItem>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    appointment.PatientId = userId;

                   

                    var selectedDoctor = _context.Doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
                    if (selectedDoctor != null)
                    {
                        _context.Add(appointment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(MyAppointments));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid doctor selected.");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }

            ViewBag.Specializations = _context.Doctors.Select(d => d.Specialization).Distinct().ToList();
            ViewBag.Doctors = new List<SelectListItem>();
            return View(appointment);
        }





        public IActionResult GetDoctors(string specialization)
        {
            var doctors = _context.Doctors.Where(d => d.Specialization == specialization).ToList();
            var doctorList = new List<SelectListItem>();

            foreach (var doctor in doctors)
            {
                doctorList.Add(new SelectListItem { Value = doctor.Id.ToString(), Text = doctor.Name });
            }

            return Json(doctorList);
        }

        public IActionResult GetSlots(int doctorId, DateTime date)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == doctorId);
            var appointments = _context.Appointments.Where(a => a.DoctorId == doctorId && a.Date.Date == date.Date).ToList();
            var slots = new List<SelectListItem>();

            // Generate time slots from 9 AM to 5 PM at 30 minutes interval
            var startTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            var endTime = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);

            while (startTime < endTime)
            {
                var slotTime = startTime.ToString("HH:mm");

                // Check if the slot is available
                if (appointments.All(a => a.Slot.ToString("HH:mm") != slotTime))
                {
                    slots.Add(new SelectListItem { Value = slotTime, Text = slotTime });
                }

                startTime = startTime.AddMinutes(30);
            }

            return Json(slots);
        }

        [Authorize]
        public IActionResult MyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = _context.Appointments.Where(a => a.PatientId == userId).ToList();
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyAppointments));
        }
    }
}
