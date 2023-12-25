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
    public class PlacesOfBirthsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacesOfBirthsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlacesOfBirths
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlacesOfBirth.ToListAsync());
        }

        // GET: PlacesOfBirths/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placesOfBirth = await _context.PlacesOfBirth
                .FirstOrDefaultAsync(m => m.BirthPlaceId == id);
            if (placesOfBirth == null)
            {
                return NotFound();
            }

            return View(placesOfBirth);
        }

        // GET: PlacesOfBirths/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlacesOfBirths/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BirthPlaceId,Country,Region,City")] PlacesOfBirth placesOfBirth)
        {
            if (ModelState.IsValid)
            {
                _context.Add(placesOfBirth);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(placesOfBirth);
        }

        // GET: PlacesOfBirths/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placesOfBirth = await _context.PlacesOfBirth.FindAsync(id);
            if (placesOfBirth == null)
            {
                return NotFound();
            }
            return View(placesOfBirth);
        }

        // POST: PlacesOfBirths/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BirthPlaceId,Country,Region,City")] PlacesOfBirth placesOfBirth)
        {
            if (id != placesOfBirth.BirthPlaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placesOfBirth);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlacesOfBirthExists(placesOfBirth.BirthPlaceId))
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
            return View(placesOfBirth);
        }

        // GET: PlacesOfBirths/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placesOfBirth = await _context.PlacesOfBirth
                .FirstOrDefaultAsync(m => m.BirthPlaceId == id);
            if (placesOfBirth == null)
            {
                return NotFound();
            }

            return View(placesOfBirth);
        }

        // POST: PlacesOfBirths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placesOfBirth = await _context.PlacesOfBirth.FindAsync(id);
            if (placesOfBirth != null)
            {
                _context.PlacesOfBirth.Remove(placesOfBirth);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlacesOfBirthExists(int id)
        {
            return _context.PlacesOfBirth.Any(e => e.BirthPlaceId == id);
        }
    }
}
