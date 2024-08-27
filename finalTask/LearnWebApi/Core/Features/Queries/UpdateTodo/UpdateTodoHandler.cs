using MediatR;
using Persistence.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.UpdateTodo
{
    public class UpdateTodoHandler : IRequestHandler<UpdateTodoQuery, Unit>
    {
        private readonly ITodoRepository _todoRepository;

        public UpdateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(UpdateTodoQuery request, CancellationToken cancellationToken)
        {
            var todo = new Persistence.Models.Todo
            {
                TodoId = request.TodoId,
                Day = request.Day,
                Note = request.Note,
                TodayDate = DateTime.UtcNow
            };

            await _todoRepository.UpdateTodoAsync(todo);

            return Unit.Value;
        }
    }
}
