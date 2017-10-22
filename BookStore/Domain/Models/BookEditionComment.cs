using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("book_edition_comments")]
    public class BookEditionComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual BookEdition BookEdition { get; set; }
        public virtual User User { get; set; }
        public virtual User AtUser { get; set; }
    }
}