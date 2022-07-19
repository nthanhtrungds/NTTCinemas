using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTTCinemas.Data;
using NTTCinemas.Models.DbModels;
using NTTCinemas.Models.ViewModels;

namespace NTTCinemas.Controllers
{
    [Authorize]
    public class RoomsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoomsController> _logger;

        static string? PrePage;

        public RoomsController(ApplicationDbContext context, ILogger<RoomsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int? branchId)
        {

            var rooms = await _context.Rooms.Include(r => r.Branch).Include(r => r.Seats).Where(b => b.Status == 1).ToListAsync();

            var branches = await _context.Branches.Where(b => b.Status == 1).ToListAsync();

            ViewData["Branches"] = branches;
            
            ViewData["SelectListBranch"] = new SelectList(branches, "BranchId", "BranchName", branchId);
            
            if(branchId != null)
            {
                ViewData["Branches"] = branches.Where(b => b.BranchId == branchId).ToList();
            }
            
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? roomId)
        {
            if (roomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Branch)
                .Include(r => r.Seats)
                .FirstOrDefaultAsync(m => m.RoomId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            List<SelectListItem> statusItems = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Đang mở", Value = "1"},
                new SelectListItem(){Text = "Đang đóng", Value = "0"}
            };

            ViewData["SelectListRoomStatus"] = new SelectList(statusItems, "Value", "Text", room.Status);
            return View(room);
        }

        // GET: Rooms/Create
        public async Task<IActionResult> Create(int branchId)
        {
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null) return NotFound();

            List<SelectListItem> statusItems = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Đang mở", Value = "1"},
                new SelectListItem(){Text = "Đang đóng", Value = "0"}
            };

            ViewData["SelectListRoomStatus"] = new SelectList(statusItems, "Value", "Text");

            PrePage = Request.Headers["Referer"].ToString();
            ViewData["PrePage"] = PrePage;
            return View(new Room() { Branch = branch });
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int branchId, Room room)
        {
            var branch = await _context.Branches.Include(b => b.Rooms).Where(b => b.BranchId == branchId).FirstOrDefaultAsync();
            if(branch == null) return NotFound();

            if( room != null)
            {
                try
                {
                    Room? r = branch.Rooms.Where(r => r.RoomName == room.RoomName).FirstOrDefault();
                        
                    if (r != null)
                    {
                        SetAlertNotice("KHÔNG thành công. Tên phòng "+room.RoomName+" đã tồn tại!.", "warning");
                    }
                    else
                    {
                        room.CreationTime = room.LastUpdate = DateTime.Now;
                        await _context.Rooms.AddAsync(room);
                        await _context.SaveChangesAsync();
                        SetAlertNotice("Tạo phòng thành công", "success");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error: " + ex.Message);
                }
            }
            ViewData["PrePage"] = PrePage;
            return View(new Room() { Branch = branch });
        }

        //GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? roomId)
        {
            if (roomId == null || _context.Rooms == null)
                return NotFound();
            var room = await _context.Rooms.FindAsync(roomId);
            return View(room);
        }

        //POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult?> Edit(int roomId, Room room)
        {
            if (roomId != room.RoomId)
                return NotFound();
            
            try
            {
                var roomInContext = _context.Rooms.Find(roomId);

                //Cập nhật phòng mới
                if(roomInContext != null)
                {
                    //Nếu kích thước phòng thay đổi thì sơ đồ phòng cần reset
                    if(roomInContext.WidthOfRoomMap != room.WidthOfRoomMap
                        || roomInContext.LengthOfRoomMap != room.LengthOfRoomMap)
                    {
                        var seats = _context.Seats.Where(s => s.RoomId == roomId).ToList();
                        if(seats != null)
                        { 
                            _context.RemoveRange(seats);
                        }
                    }

                    roomInContext.BranchId = room.BranchId;
                    roomInContext.RoomName = room.RoomName;
                    roomInContext.WidthOfRoomMap = room.WidthOfRoomMap;
                    roomInContext.LengthOfRoomMap = room.LengthOfRoomMap;
                    roomInContext.CreationTime = room.CreationTime;
                    roomInContext.LastUpdate = DateTime.Now;
                    roomInContext.Description = room.Description;

                    _context.Update<Room>(roomInContext);

                    await _context.SaveChangesAsync();
                    SetAlertNotice("Cập nhật thông tin phòng mới thành công", "success");
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.RoomId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            SetAlertNotice("Cập nhật thông tin phòng KHÔNG thành công", "warning");
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? roomId)
        {
            if (roomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(m => m.RoomId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int roomId, string textConfirm)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null && room.RoomName == textConfirm)
            {
                _context.Rooms.Remove(room);
            }
            else
            {
                return View(await _context.Rooms
                    .Include(r => r.Branch)
                    .FirstOrDefaultAsync(m => m.RoomId == roomId));
            }

            await _context.SaveChangesAsync();
            SetAlertNotice("Xóa phòng thành công", "success");
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int branchId)
        {
            return (_context.Rooms?.Any(e => e.RoomId == branchId)).GetValueOrDefault();
        }

        public async Task<IActionResult> SetRoomMap(int? roomId)
        {
            if (roomId == null) return NotFound();

            var room = await _context.Rooms
            .Include(r => r.Branch)
            .Include(r => r.Seats)
            .FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room == null) return NotFound();

            return View(room);
        }

        public async Task SaveRoomMap(string roomInfo, string seatsData)
        {
            try
            {
                var room = JsonConvert.DeserializeObject<Room>(roomInfo);
                var seatPoints = JsonConvert.DeserializeObject<List<SeatPoint>>(seatsData);

                if (room != null && seatPoints != null)
                {
                    List<Seat> seats = new List<Seat>();

                    // Get position of the first seat in the room. It's equivalent to point 'A1'
                    int x = seatPoints.First().X;
                    int y = seatPoints.First().Y;
                    foreach (var i in seatPoints)
                    {
                        if (i.X < x) x = i.X;
                        if (i.Y < y) y = i.Y;
                    }

                    foreach (SeatPoint seatPoint in seatPoints)
                    {
                        if (seatPoint != null)
                        {
                            //set label for seats
                            string seatLabel = Convert.ToChar((seatPoint.X - x) + 65) + (seatPoint.Y - y + 1).ToString();

                            Seat seat = new Seat()
                            {
                                BranhId = room.BranchId,
                                RoomId = room.RoomId,
                                SeatLabel = seatLabel,
                                AxisX = seatPoint.X,
                                AxisY = seatPoint.Y,
                                Note = "",
                            };
                            seats.Add(seat);
                        }
                    }

                    //Remove old state
                    var oldSeats = _context.Seats.Where(s => s.BranhId == room.BranchId && s.RoomId == room.RoomId).ToList();
                    _context.RemoveRange(oldSeats);

                    //Update new state
                    await _context.Seats.AddRangeAsync(seats);
                    await _context.SaveChangesAsync();
                    SetAlertNotice("Cập nhật sơ đồ phòng thành công", "success");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task UpdateRoomStatus(int roomId, int status)
        {
            try
            {
                var room = await  _context.Rooms.FindAsync(roomId);
                if(room != null)
                {
                    room.Status = status;
                    await _context.SaveChangesAsync();
                    SetAlertNotice("Cập nhật trạng thái phòng thành công", "success");
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }

    }
}


