using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using Microsoft.AspNetCore.Authorization;
using WishList.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace WishList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
       public ItemController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var x= _userManager.GetUserAsync(HttpContext.User);
            var model = _context.Items.Where(s => s.Id == x.Id);

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            _userManager.GetUserAsync(HttpContext.User);
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(e => e.Id == id);
            var x= _userManager.GetUserAsync(HttpContext.User);
            if(item.User.Id == Convert.ToString(x.Id))
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
           
            
            return RedirectToAction("Index");
        }
    }
}
