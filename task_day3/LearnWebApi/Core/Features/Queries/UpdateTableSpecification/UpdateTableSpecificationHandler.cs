using MediatR;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Queries.UpdateTableSpecification
{
    public class UpdateTableSpecificationHandler : IRequestHandler<UpdateTableSpecificationQuery, UpdateTableSpecificationResponse>
    {
        private readonly ITableSpecificationRepository _tableSpecificationRepository;

        public UpdateTableSpecificationHandler(ITableSpecificationRepository tableSpecificationRepository)
        {
            _tableSpecificationRepository = tableSpecificationRepository;
        }

        public async Task<UpdateTableSpecificationResponse> Handle(UpdateTableSpecificationQuery request, CancellationToken cancellationToken)
        {
            var tableSpecification = _tableSpecificationRepository.GetById(request.TableId);
            if (tableSpecification == null)
            {
                return new UpdateTableSpecificationResponse { Success = false };
            }

            tableSpecification.TableNumber = request.TableNumber;
            tableSpecification.ChairNumber = request.ChairNumber;
            tableSpecification.TablePic = request.TablePic;
            tableSpecification.TableType = request.TableType;

            _tableSpecificationRepository.Update(tableSpecification);
            await _tableSpecificationRepository.SaveChangesAsync();

            return new UpdateTableSpecificationResponse { Success = true };
        }
    }
}
