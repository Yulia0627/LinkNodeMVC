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
    public class InvitesController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public InvitesController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: Invites
        public async Task<IActionResult> Index()
        {
            var dbLinkNodeContext = _context.Invites.Include(i => i.Freelancer).Include(i => i.Status).Include(i => i.Vacancy);
            return View(await dbLinkNodeContext.ToListAsync());
        }

        // GET: Invites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invite = await _context.Invites
                .Include(i => i.Freelancer)
                .Include(i => i.Status)
                .Include(i => i.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invite == null)
            {
                return NotFound();
            }

            return View(invite);
        }

        // GET: Invites/Create
        public IActionResult Create()
        {
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.InviteStatuses, "Id", "InviteStatus1");
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description");
            return View();
        }

        // POST: Invites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FreelancerId,StatusId,VacancyId,Id")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", invite.FreelancerId);
            ViewData["StatusId"] = new SelectList(_context.InviteStatuses, "Id", "InviteStatus1", invite.StatusId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", invite.VacancyId);
            return View(invite);
        }

        // GET: Invites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invite = await _context.Invites.FindAsync(id);
            if (invite == null)
            {
                return NotFound();
            }
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", invite.FreelancerId);
            ViewData["StatusId"] = new SelectList(_context.InviteStatuses, "Id", "InviteStatus1", invite.StatusId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", invite.VacancyId);
            return View(invite);
        }

        // POST: Invites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FreelancerId,StatusId,VacancyId,Id")] Invite invite)
        {
            if (id != invite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InviteExists(invite.Id))
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
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", invite.FreelancerId);
            ViewData["StatusId"] = new SelectList(_context.InviteStatuses, "Id", "InviteStatus1", invite.StatusId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", invite.VacancyId);
            return View(invite);
        }

        // GET: Invites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invite = await _context.Invites
                .Include(i => i.Freelancer)
                .Include(i => i.Status)
                .Include(i => i.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invite == null)
            {
                return NotFound();
            }

            return View(invite);
        }

        // POST: Invites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invite = await _context.Invites.FindAsync(id);
            if (invite != null)
            {
                _context.Invites.Remove(invite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InviteExists(int id)
        {
            return _context.Invites.Any(e => e.Id == id);
        }
    }
}
