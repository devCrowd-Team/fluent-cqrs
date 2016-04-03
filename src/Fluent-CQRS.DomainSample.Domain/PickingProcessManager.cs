using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Commands;
using Fluent_CQRS.DomainSample.Contracts.Events;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.DomainSample.Domain
{
    class PickingProcessManager : ProcessManager
    {
        public PickingProcessManager(string id, IEnumerable<IAmAnEventMessage> history)
            : base(id, history)
        {
            Receive<OrderlinePickingFailed>(e => { });
            Receive<OrderlinePickingSucceeded>(e => { });
            Receive<ProductStocked>(e => RetryFailedPickings());
        }

        IEnumerable<OrderlinePickingFailed> StillFailingOrderLines
        {
            get
            {
                var failed = MessagesOfType<OrderlinePickingFailed>().ToList();
                var succeeded = MessagesOfType<OrderlinePickingSucceeded>().ToList();

                return failed
                    .Select(o => o.OrderId)
                        .Except(succeeded.Select(o => o.OrderId))
                        .Distinct()
                        .Select(orderId => failed.First(o => o.OrderId == orderId));
            }
        }

        private void RetryFailedPickings()
        {
            StillFailingOrderLines
                .Select(line => new PickOrderline { Id = Id, OrderId = line.OrderId, Quantity = line.Quantity })
                .ToList()
                .ForEach(cmd => Commands.Add(cmd));
        }
    }
}
