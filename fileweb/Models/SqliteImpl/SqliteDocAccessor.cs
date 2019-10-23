using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SQLite;
using EnsureThat;

namespace fileweb.Models.SqliteImpl
{
    public class SqliteDocAccessor
        : IDocAccessor
    {
        private readonly string _connectionString;

        public SqliteDocAccessor(string connectionString)
        {
            Ensure.That(connectionString, nameof(connectionString)).IsNotEmptyOrWhitespace();

            this._connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllCategory1(CancellationToken cancellationToken = default)
        {
            var result = new List<string>();

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT Category1 FROM files GROUP BY Category1";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category1"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DocDto>> GetDocDtos(string category1, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(category1))
                category1 = null;

            var result = new List<DocDto>();

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT Id, Category1, Category2, Category3, Category4, Title, Description, Url, NewWindow, Visible, Icon FROM files WHERE Category1 = IFNULL(@Category1, Category1)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@Category1", category1 == null ? DBNull.Value : (object)category1));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new DocDto()
                            {
                                Id = reader.GetInt32(0),
                                Category1 = reader.GetString(1),
                                Category2 = reader.GetString(2),
                                Category3 = reader.GetString(3),
                                Category4 = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Title = reader.GetString(5),
                                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Url = reader.IsDBNull(7) ? null : reader.GetString(7),
                                NewWindow = reader.GetInt32(8) > 0,
                                Visible = reader.GetInt32(9) > 0,
                                Icon = reader.IsDBNull(10) ? null : reader.GetString(10)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
