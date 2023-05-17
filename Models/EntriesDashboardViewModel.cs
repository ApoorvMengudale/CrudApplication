namespace Crud_Application.Models
{
    // Represents the view model for the entries dashboard
    public class EntriesDashboardViewModel
    {
        public DashboardStatistics DashboardStatistics { get; set; } // The statistics for the dashboard
        public List<Entry> Entries { get; set; } // The list of entries displayed on the dashboard
    }
}
