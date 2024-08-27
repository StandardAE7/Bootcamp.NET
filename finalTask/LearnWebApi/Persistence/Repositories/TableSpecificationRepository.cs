using Persistence.DatabaseContext;
using Persistence.Models;

namespace Persistence.Repositories;

public class TableSpecificationRepository : GenericRepository<TableSpecification>, ITableSpecificationRepository
{
    public TableSpecificationRepository(TableContext context) : base(context)
    {
    }
    
}