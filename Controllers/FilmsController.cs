using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NTTCinemas.Data;
using NTTCinemas.Models.DbModels;

namespace NTTCinemas.Controllers
{
    [Authorize]
    public class FilmsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FilmsController> _logger;

        public FilmsController(ApplicationDbContext context, ILogger<FilmsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Films
        public async Task<IActionResult> Index(string? filter, string? sort, string? key_search)
        {
            List<Film> films = await _context.Films.ToListAsync();
            
            //Tìm kiếm
            if(key_search != null)
            {
                films = films.Where(f => f.FilmName.Contains(key_search)).ToList();
            }
            
            //lọc phim
            if(filter != null)
            {
                switch (filter)
                {
                    case "dang_chieu":
                        films = films.Where(f => f.Status == 1).ToList();
                        break;
                    case "sap_chieu":
                        films = films.Where(f => f.Status == 0).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                films = films.Where(f => f.Status == 2 || f.Status == 1).ToList();
            }

            //sắp xếp phim
            if(sort != null)
            {
                switch (sort)
                {
                    case "ten_phim":
                        films = films.OrderBy(f => f.FilmName).ToList();
                        break;
                    case "ngay_tao":
                        films = films.OrderBy(f => f.CreationTime).ToList();
                        break;
                    case "ngay_khoi_chieu":
                        films = films.OrderBy(f => f.ReleaseDate).ToList();
                        break;
                    default:
                        break;
                }
            }

            List<SelectListItem> filterSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Tất cả phim", Value = "all"},
                new SelectListItem(){ Text = "Phim đang chiếu", Value = "dang_chieu"},
                new SelectListItem(){ Text = "Phim sắp chiếu", Value = "sap_chieu"}
            };
            ViewData["FilterSelectListItems"] = new SelectList(filterSelectListItems, "Value", "Text", filter);

            List<SelectListItem> sortSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Theo tên phim", Value = "ten_phim"},
                new SelectListItem(){ Text = "Theo thời gian tạo", Value = "ngay_tao"},
                new SelectListItem(){ Text = "Theo ngày khởi chiếu", Value = "ngay_khoi_chieu"}
            };
            ViewData["SortSelectListItems"] = new SelectList(sortSelectListItems, "Value", "Text", sort);


            return View(films);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? filmId)
        {
            if (filmId == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.FilmId == filmId);
            if (film == null)
            {
                return NotFound();
            }

            List<SelectListItem> items = new List<SelectListItem>()
            {
               new SelectListItem(){Text="Đang hoạt động", Value="1"},
               new SelectListItem(){Text="Sắp chiếu", Value="0", Disabled = false},
               new SelectListItem(){Text="Ngừng chiếu", Value="-1"},
            };
            ViewBag.FilmsStatus = new SelectList(items, "Value", "Text", film.Status);

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
               new SelectListItem(){Text="Đang chiếu", Value="1"},
               new SelectListItem(){Text="Sắp chiếu", Value="0"},
               new SelectListItem(){Text="Ngừng chiếu", Value="-1"}
            };
            ViewBag.FilmsStatus = new SelectList(items, "Value", "Text");
            ViewBag.CreateFilmMessage = null;
            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Film film, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    SaveFilmImage(film, formFile);
                }

                if (film.ReleaseDate <= DateTime.Today)
                {
                    film.Status = 1;
                }

                film.CreationTime = DateTime.Now;
                film.LastUpdate = DateTime.Now;

                _context.Add(film);
                await _context.SaveChangesAsync();
                SetAlertNotice("Thêm phim thành công", "success");
                return RedirectToAction(nameof(Index));
            }
            SetAlertNotice("Thêm phim KHÔNG thành công", "warning");
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? filmId)
        {
            if (filmId == null || _context.Films == null)
                return NotFound();
            
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
                return NotFound();
            return View(film);
        }

        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int filmId, Film film)
        {
            if (filmId != film.FilmId)
                return NotFound();
            
            try
            {
                film.LastUpdate = DateTime.Now;
                _context.Update(film);
                await _context.SaveChangesAsync();
                SetAlertNotice("Cập nhật thông tin phim thành công", "success");
                return RedirectToAction("Details", new {filmId = filmId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            SetAlertNotice("Cập nhật phim KHÔNG thành công", "warning");
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? filmId)
        {
            if (filmId == null || _context.Films == null)
                return NotFound();
            
            var film = await _context.Films.FirstOrDefaultAsync(m => m.FilmId == filmId);
            if (film == null)
                return NotFound();
            
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int filmId, string textConfirm)
        {
            if (_context.Films == null)
                return Problem("Entity set 'ApplicationDbContext.Films'  is null.");
            var film = await _context.Films.FindAsync(filmId);
            if (film != null && film.FilmName == textConfirm)
            {
                _context.Films.Remove(film);
            }
            await _context.SaveChangesAsync();
            SetAlertNotice("Xóa phim thành công", "success");
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.FilmId == id)).GetValueOrDefault();
        }

        public bool SaveFilmImage(Film film, IFormFile? formFile)
        {
            string fileName = "";
            if (film != null && formFile != null)
            {
                try
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\films\\");
                    fileName = Guid.NewGuid().ToString() + formFile.FileName;
                    var filePath = Path.Combine(folderPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }
                    film.Image = fileName;
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return false;
        }

        public async Task<IActionResult> EditImage(int? filmId)
        {
            if (filmId == null || _context.Branches == null)
            {
                return NotFound();
            }
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int? filmId, IFormFile? formFile)
        {
            Film? film = await _context.Films.FindAsync(filmId);
            var result = false;
            if (film != null && formFile != null)
            {
                result = SaveFilmImage(film, formFile);
                if (result == true)
                {
                    _context.SaveChanges();
                    SetAlertNotice("Cập nhật hình ảnh thành công", "success");
                    return RedirectToAction("Details", new { filmId = film.FilmId });
                }
            }
            SetAlertNotice("Cập nhật hình ảnh KHÔNG thành công", "success");
            return View(film);
        }

        [HttpPost]
        public async Task ChangeFilmStatus(int filmId, int status)
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film != null)
            {
                film.Status = status;
                _context.Films.Update(film);
                _context.SaveChanges();
                SetAlertNotice("Cập nhật trạng thái phim thành công", "success");
            }
        }

    }
}
