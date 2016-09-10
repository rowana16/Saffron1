using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Saffron1.Models;
using Microsoft.AspNet.Identity;

namespace Saffron1.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Household.ToList());
        }

        // GET: Households/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Household.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        // GET: Households/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

//============================== Create and Join Household ============================================================================

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            household.Users.Add(currUser);

            if (ModelState.IsValid)
            {
                db.Household.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index", "Home");
        }

//============================== Leave Household ============================================================================


        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Household.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }



 //============================== Leave Household ============================================================================

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Household.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

 


        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Household.Find(id);
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            household.Users.Remove(currUser);

            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
//============================== Invite to Household ============================================================================


        public ActionResult Invite ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Invite (string Email)
        {
            Invitee newInvitee = new Invitee();
            newInvitee.Email = Email;

            db.i


        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
