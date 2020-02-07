using Microsoft.EntityFrameworkCore;
using webapi1.Models;
namespace webapi1.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }

        public DbSet<Value> values { get; set; }
        public DbSet<User> Users { get; set; }

    }
}