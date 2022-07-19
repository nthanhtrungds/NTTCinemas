using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NTTCinemas.Data;
using NTTCinemas.Models.DbModels;

namespace NTTCinemas.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        private Random rand = new Random();

        public CustomersController(ApplicationDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string? search)
        {
            var customers = await _context.Customers.OrderByDescending(c => c.CreationTime).ToListAsync();
            ViewBag.AmountCustomer = customers.Count;
            
            if(_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            
            if(search != null){
                customers = customers.Where(c => c.CustomerName.Contains(search) || c.Email == search || c.PhoneNumber == search).ToList();
            }
            else
            {
                customers = customers.Take(20).ToList();
            }

            ViewBag.searchKey = search;


            //CreateDataCus();
            
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Email,PhoneNumber,Birthday,Address,Gender,CreationTime,LastUpdate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreationTime = DateTime.Now;
                customer.LastUpdate = DateTime.Now;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                SetAlertNotice("Thêm khách hàng mới thành công", "success");
                return RedirectToAction(nameof(Index));
            }
            SetAlertNotice("Thêm khách hàng mới KHÔNG thành công", "warning");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Email,PhoneNumber,Birthday,Address,Gender,CreationTime,LastUpdate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.LastUpdate = DateTime.Now;
                    _context.Update(customer);
                    SetAlertNotice("Cập nhật thông tin khách hàng thành công", "success");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            SetAlertNotice("Cập nhật thông tin khách hàng KHÔNG thành công", "warning");
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            SetAlertNotice("Xóa thông tin khách hàng thành công", "success");
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }




        // private void CreateDataCus()
        // {
        //     List<Customer> customers = new List<Customer>();

        //     for (int i = 0; i < 100; i++)
        //     {
        //         Customer cus = new Customer();

        //         cus.CustomerName = CreateName();
        //         cus.Email = "customer0" + i.ToString() + "@gmail.com";
        //         cus.PhoneNumber = "0987654321";
        //         cus.Address = "Unknown";
        //         cus.Birthday = null;
        //         cus.CreationTime = DateTime.Now;
        //         _logger.LogInformation(i.ToString() + " ... " + CreateName());

        //         customers.Add(cus);

        //     }
        //     _context.AddRange(customers);
        //     _context.SaveChanges();
        // }


        // private string CreateName()
        // {

        //     string[] firstName = new string[] { "Nguyễn", "Trần", "Lê", "Đinh", "Lý" };

        //     string[] midName_fel = new string[] { "Thị", "Mộng", "Thơ", "Mỹ", "Thanh" };
        //     string[] midName_mal = new string[] { "Văn", "Thành", "Hùng", "Quốc", "Công" };

        //     string[] lastName_fel = new string[] { "Hà", "Thu", "Hồng", "Huệ", "Quyên", "Hạnh", "Phúc" };
        //     string[] lastName_mal = new string[] { "Trung", "Sơn", "Trí", "Huy", "Trường", "Danh", "Hải" };

        //     string fullname = firstName[rand.Next(firstName.Length)];

        //     int gt = rand.Next(2);

        //     if (gt == 0)
        //     {
        //         fullname = fullname + " " + midName_fel[rand.Next(midName_fel.Length)] + " " + lastName_fel[rand.Next(lastName_fel.Length)];
        //     }
        //     else if (gt == 1)
        //     {
        //         fullname = fullname + " " + midName_mal[rand.Next(midName_mal.Length)] + " " + lastName_mal[rand.Next(lastName_mal.Length)];
        //     }

        //     return fullname;
        // }

    }
}
