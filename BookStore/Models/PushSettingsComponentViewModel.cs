using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Models;

namespace BookStore.Models
{
    public class PushSettingsComponentViewModel
    {
        public List<PushSetting> PushSettings { get; set; }
        public int EditionId { get; set; }

        public User LoginedUser { get; set; }
    }
}
