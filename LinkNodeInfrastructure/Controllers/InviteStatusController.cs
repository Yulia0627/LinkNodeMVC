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
    public class InviteStatusController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public InviteStatusController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: InviteStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.InviteStatuses.ToListAsync());
        }

        // GET: InviteStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inviteStatus = await _context.InviteStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inviteStatus == null)
            {
                return NotFound();
            }

            return View(inviteStatus);
        }

        // GET: InviteStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InviteStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InviteStatus1,Id")] InviteStatus inviteStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inviteStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inviteStatus);
        }

        // GET: InviteStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inviteStatus = await _context.InviteStatuses.FindAsync(id);
            if (inviteStatus == null)
            {
                return NotFound();
            }
            return View(inviteStatus);
        }

        // POST: InviteStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InviteStatus1,Id")] InviteStatus inviteStatus)
        {
            if (id != inviteStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inviteStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InviteStatusExists(inviteStatus.Id))
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
            return View(inviteStatus);
        }

        // GET: InviteStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inviteStatus = await _context.InviteStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inviteStatus == null)
            {
                return NotFound();
            }

            return View(inviteStatus);
        }

        // POST: InviteStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inviteStatus = await _context.InviteStatuses.FindAsync(id);
            if (inviteStatus != null)
            {
                _context.InviteStatuses.Remove(inviteStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InviteStatusExists(int id)
        {
            return _context.InviteStatuses.Any(e => e.Id == id);
        }
    }
}
