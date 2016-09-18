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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            TransactionViewModel viewModel = new TransactionViewModel();
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            if (currUser == null) { return RedirectToAction("Login", "Account"); }
            List<Transaction> currTransactions = GetTransactions(currUser);
            List<AccountKey> AccountDisplay = GetAccountDisplay(currUser);

            //foreach (var currTransaction in allTransactions)
            //{
            //    if (currTransaction.Account.HouseholdId == currUser.HouseholdId)
            //    {
            //        Transaction currKey = new Transaction();
            //        currKey.InstitutionName = currTransaction.Account.Institution.Name + " " + currTransaction.Account.AccountType.Name;
            //        currKey.Category.Name = currTransaction.Category.Name;
            //        currKey.Type.Name = currTransaction.Type.Name;
            //        currKey.Date = currTransaction.Date;
            //        currKey.Amount = currTransaction.Amount;

            //        currTransactions.Add(currKey);
            //    }
            //}


            viewModel.currTransactions = currTransactions;
            viewModel.AccountId = new SelectList(AccountDisplay, "Id", "InstitutionName");
            viewModel.CategoryId = new SelectList(db.Category, "Id", "Name");
            viewModel.TypeTransactionId = new SelectList(db.TypeTransaction, "Id", "Name");
            return View(viewModel);

        }

        // =========================  HelperFunctions ===========================================

        public List<AccountKey> GetAccountDisplay (ApplicationUser currUser)
        {
            //List<AccountKey> AccountDisplay = currUser.Household.Accounts.ToList();
            List<AccountKey> AccountDisplay = new List<AccountKey>();
            foreach (var account in currUser.Household.Accounts)
            {
                AccountKey currKey = new AccountKey();
                currKey.Id = account.Id;
                currKey.InstitutionName = account.Institution.Name + " " + account.AccountType.Name;
                AccountDisplay.Add(currKey);
            }

            return AccountDisplay;

        }

        public List<Transaction> GetTransactions(ApplicationUser currUser)
        {
            List<Transaction> allTransactions = db.Transaction.Where(i => i.Account.HouseholdId == currUser.HouseholdId).ToList();
            //List<Transaction> currTransactions = new List<Transaction>();
            //foreach (var currTransaction in allTransactions)
            //{

            //    if (currTransaction.Account.HouseholdId == currUser.HouseholdId)
            //    {
            //        Transaction currKey = new Transaction();
            //        currKey.InstitutionName = currTransaction.Account.Institution.Name + " " + currTransaction.Account.AccountType.Name;
            //        currKey.Category.Name = currTransaction.Category.Name;
            //        currKey.Type.Name = currTransaction.Type.Name;
            //        currKey.Date = currTransaction.Date;
            //        currKey.Amount = currTransaction.Amount;

            //        currTransactions.Add(currKey);
            //    }
            //}
            return allTransactions;
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //============================================ GET: Transactions/Create ========================================================================

        public ActionResult Create()
        {
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.AccountId = new SelectList(currUser.Household.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            ViewBag.TypeTransactionId = new SelectList(db.TypeTransaction, "Id", "Name");
            return View();
        }

        //============================================POST: Transactions/Create ========================================================================

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Description,Date,Amount,TypeTransactionId,CategoryId,EnteredById,Reconciled,ReconciledAmount,EnteredBy_Id")] Transaction transaction)
        {
            Account currAccount = db.Account.Find(transaction.AccountId);
            //if (transaction.TypeTransactionId == 2 || transaction.TypeTransactionId == 3) //Withdrawl or Transfer Action
            //{
            //    if(transaction.Amount > currAccount.Balance)
            //    {
            //        float overdraftAmount = currAccount.Balance - transaction.Amount;
            //        ViewBag.overdraftAmount = overdraftAmount;
            //        return RedirectToAction("OverdraftWarning");
            //    }
            //}

            if (ModelState.IsValid)
            {
                db.Transaction.Add(transaction);
                db.SaveChanges();

                bool overdraft = UpdateBalances(transaction);
                if (overdraft)
                {
                    RedirectToAction("OverdraftWarning");
                }

                return RedirectToAction("Index");
            }

            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.AccountId = new SelectList(currUser.Household.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transaction.CategoryId);
            ViewBag.TypeTransactionId = new SelectList(db.TypeTransaction, "Id", "Name", transaction.TypeTransactionId);
            return View(transaction);
        }

        //============================================ Overdraft ========================================================================

        public ActionResult OverdraftWarning()
        {
            return View();
        }

        //============================================ GET: Edit ========================================================================

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transaction.CategoryId);
            ViewBag.TypeTransactionId = new SelectList(db.TypeTransaction, "Id", "Name", transaction.TypeTransactionId);
            return View(transaction);
        }

        //============================================ Post: Edit ========================================================================


        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Description,Date,Amount,TypeTransactionId,CategoryId,EnteredById,Reconciled,ReconciledAmount,EnteredBy_Id")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Transaction originalTransaction = db.Transaction.Find(transaction.Id);
                bool overdraft = BalanceChangeCheck(transaction, originalTransaction);
                db.Entry(originalTransaction).CurrentValues.SetValues(transaction);
                db.SaveChanges();

                if (overdraft)
                {
                    RedirectToAction("OverdraftWarning");
                }
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transaction.CategoryId);
            ViewBag.TypeTransactionId = new SelectList(db.TypeTransaction, "Id", "Name", transaction.TypeTransactionId);
            return View(transaction);
        }

        

        //============================================ Delete: Edit ========================================================================

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //============================================ Post: Edit ========================================================================

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transaction.Find(id);
            BackOutTransaction(transaction);

            db.Transaction.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        //============================================ Helper Functions ========================================================================

        //============================================ Write Functions ========================================================================
        public bool DepositFunds(float amount, int accountId, bool reconciled)
        {
            Account currAccount = db.Account.Find(accountId);
            currAccount.Balance = currAccount.Balance + amount;
            if (reconciled) { currAccount.ReconciledBalance += amount; }
            db.Entry(currAccount).State = EntityState.Modified;
            db.SaveChanges();

            if(currAccount.Balance < 0) { return true; }
            else { return false; }
        }

        public bool WithdrawFunds(float amount, int accountId, bool reconciled)
        {
            Account currAccount = db.Account.Find(accountId);
            currAccount.Balance = currAccount.Balance - amount;
            if (reconciled) { currAccount.ReconciledBalance -= amount; }
            db.Entry(currAccount).State = EntityState.Modified;
            db.SaveChanges();

            if (currAccount.Balance < 0) { return true; }
            else { return false; }
        }

        //============================================ Sorting Operation ========================================================================

        public bool UpdateBalances(Transaction transaction)
        {
            if (transaction.TypeTransactionId == 1 || transaction.TypeTransactionId == 4)
            {
                bool overdraft = DepositFunds(transaction.Amount, transaction.AccountId, transaction.Reconciled);
                return overdraft;
            }
            if (transaction.TypeTransactionId == 2 || transaction.TypeTransactionId == 3)
            {
                bool overdraft = WithdrawFunds(transaction.Amount, transaction.AccountId, transaction.Reconciled);
                return overdraft;
            }

            return false;
        }

        //============================================ Delete ========================================================================

        public bool BackOutTransaction (Transaction backOutTransaction)
        {
            backOutTransaction.Amount *= -1;
            bool overdraft = UpdateBalances(backOutTransaction);
            backOutTransaction.Amount *= -1;
            return overdraft;
        }

        //============================================ Edit Amount ========================================================================

        public bool ChangeTransactionAmount (Transaction deltaTransaction, float newAmount)
        {
            BackOutTransaction(deltaTransaction);
            deltaTransaction.Amount = newAmount;
            bool overdraft = UpdateBalances(deltaTransaction);
            return overdraft;

        }

        //============================================ Edit Type ========================================================================

        public bool ChangeTransactionType (Transaction deltaTransaction, int newType)
        {
            //Transaction backOutTransaction = new Transaction();
            //backOutTransaction.AccountId = deltaTransaction.AccountId;
            //backOutTransaction.Amount = deltaTransaction.Amount;
            //backOutTransaction.Reconciled = deltaTransaction.Reconciled;
            //backOutTransaction.TypeTransactionId = deltaTransaction.TypeTransactionId;
            
            BackOutTransaction(deltaTransaction);

            deltaTransaction.TypeTransactionId = newType;
            bool overdraft = UpdateBalances(deltaTransaction);
            return overdraft;
        }

        //============================================ Sorting by Change Type ========================================================================

        public bool BalanceChangeCheck(Transaction transaction, Transaction originalTransaction)
        {
            if (transaction.Amount != originalTransaction.Amount)
            {
                bool overdraft = ChangeTransactionAmount(originalTransaction, transaction.Amount);
                return overdraft;
            }

            if (transaction.TypeTransactionId != originalTransaction.TypeTransactionId)
            {
                bool overdraft = ChangeTransactionType(originalTransaction, transaction.TypeTransactionId);
                return overdraft;
            }

            

            return false;

        }

        //============================================ Garbage ========================================================================

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
