using MediatR;
using System;
using System.Collections.Generic;

namespace Core.Features.Query.AddTodoDetails
{
    public class InsertTodoDetailBulkCommand : IRequest<List<Guid>>
    {
        public List<TodoDetailRequest> TodoDetails { get; set; }
    }

    public class TodoDetailRequest
    {
        public string Activity { get; set; }
        public string Category { get; set; }
        public string DetailNote { get; set; }
        public Guid TodoId { get; set; }
    }
}
