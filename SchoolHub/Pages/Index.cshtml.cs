using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string RegisterName { get; set; } = string.Empty;

        [BindProperty]
        public string RegisterLogin { get; set; } = string.Empty;

        [BindProperty]
        public string RegisterPassword { get; set; } = string.Empty;

        [BindProperty]
        public string LoginLogin { get; set; } = string.Empty;

        [BindProperty]
        public string LoginPassword { get; set; } = string.Empty;

        public bool IsAuthorized { get; set; }

        public string CurrentUserName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
            LoadUser();
        }

        public IActionResult OnPostRegister()
        {
            LoadUser();

            if (string.IsNullOrWhiteSpace(RegisterName) ||
                string.IsNullOrWhiteSpace(RegisterLogin) ||
                string.IsNullOrWhiteSpace(RegisterPassword))
            {
                Message = "Заполните все поля регистрации.";
                return Page();
            }

            bool loginExists = _context.Users.Any(u => u.Login == RegisterLogin);

            if (loginExists)
            {
                Message = "Пользователь с таким логином уже существует.";
                return Page();
            }

            var user = new User
            {
                Name = RegisterName,
                Login = RegisterLogin,
                Password = RegisterPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToPage();
        }

        public IActionResult OnPostLogin()
        {
            LoadUser();

            if (string.IsNullOrWhiteSpace(LoginLogin) ||
                string.IsNullOrWhiteSpace(LoginPassword))
            {
                Message = "Введите логин и пароль.";
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u =>
                u.Login == LoginLogin && u.Password == LoginPassword);

            if (user == null)
            {
                Message = "Неверный логин или пароль.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage();
        }

        private void LoadUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            IsAuthorized = userId != null;

            if (!string.IsNullOrEmpty(userName))
            {
                CurrentUserName = userName;
            }
        }
    }
}
