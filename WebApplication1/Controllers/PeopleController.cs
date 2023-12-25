using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using X.PagedList;


namespace WebApplication1.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        // GET: People
        public async Task<IActionResult> Index(int? page, string sortOrder, string searchString)
        {
            const int pageSize = 10;

            var applicationDbContext = _context.Person.Include(p => p.BirthPlace).Include(p => p.RegistrationPlace).AsQueryable();

            ViewData["DefaultSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "surname" ? "surname_desc" : "surname";
            ViewData["DateOfBirthSortParm"] = sortOrder == "DateOfBirth" ? "DateOfBirth_desc" : "DateOfBirth";

            ViewData["CurrentSort"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(p => p.PersonSurname.Contains(searchString) || p.PersonName.Contains(searchString));
            }

            var people = from p in _context.Person
                         select p;

            switch (sortOrder)
            {
                case "name_desc":
                    people = people.OrderByDescending(p => p.PersonName);
                    break;
                case "Surname":
                    people = people.OrderBy(p => p.PersonSurname);
                    break;
                case "surname_desc":
                    people = people.OrderByDescending(p => p.PersonSurname);
                    break;
                case "DateOfBirth":
                    people = people.OrderBy(p => p.PersonDateOfBirth);
                    break;
                case "dateOfBirth_desc":
                    people = people.OrderByDescending(p => p.PersonDateOfBirth);
                    break;
                // Add more cases for other fields
                default:
                    people = people.OrderBy(p => p.PersonName);
                    break;
            }

            int pageNumber = page ?? 1;
            var pagedModel = await applicationDbContext.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedModel);
        }



        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.RegistrationPlace)
                .Include(p => p.BirthPlace)
                .Include(p => p.PersonPassport)
                .FirstOrDefaultAsync(m => m.PersonId == id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,PersonName,PersonSurname,PersonSex,PersonDateOfBirth,PersonNationality,RNKNumber,RegistrationPlaceId,BirthPlaceId,PersonPassportId")] Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (person.RegistrationPlace == null)
                    {
                        person.RegistrationPlace = new RegistrationPlaces();
                        _context.RegistrationPlaces.Add(person.RegistrationPlace);
                        await _context.SaveChangesAsync();
                    }

                    // Check if the person has a birth place
                    if (person.BirthPlace == null)
                    {
                        person.BirthPlace = new PlacesOfBirth();
                        _context.PlacesOfBirth.Add(person.BirthPlace);
                        await _context.SaveChangesAsync();
                    }

               
                    _context.Person.Add(person);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating the record. Please contact the administrator.";
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View("Create", person);
        }



        // POST: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var person = await _context.Person.FindAsync(id);
                if (person == null)
                {
                    return NotFound();
                }

                person.BirthPlace = await _context.PlacesOfBirth.FindAsync(person.BirthPlaceId);
                person.RegistrationPlace = await _context.RegistrationPlaces.FindAsync(person.RegistrationPlaceId);

                ViewData["BirthPlaceId"] = new SelectList(_context.PlacesOfBirth, "BirthPlaceId", "BirthPlaceId", person.BirthPlaceId);
                ViewData["RegistrationPlaceId"] = new SelectList(_context.RegistrationPlaces, "RegistrationPlaceId", "RegistrationPlaceId", person.RegistrationPlaceId);
                return View(person);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to fetch the person for editing. Please try again.");

                return RedirectToAction();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var person = await _context.Person
                    .Include(p => p.BirthPlace)
                    .Include(p => p.RegistrationPlace)
                    .FirstOrDefaultAsync(m => m.PersonId == id);

                if (person == null)
                {
                    return NotFound();
                }

                if (await TryUpdateModelAsync(person, "",
                    p => p.PersonName, p => p.PersonSurname, p => p.PersonSex,
                    p => p.PersonDateOfBirth, p => p.PersonNationality, p => p.RNKNumber,
                    p => p.RegistrationPlace, p => p.BirthPlace, p => p.PersonPassportId))
                {
                    // Оновлення пов'язаних об'єктів
                    _context.RegistrationPlaces.Update(person.RegistrationPlace);
                    _context.PlacesOfBirth.Update(person.BirthPlace);

                    // Оновлення основного об'єкта
                    _context.Person.Update(person);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                ViewData["BirthPlaceId"] = new SelectList(_context.PlacesOfBirth, "BirthPlaceId", "City", person.BirthPlaceId);
                ViewData["RegistrationPlaceId"] = new SelectList(_context.RegistrationPlaces, "RegistrationPlaceId", "City", person.RegistrationPlaceId);
                return View(person);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to update the person. Please try again.");

                return RedirectToAction();
            }
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.BirthPlace)
                .Include(p => p.RegistrationPlace)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var person = await _context.Person
                    .Include(p => p.PersonPassport) 
                    .FirstOrDefaultAsync(m => m.PersonId == id);

                if (person == null)
                {
                    return NotFound();
                }

                if (person.PersonPassport != null)
                {
                    _context.PersonPassports.Remove(person.PersonPassport);
                }

                _context.Person.Remove(person);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while deleting the person";
                return RedirectToAction(nameof(Index));
            }
        }



        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}
