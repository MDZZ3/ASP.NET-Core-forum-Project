using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace firstWeb.Domain.Enums
{
    public enum CommentCatetory
    {
        [Description("回复评价的")]
        Reply=1,
        [Description("评价帖子的")]
        evaluate=2
    }
}
