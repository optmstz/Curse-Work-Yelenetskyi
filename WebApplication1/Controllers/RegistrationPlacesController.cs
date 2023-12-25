using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegistrationPlacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationPlacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RegistrationPlaces
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistrationPlaces.ToListAsync());
        }

        // GET: RegistrationPlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationPlaces = await _context.RegistrationPlaces
                .FirstOrDefaultAsync(m => m.RegistrationPlaceId == id);
            if (registrationPlaces == null)
            {
                return NotFound();
            }

            return View(registrationPlaces);
        }

        // GET: RegistrationPlaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistrationPlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationPlaceId,Country,Region,City")] RegistrationPlaces registrationPlaces)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrationPlaces);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registrationPlaces);
        }

        // GET: RegistrationPlaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationPlaces = await _context.RegistrationPlaces.FindAsync(id);
            if (registrationPlaces == null)
            {
                return NotFound();
            }
            return View(registrationPlaces);
        }

        // POST: RegistrationPlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationPlaceId,Country,Region,City")] RegistrationPlaces registrationPlaces)
        {
            if (id != registrationPlaces.RegistrationPlaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationPlaces);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationPlacesExists(registrationPlaces.RegistrationPlaceId))
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
            return View(registrationPlaces);
        }

        // GET: RegistrationPlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationPlaces = await _context.RegistrationPlaces
                .FirstOrDefaultAsync(m => m.RegistrationPlaceId == id);
            if (registrationPlaces == null)
            {
                return NotFound();
            }

            return View(registrationPlaces);
        }

        // POST: RegistrationPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registrationPlaces = await _context.RegistrationPlaces.FindAsync(id);
            if (registrationPlaces != null)
            {
                _context.RegistrationPlaces.Remove(registrationPlaces);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationPlacesExists(int id)
        {
            return _context.RegistrationPlaces.Any(e => e.RegistrationPlaceId == id);
        }
    }
}
