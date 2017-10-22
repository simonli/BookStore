using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public virtual ICollection<BookTag> BookTags { get; set; }
    }
}