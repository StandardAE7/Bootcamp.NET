using MediatR;
using Persistence.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Queries.GetTodoAll;

public class GetTodoAllHandler : IRequestHandler<GetTodoAllQuery, GetTodoAllResponse>
{
    private readonly ITodoRepository _todoRepository;

    public GetTodoAllHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<GetTodoAllResponse> Handle(GetTodoAllQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = 1;
        var pageSize = 100;


        // Recalculate DetailCount for all Todo items
        await _todoRepository.UpdateAllTodoDetailCountsAsync();

        var todos = await _todoRepository
            .GetTodosWithDetails(pageNumber, pageSize)
            .ToListAsync(cancellationToken);

        var todoResponses = todos.Select(todo => new TodoResponse
        {
            TodoId = todo.TodoId,
            Day = todo.Day,
            TodayDate = todo.TodayDate,
            Note = todo.Note,
            DetailCount = todo.TodoDetails.Count,
            TodoDetails = todo.TodoDetails.Select(detail => new TodoDetailResponse
            {
                TodoDetailId = detail.TodoDetailId,
                Activity = detail.Activity,
                Category = detail.Category,
                DetailNote = detail.DetailNote
            }).ToList()
        }).ToList();

        return new GetTodoAllResponse
        {
            Todo = todoResponses,
            TotalCount = await _todoRepository.CountAsync()
        };
    }
}

