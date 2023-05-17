using Crud_Application.Models;
using System.Security.Claims;

namespace Crud_Application.Services.Interfaces
{
    public interface IAuditService
    {
        List<Audit> GetAuditEntries();
        List<Audit> GetAuditEntriesByUser(User user);
        void Create(string table, string field, string oldValue, string newValue,string account, User user);
    }
}
