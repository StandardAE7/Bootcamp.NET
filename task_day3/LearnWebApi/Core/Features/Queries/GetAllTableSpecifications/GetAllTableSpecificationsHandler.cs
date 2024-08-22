using MediatR;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Queries.GetTableSpecifications;

namespace Core.Features.Queries.GetAllTableSpecifications
{
    public class GetAllTableSpecificationsHandler : IRequestHandler<GetAllTableSpecificationsQuery, List<GetTableSpecificationsResponse>>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;

        public GetAllTableSpecificationsHandler(ITableSpecificationRepository tableSpecificationRepository)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
        }

        public async Task<List<GetTableSpecificationsResponse>> Handle(GetAllTableSpecificationsQuery query, CancellationToken cancellationToken)
        {
            var tableSpecifications = _tableSpecificationRepository.GetAll();

            var response = new List<GetTableSpecificationsResponse>();

            foreach (var spec in tableSpecifications)
            {
                response.Add(new GetTableSpecificationsResponse
                {
                    TableId = spec.TableId,
                    TableNumber = spec.TableNumber,
                    ChairNumber = spec.ChairNumber,
                    TablePic = spec.TablePic,
                    TableType = spec.TableType
                });
            }

            return response;
        }
    }
}
