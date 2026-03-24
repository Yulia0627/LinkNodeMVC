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
    public class ProposalsController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public ProposalsController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: Proposals
        public async Task<IActionResult> Index()
        {
            var dbLinkNodeContext = _context.Proposals.Include(p => p.Freelancer).Include(p => p.Vacancy);
            return View(await dbLinkNodeContext.ToListAsync());
        }

        // GET: Proposals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals
                .Include(p => p.Freelancer)
                .ThenInclude(f => f.FreelancerNavigation)
                .Include(p => p.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        // GET: Proposals/Create
        public IActionResult Create(int vacancyId)
        {
            
                int currentFreelancerId = 1;

                var proposal = new Proposal
                {
                    VacancyId = vacancyId
                };

            //ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id");
           ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Title");
            return View(proposal);
        }

        // POST: Proposals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacancyId,FreelancerId,Price,Description,CreatedDate,Id")] Proposal proposal)
        {
            ModelState.Remove("Vacancy");
            ModelState.Remove("Freelancer");
            ModelState.Remove("CreatedDate");
            if (ModelState.IsValid)
            {
                proposal.FreelancerId = 4;
                proposal.CreatedDate = DateTime.Now;
                _context.Add(proposal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", proposal.FreelancerId);
            //ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", proposal.VacancyId);
            return View(proposal);
        }

        // GET: Proposals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", proposal.FreelancerId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", proposal.VacancyId);
            return View(proposal);
        }

        // POST: Proposals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacancyId,FreelancerId,Price,Description,CreatedDate,Id")] Proposal proposal)
        {
            if (id != proposal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proposal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProposalExists(proposal.Id))
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
            ViewData["FreelancerId"] = new SelectList(_context.Freelancers, "Id", "Id", proposal.FreelancerId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", proposal.VacancyId);
            return View(proposal);
        }

        // GET: Proposals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals
                .Include(p => p.Freelancer)
                .Include(p => p.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        // POST: Proposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                _context.Proposals.Remove(proposal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProposalExists(int id)
        {
            return _context.Proposals.Any(e => e.Id == id);
        }
    }
}
