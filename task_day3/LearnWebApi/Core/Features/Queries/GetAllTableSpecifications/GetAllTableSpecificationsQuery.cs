using MediatR;
using System.Collections.Generic;
using Core.Features.Queries.GetTableSpecifications;

namespace Core.Features.Queries.GetAllTableSpecifications
{
    public class GetAllTableSpecificationsQuery : IRequest<List<GetTableSpecificationsResponse>>
    {
    }
}
