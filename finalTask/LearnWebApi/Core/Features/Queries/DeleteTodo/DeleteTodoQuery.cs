using MediatR;
using System;

namespace Core.Features.Queries.DeleteTodo
{
    public class DeleteTodoQuery : IRequest<Unit>
    {
        public Guid TodoId { get; set; }
    }
}
