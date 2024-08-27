using MediatR;
using System;

namespace Core.Features.Queries.DeleteTodoDetail
{
    public class DeleteTodoDetailQuery : IRequest<Unit>
    {
        public Guid TodoDetailId { get; set; }
    }
}
