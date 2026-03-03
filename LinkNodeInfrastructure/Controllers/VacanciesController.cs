using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkNodeDomain.Model;
using LinkNodeInfrastructure;

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
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Categories", "Index");
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = name;
            var vacancyByCategory = _context.Vacancies.Where(v => v.CategoryId == id).Include(v => v.Category).Include(v => v.Client).Include(v => v.EmpType);
            return View(await vacancyByCategory.ToListAsync());
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Vacancies/Create
        public IActionResult Create(int categoryId)
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1");
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = category.Id;
            ViewBag.CategoryName = category.Category1;
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType");
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, [Bind("ClientId,Title,EmpTypeId,CategoryId,Price,Description,CreatedDate,ClosedDate,Id")] Vacancy vacancy)
        {
            vacancy.CategoryId = categoryId;
            if (ModelState.IsValid)
            {
                vacancy.CreatedDate = DateTime.Now;
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Vacancies", new { id = categoryId, name = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Category1});
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", vacancy.CategoryId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", vacancy.ClientId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", vacancy.EmpTypeId);
            //return View(vacancy);
            return RedirectToAction("Index", "Vacancies", new { id = categoryId, name = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Category1 });
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", vacancy.ClientId);
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
