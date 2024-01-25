
using ExamFerid.DAL;
using ExamFerid.Models;
using ExamFerid.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExamFerid.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Portfolio> portfolios = await _context.portfolios.ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Portfolios = portfolios,
            };
            return View(homeVM);
        }


    }
}