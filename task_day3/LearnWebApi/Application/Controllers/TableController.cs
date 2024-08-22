using Core.Features.Queries.GetTableSpecifications;
using MediatR;
using Persistence.DatabaseContext;
using Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Core.Features.Queries.GetAllTableSpecifications;
using Core.Features.Queries.AddTableSpecification;
using Core.Features.Queries.UpdateTableSpecification;
using Core.Features.Queries.DeleteTableSpecification;

namespace Application.Controllers;

public class TableController : BaseController
{
    private readonly IMediator _mediator;

    public TableController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("v1/table/specification")]
    public async Task<List<GetTableSpecificationsResponse>> GetAllTableSpecifications()
    {
        var query = new GetAllTableSpecificationsQuery();
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpGet("v1/table/specification/{id}")]
    public async Task<GetTableSpecificationsResponse> GetTableSpecifications(Guid id)
    {
        var query = new GetTableSpecificationsQuery()
        {
            TableSpecificationId = id
        };
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpPost("v1/table/specification")]
    public async Task<IActionResult> AddTableSpecification([FromBody] AddTableSpecificationQuery query)
    {
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return CreatedAtAction(nameof(GetTableSpecifications), new { id = response.TableId }, response);
        }
        return BadRequest("Error adding table specification.");
    }

    [HttpPut("v1/table/specification")]
    public async Task<IActionResult> UpdateTableSpecification([FromBody] UpdateTableSpecificationQuery query)
    {
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound("Table specification not found.");
    }

    [HttpDelete("v1/table/specification/{id}")]
    public async Task<DeleteTableSpecificationResponse> DeleteTableSpecification(Guid id)
    {
        var query = new DeleteTableSpecificationQuery { TableId = id };
        var response = await _mediator.Send(query);

        return response;
    }
}