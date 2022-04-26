using DataAccess.Entities.Base;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Role : Entity
    {
        public int Id { get; set; }
        public string Role1 { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
