using Persistence.Models;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public interface ITableSpecificationRepository : IGenericRepository<TableSpecification>
{
    Task<TableSpecification?> GetByIdAsync(Guid id);
    void Add(TableSpecification entity);
    Task SaveChangesAsync();
}
