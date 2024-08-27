using System.Linq;
using Persistence.Models;

namespace Persistence.Repositories;

public interface ITodoRepository
{
    IQueryable<Todo> GetTodosWithDetails(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task BulkInsertTodoAsync(List<Todo> todos);
    Task BulkInsertTodoDetailAsync(List<TodoDetail> todoDetails);
    Task<bool> TodoExistsAsync(Guid todoId);
    Task<int> GetTodoDetailsCountByTodoIdAsync(Guid todoId);
    Task UpdateTodoAsync(Todo todo);
    Task UpdateTodoDetailAsync(TodoDetail detail);
    Task DeleteTodoAsync(Guid todoId);
    Task DeleteTodoDetailAsync(Guid todoDetailId);
    Task UpdateDetailCountAsync(Guid todoId);
    Task UpdateAllDetailCountsAsync();
    Task<TodoDetail> GetTodoDetailByIdAsync(Guid todoDetailId);
    Task UpdateAllTodoDetailCountsAsync();
}
