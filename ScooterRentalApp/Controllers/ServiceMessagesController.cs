using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;

namespace ScooterRentalApp.Controllers
{
    [Authorize]
    public class SupportMessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<SelectListItem> GetClients(string clientId)
        {
            var adminRoleId = _context.Roles.First(r => r.Name == SystemRoles.Administrator).Id;
            var adminUsersId = _context.UserRoles.Where(r => r.RoleId == adminRoleId).Select(r => r.UserId);


            return _context
                .Clients
                .Where(c => !adminUsersId.Contains(c.Id))
                .Select(p => new SelectListItem
                {
                    Value = p.Id,
                    Text = p.FirstName + " " + p.LastName,
                    Selected = p.Id == clientId
                })
                .ToList();
        }

        public async Task<IActionResult> Index(string clientId)
        {
            if (!User.IsInRole(SystemRoles.Administrator))
            {
                clientId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            }

            ViewBag.Clients = GetClients(clientId);
            ViewBag.ClientId = clientId;

            IQueryable<SupportMessage> query = _context.SupportMessages.Include(m => m.Client);
            if (!string.IsNullOrEmpty(clientId))
            {
                query = query.Where(dm => dm.Client != null && dm.Client.Id == clientId);
            }

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessages.Include(m => m.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportMessage == null)
            {
                return NotFound();
            }

            return View(supportMessage);
        }

        public IActionResult Create(string clientId)
        {
            if (!User.IsInRole(SystemRoles.Administrator))
            {
                clientId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            }

            ViewBag.Clients = GetClients(clientId);
            ViewBag.ClientId = clientId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,FromClient,Created")] SupportMessage supportMessage, [Bind("ClientId")] string clientId)
        {
            if (!User.IsInRole(SystemRoles.Administrator))
            {
                clientId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                supportMessage.FromClient = true;
            }

            supportMessage.Client = _context.Clients.FirstOrDefault(p => p.Id == clientId);


            if (!string.IsNullOrEmpty(supportMessage.Message) && !string.IsNullOrEmpty(clientId))
            {
                supportMessage.Created = DateTime.Now;
                _context.Add(supportMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = GetClients(clientId);
            ViewBag.ClientId = clientId;
            return View(supportMessage);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessages.Include(m => m.Client).FirstOrDefaultAsync(dm => dm.Id == id);
            if (supportMessage == null)
            {
                return NotFound();
            }
            return View(supportMessage);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,FromClient,Created")] SupportMessage supportMessage)
        {
            if (id != supportMessage.Id)
            {
                return NotFound();
            }

            var supportMessageDb = await _context.SupportMessages.Include(m => m.Client).FirstOrDefaultAsync(dm => dm.Id == id);
            if (supportMessageDb == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(supportMessage.Message))
            {
                try
                {

                    supportMessageDb.Message = supportMessage.Message;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportMessageExists(supportMessage.Id))
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
            return View(supportMessage);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportMessage == null)
            {
                return NotFound();
            }

            return View(supportMessage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportMessage = await _context.SupportMessages.FindAsync(id);
            if (supportMessage != null)
            {
                _context.SupportMessages.Remove(supportMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Complaint(int rentalId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complaint(int rentalId, [Bind("Id,Message,FromClient,Created")] SupportMessage supportMessage)
        {
            if (User.IsInRole(SystemRoles.Administrator))
            {
                RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(supportMessage.Message))
            {

                supportMessage.Client = _context.Users.First(u => u.UserName == User.Identity.Name);
                supportMessage.FromClient = true;
                supportMessage.Created = DateTime.Now;
                _context.Add(supportMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(supportMessage);
        }


        private bool SupportMessageExists(int id)
        {
            return _context.SupportMessages.Any(e => e.Id == id);
        }
    }

}
