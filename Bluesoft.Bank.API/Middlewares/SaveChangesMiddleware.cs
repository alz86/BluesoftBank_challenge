using Bluesoft.Bank.Business;

namespace Bluesoft.Bank.API.Middlewares
{
    public class SaveChangesMiddleware
    {
        private readonly RequestDelegate _next;

        public SaveChangesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            await _next(context);

            if (context.Response.StatusCode < 400) // Check if the response is successful
            {
                await unitOfWork.SaveAsync();
            }
        }
    }

}