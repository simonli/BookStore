using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Models
{
    [Table("push_settings")]
    public class PushSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string PushEmail { get; set; }

        public int IsDefault { get; set; }

        public DateTime? CreateTime { get; set; }

        public virtual User User { get; set; }


    }
}
