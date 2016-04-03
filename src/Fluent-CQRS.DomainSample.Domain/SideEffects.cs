using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Commands;
using System;

namespace Fluent_CQRS.DomainSample.Domain
{
    class SideEffects:Handler<IAmACommandMessage>
    {
        public SideEffects()
        {
            Receive<ShipOrder>(cmd => OnShipOrder(cmd));
            Receive<NotifyAboutDelayForOrder>(cmd => OnNotifyAboutDelayForOrder(cmd));
        }

        private void OnShipOrder(ShipOrder cmd)
        {
            Console.WriteLine();
            Console.WriteLine($"====Shipment completed \"{cmd.Id}\"  ====");
            Console.WriteLine();
        }

        private void OnNotifyAboutDelayForOrder(NotifyAboutDelayForOrder cmd)
        {
            Console.WriteLine();
            Console.WriteLine($"====Shipment delayed \"{cmd.Id}\" ====");
            Console.WriteLine();
        }
    }
}
