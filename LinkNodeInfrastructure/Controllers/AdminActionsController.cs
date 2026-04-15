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
    public class AdminActionsController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public AdminActionsController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: AdminActions
        public async Task<IActionResult> Index()
        {
            var dbLinkNodeContext = _context.AdminActions.Include(a => a.Action).Include(a => a.Admin).Include(a => a.TargetUser).Include(a => a.TargetVacancy);
            return View(await dbLinkNodeContext.ToListAsync());
        }

        // GET: AdminActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminAction = await _context.AdminActions
                .Include(a => a.Action)
                .Include(a => a.Admin)
                .Include(a => a.TargetUser)
                .Include(a => a.TargetVacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminAction == null)
            {
                return NotFound();
            }

            return View(adminAction);
        }

        // GET: AdminActions/Create
        public IActionResult Create()
        {
            ViewData["ActionId"] = new SelectList(_context.ActionTypes, "Id", "Action");
            ViewData["AdminId"] = new SelectList(_context.Users, "Id", "Country");
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Country");
            ViewData["TargetVacancyId"] = new SelectList(_context.Vacancies, "Id", "Description");
            return View();
        }

        // POST: AdminActions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,ActionId,TargetUserId,TargetVacancyId,Description,CreatedDate,Id")] AdminAction adminAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionId"] = new SelectList(_context.ActionTypes, "Id", "Action", adminAction.ActionId);
            ViewData["AdminId"] = new SelectList(_context.Users, "Id", "Country", adminAction.AdminId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Country", adminAction.TargetUserId);
            ViewData["TargetVacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", adminAction.TargetVacancyId);
            return View(adminAction);
        }

        // GET: AdminActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminAction = await _context.AdminActions.FindAsync(id);
            if (adminAction == null)
            {
                return NotFound();
            }
            ViewData["ActionId"] = new SelectList(_context.ActionTypes, "Id", "Action", adminAction.ActionId);
            ViewData["AdminId"] = new SelectList(_context.Users, "Id", "Country", adminAction.AdminId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Country", adminAction.TargetUserId);
            ViewData["TargetVacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", adminAction.TargetVacancyId);
            return View(adminAction);
        }

        // POST: AdminActions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,ActionId,TargetUserId,TargetVacancyId,Description,CreatedDate,Id")] AdminAction adminAction)
        {
            if (id != adminAction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminActionExists(adminAction.Id))
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
            ViewData["ActionId"] = new SelectList(_context.ActionTypes, "Id", "Action", adminAction.ActionId);
            ViewData["AdminId"] = new SelectList(_context.Users, "Id", "Country", adminAction.AdminId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Country", adminAction.TargetUserId);
            ViewData["TargetVacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", adminAction.TargetVacancyId);
            return View(adminAction);
        }

        // GET: AdminActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminAction = await _context.AdminActions
                .Include(a => a.Action)
                .Include(a => a.Admin)
                .Include(a => a.TargetUser)
                .Include(a => a.TargetVacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminAction == null)
            {
                return NotFound();
            }

            return View(adminAction);
        }

        // POST: AdminActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminAction = await _context.AdminActions.FindAsync(id);
            if (adminAction != null)
            {
                _context.AdminActions.Remove(adminAction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminActionExists(int id)
        {
            return _context.AdminActions.Any(e => e.Id == id);
        }
    }
}
