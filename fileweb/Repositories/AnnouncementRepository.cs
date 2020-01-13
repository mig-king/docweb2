
using Dapper;
using EnsureThat;
using fileweb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly string _connectionString;

        public AnnouncementRepository(string connectionString)
        {
            Ensure.That(connectionString, nameof(connectionString)).IsNotEmptyOrWhitespace();

            _connectionString = connectionString;
        }

        public async Task CreateAnnouncement(AnnouncementDto announcementDto, CancellationToken cancellationToken = default)
        {
            const string sql = @"INSERT INTO [cms].[Announcement] 
            ([ID],
                [Title],
                [Content],
                [CreatedAt],
                [CreatedBy],
                [IsExpired],
                [ExpiredAt])
            VALUES (@Id,
                @Title,
                @Content,
                GETDATE(),
                @CreatedBy,
                0,
                @ExpiredAt);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                await connection.ExecuteAsync(sql, announcementDto).ConfigureAwait(false);
            }
        }

        public async Task DeleteAnnouncement(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                var sql = @"UPDATE [cms].[Announcement] SET [IsExpired] = 1, ExpiredAt = GETDATE() WHERE ID = @id";

                await connection.ExecuteAsync(sql, new { id }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAvailableAnnouncements(CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                var sql = @"SELECT [ID],[Title],[Content],[CreatedAt],[CreatedBy],[IsExpired], [ModifiedAt], [ExpiredAt] FROM [cms].[Announcement] " +
                    " WHERE IsExpired = 0 OR ExpiredAt < GETDATE();";

                return await connection.QueryAsync<AnnouncementDto>(sql).ConfigureAwait(false);
            }
        }

        public async Task<AnnouncementDto> GetLatestAnnouncement(CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                var sql = @"SELECT TOP 1 [ID],[Title],[Content],[CreatedAt],[CreatedBy],[IsExpired], [ModifiedAt], [ExpiredAt] FROM [cms].[Announcement] " +
                    " WHERE IsExpired = 0 OR ExpiredAt < GETDATE() ORDER BY CreatedAt DESC;";

                return await connection.QuerySingleOrDefaultAsync<AnnouncementDto>(sql).ConfigureAwait(false);
            }
        }
    }
}
