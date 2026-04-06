using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFirstWebApp.Pages
{

    public class Participant
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
    }

    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public int Age { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string ClassName { get; set; }
        public string Error { get; set; }

        public static List<Participant> Participants = new List<Participant>();
        public void OnPostAdd()
        {
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

            if (string.IsNullOrEmpty(Email))
            {
                Error = "Введите email";
                return;
            }

            if (string.IsNullOrEmpty(ClassName))
            {
                Error = "Введите класс";
                return;
            }

            Age = age;

            if (Age <= 0)
            {
                Error = "Возраст должен быть больше 0";
                return;
            }


            // Добавляем в список
            Participants.Add(new Participant
            {
                Name = Name,
                Age = Age,
                Subject = Subject,
                Email = Email,
                ClassName = ClassName
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
