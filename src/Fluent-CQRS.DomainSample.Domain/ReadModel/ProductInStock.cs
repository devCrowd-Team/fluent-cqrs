using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.DomainSample.Domain.ReadModel
{
    class ProductInStock
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
