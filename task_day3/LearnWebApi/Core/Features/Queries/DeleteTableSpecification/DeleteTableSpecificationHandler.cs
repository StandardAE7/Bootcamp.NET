using MediatR;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Queries.DeleteTableSpecification
{
    public class DeleteTableSpecificationHandler : IRequestHandler<DeleteTableSpecificationQuery, DeleteTableSpecificationResponse>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;

        public DeleteTableSpecificationHandler(ITableSpecificationRepository tableSpecificationRepository)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
        }

        public async Task<DeleteTableSpecificationResponse> Handle(DeleteTableSpecificationQuery request, CancellationToken cancellationToken)
        {
            var tableSpecification = _tableSpecificationRepository.GetById(request.TableId);
            if (tableSpecification == null)
            {
                return new DeleteTableSpecificationResponse { Success = false };
            }

            _tableSpecificationRepository.Remove(tableSpecification);
            await _tableSpecificationRepository.SaveChangesAsync();

            return new DeleteTableSpecificationResponse { Success = true };
        }
    }
}
