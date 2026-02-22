using CrudOperation.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.DbData
{
   
        public class AppData : DbContext
        {
            public AppData()
            {
            }

            public AppData(DbContextOptions<AppData> options)
                : base(options)
            {
            }


            public virtual DbSet<RoleTable> RoleTables { get; set; }
            public virtual DbSet<Session> Sessions { get; set; }
            public virtual DbSet<User> Users { get; set; }



        }
    
}
