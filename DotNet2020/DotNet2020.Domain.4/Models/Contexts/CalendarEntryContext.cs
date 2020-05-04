// using System.Collections.Generic;
// using System.Linq;
// using DotNet2020.Data;
// using DotNet2020.Domain._4.Models;
// using DotNet2020.Domain.Models.ModelView;
// using Kendo.Mvc.Examples.Models.Scheduler;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
//
// namespace DotNet2020.Domain._4.Models
// {
//     public class CalendarEntryContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
//     {
//         public CalendarEntryContext(DbContextOptions<CalendarEntryContext> options) : base(options)
//         {
//         }
//
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);
//
//             modelBuilder.Entity<AbstractCalendarEntry>()
//                 .HasDiscriminator<AbsenceType>(nameof(AbstractCalendarEntry.AbsenceType))
//                 .HasValue<Vacation>
//                     (AbsenceType.Vacation)
//                 .HasValue<SickDay>
//                     (AbsenceType.SickDay)
//                 .HasValue<Illness>
//                     (AbsenceType.Illness);
//         }
//
//         public DbSet<Holiday> Holidays { get; set; }
//         public DbSet<Recommendation> Recommendations { get; set; }
//         public virtual DbSet<AbstractCalendarEntry> CalendarEntries { get; set; }
//
//         public List<CalendarEventViewModel> GetAllVacations()
//         {
//             var allVacations = CalendarEntries
//                 .Include(v => v.User)
//                 .ToList()
//                 .Select(m =>
//                 {
//                     var color = "brown";
//                     switch (m.AbsenceType)
//                     {
//                         case AbsenceType.Vacation:
//                             if ((m as Vacation).IsApproved)
//                                 color = "#59d27c";
//                             else color = "#ff4242";
//                             break;
//                         case AbsenceType.SickDay:
//                             color = "#95c8fd";
//                             break;
//                         case AbsenceType.Illness:
//                             if ((m as Illness).IsApproved)
//                                 color = "#6e84fe";
//                             else color = "#ffff92";
//                             break;
//                     }
//                     return new CalendarEventViewModel()
//                     {
//                         Id = m.Id,
//                         Title = m.AbsenceType.ToString(),
//                         Start = m.From,
//                         End = m.To,
//                         UserEmail = m.User?.Email,
//                         ColorId = color
//                     };
//                 }
//                 ).ToList();
//             return allVacations;
//         }
//
//         public List<UserViewModel> GetAllUsers()
//         {
//             var users = Users
//                 .OrderBy(x => x.UserName)
//                 .Select(u =>
//                     new UserViewModel()
//                     {
//                         Name = $"{u.FirstName} {u.LastName}" == " " ? u.Email : $"{u.FirstName} {u.LastName}",
//                         Email = u.Email,
//                         Color = "#6eb3fa"
//                     })
//                 .ToList();
//             return users;
//         }
//
//         public List<string> GetAllHolidays()
//         {
//             var holidays = Holidays
//                 .ToList()
//                 .Select(u =>
//                 {
//                     var year = u.Date.Year.ToString();
//                     var month = u.Date.Month.ToString().StartsWith('0') ? u.Date.Month.ToString().Skip(1) : u.Date.Month.ToString();
//                     var day = u.Date.Day.ToString().StartsWith('0') ? u.Date.Day.ToString().Skip(1) : u.Date.Day.ToString();
//                     return $"{year}/{month}/{day}";
//                 })
//                 .ToList();
//             return holidays;
//         }
//     }
// }
