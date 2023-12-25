using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PassportApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public PassportApplicationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PassportApplications
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PassportApplication.Include(p => p.Employees).Include(p => p.PassportServices).Include(p => p.PersonPassport);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PassportApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportApplication = await _context.PassportApplication
                .Include(p => p.Employees)
                .Include(p => p.PassportServices)
                .Include(p => p.PersonPassport)
                .FirstOrDefaultAsync(m => m.PassportApplicationId == id);
            if (passportApplication == null)
            {
                return NotFound();
            }

            return View(passportApplication);
        }

        // GET: PassportApplications/Create
        public IActionResult Create()
        {
            ViewData["EmployeesId"] = new SelectList(_context.Employees, "EmployeesId", "EmployeesId");
            ViewData["ServiceTypes"] = new SelectList(_context.PassportServices, "ServiceId", "ServiceType");
            ViewData["PersonPassportId"] = new SelectList(_context.PersonPassports, "PersonPassportId", "Passport_Number");

            var currentDate = DateTime.Now;
            ViewData["CurrentDate"] = currentDate.ToString("yyyy-MM-dd"); 

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PassportApplicationId,DesiredPassportType,DateOfSubmission,DateOfExecution,EmployeesId,ServiceId,PersonPassportId")] PassportApplication passportApplication)
        {
            if (ModelState.IsValid)
            {
                passportApplication.DateOfSubmission = DateTime.Now;

                passportApplication.ApplicationStatus = "Awaiting acceptance";

                var selectedService = _context.PassportServices.Find(passportApplication.ServiceId);
                if (selectedService != null)
                {
                    var currentDate = DateTime.Now;
                    passportApplication.DateOfExecution = currentDate.AddDays(selectedService.ExecutionDate);
                }

                _context.Add(passportApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeesId"] = new SelectList(_context.Employees, "EmployeesId", "EmployeesId", passportApplication.EmployeesId);
            ViewData["ServiceId"] = new SelectList(_context.PassportServices, "ServiceId", "ServiceId", passportApplication.ServiceId);
            ViewData["PersonPassportId"] = new SelectList(_context.PersonPassports, "PersonPassportId", "PersonPassportId", passportApplication.PersonPassportId);
            return View(passportApplication);
        }




        // GET: PassportApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportApplication = await _context.PassportApplication.FindAsync(id);
            if (passportApplication == null)
            {
                return NotFound();
            }
            ViewData["EmployeesId"] = new SelectList(_context.Employees, "EmployeesId", "EmployeesId", passportApplication.EmployeesId);
            ViewData["ServiceId"] = new SelectList(_context.PassportServices, "ServiceId", "ServiceId", passportApplication.ServiceId);
            ViewData["PersonPassportId"] = new SelectList(_context.PersonPassports, "PersonPassportId", "PersonPassportId", passportApplication.PersonPassportId);
            return View(passportApplication);
        }

        // POST: PassportApplications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PassportApplicationId,DesiredPassportType,DateOfSubmission,DateOfExecution,EmployeesId,ServiceId,PersonPassportId")] PassportApplication passportApplication)
        {
            if (id != passportApplication.PassportApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passportApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportApplicationExists(passportApplication.PassportApplicationId))
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
            ViewData["EmployeesId"] = new SelectList(_context.Employees, "EmployeesId", "EmployeesId", passportApplication.EmployeesId);
            ViewData["ServiceId"] = new SelectList(_context.PassportServices, "ServiceId", "ServiceId", passportApplication.ServiceId);
            ViewData["PersonPassportId"] = new SelectList(_context.PersonPassports, "PersonPassportId", "PersonPassportId", passportApplication.PersonPassportId);
            return View(passportApplication);
        }

        // GET: PassportApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportApplication = await _context.PassportApplication
                .Include(p => p.Employees)
                .Include(p => p.PassportServices)
                .Include(p => p.PersonPassport)
                .FirstOrDefaultAsync(m => m.PassportApplicationId == id);
            if (passportApplication == null)
            {
                return NotFound();
            }

            return View(passportApplication);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportApplication = await _context.PassportApplication.FindAsync(id);
            if (passportApplication == null)
            {
                return NotFound();
            }

            if (passportApplication.ApplicationStatus == "Awaiting acceptance")
            {
                passportApplication.ApplicationStatus = "In progress";
                passportApplication.EmployeesId = GetCurrentEmployeeId(); 

                try
                {
                    _context.Update(passportApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportApplicationExists(passportApplication.PassportApplicationId))
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
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Done(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportApplication = await _context.PassportApplication.FindAsync(id);
            if (passportApplication == null)
            {
                return NotFound();
            }

            if (passportApplication.ApplicationStatus == "In progress")
            {
                passportApplication.ApplicationStatus = "Completed";

                try
                {
                    _context.Update(passportApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportApplicationExists(passportApplication.PassportApplicationId))
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
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private int GetCurrentEmployeeId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employee = _context.Employees.SingleOrDefault(e => e.UserId == userId);

            if (employee != null)
            {
                return employee.EmployeesId;
            }
            else
            {
                return -1;
            }
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passportApplication = await _context.PassportApplication.FindAsync(id);
            if (passportApplication != null)
            {
                _context.PassportApplication.Remove(passportApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassportApplicationExists(int id)
        {
            return _context.PassportApplication.Any(e => e.PassportApplicationId == id);
        }
    }
}
