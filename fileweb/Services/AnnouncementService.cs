using fileweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task DeleteAnnouncement(int id, CancellationToken cancellationToken = default)
        {
            await _announcementRepository.DeleteAnnouncement(id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Announcement>> GetAvailableAnnouncements(CancellationToken cancellationToken = default)
        {
            var dtos = await _announcementRepository.GetAvailableAnnouncements(cancellationToken).ConfigureAwait(false);

            return dtos?.ToAnnouncements();
        }

        public async Task<Announcement> GetLatestAnnouncement(CancellationToken cancellationToken = default)
        {
            var dto = await _announcementRepository.GetLatestAnnouncement(cancellationToken).ConfigureAwait(false);

            return dto?.ToAnnouncement();
        }
    }
}
