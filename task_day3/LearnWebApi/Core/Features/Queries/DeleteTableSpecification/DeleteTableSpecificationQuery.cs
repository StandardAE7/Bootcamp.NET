using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Queries.DeleteTableSpecification
{
    public class DeleteTableSpecificationQuery : IRequest<DeleteTableSpecificationResponse>
    {
        public Guid TableId { get; set; }
    }
}
