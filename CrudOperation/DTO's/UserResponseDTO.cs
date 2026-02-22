using CrudOperation.Entities;

namespace CrudOperation.DTO_s
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Role { get; set; }

    }
}
