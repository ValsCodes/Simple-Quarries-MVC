using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var id = (await userManager.GetUserAsync(User)).Id;
            
            if (User.IsInRole("Administrator"))
            {
                IEnumerable<Orders> objList = _context.Orders;
                return View(objList);
            }
            if (User.IsInRole("Tech"))
            {
                IEnumerable<Orders> objList = _context.Orders.Where(x => x.Id_Technition == id);
                return View(objList);
            }
            if(User.IsInRole("Customer"))
            {
                IEnumerable<Orders> objList = _context.Orders.Where(x=> x.ID_User == id);
                return View(objList);
            }            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Orders, "UserId", "User.Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Orders orders)
        {
            var memberId = (await userManager.GetUserAsync(User)).Id;

            if (ModelState.IsValid)
            {
                orders.ID_User = memberId;

                _context.Add(orders);
                await _context.SaveChangesAsync();
                ViewData["UserId"] = new SelectList(_context.Orders, "UserId", "User.Name");
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Orders, "UserId", "User.Name");
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Orders, "Users", "Users");
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["UserId"] = new SelectList(_context.Orders, "UserId", "User.Name");
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Orders, "Users", "Users");
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
