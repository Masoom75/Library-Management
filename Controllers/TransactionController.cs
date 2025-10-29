using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers
{
    public class TransactionController : Controller
    {
        private readonly FirebaseService _firebase;

        public TransactionController()
        {
            _firebase = new FirebaseService();
        }

        private bool CheckAuth()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId"));
        }

        public async Task<IActionResult> Index()
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var transactions = await _firebase.GetAllAsync<Transaction>("transactions");
            var transList = transactions?.Select(kvp => 
            {
                var trans = kvp.Value;
                trans.Id = kvp.Key;
                
                // Update status based on due date
                if (trans.Status == "Issued" && trans.DueDate < DateTime.Now)
                {
                    trans.Status = "Overdue";
                    var daysOverdue = (DateTime.Now - trans.DueDate).Days;
                    trans.Fine = daysOverdue * 5;
                }
                
                return trans;
            }).OrderByDescending(t => t.IssueDate).ToList() ?? new List<Transaction>();
            
            return View(transList);
        }

        public async Task<IActionResult> IssueBook()
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var books = await _firebase.GetAllAsync<Book>("books");
            var members = await _firebase.GetAllAsync<Member>("members");

            ViewBag.Books = books?.Where(b => b.Value.AvailableQuantity > 0)
                .Select(kvp => 
                {
                    var book = kvp.Value;
                    book.Id = kvp.Key;
                    return book;
                }).ToList() ?? new List<Book>();

            ViewBag.Members = members?.Where(m => m.Value.Status == "Active")
                .Select(kvp => 
                {
                    var member = kvp.Value;
                    member.Id = kvp.Key;
                    return member;
                }).ToList() ?? new List<Member>();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IssueBook(string bookId, string memberId, int days = 14)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var book = await _firebase.GetAsync<Book>($"books/{bookId}");
            var member = await _firebase.GetAsync<Member>($"members/{memberId}");

            if (book != null && member != null && book.AvailableQuantity > 0)
            {
                var transaction = new Transaction
                {
                    BookId = bookId,
                    MemberId = memberId,
                    BookTitle = book.Title,
                    MemberName = member.Name,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(days),
                    Status = "Issued",
                    Fine = 0
                };

                await _firebase.PostAsync("transactions", transaction);

                // Update book quantity
                book.AvailableQuantity--;
                await _firebase.PutAsync($"books/{bookId}", book);

                TempData["Success"] = "Book issued successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Unable to issue book. Please check availability.";
            return RedirectToAction("IssueBook");
        }

        public async Task<IActionResult> ReturnBook(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var transaction = await _firebase.GetAsync<Transaction>($"transactions/{id}");
            
            if (transaction != null && transaction.Status != "Returned")
            {
                transaction.ReturnDate = DateTime.Now;
                transaction.Status = "Returned";

                // Calculate fine if overdue
                if (DateTime.Now > transaction.DueDate)
                {
                    var daysOverdue = (DateTime.Now - transaction.DueDate).Days;
                    transaction.Fine = daysOverdue * 5;
                }

                await _firebase.PutAsync($"transactions/{id}", transaction);

                // Update book quantity
                var book = await _firebase.GetAsync<Book>($"books/{transaction.BookId}");
                if (book != null)
                {
                    book.AvailableQuantity++;
                    await _firebase.PutAsync($"books/{transaction.BookId}", book);
                }

                TempData["Success"] = transaction.Fine > 0 
                    ? $"Book returned with fine: ${transaction.Fine}" 
                    : "Book returned successfully!";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var transaction = await _firebase.GetAsync<Transaction>($"transactions/{id}");
            if (transaction == null) return NotFound();
            
            transaction.Id = id;
            
            // Get book and member details
            var book = await _firebase.GetAsync<Book>($"books/{transaction.BookId}");
            var member = await _firebase.GetAsync<Member>($"members/{transaction.MemberId}");
            
            ViewBag.Book = book;
            ViewBag.Member = member;
            
            return View(transaction);
        }
    }
}