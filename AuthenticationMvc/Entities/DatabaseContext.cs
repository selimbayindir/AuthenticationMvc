using Microsoft.EntityFrameworkCore;

namespace AuthenticationMvc.Entities
{
    public class DatabaseContext :DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

    }

}
