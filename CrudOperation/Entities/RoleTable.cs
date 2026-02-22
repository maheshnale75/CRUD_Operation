using System.ComponentModel.DataAnnotations.Schema;

namespace CrudOperation.Entities
{
    
        [Table("RoleTable", Schema = "Role")]

        public class RoleTable
        {
            public int Id { get; set; }
            public string Role { get; set; }

            public virtual ICollection<Session> Sessions { get; set; }
            public virtual ICollection<User> Users { get; set; }
        }
    
}
