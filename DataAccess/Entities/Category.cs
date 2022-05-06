using DataAccess.Entities.Base;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Category : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
