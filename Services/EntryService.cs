using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Crud_Application.Services
{
    // Represents a service for managing entries in the application
    public class EntryService : IEntryService
    {
        private readonly AppDBContext _dbContext; // The database context used for accessing the Entry table

        public EntryService(AppDBContext dbContext)
        {
            _dbContext = dbContext; // Initialize the database context through dependency injection
        }

        // Retrieves all entries from the database
        public List<Entry> GetAllEntries()
        {
            return _dbContext.Entries.ToList(); // Retrieve all entries from the Entry table and return them as a list
        }

        // Retrieves an entry by its ID from the database
        public Entry GetEntryById(int id)
        {
            return _dbContext.Entries.Find(id); // Find the entry with the specified ID in the Entry table
        }

        // Retrieves all entries associated with a specific user from the database
        public List<Entry> GetEntriesByUser(int userId)
        {
            return _dbContext.Entries.Where(x => x.User.Id == userId).ToList(); // Retrieve entries where the User ID matches the specified user ID and return them as a list
        }

        // Creates a new entry in the database
        public void CreateEntry(Entry entry)
        {
            _dbContext.Entries.Add(entry); // Add the entry to the Entry table
            _dbContext.SaveChanges(); // Save the changes to the database
        }

        // Updates an existing entry in the database
        public void UpdateEntry(Entry entry)
        {
            var existingEntry = _dbContext.Entries.Find(entry.Id); // Find the existing entry with the specified ID in the Entry table
            if (existingEntry != null)
            {
                existingEntry.Narration = entry.Narration;
                existingEntry.Currency = entry.Currency; 
                existingEntry.Type = entry.Type; 
                existingEntry.Balance = entry.Balance;

                _dbContext.SaveChanges(); // Save the changes to the database
            }
        }

        // Deletes an entry from the database
        public void DeleteEntry(int id)
        {
            var entry = _dbContext.Entries.Find(id); // Find the entry with the specified ID in the Entry table
            if (entry != null)
            {
                _dbContext.Entries.Remove(entry); // Remove the entry from the Entry table
                _dbContext.SaveChanges(); // Save the changes to the database
            }
        }

        // Retrieves dashboard statistics for a specific user from the database
        public DashboardStatistics GetDashboardStatistics(int userId)
        {
            decimal savingsBalance = _dbContext.Entries.Where(x => x.User.Id == userId && x.Account == "Savings").Sum(x => x.Balance); // Calculate the total balance for savings accounts
            decimal salaryBalance = _dbContext.Entries.Where(x => x.User.Id == userId && x.Account == "Salary").Sum(x => x.Balance); // Calculate the total balance for salary accounts
            decimal currentBalance = _dbContext.Entries.Where(x => x.User.Id == userId && x.Account == "Current").Sum(x => x.Balance); // Calculate the total balance for current accounts
            int totalCreditCount = _dbContext.Audit.Count(x => x.User.Id == userId && x.NewValue == TransactionType.Credit.ToString()); // Count for all credits till Date
            int totalDebitCount = _dbContext.Audit.Count(x => x.User.Id == userId && x.NewValue == TransactionType.Debit.ToString()); // Count for all debits till Date


            DashboardStatistics statistics = new DashboardStatistics
            {
                SavingsBalance = savingsBalance, 
                SalaryBalance = salaryBalance, 
                CurrentBalance = currentBalance,
                TotalCreditCount = totalCreditCount,
                TotalDebitCount = totalDebitCount,
            };

            return statistics; // Return the dashboard statistics
        }

        // Retrieves an entry for a specific user and account from the database
        public Entry GetEntryByUserAndAccount(int userId, string account)
        {
            return _dbContext.Entries.FirstOrDefault(x => x.User.Id == userId && x.Account == account); // Find the first entry where the User ID matches the specified user ID and the Account matches the specified account name
        }

        // Retrieves the total balance for a specific user and account from the database
        public decimal GetBalance(int userId, string account)
        {
            return _dbContext.Entries.Where(x => x.User.Id == userId && x.Account == account).Sum(x => x.Balance); // Calculate the total balance for entries where the User ID matches the specified user ID and the Account matches the specified account name
        }

        // Checks if an account exists for a specific user in the database
        public int? CheckAccountExists(int userId, string account)
        {
            var entryId = _dbContext.Entries.FirstOrDefault(x => x.User.Id == userId && x.Account == account); // Find the first entry where the User ID matches the specified user ID and the Account matches the specified account name
            return entryId?.Id; // Return the entry ID if found, or null if not found
        }
    }
}
