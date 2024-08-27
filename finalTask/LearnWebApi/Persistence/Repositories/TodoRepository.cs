using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public IQueryable<Todo> GetTodosWithDetails(int pageNumber, int pageSize)
        {
            return _context.Todo
                .Include(todo => todo.TodoDetails)
                .OrderBy(todo => todo.TodayDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Todo.CountAsync();
        }

        public async Task BulkInsertTodoAsync(List<Todo> todos)
        {
            await _context.Todo.AddRangeAsync(todos);
            await _context.SaveChangesAsync();
        }

        public async Task BulkInsertTodoDetailAsync(List<TodoDetail> todoDetails)
        {
            await _context.TodoDetail.AddRangeAsync(todoDetails);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TodoExistsAsync(Guid todoId)
        {
            return await _context.Todo.AnyAsync(t => t.TodoId == todoId);
        }

        public async Task<int> GetTodoDetailsCountByTodoIdAsync(Guid todoId)
        {
            return await _context.TodoDetail.CountAsync(td => td.TodoId == todoId);
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            _context.Todo.Update(todo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTodoDetailAsync(TodoDetail detail)
        {
            _context.TodoDetail.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoAsync(Guid todoId)
        {
            var todo = await _context.Todo.FindAsync(todoId);
            if (todo != null)
            {
                _context.Todo.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTodoDetailAsync(Guid todoDetailId)
        {
            var todoDetail = await _context.TodoDetail.FindAsync(todoDetailId);
            if (todoDetail != null)
            {
                _context.TodoDetail.Remove(todoDetail);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateDetailCountAsync(Guid todoId)
        {
            var todo = await _context.Todo
                .Include(t => t.TodoDetails)
                .SingleOrDefaultAsync(t => t.TodoId == todoId);

            if (todo != null)
            {
                todo.DetailCount = todo.TodoDetails.Count;
                _context.Todo.Update(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAllDetailCountsAsync()
        {
            var todos = await _context.Todo.Include(t => t.TodoDetails).ToListAsync();

            foreach (var todo in todos)
            {
                todo.DetailCount = todo.TodoDetails.Count;
            }

            _context.Todo.UpdateRange(todos);
            await _context.SaveChangesAsync();
        }
        public async Task<TodoDetail> GetTodoDetailByIdAsync(Guid todoDetailId)
        {
            return await _context.TodoDetail
                .Where(td => td.TodoDetailId == todoDetailId)
                .Include(td => td.Todo)
                .SingleOrDefaultAsync();
        }
        public async Task UpdateAllTodoDetailCountsAsync()
        {
            var todos = await _context.Todo.Include(t => t.TodoDetails).ToListAsync();

            foreach (var todo in todos)
            {
                todo.DetailCount = todo.TodoDetails.Count;
            }

            _context.Todo.UpdateRange(todos);
            await _context.SaveChangesAsync();
        }
    }
}
