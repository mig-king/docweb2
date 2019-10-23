using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public interface IDocAccessor
    {
        Task<IEnumerable<DocDto>> GetDocDtos(string category1, CancellationToken cancellationToken=default);
        Task<IEnumerable<string>> GetAllCategory1(CancellationToken cancellationToken = default);
    }
}
