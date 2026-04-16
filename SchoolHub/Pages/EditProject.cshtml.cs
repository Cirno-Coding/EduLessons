using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolHub.Data;

namespace SchoolHub.Pages
{
    public class EditProjectModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditProjectModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public string Category { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

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

        public IActionResult OnGet(int id)
        {
            // Проверяем, вошёл ли пользователь
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            // Ищем проект
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToPage("/MyProjects");
            }

            // Проверяем, что это проект текущего пользователя
            if (project.AuthorId != userId.Value)
            {
                return RedirectToPage("/Projects");
            }

            // Заполняем форму текущими значениями
            Id = project.Id;
            Title = project.Title;
            Description = project.Description;
            Category = project.Category;

            return Page();
        }

        public IActionResult OnPost()
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
                return Page();
            }

            // Снова ищем проект в базе
            var project = _context.Projects.FirstOrDefault(p => p.Id == Id);

            if (project == null)
            {
                return RedirectToPage("/MyProjects");
            }

            // Снова проверяем владельца
            if (project.AuthorId != userId.Value)
            {
                return RedirectToPage("/Projects");
            }

            // Обновляем данные
            project.Title = Title;
            project.Description = Description;
            project.Category = Category;

            _context.SaveChanges();

            return RedirectToPage("/MyProjects");
        }
    }
}
