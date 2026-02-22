using System.ComponentModel.DataAnnotations.Schema;

namespace CrudOperation.Entities
{
    [Table("Sessions", Schema = "Session")]

    public class Session
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public Guid SessionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        public virtual RoleTable Role { get; set; }
        public virtual User User { get; set; }

    }
}
