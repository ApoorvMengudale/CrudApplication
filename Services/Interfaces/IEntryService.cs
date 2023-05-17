using Crud_Application.Models;
using System.Security.Claims;

namespace Crud_Application.Services.Interfaces
{
    public interface IEntryService
    {
        List<Entry> GetAllEntries();
        Entry GetEntryById(int id);
        List<Entry> GetEntriesByUser(int userId);
        void CreateEntry(Entry entry);
        void UpdateEntry(Entry entry);
        void DeleteEntry(int id);
        DashboardStatistics GetDashboardStatistics(int userId);
        decimal GetBalance(int userId,string account);
        int? CheckAccountExists(int userId,string account);
        Entry GetEntryByUserAndAccount(int userId, string account);
    }
}
