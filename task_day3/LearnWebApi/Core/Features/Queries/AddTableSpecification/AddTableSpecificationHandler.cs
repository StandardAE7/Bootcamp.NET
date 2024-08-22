using MediatR;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Queries.AddTableSpecification
{
    public class AddTableSpecificationHandler : IRequestHandler<AddTableSpecificationQuery, AddTableSpecificationResponse>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;

        public AddTableSpecificationHandler(ITableSpecificationRepository tableSpecificationRepository)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
        }

        public async Task<AddTableSpecificationResponse> Handle(AddTableSpecificationQuery request, CancellationToken cancellationToken)
        {
            var newTableSpecification = new TableSpecification
            {
                TableId = Guid.NewGuid(), // Or use a different method to generate IDs
                TableNumber = request.TableNumber,
                ChairNumber = request.ChairNumber,
                TablePic = request.TablePic,
                TableType = request.TableType
            };

            // Add the new table specification to the repository
            // Note: Ensure that the repository method is properly defined for adding entities
            _tableSpecificationRepository.Add(newTableSpecification);
            await _tableSpecificationRepository.SaveChangesAsync(); // Ensure this method is implemented for saving changes

            return new AddTableSpecificationResponse
            {
                TableId = newTableSpecification.TableId,
                Success = true
            };
        }
    }
}
