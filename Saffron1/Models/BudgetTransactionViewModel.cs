using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class BudgetTransactionViewModel
    {
        public List<Budget> Budgets { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }
        public List<Transaction> Transactions { get; set; }

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
}