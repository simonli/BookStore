using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Models
{
    [Table("book_tag")]
    public class BookTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public long BookId { get; set; }
        public Book Book { get; set; }


        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}