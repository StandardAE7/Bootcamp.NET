using MediatR;
using System;

namespace Core.Features.Queries.UpdateTodo
{
    public class UpdateTodoQuery : IRequest<Unit>
    {
        public Guid TodoId { get; set; }
        public string Day { get; set; }
        public string Note { get; set; }
    }
}
