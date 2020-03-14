using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    public class WorkerContext:DbContext
    {
        public WorkerContext(DbContextOptions<WorkerContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WorkerModel> Workers { get; set; }
    }
}