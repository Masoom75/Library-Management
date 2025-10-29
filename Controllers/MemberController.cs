using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers
{
    public class MemberController : Controller
    {
        private readonly FirebaseService _firebase;

        public MemberController()
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

            var members = await _firebase.GetAllAsync<Member>("members");
            var memberList = members?.Select(kvp => 
            {
                var member = kvp.Value;
                member.Id = kvp.Key;
                return member;
            }).OrderByDescending(m => m.JoinDate).ToList() ?? new List<Member>();
            
            return View(memberList);
        }

        public IActionResult Create()
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                member.JoinDate = DateTime.Now;
                member.MembershipNumber = $"MEM{DateTime.Now:yyyyMMddHHmmss}";
                member.Status = "Active";

                await _firebase.PostAsync("members", member);
                TempData["Success"] = "Member added successfully!";
                return RedirectToAction("Index");
            }

            return View(member);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var member = await _firebase.GetAsync<Member>($"members/{id}");
            if (member == null) return NotFound();
            
            member.Id = id;
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Member member)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                await _firebase.PutAsync($"members/{member.Id}", member);
                TempData["Success"] = "Member updated successfully!";
                return RedirectToAction("Index");
            }

            return View(member);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            await _firebase.DeleteAsync($"members/{id}");
            TempData["Success"] = "Member deleted successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!CheckAuth()) return RedirectToAction("Login", "Home");

            var member = await _firebase.GetAsync<Member>($"members/{id}");
            if (member == null) return NotFound();
            
            member.Id = id;
            
            // Get member's transaction history
            var transactions = await _firebase.GetAllAsync<Transaction>("transactions");
            var memberTransactions = transactions?.Where(t => t.Value.MemberId == id)
                .Select(kvp => 
                {
                    var trans = kvp.Value;
                    trans.Id = kvp.Key;
                    return trans;
                })
                .OrderByDescending(t => t.IssueDate)
                .ToList() ?? new List<Transaction>();
            
            ViewBag.Transactions = memberTransactions;
            
            return View(member);
        }
    }
}