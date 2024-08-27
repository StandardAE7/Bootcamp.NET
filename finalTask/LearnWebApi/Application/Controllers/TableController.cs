using Core.Features.Queries.GetTableSpecifications;
using MediatR;
using Persistence.DatabaseContext;
using Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace Application.Controllers;

public class TableController : BaseController
{
    private readonly IMediator _mediator;

    public TableController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("v1/table/specification/{id}")]
    public async Task<GetTableSpecificationsResponse> GetTableSpecifications(Guid id)
    {
        var request = new GetTableSpecificationsQuery()
        {
            TableSpecificationId = id
        };
        var response = await _mediator.Send(request);
        return response;
    }
}