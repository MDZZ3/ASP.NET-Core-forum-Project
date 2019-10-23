using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Services.forum_Category
{
    public interface IforumCategoryService
    {
        List<string> GetCategoryNameList();

        List<firstWeb.Domain.Model.forumCategory> GetCategoryList();
    }
}
