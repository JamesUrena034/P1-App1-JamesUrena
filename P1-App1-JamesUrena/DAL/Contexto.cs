using P1_App1_JamesUrena.Models;
using Microsoft.EntityFrameworkCore;



namespace P1_App1_JamesUrena.DAL
{
    public class Contexto : DbContext
{ 
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    public DbSet<Registros> Registros { get; set; }


}
}





