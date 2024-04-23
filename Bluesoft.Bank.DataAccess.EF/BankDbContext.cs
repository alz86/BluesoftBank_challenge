using Bluesoft.Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bluesoft.Bank.DataAccess.EF
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountMovement> AccountMovements { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<AccountMovementDailyConsolidation> DailyConsolidations { get; set; }
        public DbSet<AccountMovementMonthlyConsolidation> MonthlyConsolidations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Client - Account
            modelBuilder.Entity<Client>()
            .HasOne(c => c.User)
            .WithOne(u => u.Client)
            .HasForeignKey<Client>("UserId");

            // Branch - Account
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Branch);

            // Branch - DepositDetails
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.DepositDetails)
                .WithOne(d => d.Branch);

            // Branch - WithdrawalDetails
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.WithdrawalDetails)
                .WithOne(w => w.Branch);

            // City - Branches
            modelBuilder.Entity<City>()
                .HasMany(c => c.Branches)
                .WithOne(b => b.City);

            // AccountMovement - DepositDetails
            modelBuilder.Entity<AccountMovement>()
                .HasOne(am => am.DepositDetails)
                .WithOne(dd => dd.AccountMovement)
                .HasForeignKey<DepositDetails>("AccountMovementId");

            // AccountMovement - WithdrawalDetails
            modelBuilder.Entity<AccountMovement>()
                .HasOne(am => am.WithdrawalDetails)
                .WithOne(wd => wd.AccountMovement)
                .HasForeignKey<WithdrawalDetails>("AccountMovementId");


            // Indices for Account
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Creation);

            // Indices for AccountMovement
            modelBuilder.Entity<AccountMovement>()
                .HasIndex(am => am.Date);

            // Indices for Client
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.FullName);

            // Indices for Branch
            modelBuilder.Entity<Branch>()
                .HasIndex(b => b.Name);

            // Indices for City
            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name);

            // Indices for User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username);

            // Indices for DepositDetails
            modelBuilder.Entity<DepositDetails>()
                .HasIndex(dd => dd.Type);

            // Indices for WithdrawalDetails
            modelBuilder.Entity<WithdrawalDetails>()
                .HasIndex(wd => wd.Type);

            // Apply RowVersion configuration to each entity
            var entityTypes = modelBuilder.Model.GetEntityTypes().Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity)));
            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType.Name)
                            .Property(typeof(byte[]), "RowVersion")
                            .IsRowVersion();
            }
        }
    }
}
