using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
