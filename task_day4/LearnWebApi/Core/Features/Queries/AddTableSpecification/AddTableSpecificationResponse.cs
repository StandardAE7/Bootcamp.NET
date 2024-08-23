using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Queries.AddTableSpecification
{
    public class AddTableSpecificationResponse
    {
        public Guid TableId { get; set; }
        public bool Success { get; set; }
    }
}