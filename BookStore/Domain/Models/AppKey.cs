using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("app_keys")]
    public class AppKey
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long MaxId { get; set; }
    }
}