using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public interface IDocAccessor
    {
        Task<IEnumerable<DocDto>> GetDocDtos(string category2, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllCategory2(CancellationToken cancellationToken = default);
        Task<IEnumerable<DocsCategoryModel>> GetAllCategories(CancellationToken cancellationToken = default);
    }
}
