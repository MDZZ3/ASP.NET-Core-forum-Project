using firstWeb.Domain.Model;
using firstWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace firstWeb.Domain.Services.forum
{
    public interface IForumService
    {
        List<forumViewModel> GetForumsItem(int? pageIndex, int number,out forumItemViewModel itemViewModel);

        void InsertForum(Forum forum);

        Task<Forum> GetForumAsync(string id);

        Task<bool> isExistAsync(string id);

        List<forumViewModel> GetCategoryItemForums(int categoryid, int? pageIndex, int number, out forumItemViewModel itemViewModel);



    }
}
