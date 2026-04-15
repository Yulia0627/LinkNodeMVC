using LinkNodeDomain.Model;
using LinkNodeInfrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkNodeInfrastructure.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly DbLinkNodeContext _context;
        private readonly UserManager<User> _userManager; 

        public VacanciesController(DbLinkNodeContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Vacancies
        public async Task<IActionResult> Index(int? categoryId, decimal? price, int? empTypeId, int? searchId)
        {
            var user = await _userManager.GetUserAsync(User);

            
            if (user != null && !user.IsActive && !User.IsInRole("admin"))
            {
                
                var emptyList = new List<Vacancy>();
 

                return View(emptyList);
            }

            var userIdString = _userManager.GetUserId(User);
            int currentUserId = 0;
            if (!string.IsNullOrEmpty(userIdString))
            {
                int.TryParse(userIdString, out currentUserId);
            }

            var query = _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.Client)
                .Include(v => v.EmpType)
                .AsQueryable();
            if (User.IsInRole("admin"))
            {
               

                
                if (searchId.HasValue)
                {
                    query = query.Where(v => v.Id == searchId.Value);
                }
            }
            else if (User.IsInRole("client"))
            {
                
                query = query.Where(v => v.ClientId == currentUserId);
            }
            else
            {

                query = query.Where(v => v.ClosedDate == null);
            }

           
            if (categoryId.HasValue) query = query.Where(v => v.CategoryId == categoryId.Value);
            if (price.HasValue) query = query.Where(v => v.Price >= price.Value);
            if (empTypeId.HasValue) query = query.Where(v => v.EmpTypeId == empTypeId.Value);

            query = query.OrderByDescending(v => v.CreatedDate);

           
            ViewBag.CurrentClientId = currentUserId;
            ViewData["CurrentPrice"] = price;
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category1", categoryId);
            ViewBag.EmpTypeId = new SelectList(_context.EmploymentTypes, "Id", "EmpType", empTypeId);

            var vacancies = await query.ToListAsync();


            if (User.IsInRole("admin"))
            {
                return View("ForAdmin", vacancies);
            }

            return View(vacancies);
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vacancy = await _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.EmpType)
                .Include(v => v.Client)
                .Include(v => v.Proposals)
                    .ThenInclude(p => p.Freelancer)
                    .ThenInclude(f => f.FreelancerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vacancy == null) return NotFound();

            return View(vacancy);
        }

        // GET: Vacancies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1");
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType");
            return View();
        }

        // POST: Vacancies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Create([Bind("Title,EmpTypeId,CategoryId,Price,Description")] Vacancy vacancy)
        {
            var userIdString = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();

            
            ModelState.Remove("ClientId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("Category");
            ModelState.Remove("Client");
            ModelState.Remove("EmpType");

            if (ModelState.IsValid)
            {
                vacancy.ClientId = int.Parse(userIdString);
                vacancy.CreatedDate = DateTime.Now;

                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null) return NotFound();

           
            var userIdString = _userManager.GetUserId(User);
            if (vacancy.ClientId.ToString() != userIdString) return Forbid();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,EmpTypeId,CategoryId,Price,Description,CreatedDate,ClientId")] Vacancy vacancy)
        {
            if (id != vacancy.Id) return NotFound();

            
            var userIdString = _userManager.GetUserId(User);
            if (vacancy.ClientId.ToString() != userIdString) return Forbid();

            ModelState.Remove("Category");
            ModelState.Remove("Client");
            ModelState.Remove("EmpType");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyExists(vacancy.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        [Authorize(Roles = "client,admin")] 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vacancy = await _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.Client)
                .Include(v => v.EmpType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vacancy == null) return NotFound();

            var userIdString = _userManager.GetUserId(User);

      
            if (!User.IsInRole("admin") && vacancy.ClientId.ToString() != userIdString)
            {
                return Forbid();
            }

            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client,admin")] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null) return NotFound();

            var userIdString = _userManager.GetUserId(User);

       
            if (!User.IsInRole("admin") && vacancy.ClientId.ToString() != userIdString)
            {
                return Forbid();
            }

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Vacancies/Close/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Close(int id)
        {
            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy == null) return NotFound();

            var userIdString = _userManager.GetUserId(User);

           
            if (vacancy.ClientId.ToString() == userIdString)
            {
                vacancy.ClosedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancies.Any(e => e.Id == id);
        }
    }
}