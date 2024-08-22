using Persistence.Models;

namespace Persistence.Repositories;

public interface ITableSpecificationRepository : IGenericRepository<TableSpecification>
{
    TableSpecification GetById(Guid id);
    List<TableSpecification> GetAll();
}