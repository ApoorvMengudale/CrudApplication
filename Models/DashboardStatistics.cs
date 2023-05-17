namespace Crud_Application.Models
{
    // Represents the statistics for a dashboard
    public class DashboardStatistics
    {
        public decimal SavingsBalance { get; set; } // The balance of savings
        public decimal SalaryBalance { get; set; } // The balance of salary
        public decimal CurrentBalance { get; set; } // The balance of current account
        public int TotalCreditCount { get; set; } // The count of total credits till date
        public int TotalDebitCount { get; set; } // The count of total debits till date
    }
}
