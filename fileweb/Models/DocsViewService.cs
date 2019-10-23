using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;

namespace fileweb.Models
{
    public class Sequence
    {
        private int _value = 0;

        public int Value
        {
            get
            {
                return this._value;
            }
        }

        public int Increment()
        {
            return ++this._value;
        }
    }

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

        public static DocsViewModel GetDocsViewModel(this DocsModel docsModel)
        {
            Ensure.That(docsModel, nameof(docsModel)).IsNotNull();

            return new DocsViewModel()
            {
                Category1 = docsModel.CategoryName,
                Category2List = docsModel.DocsSubCategories?.Select(d => d.GetDocsListViewModel()),
            };
        }

        public static DocsListViewModel GetDocsListViewModel(this DocsModel docsModel)
        {
            Ensure.That(docsModel, nameof(docsModel)).IsNotNull();

            var seqNo = new Sequence();

            return new DocsListViewModel()
            {
                CategoryName = docsModel.CategoryName,
                Items = docsModel.GetDocsListItems(seqNo)
            };
        }

        public static IEnumerable<DocsListItemViewModel> GetDocsListItems(this DocsModel docsModel, Sequence seqNo, bool skipCategory = false)
        {
            Ensure.That(docsModel, nameof(docsModel)).IsNotNull();
            Ensure.That(seqNo, nameof(seqNo)).IsNotNull();

            var itemsList = new List<DocsListItemViewModel>();

            if (!skipCategory)
                itemsList.Add(new DocsListItemViewModel()
                {
                    Level = docsModel.Level,
                    IsHeader = true,
                    SeqenceNo = seqNo.Increment(),
                    Title = docsModel.CategoryName
                });

            if (docsModel.Docs != null)
                itemsList.AddRange(docsModel.Docs.OrderBy(d => d.Title).Select(d => new DocsListItemViewModel()
                {
                    IsHeader = false,
                    Title = d.Title,
                    Description = d.Description,
                    Url = d.Url,
                    NewWindow = d.NewWindow,
                    SeqenceNo = seqNo.Increment(),
                    Icon = d.Icon
                }));

            if (docsModel.DocsSubCategories != null)
                foreach (var sub in docsModel.DocsSubCategories.OrderBy(d => d.CategoryName))
                    itemsList.AddRange(sub.GetDocsListItems(seqNo));

            return itemsList;
        }

        public static DocsModel GetDocsModel(this IEnumerable<DocDto> docDtos)
        {
            Ensure.That(docDtos, nameof(docDtos)).IsNotNull();

            return GetDocsListViewModelFromSub(docDtos, 0, new Func<DocDto, string>[]
                {
                    (d=>d.Category1 ),
                    (d=>d.Category2 ),
                    (d=>d.Category3 ),
                    (d=>d.Category4 )
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
