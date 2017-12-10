using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test5.Models;
using test5.Models.ManageViewModels;

namespace test5.Controllers
{
    public class User2Controller : Controller
    {
        private readonly User2Context _context;

        public User2Controller(User2Context context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.AspNetUsers.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.AspNetUsers
                .SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,first,last,address1,address2,state,zip,email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

   
        public async Task<IActionResult> Edit(IndexViewModel user)
        {
            User2 theUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email.Equals(user.Email));
            if(theUser != null )
            {
                theUser.First = user.First;
                theUser.Last = user.Last;
                theUser.Address1 = user.Address1;
                theUser.Address2 = user.Address2;
                theUser.City = user.City;
                theUser.State = user.State;
                theUser.Zip = user.Zip;
                theUser.Phone = user.PhoneNumber;
                theUser.NameOnCard = user.NameOnCard;
                theUser.CardNumber = user.CardNumber;
                theUser.ExpDate = user.ExpDate;
                theUser.Svc = user.Svc;

                _context.Update(theUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Manage");

        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.AspNetUsers
                .SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.AspNetUsers.SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
            _context.AspNetUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.AspNetUsers.Any(e => Int32.Parse(e.Id) == id);
        }
    }
}
