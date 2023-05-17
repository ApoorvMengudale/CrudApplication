using System.ComponentModel.DataAnnotations;

namespace Crud_Application.Models
{
    // Represents an audit log entry for tracking changes in the system
    public class Audit
    {
        public int Id { get; set; } // The unique identifier for the audit log entry
        public string Table { get; set; } // The table or entity being audited
        public string Field { get; set; } // The field or property being audited
        public string OldValue { get; set; } // The old value of the field before the change
        public string NewValue { get; set; } // The new value of the field after the change

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } // The timestamp when the audit log entry was created

        public User User { get; set; } // The user associated with the change
        public string Account { get; set; } // The account or username associated with the change
    }
}
