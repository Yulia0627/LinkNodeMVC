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
    public class EmploymentTypesController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public EmploymentTypesController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: EmploymentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmploymentTypes.ToListAsync());
        }

        // GET: EmploymentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentType = await _context.EmploymentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employmentType == null)
            {
                return NotFound();
            }

            return View(employmentType);
        }

        // GET: EmploymentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmploymentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpType,Id")] EmploymentType employmentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employmentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employmentType);
        }

        // GET: EmploymentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentType = await _context.EmploymentTypes.FindAsync(id);
            if (employmentType == null)
            {
                return NotFound();
            }
            return View(employmentType);
        }

        // POST: EmploymentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpType,Id")] EmploymentType employmentType)
        {
            if (id != employmentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employmentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploymentTypeExists(employmentType.Id))
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
            return View(employmentType);
        }

        // GET: EmploymentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentType = await _context.EmploymentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employmentType == null)
            {
                return NotFound();
            }

            return View(employmentType);
        }

        // POST: EmploymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employmentType = await _context.EmploymentTypes.FindAsync(id);
            if (employmentType != null)
            {
                _context.EmploymentTypes.Remove(employmentType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmploymentTypeExists(int id)
        {
            return _context.EmploymentTypes.Any(e => e.Id == id);
        }
    }
}
