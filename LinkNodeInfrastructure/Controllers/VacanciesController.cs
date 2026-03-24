using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkNodeDomain.Model;
using LinkNodeInfrastructure;
using Microsoft.AspNetCore.Identity;

namespace LinkNodeInfrastructure.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public VacanciesController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: Vacancies
        public async Task<IActionResult> Index(int? categoryId, decimal? price, int? empTypeId)
        {
            var currentUserId = 9;
            //ViewData["CurrentCategory"] = categoryId;
            ViewData["CurrentPrice"] = price;
            //ViewData["CurrentEmpType"] = empTypeId;
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category1", categoryId);
            ViewBag.EmpTypeId = new SelectList(_context.EmploymentTypes, "Id", "EmpType", empTypeId);
            var query = _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.Client)
                .Include(v => v.EmpType)
                .AsQueryable();
                
           if (currentUserId == 9)
           {

              query = query.Where(v => v.ClosedDate == null);
           }
            if (categoryId.HasValue)
            {
                query = query.Where(v => v.CategoryId == categoryId.Value);
            }
            if (price.HasValue)
            {
            query = query.Where(v => v.Price >= price.Value);
            }
            if (empTypeId.HasValue)
            {
                query = query.Where(v => v.EmpTypeId == empTypeId.Value);
            }
            query = query.Where(v => v.ClosedDate == null);
            query = query.OrderByDescending(v => v.CreatedDate);
            return View(await query.ToListAsync());
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .Include(v => v.Proposals)
                .ThenInclude(p=>p.Freelancer)
                .ThenInclude(f=>f.FreelancerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // GET: Vacancies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType");
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,Title,EmpTypeId,CategoryId,Price,Description,CreatedDate,ClosedDate,Id")] Vacancy vacancy)
        {
            ModelState.Remove("ClientId");
            ModelState.Remove("EmpTypeId");
            ModelState.Remove("CategoryId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ClosedDate");
            ModelState.Remove("Category");
            ModelState.Remove("Client");
            ModelState.Remove("EmpType");

            if (ModelState.IsValid)
            {
                vacancy.ClientId = 7;
                vacancy.CreatedDate = DateTime.Now;
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
           // ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", vacancy.ClientId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Title,EmpTypeId,CategoryId,Price,Description,CreatedDate,ClosedDate,Id")] Vacancy vacancy)
        {
            if (id != vacancy.Id)
            {
                return NotFound();
            }
            ModelState.Remove("ClientId");
            ModelState.Remove("EmpTypeId");
            ModelState.Remove("CategoryId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ClosedDate");
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
                    if (!VacancyExists(vacancy.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", vacancy.ClientId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.Client)
                .Include(v => v.EmpType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy != null)
            {
                _context.Vacancies.Remove(vacancy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancies.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> Close(int id)
        {
            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);

            if (vacancy != null)
            {
                vacancy.ClosedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
