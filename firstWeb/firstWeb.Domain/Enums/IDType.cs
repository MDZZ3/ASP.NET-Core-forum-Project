using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace firstWeb.Domain.Enums
{
    public enum IDType
    {
        [Description("使用用户ID获取评论(Comment)")]
        UserID=1,

        [Description("使用帖子ID获取评论(Comment)")]
        ForumID=2,
        
    }
}
