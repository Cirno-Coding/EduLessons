using Microsoft.AspNetCore.Mvc;
using SchoolHub.Dtos;
using SchoolHub.Models;
using SchoolHub.Services;

namespace SchoolHub.Controllers.Api
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsApiController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;

        public ProjectsApiController(
            IProjectService projectService,
            ICurrentUserService currentUserService)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
        }
        [HttpGet]
        public ActionResult<List<ProjectDto>> GetAll()
        {
            var projects = _projectService.GetAllProjects();

            var result = projects.Select(project => ToDto(project)).ToList();

            return Ok(result);
        }

        // GET /api/projects/5
        // Возвращает один проект по id.
        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetById(int id)
        {
            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Проект не найден"
                });
            }

            return Ok(ToDto(project));
        }

        // POST /api/projects
        // Создаёт новый проект.
        //
        // Для простоты используем текущего пользователя из Session.
        // Это удобно для учебного проекта, потому что авторизация уже сделана через Session.
        [HttpPost]
        public ActionResult<ProjectDto> Create(CreateProjectDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId(HttpContext);

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "Для создания проекта нужно войти в аккаунт"
                });
            }

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                AuthorId = userId.Value
            };

            _projectService.AddProject(project);

            // После сохранения заново получаем проект,
            // чтобы подгрузить автора через Include.
            var createdProject = _projectService.GetProjectById(project.Id);

            if (createdProject == null)
            {
                return BadRequest(new
                {
                    message = "Проект был создан, но его не удалось загрузить"
                });
            }

            // CreatedAtAction возвращает статус 201 Created
            // и добавляет ссылку на созданный ресурс.
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProject.Id },
                ToDto(createdProject)
            );
        }

        // PUT /api/projects/5
        // Обновляет проект.
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProjectDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId(HttpContext);

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "Для редактирования проекта нужно войти в аккаунт"
                });
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Проект не найден"
                });
            }

            // Защита: редактировать можно только свой проект.
            if (project.AuthorId != userId.Value)
            {
                return Forbid();
            }

            // Правило из прошлого занятия:
            // завершённый проект редактировать нельзя.
            if (project.Status == "Завершён")
            {
                return BadRequest(new
                {
                    message = "Завершённый проект нельзя редактировать"
                });
            }

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Category = dto.Category;
            project.Status = dto.Status;

            _projectService.UpdateProject(project);

            // Для PUT часто возвращают 204 No Content,
            // то есть действие выполнено успешно, но тело ответа не нужно.
            return NoContent();
        }

        // DELETE /api/projects/5
        // Удаляет проект.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = _currentUserService.GetCurrentUserId(HttpContext);

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "Для удаления проекта нужно войти в аккаунт"
                });
            }

            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Проект не найден"
                });
            }

            // Защита: удалить можно только свой проект.
            if (project.AuthorId != userId.Value)
            {
                return Forbid();
            }

            _projectService.DeleteProject(project);

            return NoContent();
        }

        // Вспомогательный метод:
        // переводит Entity Project в DTO ProjectDto.
        private static ProjectDto ToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Category = project.Category,
                Status = project.Status,
                CreatedAt = project.CreatedAt,
                AuthorId = project.AuthorId,
                AuthorName = project.Author?.Name
            };
        }
    }
}
