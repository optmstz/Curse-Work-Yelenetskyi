using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PassportServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassportServicesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="Admin")]
        // GET: PassportServices
        public async Task<IActionResult> Index()
        {
            return View(await _context.PassportServices.ToListAsync());
        }

        // GET: PassportServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportServices = await _context.PassportServices
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (passportServices == null)
            {
                return NotFound();
            }

            return View(passportServices);
        }

        // GET: PassportServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PassportServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceType,ServicePrice,ExecutionDate")] PassportServices passportServices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passportServices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passportServices);
        }

        // GET: PassportServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportServices = await _context.PassportServices.FindAsync(id);
            if (passportServices == null)
            {
                return NotFound();
            }
            return View(passportServices);
        }

        // POST: PassportServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceType,ServicePrice,ExecutionDate")] PassportServices passportServices)
        {
            if (id != passportServices.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passportServices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportServicesExists(passportServices.ServiceId))
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
            return View(passportServices);
        }

        // GET: PassportServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passportServices = await _context.PassportServices
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (passportServices == null)
            {
                return NotFound();
            }

            return View(passportServices);
        }

        // POST: PassportServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passportServices = await _context.PassportServices.FindAsync(id);
            if (passportServices != null)
            {
                _context.PassportServices.Remove(passportServices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassportServicesExists(int id)
        {
            return _context.PassportServices.Any(e => e.ServiceId == id);
        }
    }
}
