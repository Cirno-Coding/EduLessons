using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFirstWebApp.Pages
{

    public class Participant
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Subject { get; set; }
    }

    public class IndexModel : PageModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Subject { get; set; }
        public string Error { get; set; }

        public static List<Participant> Participants = new List<Participant>();
        public void OnPostAdd()
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

            // Добавляем в список
            Participants.Add(new Participant
            {
                Name = Name,
                Age = Age,
                Subject = Subject
            });
        }
        //Домашка
        public void OnPostDelete(int index)
        {
            if (index >= 0 && index < Participants.Count)
            {
                Participants.RemoveAt(index);
            }
        }
    }
}
