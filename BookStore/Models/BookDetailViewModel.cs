using System.Collections.Generic;
using BookStore.Domain.Models;

namespace BookStore.Models
{
    public class BookDetailViewModel
    {
        public Book Book { get; set; }
        public List<Book> RelatedBooks { get; set; }
    }
}