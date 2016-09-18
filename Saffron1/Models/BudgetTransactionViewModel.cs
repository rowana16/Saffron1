using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Saffron1.Models;

namespace Saffron1.Models
{
    public class BudgetTransactionViewModel
    {
        public List<Budget> Budgets { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<BudgetViewItem> GraphData { get; set; }
        

    }

    public class AccountViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<AccountType> Types { get; set; }
    }

    public class AccountDetailViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public Account Account { get; set; }
    }

    public class BudgetViewItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string TotalValue { get; set; }
        public string SumValue { get; set; }        

    }
}