using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookStore.Domain.Models;

namespace BookStore.Models
{
    public class BookViewModel
    {
        
    }
    
    public class BookDetailViewModel
    {
        public Book Book { get; set; }
        public List<Book> RelatedBooks { get; set; }
    }
    
    public class BookEditionViewModel
    {
        public BookEdition BookEdition { get; set; }
        
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "评论")]
        public string Comment { get; set; }
    }

    public class BookListViewModel
    {
        public IList<Book> Books { get; set; }
    }
}