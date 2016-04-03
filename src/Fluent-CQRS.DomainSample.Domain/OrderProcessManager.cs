using Fluent_CQRS.DomainSample.Contracts.Events;
using Fluent_CQRS.DomainSample.Contracts.Commands;
using System.Collections.Generic;
using Fluent_CQRS;
using System.Linq;
using Stateless;
using Fluent_CQRS.DomainSample.Contracts.Values;
using System;

namespace Fluent_CQRS.DomainSample.Domain
{
    internal class OrderProcessManager : ProcessManager
    {
        private StateMachine<State, Trigger> statemachine;

        public OrderProcessManager(string id, IEnumerable<IAmAnEventMessage> history)
            : base(id, history)
        {
            SetupStateMachine();
            SetupReceivers();
            ReplayHistory();
        }

        private void SetupStateMachine()
        {
            statemachine = new StateMachine<State, Trigger>(State.New);

            statemachine.Configure(State.New)
                .Permit(Trigger.OrderReceived, State.Open);

            statemachine.Configure(State.Open)
                .OnEntry(() => PrepareProductsForShipment())
                .PermitIf(Trigger.LineItemPicked, State.Completed, () => ReadyForShipping)
                .IgnoreIf(Trigger.LineItemPicked, () => !ReadyForShipping)
                .Permit(Trigger.ProductNotAvailable, State.Deferred);

            statemachine.Configure(State.Deferred)
                .OnEntry(() => NotifyAboutDelay())
                .SubstateOf(State.Open)
                .Ignore(Trigger.ProductNotAvailable);

            statemachine.Configure(State.Completed)
                .OnEntry(() => ShipOrder());
        }

        public  void SetupReceivers()
        {
            Receive<OrderPlaced>(@event => statemachine.Fire(Trigger.OrderReceived));
            Receive<OrderlinePickingSucceeded>(@event => statemachine.Fire(Trigger.LineItemPicked));
            Receive<OrderlinePickingFailed>(@event => statemachine.Fire(Trigger.ProductNotAvailable));
        }

        IEnumerable<string> CompletedProductIds => MessagesOfType<OrderlinePickingSucceeded>().Select(p => p.Id);

        IEnumerable<Orderline> OrderLines => MessagesOfType<OrderPlaced>().First().OrderLines;

        bool ReadyForShipping =>  OrderLines.All(lineItem => CompletedProductIds.Any(id => id == lineItem.ProductId));
        
        private void PrepareProductsForShipment()
        {
            foreach (var line in OrderLines)
            {
                Commands.Add(new PickOrderline { Id = line.ProductId, Quantity = line.Quantity, OrderId = Id });
            }
        }

        private void ShipOrder()
        {
            Commands.Add(new ShipOrder { Id = Id });
        }

        private void NotifyAboutDelay()
        {
            Commands.Add(new NotifyAboutDelayForOrder { Id = Id });
        }

        enum State
        {
            New,
            Open,
            Deferred,
            Completed
        }

        enum Trigger
        {
            OrderReceived,
            LineItemPicked,
            ProductNotAvailable
        }
    }
}