using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("push_settings")]
    public class PushSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [StringLength(500)]
        public string PushEmail { get; set; }

        public int IsDefault { get; set; }

        public DateTime? CreateTime { get; set; }

        public virtual User User { get; set; }
    }
}