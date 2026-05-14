using Microsoft.AspNetCore.Mvc;
using SchoolHub.Services;
using SchoolHub.ViewModels;

namespace SchoolHub.Controllers
{
    public class AdminProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;

        // Сервисы приходят через Dependency Injection.
        // Это продолжение темы IoC / DI из прошлого урока.
        public AdminProjectsController(
            IProjectService projectService,
            ICurrentUserService currentUserService)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
        }

        // GET: /AdminProjects
        // Показывает список всех проектов.
        public IActionResult Index()
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var projects = _projectService.GetAllProjects();

            return View(projects);
        }

        // GET: /AdminProjects/Details/5
        // Показывает один проект по id.
        public IActionResult Details(int id)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: /AdminProjects/Edit/5
        // Открывает форму редактирования проекта.
        public IActionResult Edit(int id)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            // Перекладываем данные из Project в ViewModel.
            var viewModel = new AdminProjectEditViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Category = project.Category,
                Status = project.Status
            };

            return View(viewModel);
        }

        // POST: /AdminProjects/Edit/5
        // Сохраняет изменения проекта.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminProjectEditViewModel viewModel)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            // ModelState — это состояние данных формы.
            // Если поля не прошли проверку атрибутов [Required],
            // возвращаем пользователя обратно на форму.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var project = _projectService.GetProjectById(viewModel.Id);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            // В MVC мы меняем объект и сохраняем через сервис.
            project.Title = viewModel.Title;
            project.Description = viewModel.Description;
            project.Category = viewModel.Category;
            project.Status = viewModel.Status;

            _projectService.UpdateProject(project);

            return RedirectToAction("Index");
        }

        // GET: /AdminProjects/Delete/5
        // Показывает страницу подтверждения удаления.
        public IActionResult Delete(int id)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // POST: /AdminProjects/DeleteConfirmed/5
        // Удаляет проект после подтверждения.
        // Удаление, изменение и создание данных лучше делать через POST,
        // а не через GET.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            _projectService.DeleteProject(project);

            return RedirectToAction("Index");
        }
    }
}
