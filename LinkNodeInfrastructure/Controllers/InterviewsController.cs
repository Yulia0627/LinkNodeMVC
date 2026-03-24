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
    public class InterviewsController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public InterviewsController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: Interviews
        public async Task<IActionResult> Index()
        {
            var dbLinkNodeContext = _context.Interviews.Include(i => i.IntroStatus).Include(i => i.Prop);
            return View(await dbLinkNodeContext.ToListAsync());
        }

        // GET: Interviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews
         .Include(i => i.IntroStatus)
         .Include(i => i.Prop)
         .Include(i => i.CallStatusStories)
             .ThenInclude(s => s.OldStatus)
         .Include(i => i.CallStatusStories)
             .ThenInclude(s => s.NewStatus)
         .FirstOrDefaultAsync(m => m.Id == id);
            if (interview == null)
            {
                return NotFound();
            }

            return View(interview);
        }

        // GET: Interviews/Create
        public IActionResult Create(int proposalId)
        {
            DateTime tomorrow = DateTime.Today.AddDays(1).AddHours(12);
            var interview = new Interview
            {
                PropId = proposalId,
                DateTime = tomorrow
            };
            ViewData["IntroStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status");
            ViewData["PropId"] = new SelectList(_context.Proposals, "Id", "Description");
            ModelState.Clear();
            return View(interview);
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropId,DateTime,Reference,InterviewRound,IntroStatusId,CreatedDate,Id")] Interview interview)
        {
            ModelState.Remove("Prop");
            ModelState.Remove("IntroStatus");
            ModelState.Remove("CreatedDate");

            if (ModelState.IsValid)
            {
                interview.IntroStatusId = 1;
                interview.CreatedDate = DateTime.Now;
                _context.Add(interview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IntroStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", interview.IntroStatusId);
            ViewData["PropId"] = new SelectList(_context.Proposals, "Id", "Description", interview.PropId);
            return View(interview);
        }

        // GET: Interviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null)
            {
                return NotFound();
            }
            ViewData["IntroStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", interview.IntroStatusId);
            ViewData["PropId"] = new SelectList(_context.Proposals, "Id", "Description", interview.PropId);
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropId,DateTime,Reference,InterviewRound,IntroStatusId,CreatedDate,Id")] Interview interview)
        {
            ModelState.Remove("Prop");
            ModelState.Remove("IntroStatus");

            if (id != interview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldInterview = await _context.Interviews
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

                    if (oldInterview != null)
                    {
                        int changedStatusId = 4;
                        if (interview.IntroStatusId != changedStatusId)
                        {
                            var story = new CallStatusStory
                            {
                                IntroId = id,
                                OldStatusId = oldInterview.IntroStatusId,
                                NewStatusId = changedStatusId,
                                ChangedDate = DateTime.Now
                            };
                            _context.CallStatusStories.Add(story);
                            interview.IntroStatusId = changedStatusId;
                        }
                    }
                    _context.Update(interview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterviewExists(interview.Id))
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
            ViewData["IntroStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", interview.IntroStatusId);
            ViewData["PropId"] = new SelectList(_context.Proposals, "Id", "Description", interview.PropId);
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews
                .Include(i => i.IntroStatus)
                .Include(i => i.Prop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interview == null)
            {
                return NotFound();
            }

            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interview = await _context.Interviews.FindAsync(id);
            if (interview != null)
            {
                int cancelledStatusId = 5;
                var story = new CallStatusStory
                {
                    IntroId = interview.Id,
                    OldStatusId = interview.IntroStatusId,
                    NewStatusId = cancelledStatusId,
                    ChangedDate = DateTime.Now
                };
                _context.CallStatusStories.Add(story);
                interview.IntroStatusId = cancelledStatusId;
                _context.Update(interview);
                await _context.SaveChangesAsync();
                

            }
            return RedirectToAction(nameof(Index));
        }
   

        private bool InterviewExists(int id)
        {
            return _context.Interviews.Any(e => e.Id == id);
        }
    }
}
