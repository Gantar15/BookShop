﻿using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Rating { get; set; }
        public string AgeRestriction { get; set; }
        public string Format { get; set; }
        public int? PagesCount { get; set; }
        public int? PublicationYear { get; set; }
        public int? AuthorId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        public Product Product { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Photo> Photos { get; set; } = new List<Photo>();
    }
}
