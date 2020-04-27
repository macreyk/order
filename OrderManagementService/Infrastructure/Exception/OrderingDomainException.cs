using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementService.Infrastructure.Exception
{
    public class OrderingDomainException: System.Exception
    {
        public OrderingDomainException()
        { }

        public OrderingDomainException(string message)
            : base(message)
        { }

        public OrderingDomainException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
