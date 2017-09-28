using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.Domain.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Author { get; set; }

        public string AuthorIntroduction { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }

        [StringLength(500)]
        public string Translator { get; set; }

        [StringLength(500)]
        public string Publisher { get; set; }

        [StringLength(100)]
        public string Isbn { get; set; }

        public string BookCatelog { get; set; }
        public string Introduction { get; set; }
        public int DoubanId { get; set; }
        [StringLength(500)]
        public string DoubanUrl { get; set; }
        public float DoubanRatingScore { get; set; }
        public int DoubanRatingPeople { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDelete { get; set; }
        public virtual ICollection<BookEdition> BookEditions { get; set; }
        public virtual ICollection<BookTag> BookTags { get; set; }
    }
}