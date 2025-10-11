using P1_App1_JamesUrena.Models;
using Microsoft.EntityFrameworkCore;



namespace P1_App1_JamesUrena.DAL;

 public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    public DbSet<EntradasHuacales> EntradasHuacales { get; set; }
    public DbSet<TiposHuacales> TiposHuacales { get; set; }
    public DbSet<EntradasHuacalesDetalles> EntradasHuacalesDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TiposHuacales>().HasData(
             new List<TiposHuacales>()
             {
            new()
            {
                TipoId = 1,
                Descripcion = "Huacales verdes",
                Existencia =0
            },
            new()
            {
                TipoId = 2,
                Descripcion = "Huacales azules",
                Existencia =0
            },
            new()
            {
                TipoId = 3,
                Descripcion = "Huacales negros",
                Existencia =0
            },
             }
         );
        base.OnModelCreating(modelBuilder);
    }


}