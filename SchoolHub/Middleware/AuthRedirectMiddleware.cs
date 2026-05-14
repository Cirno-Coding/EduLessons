namespace SchoolHub.Middleware
{
    public class AuthRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // ДОБАВИЛИ ДЛЯ API:
            // API-запросы не перенаправляем на /Index.
            // Для API правильнее возвращать 401/403/404 из контроллера.
            if (path.StartsWith("/api"))
            {
                await _next(context);
                return;
            }

            // ДОБАВИЛИ ДЛЯ SWAGGER:
            // Swagger должен открываться без редиректа,
            // чтобы можно было тестировать API.
            if (path.StartsWith("/swagger"))
            {
                await _next(context);
                return;
            }

            bool isProtectedPage =
                path.StartsWith("/projects") ||
                path.StartsWith("/myprojects") ||
                path.StartsWith("/editproject") ||
                path.StartsWith("/adminprojects");

            bool isAuthenticated = context.Session.GetInt32("UserId") != null;

            if (isProtectedPage && !isAuthenticated)
            {
                context.Response.Redirect("/Index");
                return;
            }

            await _next(context);
        }
    }
}
