using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data
{
    public class ToDoManagerDBContext: DbContext
    {
        public ToDoManagerDBContext(DbContextOptions<ToDoManagerDBContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<Users> Users { get; set; }

        public DbSet<ResetPassword> ResetPassword { get; set; }
    }
}
