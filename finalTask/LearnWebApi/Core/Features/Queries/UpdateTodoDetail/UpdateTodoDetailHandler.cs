using MediatR;
using Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.UpdateTodoDetail
{
    public class UpdateTodoDetailHandler : IRequestHandler<UpdateTodoDetailQuery, Unit>
    {
        private readonly ITodoRepository _todoRepository;

        public UpdateTodoDetailHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(UpdateTodoDetailQuery request, CancellationToken cancellationToken)
        {
            var todoDetail = new Persistence.Models.TodoDetail
            {
                TodoDetailId = request.TodoDetailId,
                Activity = request.Activity,
                Category = request.Category,
                DetailNote = request.DetailNote
            };

            await _todoRepository.UpdateTodoDetailAsync(todoDetail);

            return Unit.Value;
        }
    }
}
