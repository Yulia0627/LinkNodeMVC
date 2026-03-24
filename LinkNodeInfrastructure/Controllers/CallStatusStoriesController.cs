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
    public class CallStatusStoriesController : Controller
    {
        private readonly DbLinkNodeContext _context;

        public CallStatusStoriesController(DbLinkNodeContext context)
        {
            _context = context;
        }

        // GET: CallStatusStories
        public async Task<IActionResult> Index()
        {
            var stories = _context.CallStatusStories
                .Include(c => c.Intro)
                .Include(c => c.NewStatus)
                .Include(c => c.OldStatus)
                .OrderByDescending(c => c.ChangedDate)
                .ToListAsync();
            return View(stories);
        }

        // GET: CallStatusStories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var callStatusStory = await _context.CallStatusStories
                .Include(c => c.Intro)
                .Include(c => c.NewStatus)
                .Include(c => c.OldStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (callStatusStory == null)
            {
                return NotFound();
            }

            return View(callStatusStory);
        }
    }
}
//        // GET: CallStatusStories/Create
//        public IActionResult Create()
//        {
//            ViewData["IntroId"] = new SelectList(_context.Interviews, "Id", "Reference");
//            ViewData["NewStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status");
//            ViewData["OldStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status");
//            return View();
//        }

//        // POST: CallStatusStories/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("IntroId,OldStatusId,NewStatusId,ChangedDate,Id")] CallStatusStory callStatusStory)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(callStatusStory);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["IntroId"] = new SelectList(_context.Interviews, "Id", "Reference", callStatusStory.IntroId);
//            ViewData["NewStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.NewStatusId);
//            ViewData["OldStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.OldStatusId);
//            return View(callStatusStory);
//        }

//        // GET: CallStatusStories/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var callStatusStory = await _context.CallStatusStories.FindAsync(id);
//            if (callStatusStory == null)
//            {
//                return NotFound();
//            }
//            ViewData["IntroId"] = new SelectList(_context.Interviews, "Id", "Reference", callStatusStory.IntroId);
//            ViewData["NewStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.NewStatusId);
//            ViewData["OldStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.OldStatusId);
//            return View(callStatusStory);
//        }

//        // POST: CallStatusStories/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("IntroId,OldStatusId,NewStatusId,ChangedDate,Id")] CallStatusStory callStatusStory)
//        {
//            if (id != callStatusStory.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                { 
//                    _context.Update(callStatusStory);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CallStatusStoryExists(callStatusStory.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["IntroId"] = new SelectList(_context.Interviews, "Id", "Reference", callStatusStory.IntroId);
//            ViewData["NewStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.NewStatusId);
//            ViewData["OldStatusId"] = new SelectList(_context.IntroStatuses, "Id", "Status", callStatusStory.OldStatusId);
//            return View(callStatusStory);
//        }

//        // GET: CallStatusStories/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var callStatusStory = await _context.CallStatusStories
//                .Include(c => c.Intro)
//                .Include(c => c.NewStatus)
//                .Include(c => c.OldStatus)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (callStatusStory == null)
//            {
//                return NotFound();
//            }

//            return View(callStatusStory);
//        }

//        // POST: CallStatusStories/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var callStatusStory = await _context.CallStatusStories.FindAsync(id);
//            if (callStatusStory != null)
//            {
//                _context.CallStatusStories.Remove(callStatusStory);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CallStatusStoryExists(int id)
//        {
//            return _context.CallStatusStories.Any(e => e.Id == id);
//        }
//    }
//}
