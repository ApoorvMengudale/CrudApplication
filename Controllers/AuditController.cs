using Microsoft.AspNetCore.Mvc;
using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Crud_Application.Services;
using System.Security.Claims;

namespace Crud_Application.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;

        public AuditController(IAuditService auditService, IUserService userService)
        {
            _auditService = auditService;
            _userService = userService;
        }

        // GET: /Audit/Index
        public IActionResult Index()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Name);
            User user = _userService.GetUserByEmail(userEmail);

            // Retrieve the list of audit entries from the service
            List<Audit> auditEntries = _auditService.GetAuditEntriesByUser(user);

            // Pass the audit entries to the view for rendering
            return View(auditEntries);
        }
    }
}
