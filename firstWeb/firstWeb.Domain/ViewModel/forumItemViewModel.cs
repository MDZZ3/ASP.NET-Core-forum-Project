using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWeb.Domain.ViewModel
{
    public class forumItemViewModel
    {
        public int pageCount { get; set; }

        public int CurrentPage { get; set; }

        public string categoryindex { get; set; }

        public List<forumViewModel> forumViewModels { get; set; }

        public Dictionary<string,string> CategoryList { get; set; }
    }
}
