using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Dtos
{
    public class UpdateProjectDto
    {
        [Required(ErrorMessage = "Введите название проекта")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите описание проекта")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите категорию")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите статус")]
        public string Status { get; set; } = string.Empty;
    }
}
