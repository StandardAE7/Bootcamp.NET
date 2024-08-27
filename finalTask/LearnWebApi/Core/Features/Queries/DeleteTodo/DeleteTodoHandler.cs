using MediatR;
using Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.DeleteTodo
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoQuery, Unit>
    {
        private readonly ITodoRepository _todoRepository;

        public DeleteTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(DeleteTodoQuery request, CancellationToken cancellationToken)
        {
            await _todoRepository.DeleteTodoAsync(request.TodoId);

            // Recalculate DetailCount for all todos after deletion
            await _todoRepository.UpdateAllDetailCountsAsync();

            return Unit.Value;
        }
    }

}

