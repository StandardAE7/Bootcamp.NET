using MediatR;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.AddTodos
{
    public class InsertTodoBulkHandler : IRequestHandler<InsertTodoBulkCommand, List<Guid>>
    {
        private readonly ITodoRepository _todoRepository;

        public InsertTodoBulkHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<List<Guid>> Handle(InsertTodoBulkCommand request, CancellationToken cancellationToken)
        {
            var todos = request.Todos.Select(todo => new Persistence.Models.Todo
            {
                TodoId = Guid.NewGuid(),
                Day = todo.Day,
                Note = todo.Note,
                TodayDate = DateTime.UtcNow
            }).ToList();

            var ids = todos.Select(todo => todo.TodoId).ToList();
            await _todoRepository.BulkInsertTodoAsync(todos);

            // Recalculate DetailCount for all todos
            foreach (var todoId in ids)
            {
                await _todoRepository.UpdateDetailCountAsync(todoId);
            }

            return ids;
        }
    }


}
