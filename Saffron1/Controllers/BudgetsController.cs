using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Saffron1.Models
{
   

    public class BudgetsController : Controller
    {
        

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            if (currUser == null) { return RedirectToAction("Login", "Account"); }
            BudgetTransactionViewModel viewModel = new BudgetTransactionViewModel();

            //viewModel.Budgets = currUser.Household.Budgets.ToList();
            viewModel.BudgetItems = db.BudgetItem.Where(o => o.Budget.HouseholdId == currUser.HouseholdId).OrderBy(o => o.Category.Name).ToList();
            viewModel.Transactions = db.Transaction.Where(o => o.Account.HouseholdId == currUser.HouseholdId).ToList();
            viewModel.GraphData = Stringify(viewModel.BudgetItems);
            viewModel.GraphData = AddTransactions(viewModel.GraphData, viewModel.Transactions);
           
            return View(viewModel);
        }


        //========================== Graph Helper ========================================

        private List<BudgetViewItem> Stringify(List<BudgetItem> items)
        {
            List<BudgetViewItem> returnObj = new List<BudgetViewItem>();
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            
            foreach (var item in items)
            {
                BudgetViewItem itemList = new BudgetViewItem();
                itemList.CategoryName = javaScriptSerializer.Serialize(item.Category.Name);
                itemList.CategoryId = item.CategoryId;
                itemList.TotalValue = javaScriptSerializer.Serialize(item.Amount);
                
                returnObj.Add(itemList);
               
            }

            return returnObj;
        }

        private List<BudgetViewItem> AddTransactions(List<BudgetViewItem> items, List<Transaction> transactions)
        {
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer(); 

            foreach ( var item in items)
            {
                float sum = 0;

                foreach (var  iTransaction in transactions)
                {
                    

                    if(item.CategoryId == iTransaction.CategoryId)
                    {
                        if(iTransaction.TypeTransactionId == 1 || iTransaction.TypeTransactionId == 4)
                        {
                            sum += iTransaction.Amount;
                        }

                        if(iTransaction.TypeTransactionId == 2 || iTransaction.TypeTransactionId == 3)
                        {
                            sum -= iTransaction.Amount;
                        }
                    }
                }

                item.SumValue = javaScriptSerializer.Serialize(sum);
            }

            return items;
        }

        
        public ActionResult GraphTest()
        {
            return View();
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budget.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Household, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Budget.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Household, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budget.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Household, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Household, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budget.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budget.Find(id);
            db.Budget.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
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
