using MediatR;
using Persistence.Models;
using Persistence.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using StackExchange.Redis;

namespace Core.Features.Queries.GetTableSpecifications
{
    public class GetTableSpecificationsHandler : IRequestHandler<GetTableSpecificationsQuery, GetTableSpecificationsResponse>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;
        private readonly IDistributedCache _cache;

        public GetTableSpecificationsHandler(ITableSpecificationRepository tableSpecificationRepository, IDistributedCache cache)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
            _cache = cache;
        }

        public async Task<GetTableSpecificationsResponse> Handle(GetTableSpecificationsQuery query, CancellationToken cancellationToken)
        {
            string cacheKey = $"TableSpecification_{query.TableSpecificationId}";
            GetTableSpecificationsResponse response;

            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
                if (cachedData != null)
                {
                    response = JsonSerializer.Deserialize<GetTableSpecificationsResponse>(cachedData);
                    response.DataSource = "Redis";
                    return response;
                }
            }
            catch (RedisConnectionException)
            { }

            var tableSpecification = await _tableSpecificationRepository.GetByIdAsync(query.TableSpecificationId);

            if (tableSpecification == null)
                return new GetTableSpecificationsResponse();

            response = new GetTableSpecificationsResponse()
            {
                TableId = tableSpecification.TableId,
                ChairNumber = tableSpecification.ChairNumber,
                TableNumber = tableSpecification.TableNumber,
                TablePic = tableSpecification.TablePic,
                TableType = tableSpecification.TableType,
                DataSource = "SQL"
            };

            try
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                var serializedData = JsonSerializer.Serialize(response);

                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);
            }
            catch (RedisConnectionException)
            { }

            return response;
        }
    }
}
