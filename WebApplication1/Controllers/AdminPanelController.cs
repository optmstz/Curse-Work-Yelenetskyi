using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var completedCount = _context.PassportApplication.Count(pa => pa.ApplicationStatus == "Completed");
            var awaitingAcceptanceCount = _context.PassportApplication.Count(pa => pa.ApplicationStatus == "Awaiting acceptance");

            ViewData["CompletedCount"] = completedCount;
            ViewData["AwaitingAcceptanceCount"] = awaitingAcceptanceCount;

            return View();
        }

    }

}
