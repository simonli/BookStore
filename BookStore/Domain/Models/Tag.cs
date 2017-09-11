using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Models
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }
        public virtual ICollection<BookTag> BookTags { get; set; }
    }
}
