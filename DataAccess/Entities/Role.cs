using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Role1 { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
