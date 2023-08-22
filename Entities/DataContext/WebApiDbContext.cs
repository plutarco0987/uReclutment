using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        {

        }
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options)
        {

        }
        public DbSet<Candidates> Candidates => Set<Candidates>();
        public DbSet<Comments> Comments => Set<Comments>();
        public DbSet<Customers> Customers => Set<Customers>();
        public DbSet<EnumType> EnumType => Set<EnumType>();
        public DbSet<Log> Log => Set<Log>();
        public DbSet<Meetings> Meetings => Set<Meetings>();
        public DbSet<QuestionDetails> QuestionDetails => Set<QuestionDetails>();
        public DbSet<Questions> Questions => Set<Questions>();
        public DbSet<Settings> Settings => Set<Settings>();
        public DbSet<Vacancy> Vacancy => Set<Vacancy>();
        public DbSet<User> User => Set<User>();
        public DbSet<Files> Files => Set<Files>();
    }
}
