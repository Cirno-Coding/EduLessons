using SchoolHub.Models;

namespace SchoolHub.Services
{
    public interface ICurrentUserService
    {
        // Проверяет, вошёл ли пользователь
        bool IsAuthenticated(HttpContext httpContext);

        // Возвращает Id текущего пользователя из Session
        int? GetCurrentUserId(HttpContext httpContext);

        // Возвращает текущего пользователя из базы
        User? GetCurrentUser(HttpContext httpContext);

        // Выполняет вход: сохраняет UserId в Session
        void SignIn(HttpContext httpContext, int userId);

        // Выход: очищает Session
        void SignOut(HttpContext httpContext);
    }
}
