using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public interface IAnnouncementRepository
    {
        Task<AnnouncementDto> GetLatestAnnouncement(CancellationToken cancellationToken = default);
        Task<IEnumerable<AnnouncementDto>> GetAvailableAnnouncements(CancellationToken cancellationToken = default);
        Task DeleteAnnouncement(int id, CancellationToken cancellationToken = default);
        Task CreateAnnouncement(AnnouncementDto announcementDto, CancellationToken cancellationToken = default);
    }
}
