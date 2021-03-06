﻿using EnsureThat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fileweb.Models.SqlServer
{
    public class SqlServerDocAccessor
        : IDocAccessor
    {
        private readonly string _connectionString;

        public SqlServerDocAccessor(string connectionString)
        {
            Ensure.That(connectionString, nameof(connectionString)).IsNotEmptyOrWhitespace();

            this._connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllCategory2(CancellationToken cancellationToken = default)
        {
            var result = new List<string>();

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                var sql = @"SELECT Category2 FROM cms.files GROUP BY Category2";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DocsCategoryModel>> GetAllCategories(CancellationToken cancellationToken = default)
        {
            var result = new List<DocsCategoryModel>();

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                var sql = @"SELECT Category1,Category2,Category3,Url FROM cms.files";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            result.Add(new DocsCategoryModel
                            {
                                Category1 = reader.GetString(0),
                                Category2 = reader.GetString(1),
                                Category3 = reader.GetString(2),
                                Url = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category2"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DocDto>> GetDocDtos(string category2, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(category2))
                category2 = null;

            var result = new List<DocDto>();

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                var sql = @"SELECT Id, Category1, Category2, Category3, Title, Description, Url, NewWindow, Visible, Icon FROM cms.files WHERE Category2 = ISNULL(@Category2, Category2)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@Category2", category2 == null ? DBNull.Value : (object)category2));

                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            result.Add(new DocDto()
                            {
                                Id = reader.GetInt32(0),
                                Category1 = reader.GetString(1),
                                Category2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Category3 = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Title = reader.GetString(4),
                                Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Url = reader.IsDBNull(6) ? null : reader.GetString(6),
                                NewWindow = reader.GetBoolean(7),
                                Visible = reader.GetBoolean(8),
                                Icon = reader.IsDBNull(9) ? null : reader.GetString(9)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
