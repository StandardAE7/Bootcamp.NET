using StackExchange.Redis;
using Newtonsoft.Json;
using Persistence.Models;

public class TodoService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public TodoService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public async Task SaveTodoAsync(Guid todoId, Todo todo)
    {
        var json = JsonConvert.SerializeObject(todo);
        await _db.StringSetAsync($"todo:{todoId}", json);
    }

    public async Task<Todo> GetTodoAsync(Guid todoId)
    {
        var json = await _db.StringGetAsync($"todo:{todoId}");
        return json.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Todo>(json);
    }
}
