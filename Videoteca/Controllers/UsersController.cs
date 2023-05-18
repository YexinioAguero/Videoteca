using Videoteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Videoteca.Data;

namespace Videoteca.Controllers
{
    public class UsersController : Controller
    {
        private VideotecaContext db = new VideotecaContext();
        // GET: PersonController
        public ActionResult Index()
        {
            var personList = new List<User>();
            using (var dbContext = new VideotecaContext())
            {

                personList = dbContext.Users.ToList();

            }
            ViewBag.persona = personList.FirstOrDefault();
            return View(personList);
        }

        // GET: PersonController/Details/5
        public ActionResult Details(int id)
        {
            var person = db.Users.Find(id);
            db.Users.Where(c => c.user_id == id);

            return View(User);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
