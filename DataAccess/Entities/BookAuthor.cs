using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class BookAuthor
    {
        public BookAuthor()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public int? BookId { get; set; }
        public int? AuthorId { get; set; }

        public virtual Author Author { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
