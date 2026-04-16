using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class MyProjectsModel : PageModel
    {
        private readonly AppDbContext _context;

        public MyProjectsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Project> Projects { get; set; } = new();

        public int MyProjectsCount { get; set; }

        public string CurrentUserName { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            LoadMyProjects(userId.Value);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            var project = _context.Projects.FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToPage();
            }

            if (project.AuthorId != userId.Value)
            {
                return RedirectToPage();
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return RedirectToPage();
        }

        private void LoadMyProjects(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                CurrentUserName = user.Name;
            }

            Projects = _context.Projects
                .Include(p => p.Author)
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Id)
                .ToList();

            MyProjectsCount = Projects.Count;
        }
    }
}
