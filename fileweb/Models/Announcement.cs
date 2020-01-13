using System;
using System.Collections.Generic;
using System.Linq;

namespace fileweb.Models
{
    public class Announcement
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedAt { get; set; }
        public string ExpiredAt { get; set; }
    }

    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsExpired { get; set; }
    }

    public static class AnnouncementExtensions
    {
        public static IEnumerable<Announcement> ToAnnouncements(this IEnumerable<AnnouncementDto> announcementDtos)
        {
            return announcementDtos.Select(anncouncementDto => anncouncementDto.ToAnnouncement());
        }

        public static Announcement ToAnnouncement(this AnnouncementDto announcementDto)
        {
            return new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                CreatedAt = announcementDto.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                ExpiredAt = announcementDto.ExpiredAt.ToString("MM/dd/yyyy HH:mm:ss")
            };
        }
    }
}
