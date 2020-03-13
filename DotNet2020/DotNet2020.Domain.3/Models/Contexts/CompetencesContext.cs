using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    public class CompetencesContext:DbContext
    {
        public CompetencesContext(DbContextOptions<CompetencesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CompetencesModel> Competences { get; set; }
    }
}