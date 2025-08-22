using Microsoft.EntityFrameworkCore;
using System;

namespace TransporteApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<HistoricoEntrega> HistoricoEntregas { get; set; }
    }
}
