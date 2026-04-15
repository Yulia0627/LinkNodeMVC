using LinkNodeDomain.Model;
using LinkNodeInfrastructure;
using LinkNodeInfrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinkNodeInfrastructure.Controllers
{
    public class ClientsController : Controller
    {
        private readonly DbLinkNodeContext _context;
        private readonly IDataPortServiceFactory<Category> _categoryDataPortServiceFactory;

        public ClientsController(DbLinkNodeContext context, IDataPortServiceFactory<Category> factory)
        {
            _context = context;
            _categoryDataPortServiceFactory = factory;
        }

        // GET: Clients
        [HttpGet]
        
        public async Task<IActionResult> Index(int? searchId)
        {
           
            var query = _context.Clients
                .Include(c => c.ClientNavigation)
                .AsQueryable();

           
            bool isAdmin = User.IsInRole("admin");

            if (isAdmin)
            {
               
                if (searchId.HasValue)
                {
                    query = query.Where(c => c.Id == searchId.Value);
                }
               
            }
            else
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdString, out int currentUserId))
                {
                    query = query.Where(c => c.Id == currentUserId);
                }
            }

            var result = await query.ToListAsync();
            return View(result);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.ClientNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create(int id)
        {
            var client = new Client
            {
                Id = id
            };
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "Country");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,HireRate,AvgHourlyRatePaid,Id")] Client client)
        {
            ModelState.Remove("ClientNavigation");
            ModelState.Remove("HireRate");
            ModelState.Remove("AvgHourlyRatePaid");
            if (ModelState.IsValid)
            {
                client.HireRate = 0;
                client.AvgHourlyRatePaid = 0;
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Freelancers");
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", client.Id);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", client.Id);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyName,HireRate,AvgHourlyRatePaid,Id")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Country", client.Id);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.ClientNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
        {
            
            if (fileExcel == null || fileExcel.Length == 0)
            {
                ModelState.AddModelError("", "Будь ласка, оберіть файл для завантаження.");
                return View();
            }

           
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int clientId))
            {
                return Challenge();
            }

            try
            {
                
                var importService = _categoryDataPortServiceFactory.GetImportService(fileExcel.ContentType);

               
                using (var stream = fileExcel.OpenReadStream())
                {
                    await importService.ImportFromStreamAsync(stream, clientId, cancellationToken);
                }

                
                TempData["SuccessMessage"] = "Дані успішно імпортовано!";
                return RedirectToAction("Index", "Vacancies");
            }
            catch (ArgumentException ex)
            {
                
                ModelState.AddModelError("", $"Помилка валідації даних: {ex.Message}");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "Сталася неочікувана помилка при обробці файлу. Перевірте формат Excel.");
            }

           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Export(CancellationToken cancellationToken)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int clientId = int.Parse(userId);

            var exportService = _categoryDataPortServiceFactory.GetExportService("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            using var stream = new MemoryStream();
            await exportService.WriteToAsync(stream, clientId, cancellationToken);

            var content = stream.ToArray();
            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"MyVacancies_{DateTime.Now:ddMMyyyy}.xlsx"
            );
        }
    }
}
