using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NTTCinemas.Data;
using NTTCinemas.Models.DbModels;
using NTTCinemas.Models.ViewModels;

namespace NTTCinemas.Controllers
{
    [Authorize]
    public class ShowTimesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShowTimesController> _logger;

        public ShowTimesController(ApplicationDbContext context, ILogger<ShowTimesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ShowTimes
        public async Task<IActionResult> Index(SortShowTimeInput sortShowTimeInput)
        {

            List<ShowTime> showTimes = await _context.ShowTimes
                .Include(s => s.Room).ThenInclude(r => r.Branch)
                .Include(s => s.Film)
                .ToListAsync();

            showTimes = showTimes.Where(s =>
                s.BranchId == sortShowTimeInput.branchId
                && s.StartTime.Date >= sortShowTimeInput.date_from.Date
                && s.StartTime.Date <= sortShowTimeInput.date_to.Date).ToList();

            var branches = await _context.Branches.Where(b => b.Status == 1).ToListAsync();

            ViewData["ShowTimes"] = showTimes;
            ViewData["SelectBranches"] = new SelectList(branches, "BranchId", "BranchName", sortShowTimeInput.branchId);
            ViewData["Films"] = await _context.Films.ToListAsync();
            ViewData["SortShowTimeInput"] = sortShowTimeInput;

            return View(sortShowTimeInput);
        }

        //// GET: ShowTimes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.ShowTimes == null)
        //    {
        //        return NotFound();
        //    }

        //    var showTime = await _context.ShowTimes
        //        .Include(s => s.Film)
        //        .Include(s => s.Room)
        //        .FirstOrDefaultAsync(m => m.ShowTimeId == id);
        //    if (showTime == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(showTime);
        //}

        //// GET: ShowTimes/Create
        //public IActionResult Create()
        //{
        //    ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "CustomerAge");
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName");

        //    List<Branch> branches = _context.Branches.ToList();
        //    SelectList selectLIstBranch = new SelectList(branches, "BranchId", "BranchName");
        //    ViewData["SelectListBranch"] = selectLIstBranch;

        //    return View();
        //}

        //// POST: ShowTimes/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ShowTimeId,BranchId,RoomId,FilmId,StartTime,EndTime,LastUpdate")] ShowTime showTime)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(showTime);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "CustomerAge", showTime.FilmId);
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName", showTime.RoomId);
        //    return View(showTime);
        //}

        //// GET: ShowTimes/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.ShowTimes == null)
        //    {
        //        return NotFound();
        //    }

        //    var showTime = await _context.ShowTimes.FindAsync(id);
        //    if (showTime == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "CustomerAge", showTime.FilmId);
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName", showTime.RoomId);
        //    return View(showTime);
        //}

        //// POST: ShowTimes/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ShowTimeId,BranchId,RoomId,FilmId,StartTime,EndTime,LastUpdate")] ShowTime showTime)
        //{
        //    if (id != showTime.ShowTimeId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(showTime);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ShowTimeExists(showTime.ShowTimeId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "CustomerAge", showTime.FilmId);
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName", showTime.RoomId);
        //    return View(showTime);
        //}

        // GET: ShowTimes/Delete/5
        public async Task<IActionResult> Delete(int? showtimeId)
        {
            if (showtimeId == null || _context.ShowTimes == null)
            {
                return NotFound();
            }

            var showTime = await _context.ShowTimes
                .Include(s => s.Film)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.ShowTimeId == showtimeId);
            if (showTime == null)
            {
                return NotFound();
            }

            return View(showTime);
        }

        // POST: ShowTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int showtimeId)
        {
            if (_context.ShowTimes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShowTimes'  is null.");
            }
            var showTime = await _context.ShowTimes.FindAsync(showtimeId);
            if (showTime != null)
            {
                _context.ShowTimes.Remove(showTime);
            }

            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        private bool ShowTimeExists(int showtimeId)
        {
            return (_context.ShowTimes?.Any(e => e.ShowTimeId == showtimeId)).GetValueOrDefault();
        }

        public async Task<IActionResult> CreateShowTime()
        {

            ViewData["Branches"] = await _context.Branches.Where(b => b.Status == 1).ToListAsync();
            ViewData["Films"] = await _context.Films.Where(f => f.Status == 1 || f.Status == 0).OrderBy(f => f.ReleaseDate).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShowTime(DateTime dateFrom, DateTime dateTo, string branchIds, string filmIds)
        {
            ViewData["Branches"] = await _context.Branches.Where(b => b.Status == 1).ToListAsync();
            ViewData["Films"] = await _context.Films.Where(f => f.Status == 1 || f.Status == 0).ToListAsync();

            List<Branch> branches = ConvertToListBranch(branchIds);
            List<Film> films = ConvertToListFilm(filmIds);
            if (branches.Count == 0 || films.Count == 0)
            {
                SetAlertNotice("Tạo lịch KHÔNG thành công. Vui lòng kiểm tra các chi nhánh và phim", "warning");
                return View();
            }

            var dateValid = CheckDateCreateShowTime(dateFrom, dateTo);
            var existShowtime = CheckExistShowTime(branches, dateFrom, dateTo);
            //Kiểm tra lỗi trước khi tạo
            if (!(dateValid && existShowtime))
            {
                SetAlertNotice("Tạo lịch KHÔNG thành công. Vui lòng kiểm tra thật kỹ thời gian tạo lịch!", "warning");
                return View();
            }

            try
            {
                List<ShowTime> showTimes = new List<ShowTime>();

                for (DateTime date = dateFrom; date <= dateTo.AddHours(22); date = date.AddDays(1))
                {
                    //Tạo lịch cho từng chi nhánh
                    foreach (var branch in branches)
                    {
                        //Các phim "đang hoạt động" trong ngày
                        var filmsInDay = films.Where(f => f.Status == 1 || f.Status == 0 && f.ReleaseDate.Date >= date.Date);

                        //Các lịch chiếu trong ngày  x của chi nhánh a
                        List<ShowTime> showTimesInDay = CreateShowTimeOneDay(films, branch.Rooms.ToList(), date);
                        showTimes.AddRange(showTimesInDay);
                    }
                }

                await _context.AddRangeAsync(showTimes);
                await _context.SaveChangesAsync();

                SetAlertNotice("Tạo lịch chiếu thành công", "success");
                return RedirectToAction("Index", new { sortShowTimeInput = new SortShowTimeInput() });

            }
            catch (Exception ex)
            {
                SetAlertNotice(ex.Message, "warning");
            }

            return View();
        }

        // Chuyển đổi chuỗi các id trêm view thành danh sách chi nhánh
        public List<Branch> ConvertToListBranch(string IdsString)
        {
            List<Branch> branches = new List<Branch>();
            try
            {
                var ListBranchIds = IdsString.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in ListBranchIds)
                {
                    Branch? branch = _context.Branches.Include(b => b.Rooms).ThenInclude(r => r.Seats).FirstOrDefault(b => b.BranchId == item);
                    if (branch != null)
                        branches.Add(branch);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return branches;
        }

        // Chuyển đổi chuỗi các id trêm view thành danh sách phim
        public List<Film> ConvertToListFilm(string IdsString)
        {
            List<Film> films = new List<Film>();
            try
            {
                var ListFilmIds = IdsString.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in ListFilmIds)
                {
                    Film? film = _context.Films.Find(item);
                    if (film != null)
                        films.Add(film);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return films;
        }

        // Kiểm tra ngày tạo lịch
        public bool CheckDateCreateShowTime(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom < DateTime.Today) return false;
            if (dateTo > dateFrom.AddDays(14)) return false;
            if (dateFrom > dateTo) return false;

            return true;
        }

        //Kiểm tra đã tồn tại lịch chiếu nào trong khoảng thời gian tạo hay không?
        public bool CheckExistShowTime(List<Branch> branches, DateTime dateFrom, DateTime dateTo)
        {
            foreach (var branch in branches)
            {
                var showtime = _context.ShowTimes.Where(s =>
                    s.BranchId == branch.BranchId
                    && s.StartTime >= dateFrom
                    && s.StartTime <= dateTo).FirstOrDefault();
                if (showtime != null) return false;
            }
            return true;
        }

        // Tạo lịch cho 1 ngày
        private List<ShowTime> CreateShowTimeOneDay(List<Film> films, List<Room> rooms, DateTime date)
        {
            List<ShowTime> showTimes = new List<ShowTime>();
            int i = 0;

            foreach (var room in rooms)
            {
                for (DateTime time = date.AddHours(9); time <= date.AddHours(22);)
                {
                    if (i == films.Count) i = 0;

                    ShowTime showTime = new ShowTime()
                    {
                        BranchId = room.BranchId,
                        RoomId = room.RoomId,
                        FilmId = films[i].FilmId,
                        StartTime = time,
                        //EndTime = time.AddMinutes(films[i].Duration),
                        LastUpdate = DateTime.Now,
                    };
                    showTimes.Add(showTime);

                    time = time.AddMinutes(films[i].Duration + 30); //Cộng 30 phút nghỉ giữa các suất chiếu
                    i++;
                }
            }
            return showTimes;
        }

        //Cập nhật một lịch chiếu
        [HttpPost]
        public async Task<IActionResult> UpdateShowtime(ShowTime showtime)
        {
            try
            {
                _context.Update(showtime);
                await _context.SaveChangesAsync();
                SetAlertNotice("Cập nhật lịch chiếu thành công", "success");
            }
            catch
            {
                SetAlertNotice("Cập nhật lịch chiếu KHÔNG thành công", "warning");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> ViewShowtimeByRoom(int branchId, int roomId, DateTime dateFrom, DateTime dateTo)
        {
            List<ShowTime> showTimes = await _context.ShowTimes
                .Include(s => s.Room).ThenInclude(r => r.Branch)
                .Include(s => s.Film)
                .ToListAsync();

            showTimes = showTimes.Where(s =>
                s.BranchId == branchId
                && s.RoomId == roomId
                && s.StartTime.Date >= dateFrom.Date
                && s.StartTime.Date <= dateTo.Date).ToList();

            Branch? b = await _context.Branches.FirstOrDefaultAsync(b => b.BranchId == branchId);
            Room? r = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            ViewData["Branch"] = b;
            ViewData["Room"] = r;

            return View(showTimes);
        }
    }
}
