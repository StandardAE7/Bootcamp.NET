using Persistence.DatabaseContext;

namespace Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly TableContext _context;

    public GenericRepository(TableContext context)
    {
        _context = context;
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetById(Guid id)
    {
        return _context.Set<T>().Find(id);
    }

    public void Remove(T itemToRemove)
    {
        _context.Set<T>().Remove(itemToRemove);
    }

    public void Add(T itemToAdd)
    {
        _context.Set<T>().Add(itemToAdd);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(T itemToUpdate)
    {
        _context.Set<T>().Update(itemToUpdate);
    }
}