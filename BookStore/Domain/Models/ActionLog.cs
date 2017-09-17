using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Models
{
    [Table("action_logs")]
    public class ActionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public DateTime CreateTime { get; set; }
        public string UserAgent { get; set; }
        public string PushEmail { get; set; }
        public PushStatusEnum PushStatus { get; set; }
        public int PushUseTime { get; set; }
        public string PushFromPlatform { get; set; }
        public int CheckinPoint { get; set; } //checkIn本次得分
        public int CheckinTotalPoint { get; set; } //CheckIn累积得分
        public virtual BookEdition BookEdition { get; set; }
        public virtual User User { get; set; }
        public TaxonomyEnum Taxonomy { get; set; }
    }

    public enum PushStatusEnum
    {
        [Description("推送成功")] Success = 100,
        [Description("投递失败")] Fail = 200,
    }

    public enum TaxonomyEnum
    {
        [Description("推送")] Push = 100,
        [Description("上传")] Upload = 200,
        [Description("下载")] Download = 300,
        [Description("收藏")] Favorite = 400,
        [Description("签到")] Checkin = 900,
    }
}