using MediatR;
using System;

namespace Core.Features.Queries.UpdateTodoDetail;
public class UpdateTodoDetailQuery : IRequest<Unit>
{
    public Guid TodoDetailId { get; set; }
    public string Activity { get; set; }
    public string Category { get; set; }
    public string DetailNote { get; set; }
}
