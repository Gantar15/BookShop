using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
