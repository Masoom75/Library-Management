using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly FirebaseService _firebase;
        private readonly IWebHostEnvironment _env;

        public BookController(IWebHostEnvironment env)
        {
            _firebase = new FirebaseService();
            _env = env;
        }

        private bool CheckAuth()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId"));
        }

        public async Task<IActionResult> Index()
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var books = await _firebase.GetAllAsync<Book>("books");
            var bookList = books?.Select(kvp => 
            {
                var book = kvp.Value;
                book.Id = kvp.Key;
                return book;
            }).OrderByDescending(b => b.AddedDate).ToList() ?? new List<Book>();
            
            return View(bookList);
        }

        public IActionResult Create()
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile? bookImage)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                book.AddedDate = DateTime.Now;
                book.AvailableQuantity = book.Quantity;

                // Handle image upload
                if (bookImage != null && bookImage.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(bookImage.FileName)}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "books");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookImage.CopyToAsync(stream);
                    }
                    
                    book.ImagePath = $"/images/books/{fileName}";
                }
                else
                {
                    book.ImagePath = "/images/default-book.png";
                }

                await _firebase.PostAsync("books", book);
                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var book = await _firebase.GetAsync<Book>($"books/{id}");
            if (book == null) return NotFound();
            
            book.Id = id;
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book, IFormFile? bookImage)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (bookImage != null && bookImage.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(bookImage.FileName)}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "books");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookImage.CopyToAsync(stream);
                    }
                    
                    book.ImagePath = $"/images/books/{fileName}";
                }

                await _firebase.PutAsync($"books/{book.Id}", book);
                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            await _firebase.DeleteAsync($"books/{id}");
            TempData["Success"] = "Book deleted successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var book = await _firebase.GetAsync<Book>($"books/{id}");
            if (book == null) return NotFound();
            
            book.Id = id;
            return View(book);
        }
    }
}