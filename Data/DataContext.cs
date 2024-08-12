using exchange_rates.Models;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;

namespace exchange_rates.Data;

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