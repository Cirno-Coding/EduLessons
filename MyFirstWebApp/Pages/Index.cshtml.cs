using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFirstWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Subject { get; set; }
        public string Error { get; set; }
        public void OnPost()
        {
            Name = Request.Form["name"];
            Subject = Request.Form["subject"];

            // Проверка имени
            if (string.IsNullOrEmpty(Name))
            {
                Error = "Введите имя";
                return;
            }

            // Проверка возраста
            if (!int.TryParse(Request.Form["age"], out int age))
            {
                Error = "Возраст должен быть числом";
                return;
            }

            Age = age;
        }
    }
}
