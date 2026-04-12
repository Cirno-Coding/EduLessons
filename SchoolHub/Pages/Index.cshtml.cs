using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolHub.Data;
using SchoolHub.Models;

namespace SchoolHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public IndexModel(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // ---------------------------
        // Поля для регистрации
        // ---------------------------

        [BindProperty]
        public string RegisterName { get; set; } = string.Empty;

        [BindProperty]
        public int? RegisterAge { get; set; }

        [BindProperty]
        public string RegisterLogin { get; set; } = string.Empty;

        [BindProperty]
        public string RegisterPassword { get; set; } = string.Empty;

        [BindProperty]
        public string RegisterConfirmPassword { get; set; } = string.Empty;

        // ---------------------------
        // Поля для входа
        // ---------------------------

        [BindProperty]
        public string LoginLogin { get; set; } = string.Empty;

        [BindProperty]
        public string LoginPassword { get; set; } = string.Empty;

        // ---------------------------
        // Данные текущего пользователя
        // ---------------------------

        public bool IsAuthorized { get; set; }

        public string CurrentUserName { get; set; } = string.Empty;

        public string CurrentUserLogin { get; set; } = string.Empty;

        public int CurrentUserAge { get; set; }

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
            LoadCurrentUser();
        }

        public IActionResult OnPostRegister()
        {
            LoadCurrentUser();

            if (string.IsNullOrWhiteSpace(RegisterName) ||
                string.IsNullOrWhiteSpace(RegisterLogin) ||
                string.IsNullOrWhiteSpace(RegisterPassword) ||
                string.IsNullOrWhiteSpace(RegisterConfirmPassword) ||
                RegisterAge == null)
            {
                Message = "Заполните все поля регистрации.";
                return Page();
            }

            if (RegisterAge <= 0)
            {
                Message = "Возраст должен быть больше 0.";
                return Page();
            }

            if (RegisterPassword != RegisterConfirmPassword)
            {
                Message = "Пароли не совпадают.";
                return Page();
            }

            bool loginExists = _context.Users.Any(u => u.Login == RegisterLogin);

            if (loginExists)
            {
                Message = "Такой логин уже существует.";
                return Page();
            }

            var user = new User
            {
                Name = RegisterName,
                Age = RegisterAge.Value,
                Login = RegisterLogin
            };

            // Хэшируем пароль
            user.PasswordHash = _passwordHasher.HashPassword(user, RegisterPassword);

            _context.Users.Add(user);
            _context.SaveChanges();

            // Сохраняем вход в session
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage();
        }

        public IActionResult OnPostLogin()
        {
            LoadCurrentUser();

            if (string.IsNullOrWhiteSpace(LoginLogin) ||
                string.IsNullOrWhiteSpace(LoginPassword))
            {
                Message = "Введите логин и пароль.";
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Login == LoginLogin);

            if (user == null)
            {
                Message = "Неверный логин или пароль.";
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                LoginPassword
            );

            if (result == PasswordVerificationResult.Failed)
            {
                Message = "Неверный логин или пароль.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage();
        }

        private void LoadCurrentUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                IsAuthorized = false;
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                IsAuthorized = false;
                HttpContext.Session.Clear();
                return;
            }

            IsAuthorized = true;
            CurrentUserName = user.Name;
            CurrentUserLogin = user.Login;
            CurrentUserAge = user.Age;
        }
    }
}
