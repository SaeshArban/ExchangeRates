namespace API.Entities
{
    using Microsoft.EntityFrameworkCore;

    public class RateDBContext : DbContext
    {
        public RateDBContext(DbContextOptions<RateDBContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("ExchangeRate_seq", schema: "dbo")
                .StartsAt(0)
                .IncrementsBy(1);

            modelBuilder.Entity<ExchangeRate>()
                .Property(o => o.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.ExchangeRate_seq");
        }
    }
}
