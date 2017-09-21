using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookStore.Domain.Models;

namespace BookStore.Models
{
    public class BookEditionViewModel
    {
        public BookEdition BookEdition { get; set; }
        
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "评论")]
        public string Comment { get; set; }
    }
}