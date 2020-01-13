using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public class DocViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool NewWindow { get; set; }
        public string Icon { get; set; }
    }

    public class DocsModel
    {
        public int Level { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DocViewModel> Docs { get; set; }
        public IEnumerable<DocsModel> DocsSubCategories { get; set; }
    }

    public class DocsViewModel
    {
        public IEnumerable<DocsCategoryModel> CategoryList { get; set; }
        public string Category { get; set; }
        public IEnumerable<DocsListRowModel> Rows { get; set; }
        public IEnumerable<DocsViewModel> SubCategoryList { get; set; }
    }

    public class DocsListRowModel
    {
        public int SequenceNo { get; set; }
        public IEnumerable<DocsListItemViewModel> Items { get; set; }
    }

    public class DocsListItemViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool NewWindow { get; set; }
        public string Icon { get; set; }
    }

    public class DocsCategoryModel
    {
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Url { get; set; }
    }

    public class DocsHomeViewModel
    {
        public IEnumerable<DocsCategoryModel> CategoryList { get; set; }
        public Announcement Anncouncement { get; set; }
    }
}
