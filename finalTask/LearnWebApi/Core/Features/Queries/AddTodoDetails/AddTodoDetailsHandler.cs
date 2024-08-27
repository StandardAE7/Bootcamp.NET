using Core.Features.Query.AddTodoDetails;
using MediatR;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.AddTodoDetails;
public class InsertTodoDetailBulkHandler : IRequestHandler<InsertTodoDetailBulkCommand, List<Guid>>
{
    private readonly ITodoRepository _todoRepository;

    public InsertTodoDetailBulkHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<List<Guid>> Handle(InsertTodoDetailBulkCommand request, CancellationToken cancellationToken)
    {
        // Validate categories
        foreach (var detail in request.TodoDetails)
        {
            if (detail.Category != "Task" && detail.Category != "DailyActivity")
            {
                throw new ArgumentException($"Invalid Category: {detail.Category}. Allowed values are 'Task' or 'DailyActivity'.");
            }
        }

        var todoDetails = request.TodoDetails.Select(detail => new Persistence.Models.TodoDetail
        {
            TodoDetailId = Guid.NewGuid(),
            Activity = detail.Activity,
            Category = detail.Category,
            DetailNote = detail.DetailNote,
            TodoId = detail.TodoId
        }).ToList();

        var todoIds = todoDetails.Select(detail => detail.TodoId).Distinct().ToList();

        await _todoRepository.BulkInsertTodoDetailAsync(todoDetails);

        // Update DetailCount for all affected Todo items
        foreach (var todoId in todoIds)
        {
            await _todoRepository.UpdateDetailCountAsync(todoId);
        }

        return todoDetails.Select(detail => detail.TodoDetailId).ToList();
    }
}
