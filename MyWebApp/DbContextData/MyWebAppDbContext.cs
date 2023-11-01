using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.DbContextData
{
    public class MyWebAppDbContext: DbContext
    {
        public MyWebAppDbContext(DbContextOptions<MyWebAppDbContext> options) : base (options) { }
        public DbSet<Users> users { get; set; }
        public DbSet<Books> books { get; set; }
        public DbSet<Data> data { get; set; }
        public DbSet<EmailTemplate> emailTemplate { get; set; }


    }
}
