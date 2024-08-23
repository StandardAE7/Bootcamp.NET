using Persistence.DatabaseContext;
using Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public class TableSpecificationRepository : GenericRepository<TableSpecification>, ITableSpecificationRepository
{
    public TableSpecificationRepository(TableContext context) : base(context)
    {
    }

    public async Task<TableSpecification?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TableSpecification>().FirstOrDefaultAsync(ts => ts.TableId == id);
    }

    public void Add(TableSpecification entity)
    {
        _context.Set<TableSpecification>().Add(entity);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
