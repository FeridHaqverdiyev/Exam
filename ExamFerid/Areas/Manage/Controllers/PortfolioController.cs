using ExamFerid.Areas.Manage.ViewModels;
using ExamFerid.DAL;
using ExamFerid.Models;
using ExamFerid.Utilities.Existensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamFerid.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PortfolioController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
           _environment = environment;
        }
        public async Task< IActionResult> Index()
        {
            List<Portfolio> portfolios = await _context.portfolios.ToListAsync();
            return View(portfolios);
        }
        public  IActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateVM createVM)
        {
         
            if (!ModelState.IsValid)
            {
                return View(createVM);
            }

            bool result = await _context.portfolios.AnyAsync(c => c.Name == createVM.Name);
            if (result)
            {
                ModelState.AddModelError("Name", "ad eynidi");
                return View(createVM);
            }
            if (!createVM.Image.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Image", "seklin olcusu sefdir");
                return View(createVM);
            }

            if (!createVM.Image.ValidateType("image/"))
            {
                ModelState.AddModelError("Image", " seklin Type sefdir");
                return View(createVM);
            }
            string filname = await createVM.Image.CreateFile(_environment.WebRootPath, "assets", "img","portfolio");

            Portfolio portfolio = new Portfolio
            {
               Image= filname,
               Name = createVM.Name,
               Decs=createVM.Description
               
            };
            await _context.portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Portfolio");
        }
        public async Task<IActionResult>Update(int Id)
        {
            if (Id <= 0) return BadRequest();
            Portfolio portfolio = await _context.portfolios.FirstOrDefaultAsync(f => f.Id == Id);
            if (portfolio == null) return NotFound();
            UpdateVM updateVM = new UpdateVM
            {
                Name = portfolio.Name,
                Description =portfolio.Decs,
                ImageUrl=portfolio.Image
              
            };
            return View(updateVM);


        }
        [HttpPost]
        public  async Task<IActionResult>Update(int Id,UpdateVM updateVM)
        {
            if (Id <= 0) return BadRequest();
            Portfolio portfolio = await _context.portfolios.FirstOrDefaultAsync(p => p.Id == Id);
            if (portfolio == null) return NotFound();
            if (updateVM.Image != null)
            {
                if (!updateVM.Image.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Image", " seklin size sefdir");
                    return View(updateVM);
                }
                if (!updateVM.Image.ValidateType("image/*"))
                {
                    ModelState.AddModelError("Image", " seklin type sefdir");
                    return View(updateVM);

                }
                portfolio.Image.DeleteFile(_environment.WebRootPath, "assets", "img", "team");
                string fileName = await updateVM.Image.CreateFile(_environment.WebRootPath, "assets", "img", "portfolio");
                portfolio.Image = fileName;

            }
            portfolio.Name = portfolio.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Portfolio");
        }
        public async Task<IActionResult>Delete(int Id)
        {
            if (Id <= 0) return BadRequest();
            Portfolio portfolio = await _context.portfolios.FirstOrDefaultAsync(p => p.Id == Id);
            if (portfolio == null) return NotFound();
            portfolio.Image.DeleteFile(_environment.WebRootPath, "assets", "img", "portfolio");
            _context.portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Portfolio");

        }
        

    }
}
