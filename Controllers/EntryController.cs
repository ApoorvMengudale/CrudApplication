using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Crud_Application.Controllers
{
    [Authorize]
    public class EntryController : Controller
    {
        private readonly IEntryService _entryService;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;

        public EntryController(IEntryService entryService, IAuditService auditService, IUserService userService)
        {
            _entryService = entryService;
            _auditService = auditService;
            _userService = userService;
        }

        // GET: /Entry/Index
        public IActionResult Index()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Name);
            User user = _userService.GetUserByEmail(userEmail);

            EntriesDashboardViewModel entriesDashboardView = new EntriesDashboardViewModel();
            entriesDashboardView.Entries = _entryService.GetEntriesByUser(user.Id);
            entriesDashboardView.DashboardStatistics = _entryService.GetDashboardStatistics(user.Id);

            return View(entriesDashboardView);
        }


        // GET: /Entry/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Entry/Create
        [HttpPost]
        public IActionResult Create(CreateEditEntryViewModel entry)
        {
            if (ModelState.IsValid)
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Name);
                User user = _userService.GetUserByEmail(userEmail);

                Entry newEntry = new Entry
                {
                    Currency = entry.Currency,
                    Account = entry.Account,
                    CreatedAt = DateTime.Now,
                    Narration = entry.Narration,
                    Type = entry.Type,
                    User = user,
                    Balance = _entryService.GetBalance(user.Id, entry.Account)
                };

                if (entry.Type == "Credit")
                {
                    newEntry.Balance += entry.Amount;
                }
                else if (entry.Type == "Debit")
                {
                    newEntry.Balance -= entry.Amount;
                }

                if (newEntry.Balance < 0)
                {
                    ModelState.AddModelError(string.Empty, "Cannot process transaction as the available balance is less than the entered amount.");
                }
                else
                {
                    var existingEntry = _entryService.GetEntryByUserAndAccount(user.Id, entry.Account);
                    if (existingEntry != null)
                    {
                        newEntry.Id = existingEntry.Id;
                        var oldEntry = _entryService.GetEntryById(newEntry.Id);
                        var updatedBalance = newEntry.Balance;
                        _entryService.UpdateEntry(newEntry);

                        List<string> updatedFields = new List<string>();
                        updatedFields.Add("Type");
                        if (oldEntry.Account != entry.Account)
                        {
                            updatedFields.Add("Account");
                        }
                        if (oldEntry.Narration != entry.Narration)
                        {
                            updatedFields.Add("Narration");
                        }
                        if (oldEntry.Currency != entry.Currency)
                        {
                            updatedFields.Add("Currency");
                        }
                        if (oldEntry.Balance != updatedBalance)
                        {
                            updatedFields.Add("Balance");
                        }

                        // Iterate through the updated fields and create audit entries
                        foreach (string field in updatedFields)
                        {
                            if (field == "Balance")
                            {
                                _auditService.Create("Entries", field, oldEntry.GetType().GetProperty(field).GetValue(oldEntry)?.ToString(), entry.GetType().GetProperty("Amount").GetValue(entry)?.ToString(), entry.Account, user);
                            }
                            else
                            {
                                _auditService.Create("Entries", field, oldEntry.GetType().GetProperty(field).GetValue(oldEntry)?.ToString(), entry.GetType().GetProperty(field).GetValue(entry)?.ToString(), entry.Account, user);
                            }
                        }
                    }
                    else
                    {
                        _entryService.CreateEntry(newEntry);

                        // Iterate through the properties of the entry and create audit entries
                        foreach (PropertyInfo prop in entry.GetType().GetProperties())
                        {
                            if (prop.Name.ToLower() != "id" && prop.Name != "UpdatedAt" && prop.Name != "CreatedAt" && prop.Name != "Amount")
                            {
                                _auditService.Create("Entries", prop.Name, "", Convert.ToString(prop.GetValue(entry, null)), entry.Account, user);
                            }
                            else if (prop.Name == "Amount")
                            {
                                _auditService.Create("Entries", "Balance", Convert.ToString(0), Convert.ToString(prop.GetValue(entry)), entry.Account, user);
                            }
                        }
                    }

                    return RedirectToAction("Index", "Entry");
                }
            }

            return View(entry);
        }

        // GET: /Entry/Edit/1
        public IActionResult Edit(int id)
        {
            Entry entry = _entryService.GetEntryById(id);
            if (entry == null)
            {
                return NotFound();
            }

            CreateEditEntryViewModel viewModel = new CreateEditEntryViewModel
            {
                Id = entry.Id,
                Currency = entry.Currency,
                Account = entry.Account,
                Narration = entry.Narration,
                Type = entry.Type,
                Amount = 0,
            };

            return View(viewModel);
        }

        // POST: /Entry/Edit/1
        [HttpPost]
        public IActionResult Edit(int id, CreateEditEntryViewModel entry)
        {
            if (id != entry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Name);
                User user = _userService.GetUserByEmail(userEmail);

                Entry updateEntry = _entryService.GetEntryById(id);
                if (updateEntry == null)
                {
                    return NotFound();
                }

                updateEntry.Narration = entry.Narration;
                updateEntry.UpdatedAt = DateTime.Now;
                updateEntry.Currency = entry.Currency;

                var oldBalance = updateEntry.Balance;

                if (entry.Type == "Credit")
                {
                    updateEntry.Balance += entry.Amount;
                }
                else if (entry.Type == "Debit")
                {
                    updateEntry.Balance -= entry.Amount;
                }

                if (updateEntry.Balance < 0)
                {
                    ModelState.AddModelError(string.Empty, "Cannot process transaction as the available balance is less than the entered amount.");
                }
                else
                {
                    var oldEntry = _entryService.GetEntryById(id);
                    var updatedBalance = updateEntry.Balance;
                    _entryService.UpdateEntry(updateEntry);

                    List<string> updatedFields = new List<string>();
                    updatedFields.Add("Type");
                    if (oldEntry.Account != entry.Account)
                    {
                        updatedFields.Add("Account");
                    }
                    if (oldEntry.Narration != entry.Narration)
                    {
                        updatedFields.Add("Narration");
                    }
                    if (oldEntry.Currency != entry.Currency)
                    {
                        updatedFields.Add("Currency");
                    }
                    if (oldBalance != updatedBalance)
                    {
                        updatedFields.Add("Balance");
                    }

                    // Iterate through the updated fields and create audit entries
                    foreach (string field in updatedFields)
                    {
                        if (field == "Balance")
                        {
                            _auditService.Create("Entries", field, oldEntry.GetType().GetProperty(field).GetValue(oldEntry)?.ToString(), entry.GetType().GetProperty("Amount").GetValue(entry)?.ToString(), entry.Account, user);
                        }
                        else
                        {
                            _auditService.Create("Entries", field, oldEntry.GetType().GetProperty(field).GetValue(oldEntry)?.ToString(), entry.GetType().GetProperty(field).GetValue(entry)?.ToString(), entry.Account, user);
                        }
                    }

                    return RedirectToAction("Index", "Entry");
                }
            }

            return View(entry);
        }

        // GET: /Entry/Delete/1
        public IActionResult Delete(int id)
        {
            Entry entry = _entryService.GetEntryById(id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        // POST: /Entry/Delete/1
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _entryService.DeleteEntry(id);
            return RedirectToAction("Index", "Entry");
        }
    }
}
