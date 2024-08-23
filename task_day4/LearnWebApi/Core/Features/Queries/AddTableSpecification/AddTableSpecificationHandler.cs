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

namespace Core.Features.Queries.AddTableSpecification
{
    public class AddTableSpecificationHandler : IRequestHandler<AddTableSpecificationQuery, AddTableSpecificationResponse>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;
        private readonly IDistributedCache _cache;

        public AddTableSpecificationHandler(ITableSpecificationRepository tableSpecificationRepository, IDistributedCache cache)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
            _cache = cache;
        }

        public async Task<AddTableSpecificationResponse> Handle(AddTableSpecificationQuery request, CancellationToken cancellationToken)
        {
            var newTableSpecification = new TableSpecification
            {
                TableId = Guid.NewGuid(),
                TableNumber = request.TableNumber,
                ChairNumber = request.ChairNumber,
                TablePic = request.TablePic,
                TableType = request.TableType
            };

            _tableSpecificationRepository.Add(newTableSpecification);
            await _tableSpecificationRepository.SaveChangesAsync();

            var response = new AddTableSpecificationResponse
            {
                TableId = newTableSpecification.TableId,
                Success = true
            };

            try
            {
                var cacheKey = $"TableSpecification_{newTableSpecification.TableId}";
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                var serializedData = JsonSerializer.Serialize(newTableSpecification);

                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);
            }
            catch (RedisConnectionException)
            { }

            return response;
        }
    }
}
