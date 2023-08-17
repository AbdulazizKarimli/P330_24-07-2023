namespace P330Pronia.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BlockBrowserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlockBrowserMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string user_agent = httpContext.Request.Headers["User-Agent"];

            if (user_agent.Contains("Edg/"))
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "dinazavr.html");
                using StreamReader sr = new StreamReader(path);
                string dinoPage = await sr.ReadToEndAsync();

                await httpContext.Response.WriteAsync(dinoPage);
                return;
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BlockBrowserMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlockBrowserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlockBrowserMiddleware>();
        }
    }
}
