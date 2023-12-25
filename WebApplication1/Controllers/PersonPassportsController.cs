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
    public class PersonPassportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonPassportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PersonPassports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PersonPassports.Include(p => p.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PersonPassports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personPassports = await _context.PersonPassports
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.PersonPassportId == id);
            if (personPassports == null)
            {
                return NotFound();
            }

            return View(personPassports);
        }

        // GET: PersonPassports/Create
        public IActionResult Create(int? personId, string returnUrl)
        {
            ViewData["PersonId"] = personId;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonPassportId,Passport_Number,Passport_Type,Authority,Date_of_Issue,Date_of_Expiry,PersonId")] PersonPassports personPassports, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personPassports);
                await _context.SaveChangesAsync();

                int createdPassportId = personPassports.PersonPassportId ?? 0;

                var person = await _context.Person.FindAsync(personPassports.PersonId);

                if (person == null)
                {
                    ModelState.AddModelError("PersonId", "Invalid person ID.");
                    ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personPassports.PersonId);
                    return View(personPassports);
                }

                person.PersonPassportId = createdPassportId;
                _context.Update(person);
                await _context.SaveChangesAsync();

                return Redirect(returnUrl);
            }

            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personPassports.PersonId);
            return View(personPassports);
        }


        // GET: PersonPassports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personPassports = await _context.PersonPassports.FindAsync(id);
            if (personPassports == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personPassports.PersonId);
            return View(personPassports);
        }

        // POST: PersonPassports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("PersonPassportId,Passport_Number,Passport_Type,Authority,Date_of_Issue,Date_of_Expiry,PersonId")] PersonPassports personPassports)
        {
            if (id != personPassports.PersonPassportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personPassports);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonPassportsExists(personPassports.PersonPassportId))
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
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personPassports.PersonId);
            return View(personPassports);
        }

        // GET: PersonPassports/Delete/5
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personPassports = await _context.PersonPassports
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.PersonPassportId == id);

            if (personPassports == null)
            {
                return NotFound();
            }

            ViewData["PersonId"] = personPassports.PersonId;
            ViewData["ReturnUrl"] = returnUrl;
            return View(personPassports);
        }



        // POST: PersonPassports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, string returnUrl)
        {
            var personPassports = await _context.PersonPassports.FindAsync(id);
            var person = await _context.Person.FindAsync(personPassports.PersonId);
            person.PersonPassportId = null;

            if (personPassports != null)
            {
                _context.PersonPassports.Remove(personPassports);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Details", "People", new { id = personPassports.PersonId });
            }
        }




        private bool PersonPassportsExists(int? id)
        {
            return _context.PersonPassports.Any(e => e.PersonPassportId == id);
        }
    }
}
