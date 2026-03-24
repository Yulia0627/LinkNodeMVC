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
    public class IntroStatusController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public IntroStatusController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: IntroStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.IntroStatuses.ToListAsync());
        }

        // GET: IntroStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introStatus = await _context.IntroStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (introStatus == null)
            {
                return NotFound();
            }

            return View(introStatus);
        }

        // GET: IntroStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IntroStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Status,Id")] IntroStatus introStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(introStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(introStatus);
        }

        // GET: IntroStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introStatus = await _context.IntroStatuses.FindAsync(id);
            if (introStatus == null)
            {
                return NotFound();
            }
            return View(introStatus);
        }

        // POST: IntroStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Status,Id")] IntroStatus introStatus)
        {
            if (id != introStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(introStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntroStatusExists(introStatus.Id))
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
            return View(introStatus);
        }

        // GET: IntroStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introStatus = await _context.IntroStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (introStatus == null)
            {
                return NotFound();
            }

            return View(introStatus);
        }

        // POST: IntroStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var introStatus = await _context.IntroStatuses.FindAsync(id);
            if (introStatus != null)
            {
                _context.IntroStatuses.Remove(introStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntroStatusExists(int id)
        {
            return _context.IntroStatuses.Any(e => e.Id == id);
        }
    }
}
