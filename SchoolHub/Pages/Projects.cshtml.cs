using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProjectsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public string Category { get; set; } = string.Empty;

        public List<Project> Projects { get; set; } = new();

        public int TotalProjectsCount { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CurrentUserName { get; set; } = string.Empty;
        public List<string> Categories { get; } = new()
        {
            "Программирование",
            "Робототехника",
            "Игры",
            "Сайт",
            "Мобильное приложение",
            "Наука",
            "Дизайн",
            "Другое"
        };

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            LoadProjects(userId.Value);
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            if (string.IsNullOrWhiteSpace(Title) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Category))
            {
                Message = "Заполните все поля.";
                LoadProjects(userId.Value);
                return Page();
            }

            var project = new Project
            {
                Title = Title,
                Description = Description,
                Category = Category,
                CreatedAt = DateTime.Now,
                AuthorId = userId.Value
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToPage();
        }

        private void LoadProjects(int userId)
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
                .OrderByDescending(p => p.Id)
                .ToList();
            TotalProjectsCount = Projects.Count;
        }
    }
}
