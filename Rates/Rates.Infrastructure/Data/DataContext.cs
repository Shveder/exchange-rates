using Microsoft.EntityFrameworkCore;
using Rates.Core.Models;

namespace Rates.Infrastructure.Data;

public class DataContext : DbContext
{

    public DbSet<CurrencyRate> Rates { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            @"Host=localhost;Port=5432;Database=ExchangeRates;Username=postgres;Password=postgres"); 
    }
}