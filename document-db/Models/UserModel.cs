using System;

namespace document_db.Models
{
    public class UserModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class UserUpdateModel: UserModel
    {
        public Guid Id { get; set; }
    }
}