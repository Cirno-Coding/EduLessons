using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class ProjectDetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProjectDetailsModel(AppDbContext context)
        {
            _context = context;
        }

        // Здесь будет храниться один проект
        public Project? ProjectItem { get; set; }

        public IActionResult OnGet(int id)
        {
            // Ищем проект по id и сразу загружаем автора
            ProjectItem = _context.Projects
                .Include(p => p.Author)
                .FirstOrDefault(p => p.Id == id);

            // Если проекта нет, отправляем на общую страницу проектов
            if (ProjectItem == null)
            {
                return RedirectToPage("/Projects");
            }

            return Page();
        }
    }
}
