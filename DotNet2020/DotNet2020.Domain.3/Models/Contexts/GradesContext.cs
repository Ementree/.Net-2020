using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    public class GradesContext:DbContext
    {
        public GradesContext(DbContextOptions<GradesContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<GradesModels> Grades { get; set; }
    }
}