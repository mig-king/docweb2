using fileweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Services
{
    public interface IAnnouncementService
    {
        Task<Announcement> GetLatestAnnouncement(CancellationToken cancellationToken = default);
        Task<IEnumerable<Announcement>> GetAvailableAnnouncements(CancellationToken cancellationToken = default);
        Task DeleteAnnouncement(int id, CancellationToken cancellationToken = default);
    }
}
