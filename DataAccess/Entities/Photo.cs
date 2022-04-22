using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Photo
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public string Source { get; set; }

        public virtual Book Book { get; set; }
    }
}
