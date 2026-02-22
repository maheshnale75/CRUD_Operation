using System.ComponentModel.DataAnnotations.Schema;

namespace CrudOperation.Entities
{
    [Table("Users", Schema = "User")]

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }
        public string ProfileImagePath { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

        public int RoleId { get; set; }
        public RoleTable Role { get; set; }
    }
}
