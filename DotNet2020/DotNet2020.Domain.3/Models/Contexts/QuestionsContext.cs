using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    public class QuestionsContext:DbContext //реализовать репозиторий
    {
        public QuestionsContext(DbContextOptions<QuestionsContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<QuestionsModel> Questions;
    }
}