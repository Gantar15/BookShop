using DataAccess.Entities.Base;
using System;

namespace DataAccess
{
    public partial class User : Entity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public int? RoleId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Role Role { get; set; }
        public Basket Basket { get; set; }
    }
}
