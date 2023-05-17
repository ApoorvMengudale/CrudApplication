using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using System.Security.Claims;

namespace Crud_Application.Services
{
    // Represents a service for auditing changes in the application
    public class AuditService : IAuditService
    {
        private readonly AppDBContext _dbContext; // The database context used for accessing the Audit table

        public AuditService(AppDBContext dbContext)
        {
            _dbContext = dbContext; // Initialize the database context through dependency injection
        }

        // Retrieves all audit entries from the database
        public List<Audit> GetAuditEntries()
        {
            return _dbContext.Audit.ToList(); // Retrieve all audit entries from the Audit table and return them as a list
        }

        // Retrieves all audit entries from the database for a specific user
        public List<Audit> GetAuditEntriesByUser(User user)
        {
            return _dbContext.Audit.Where(x=>x.User == user).ToList(); // Retrieve all audit entries from the Audit table and return them as a list
        }

        // Creates a new audit entry in the database
        public void Create(string table, string field, string oldValue, string newValue, string account, User user)
        {
            var auditEntry = new Audit
            {
                Table = table,
                Field = field, 
                OldValue = oldValue, 
                NewValue = newValue, 
                CreatedAt = DateTime.Now,
                User = user,
                Account = account, 
            };

            _dbContext.Audit.Add(auditEntry); // Add the audit entry to the Audit table
            _dbContext.SaveChanges(); // Save the changes to the database
        }
    }
}
