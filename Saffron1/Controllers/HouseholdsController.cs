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
using System.Configuration;
using SendGrid;

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
        [ValidateAntiForgeryToken]
        public ActionResult Invite (string Email)
        {
            Invitee newInvitee = new Invitee();
            var apiKey = ConfigurationManager.AppSettings["SendGridAPIKey"];
            var from = ConfigurationManager.AppSettings["ContactEmail"];
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(Email);
            myMessage.From = new System.Net.Mail.MailAddress("DoNotReply@Saffron1.com");
            myMessage.Subject = "An Invitation From Saffron1";
           

            newInvitee.Email = Email;
            newInvitee.BeenUsed = false;
            newInvitee.HouseholdId = (int)currUser.HouseholdId;
            if (currUser.HouseholdId.HasValue) { newInvitee.HouseholdId = (int)currUser.HouseholdId; }

            if (ModelState.IsValid)
            {
                db.Invitee.Add(newInvitee);
                db.SaveChanges();
                int Id = newInvitee.Id;
                myMessage.Html = "You have been invited to join a household.  Click this link to Register and Join: http://arowan-budgeter.azurewebsites.net/Account/ExternalRegister/" + Id ;
                var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridAPIKey"]);
                transportWeb.DeliverAsync(myMessage);
                return RedirectToAction("Index", "Home");

            }

            return RedirectToAction("Index", "Home");

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
