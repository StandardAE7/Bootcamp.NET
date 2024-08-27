using MediatR;
using System.Collections.Generic;

namespace Core.Features.Queries.AddTodos
{
    public class InsertTodoBulkCommand : IRequest<List<Guid>>
    {
        public List<TodoRequest> Todos { get; set; }
    }

    public class TodoRequest
    {
        public string Day { get; set; }
        public string Note { get; set; }
    }
}
