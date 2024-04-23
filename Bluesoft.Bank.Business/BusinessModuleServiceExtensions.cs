using AutoMapper;
using Bluesoft.Bank.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bluesoft.Bank.Business
{
    public static class BusinessModuleServiceExtensions
    {
        public static IServiceCollection UseBusinessModule(this IServiceCollection services)
        {
            //Automapper config
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = new Mapper(config);
            services.AddSingleton(mapper);

            //Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminReportsService, AdminReportsService>();


            return services;
        }
    }
}
