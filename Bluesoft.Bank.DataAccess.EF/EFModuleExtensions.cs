using Bluesoft.Bank.Business;
using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bluesoft.Bank.DataAccess.EF
{
    public static class EFModuleExtensions
    {
        public static IServiceCollection UseEFModule(this IServiceCollection services, string? connectionString)
        {
            //db context
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("The connection string was not configured.", nameof(connectionString));

            services.AddDbContext<BankDbContext>(options => options.UseSqlServer(connectionString));

            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //repositories
            services
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IAccountMovementRepository, AccountMovementRepository>()
                .AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IBranchRepository, BranchRepository>()
                .AddScoped<IAccountMovementMonthlyConsolidationRepository, AccountMovementMonthlyConsolidationRepository>();


            return services;
        }
    }
}