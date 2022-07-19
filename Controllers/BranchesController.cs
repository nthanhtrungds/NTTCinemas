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
    public class BranchesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchesController> _logger;

        public BranchesController(ApplicationDbContext context, ILogger<BranchesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches
                .Include(r => r.Rooms)
                .ToListAsync();

            return View(branches);
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? branchId)
        {
            if (branchId == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Rooms)
                    .ThenInclude(s => s.Seats)
                .FirstOrDefaultAsync(b => b.BranchId == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            List<SelectListItem> listItems = new List<SelectListItem>()
             {
                 new SelectListItem(){Text = "Hoạt động", Value = "1"},
                 new SelectListItem(){Text = "Không hoạt động", Value = "0"},
             };
            SelectList listBranchStatus = new SelectList(listItems, "Value", "Text", branch.Status);
            ViewData["SelectBranchStatus"] = listBranchStatus;

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    branch.Manager = "Nguyễn Thành Trung";
                    branch.CreationTime = DateTime.Now;
                    branch.LastUpdate = DateTime.Now;
                    if (branch.OpeningDate < DateTime.Now)
                        branch.Status = 0;
                    else
                        branch.Status = 1;
                    SaveBranchImage(branch, formFile);

                    _context.Add(branch);
                    await _context.SaveChangesAsync();

                    SetAlertNotice("Tạo chi nhánh thành công", "success");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    SetAlertNotice("Tạo chi nhánh KHÔNG thành công", "warning");
                    _logger.LogError(ex.Message);
                }
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? branchId)
        {
            if (branchId == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return NotFound();
            }

            List<SelectListItem> listItems = new List<SelectListItem>()
             {
                 new SelectListItem(){Text = "Hoạt động", Value = "1"},
                 new SelectListItem(){Text = "Không hoạt động", Value = "0"},
             };
            SelectList listBranchStatus = new SelectList(listItems, "Value", "Text", branch.Status);
            ViewData["SelectBranchStatus"] = listBranchStatus;

            return View(branch);
        }

        // POST: Branches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int branchId, Branch branch)
        {
            if (branchId != branch.BranchId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    branch.LastUpdate = DateTime.Now;
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                    SetAlertNotice("Cập nhật thông tin chi nhánh thành công", "success");
                    return RedirectToAction("Details", new { branchId = branchId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.BranchId))
                        return NotFound();
                    else
                        throw;
                }
            }
            List<SelectListItem> listItems = new List<SelectListItem>()
             {
                 new SelectListItem(){Text = "Hoạt động", Value = "1"},
                 new SelectListItem(){Text = "Đóng cửa", Value = "0"},
             };
            SelectList listBranchStatus = new SelectList(listItems, "Value", "Text", branch.Status);
            ViewData["SelectBranchStatus"] = listBranchStatus;
            SetAlertNotice("Cập nhật thông tin KHÔNG thành công", "warning");
            return View(branch);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? branchId)
        {
            if (branchId == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .FirstOrDefaultAsync(m => m.BranchId == branchId);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int branchId, string textConfirm)
        {
            if (_context.Branches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Branches'  is null.");
            }
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch != null && textConfirm == branch.BranchName)
            {
                _context.Branches.Remove(branch);
            }
            else
            {
                SetAlertNotice("Xóa chi nhánh KHÔNG thành công", "warning");
                ViewBag.DeleteBranchMessage = "Xóa không thành công.";
                return View(branch);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int branchId)
        {
          return (_context.Branches?.Any(e => e.BranchId == branchId)).GetValueOrDefault();
        }

        public async Task<IActionResult> EditImage(int? branchId)
        {
            if (branchId == null || _context.Branches == null)
            {
                return NotFound();
            }
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int? branchId, IFormFile? formFile)
        {
            Branch? branch = await _context.Branches.FindAsync(branchId);
            var result = false;
            if (branch != null && formFile != null)
            {
                result = SaveBranchImage(branch, formFile);
                if (result == true)
                {
                    _context.SaveChanges();
                    SetAlertNotice("Cập nhật ảnh thành công", "success");
                    return RedirectToAction("Details", new {branchId = branch.BranchId});
                }
            }
            SetAlertNotice("Cập nhật ảnh KHÔNG thành công", "warning");
            return View(branch);
        }

        public bool SaveBranchImage(Branch branch, IFormFile? formFile)
        {
            string fileName = "";
            if(branch != null && formFile != null)
            {
                try
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\branches\\");
                    fileName = Guid.NewGuid().ToString() + formFile.FileName;
                    var filePath = Path.Combine(folderPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }
                    branch.Image = fileName;
                    return true;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return false;
        }

        [HttpPost]
        public async Task UpdateBranchStatus(int branchId, int status)
        {
            try{
                var b = await _context.Branches.FindAsync(branchId);
                if(b != null)
                {
                    b.Status = status;
                    await _context.SaveChangesAsync();
                    SetAlertNotice("Cập nhật trạng thái chi nhánh thành công", "success");
                }
            }
            catch(Exception ex){
                SetAlertNotice("Cập nhật trạng thái chi nhánh KHÔNG thành công", "waring");
                _logger.LogError(ex.Message);
            }
        }

    }
}
