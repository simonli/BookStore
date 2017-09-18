using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("app_keys")]
    public class AppKey
    {
        [Key]
        public string Name { get; set; }
        public long MaxId { get; set; }
    }

    public enum AppkeyEnum
    {
        Users,
        Books,
        BookEditions,
        BookEditionComments,
        BookTags,
        Tags,
        ActionLogs,
        UserPointLogs
    }

}