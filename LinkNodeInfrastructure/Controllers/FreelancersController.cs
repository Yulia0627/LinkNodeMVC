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
    public class FreelancersController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public FreelancersController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: Freelancers
        public async Task<IActionResult> Index(string search, int? categoryId, decimal? maxRate)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category1", categoryId);
            var query = _context.Freelancers
                .Include(f => f.Category)
                .Include(f => f.EmpType)
                .Include(f => f.FreelancerNavigation)
                .AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(f =>
                f.FreelancerNavigation.Name.ToLower().Contains(search.ToLower()) ||
                f.FreelancerNavigation.Surname.ToLower().Contains(search.ToLower()) ||
                (f.FreelancerNavigation.Name.ToLower() + ' ' + f.FreelancerNavigation.Surname.ToLower()).Contains(search.ToLower()));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(f => f.CategoryId == categoryId.Value);
            }
            if(maxRate.HasValue)
            {
                query = query.Where(f => f.HourlyRate <= maxRate.Value);
            }
            return View(await query.ToListAsync());
        }

        // GET: Freelancers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelancer = await _context.Freelancers
                .Include(f => f.Category)
                .Include(f => f.EmpType)
                .Include(f => f.FreelancerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freelancer == null)
            {
                return NotFound();
            }

            return View(freelancer);
        }

        // GET: Freelancers/Create
        public IActionResult Create(int id)
        {
            var freelancer = new Freelancer
            {
                Id = id
            };
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1");
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType");
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "Country");
            return View(freelancer);
        }

        // POST: Freelancers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,HourlyRate,Description,EmpTypeId,Id")] Freelancer freelancer)
        {
            ModelState.Remove("Category");
            ModelState.Remove("EmpType");
            ModelState.Remove("FreelancerNavigation");
            if (ModelState.IsValid)
            {
                _context.Add(freelancer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", freelancer.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", freelancer.EmpTypeId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", freelancer.Id);
            return View(freelancer);
        }

        // GET: Freelancers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", freelancer.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", freelancer.EmpTypeId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", freelancer.Id);
            return View(freelancer);
        }

        // POST: Freelancers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,HourlyRate,Description,EmpTypeId,Id")] Freelancer freelancer)
        {
            if (id != freelancer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(freelancer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreelancerExists(freelancer.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category1", freelancer.CategoryId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmploymentTypes, "Id", "EmpType", freelancer.EmpTypeId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", freelancer.Id);
            return View(freelancer);
        }

        // GET: Freelancers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelancer = await _context.Freelancers
                .Include(f => f.Category)
                .Include(f => f.EmpType)
                .Include(f => f.FreelancerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freelancer == null)
            {
                return NotFound();
            }

            return View(freelancer);
        }

        // POST: Freelancers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer != null)
            {
                _context.Freelancers.Remove(freelancer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreelancerExists(int id)
        {
            return _context.Freelancers.Any(e => e.Id == id);
        }
    }
}
