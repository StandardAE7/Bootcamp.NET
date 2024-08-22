namespace Persistence.Repositories;

public interface IGenericRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(Guid id);
    void Remove(T itemToRemove);
    void Add(T itemToAdd);
    Task SaveChangesAsync();
    void Update(T itemToUpdate);
}