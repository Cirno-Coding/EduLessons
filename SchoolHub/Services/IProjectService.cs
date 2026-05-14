using SchoolHub.Models;

namespace SchoolHub.Services
{
    public interface IProjectService
    {
        List<Project> GetAllProjects();

        List<Project> GetProjectsByAuthorId(int authorId);

        Project? GetProjectById(int id);

        void AddProject(Project project);

        void UpdateProject(Project project);

        void DeleteProject(Project project);
        bool ProjectExists(int id);
    }
}
