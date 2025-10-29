using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Services;
using System.Diagnostics;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirebaseService _firebase;

        public HomeController()
        {
            _firebase = new FirebaseService();
        }

        public async Task<IActionResult> Index()
        {
            var books = await _firebase.GetAllAsync<Book>("books");
            var bookList = books?.Select(kvp => 
            {
                var book = kvp.Value;
                book.Id = kvp.Key;
                return book;
            }).Take(6).ToList() ?? new List<Book>();
            
            return View(bookList);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admins = await _firebase.GetAllAsync<Admin>("admins");
            
            // Check if admins dictionary is not null and not empty
            if (admins != null && admins.Any())
            {
                var admin = admins.FirstOrDefault(a => 
                    a.Value != null && 
                    a.Value.Username == username && 
                    a.Value.Password == password);

                if (admin.Value != null)
                {
                    HttpContext.Session.SetString("AdminId", admin.Key);
                    HttpContext.Session.SetString("AdminName", admin.Value.Username);
                    TempData["Success"] = "Welcome back, " + admin.Value.Username + "!";
                    return RedirectToAction("Dashboard");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
                return RedirectToAction("Login");

            // Get statistics
            var books = await _firebase.GetAllAsync<Book>("books");
            var members = await _firebase.GetAllAsync<Member>("members");
            var transactions = await _firebase.GetAllAsync<Transaction>("transactions");

            ViewBag.TotalBooks = books?.Sum(b => b.Value.Quantity) ?? 0;
            ViewBag.TotalMembers = members?.Count ?? 0;
            ViewBag.ActiveTransactions = transactions?.Count(t => t.Value.Status == "Issued" || t.Value.Status == "Overdue") ?? 0;
            ViewBag.OverdueBooks = transactions?.Count(t => t.Value.Status == "Overdue" || (t.Value.Status == "Issued" && t.Value.DueDate < DateTime.Now)) ?? 0;

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logged out successfully!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}