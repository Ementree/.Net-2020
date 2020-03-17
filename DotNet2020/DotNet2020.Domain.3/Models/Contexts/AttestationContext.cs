using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    public class AttestationContext:DbContext
    {
        public AttestationContext(DbContextOptions<AttestationContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<AttestationModel> Attestations { get; set; }
    }
}