using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;

namespace fileweb.Models
{
    

    static class DocsViewService
    {
        public static DocViewModel ToViewModel(this DocDto docDto)
        {
            Ensure.That(docDto, nameof(docDto)).IsNotNull();

            return new DocViewModel()
            {
                Title = docDto.Title,
                Description = docDto.Description,
                Url = docDto.Url,
                NewWindow = docDto.NewWindow,
                Icon = docDto.Icon
            };
        }

        public static DocsViewModel GetDocsViewModel(this DocsModel docsModel, IEnumerable<string> categoryList)
        {
            Ensure.That(docsModel, nameof(docsModel)).IsNotNull();
            Ensure.That(categoryList, nameof(categoryList)).IsNotNull();

            return new DocsViewModel()
            {
                CategoryList = categoryList,
                Category = docsModel.CategoryName,
                Rows = docsModel.GetDocsListItems().GetDocsListRows().ToArray(),
                SubCategoryList = docsModel.DocsSubCategories?.Select(d => d.GetDocsViewModel(docsModel.DocsSubCategories?.Select(xd => xd.CategoryName).ToArray())).ToArray()
            };
        }

        public static IEnumerable<DocsListRowModel> GetDocsListRows(this IEnumerable<DocsListItemViewModel> docs)
        {
            Ensure.That(docs, nameof(docs)).IsNotNull();

            return docs
                .OrderBy(d => d.Title)
                .Select((d, index) => new { Id = index, Data = d })
                .GroupBy(d => d.Id / 4)
                .Select(dx => new DocsListRowModel()
                {
                    SequenceNo = dx.Key + 1,
                    Items = dx.Select(d => d.Data).ToArray()
                })
                .ToArray();
        }

        public static IEnumerable<DocsListItemViewModel> GetDocsListItems(this DocsModel docsModel)
        {
            Ensure.That(docsModel, nameof(docsModel)).IsNotNull();

            var itemsList = new List<DocsListItemViewModel>();

            if (docsModel.Docs != null)
                itemsList.AddRange(docsModel.Docs.OrderBy(d => d.Title).Select(d => new DocsListItemViewModel()
                {
                    Title = d.Title,
                    Description = d.Description,
                    Url = d.Url,
                    NewWindow = d.NewWindow,
                    Icon = d.Icon
                }));

            return itemsList;
        }

        public static DocsModel GetDocsModel(this IEnumerable<DocDto> docDtos)
        {
            Ensure.That(docDtos, nameof(docDtos)).IsNotNull();

            return GetDocsListViewModelFromSub(docDtos, 0, new Func<DocDto, string>[]
                {
                    (d=>d.Category1 ),
                    (d=>d.Category2 ),
                    (d=>d.Category3 )
                })
                .FirstOrDefault();
        }

        public static IEnumerable<DocsModel> GetDocsListViewModelFromSub(IEnumerable<DocDto> docDtos, int level, Func<DocDto, string>[] keySelectors)
        {
            Ensure.That(docDtos, nameof(docDtos)).IsNotNull();
            Ensure.That(level, nameof(level)).IsGte(0);
            Ensure.That(keySelectors, nameof(keySelectors)).IsNotNull();

            if (level >= keySelectors.Length || docDtos.Any() == false)
                return null;

            var result = new List<DocsModel>();

            foreach (var docGroup in docDtos.Where(d => !string.IsNullOrWhiteSpace(keySelectors[level](d))).GroupBy(d => keySelectors[level](d)))
            {
                var docs = docGroup.ToList();
                result.Add(new DocsModel()
                {
                    CategoryName = docGroup.Key,
                    Level = level,
                    Docs = GetNotCatoriedDocViewModels(docs, level + 1, keySelectors),
                    DocsSubCategories = GetDocsListViewModelFromSub(docs, level + 1, keySelectors)
                });
            }

            return result;
        }

        public static IEnumerable<DocViewModel> GetNotCatoriedDocViewModels(IEnumerable<DocDto> docDtos, int level, Func<DocDto, string>[] keySelectors)
        {
            Ensure.That(docDtos, nameof(docDtos)).IsNotNull();
            Ensure.That(level, nameof(level)).IsGte(0);
            Ensure.That(keySelectors, nameof(keySelectors)).IsNotNull();

            if (docDtos.Any() == false)
                return null;
            else if (level >= keySelectors.Length)
                return docDtos.Where(d => d.Visible == true).Select(d => d.ToViewModel()).ToArray();
            else
                return docDtos.Where(d => string.IsNullOrWhiteSpace(keySelectors[level](d)) && d.Visible == true).Select(d => d.ToViewModel()).ToArray();
        }
    }
}
