using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Enum
{
    public enum UserTypeEnum
    {
        [Description("一般用户")]
        Free = 100,
        [Description("Pro用户")]
        Pro = 500,
        [Description("Ultra用户")]
        Ultr = 900
    }
}
