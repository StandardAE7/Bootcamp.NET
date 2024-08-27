using MediatR;
using Persistence.Repositories;

namespace Core.Features.Queries.GetTableSpecifications;

public class GetTableSpecificationsHandler : IRequestHandler<GetTableSpecificationsQuery, GetTableSpecificationsResponse>
{
    private readonly ITableSpecificationRepository _tableSpecificationRepository;

    public GetTableSpecificationsHandler(ITableSpecificationRepository tableSpecificationRepository)
    {
        _tableSpecificationRepository = tableSpecificationRepository;
    }

    public async Task<GetTableSpecificationsResponse> Handle(GetTableSpecificationsQuery query, CancellationToken cancellationToken)
    {
        var tableSpecification = _tableSpecificationRepository.GetById(query.TableSpecificationId);
        
        if (tableSpecification is null)
            return new GetTableSpecificationsResponse();
        
        var response = new GetTableSpecificationsResponse()
        {
            TableId = tableSpecification.TableId,
            ChairNumber = tableSpecification.ChairNumber,
            TableNumber = tableSpecification.TableNumber,
            TablePic = tableSpecification.TablePic,
            TableType = tableSpecification.TableType
        };
        
        return response;
    }
}