using Core.Features.Queries.DeleteTodoDetail;
using MediatR;
using Persistence.Repositories;

public class DeleteTodoDetailHandler : IRequestHandler<DeleteTodoDetailQuery, Unit>
{
    private readonly ITodoRepository _todoRepository;

    public DeleteTodoDetailHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Unit> Handle(DeleteTodoDetailQuery request, CancellationToken cancellationToken)
    {
        var todoDetail = await _todoRepository.GetTodoDetailByIdAsync(request.TodoDetailId);
        if (todoDetail != null)
        {
            await _todoRepository.DeleteTodoDetailAsync(request.TodoDetailId);

            // Update DetailCount for the related Todo item
            await _todoRepository.UpdateDetailCountAsync(todoDetail.TodoId);
        }

        return Unit.Value;
    }
}
