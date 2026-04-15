using LinkNodeDomain.Model;
using LinkNodeInfrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkNodeInfrastructure.Controllers
{
    public class InvitesController : Controller
    {
        private readonly DbLinkNodeContext _context;
        private readonly UserManager<User> _userManager;

        public InvitesController(DbLinkNodeContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Invites
        public async Task<IActionResult> Index()
        {
            var userIdString = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdString)) return Challenge();
            int currentUserId = int.Parse(userIdString);

            var query = _context.Invites
                .Include(i => i.Freelancer)
                    .ThenInclude(f => f.FreelancerNavigation) 
                .Include(i => i.Status)
                .Include(i => i.Vacancy)
                .AsQueryable();

            if (User.IsInRole("client"))
            {
               
                query = query.Where(i => i.Vacancy.ClientId == currentUserId);
            }
            else if (User.IsInRole("freelancer"))
            {
                
                query = query.Where(i => i.FreelancerId == currentUserId);
            }

            return View(await query.ToListAsync());
        }

        // GET: Invites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invite = await _context.Invites
                .Include(i => i.Freelancer)
                .Include(i => i.Status)
                .Include(i => i.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invite == null) return NotFound();

            return View(invite);
        }

        // GET: Invites/Create
        [Authorize(Roles = "client")] 
        public IActionResult Create(int freelancerId)
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int currentClientId)) return Unauthorized();

            var myActiveVacancies = _context.Vacancies
                .Where(v => v.ClientId == currentClientId && v.ClosedDate == null)
                .ToList();

            var invite = new Invite { FreelancerId = freelancerId };

            ViewData["StatusId"] = new SelectList(_context.InviteStatuses, "Id", "InviteStatus1");
            ViewData["VacancyId"] = new SelectList(myActiveVacancies, "Id", "Title"); // Змінено на Title для кращого відображення

            return View(invite);
        }

        // POST: Invites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Create([Bind("FreelancerId,VacancyId,Id")] Invite invite)
        {
            ModelState.Remove("Freelancer");
            ModelState.Remove("Status");
            ModelState.Remove("Vacancy");

            if (ModelState.IsValid)
            {
                
                invite.StatusId = 3;
                _context.Add(invite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(invite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "freelancer")]
        public async Task<IActionResult> Reject(int id)
        {
            var invite = await _context.Invites.FindAsync(id);

            if (invite != null)
            {
                
                invite.StatusId = 1;
                _context.Update(invite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Invites");
        }

        
        [Authorize(Roles = "freelancer")]
        public IActionResult Apply(int vacancyId)
        {
            return RedirectToAction("Create", "Proposals", new { vacancyId = vacancyId });
        }

        private bool InviteExists(int id) => _context.Invites.Any(e => e.Id == id);
    }
}