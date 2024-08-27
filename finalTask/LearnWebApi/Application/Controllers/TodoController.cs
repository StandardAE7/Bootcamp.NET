using Core.Features.Queries.DeleteTodo;
using Core.Features.Queries.DeleteTodoDetail;
using Core.Features.Queries.AddTodos;
using Core.Features.Queries.AddTodoDetails;
using Core.Features.Queries.UpdateTodo;
using Core.Features.Queries.UpdateTodoDetail;
using Core.Features.Queries.GetTodoAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Core.Features.Query.AddTodoDetails;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;
        private readonly IDistributedCache _cache;

        public TodoController(IMediator mediator, AuthService authService, IDistributedCache cache)
        {
            _mediator = mediator;
            _authService = authService;
            _cache = cache;
        }

        [HttpGet("v1/todo/all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTodoAll()
        {
            var cacheKey = "todos_all";
            var cachedTodos = await _cache.GetStringAsync(cacheKey);

            if (cachedTodos != null)
            {
                var todos = JsonConvert.DeserializeObject<GetTodoAllResponse>(cachedTodos);
                return Ok(todos);
            }

            var request = new GetTodoAllQuery();
            var todosResult = await _mediator.Send(request);

            // Cache the result
            var serializedTodos = JsonConvert.SerializeObject(todosResult);
            await _cache.SetStringAsync(cacheKey, serializedTodos, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
            });

            return Ok(todosResult);
        }

        [HttpPost("v1/todo/insert")]
        [Authorize(Roles = "admin,user")]
        [IsNotLoggedOut]
        public async Task<IActionResult> InsertTodoBulk([FromBody] InsertTodoBulkCommand command)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (_authService.IsTokenBlacklisted(token))
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(command);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return Ok(result);
        }

        [HttpPost("v1/todo-detail/insert")]
        [Authorize(Roles = "admin,user")]
        [IsNotLoggedOut]
        public async Task<IActionResult> InsertTodoDetailBulk([FromBody] InsertTodoDetailBulkCommand command)
        {
            var result = await _mediator.Send(command);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return Ok(result);
        }

        [HttpPut("v1/todo/update")]
        [Authorize(Roles = "admin,user")]
        [IsNotLoggedOut]
        public async Task<IActionResult> UpdateTodo([FromBody] UpdateTodoQuery request)
        {
            await _mediator.Send(request);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return Ok();
        }

        [HttpPut("v1/todo-detail/update")]
        [Authorize(Roles = "admin,user")]
        [IsNotLoggedOut]
        public async Task<IActionResult> UpdateTodoDetail([FromBody] UpdateTodoDetailQuery request)
        {
            await _mediator.Send(request);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return Ok();
        }

        [HttpDelete("v1/todo/delete/{todoId}")]
        [Authorize(Roles = "admin")]
        [IsNotLoggedOut]
        public async Task<IActionResult> DeleteTodo(Guid todoId)
        {
            var command = new DeleteTodoQuery { TodoId = todoId };
            await _mediator.Send(command);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return NoContent();
        }

        [HttpDelete("v1/todo-detail/delete/{todoDetailId}")]
        [Authorize(Roles = "admin")]
        [IsNotLoggedOut]
        public async Task<IActionResult> DeleteTodoDetail(Guid todoDetailId)
        {
            var command = new DeleteTodoDetailQuery { TodoDetailId = todoDetailId };
            await _mediator.Send(command);

            // Clear the cache
            await _cache.RemoveAsync("todos_all");

            return NoContent();
        }
    }
}
