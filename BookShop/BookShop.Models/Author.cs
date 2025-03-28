﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Models
{
    using System.Collections.Generic;

    public class Author
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

      
        public int AuthorId { get; set; }

        
        public string? FirstName { get; set; }

        
        public string LastName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}