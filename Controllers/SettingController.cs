using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTTCinemas.Data;

namespace NTTCinemas.Controllers
{
    [Authorize]
    public class SettingController : BaseController
    {
        private NTTCinemasContext _context;

        public SettingController(NTTCinemasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Account()
        {
            return View(await _context.Users.ToListAsync());
        }
    }
}
