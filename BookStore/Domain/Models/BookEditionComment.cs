using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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