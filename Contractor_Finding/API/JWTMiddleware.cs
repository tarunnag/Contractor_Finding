namespace API
{
    using API.Helpers;
    using Microsoft.Extensions.Options;
    using Service.Interface;


    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _configuration;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _configuration = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJWTAuthentication jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId);
                return;
            }

            await _next(context);
        }
    }
}
