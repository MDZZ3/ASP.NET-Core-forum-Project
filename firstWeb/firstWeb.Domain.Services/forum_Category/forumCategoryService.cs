using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Services.forum_Category
{
    public class forumCategoryService : IforumCategoryService
    {
        private readonly IRepository<forumCategory> _forumCategoryrepository;

        public forumCategoryService(IRepository<forumCategory> forumcategory)
        {
            _forumCategoryrepository = forumcategory;
        }

        public List<forumCategory> GetCategoryList()
        {
            return _forumCategoryrepository.Table.ToList();
        }

        public List<string> GetCategoryNameList()
        {
            return _forumCategoryrepository.Table.Select(c => c.Category).ToList();
        }
    }
}
