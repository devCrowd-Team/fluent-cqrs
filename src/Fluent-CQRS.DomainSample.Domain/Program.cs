using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Commands;
using Fluent_CQRS.DomainSample.Contracts.Values;
using Fluent_CQRS.DomainSample.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Fluent_CQRS.DomainSample.Domain
{
    class Program
    {

        private static IList<ProductInStock> _productsInStockReadModel;

        private static Dispatch<IAmACommandMessage> _commandHandlers;

        static void Main(string[] args)
        {
            Bootstrap();

            _commandHandlers.Tell(new DefineProduct { Name = "Apple", Id = "AppleId" });
            _commandHandlers.Tell(new DefineProduct { Name = "Banana", Id = "BananaId" });
            _commandHandlers.Tell(new DefineProduct { Name = "Orange", Id = "OrangeId" });

            _commandHandlers.Tell(new StockProduct { Quantity = 2, Id = "AppleId" });
            _commandHandlers.Tell(new StockProduct { Quantity = 3, Id = "BananaId" });

            StockReport();

            _commandHandlers.Tell(new StockProduct { Quantity = 3, Id = "AppleId" });

            StockReport();

            _commandHandlers.Tell(new PlaceOrder
            {
                Id = "Nice Order",
                OrderLines = new[]{
                    new Orderline { ProductId= "AppleId", Quantity= 2 },
                    new Orderline { ProductId= "BananaId", Quantity = 1 }                }
            });

            StockReport();

            _commandHandlers.Tell(new PlaceOrder
            {
                Id = "Too many Apples",
                OrderLines = new[]{
                    new Orderline { ProductId= "AppleId", Quantity= 5 },
                    new Orderline { ProductId= "BananaId", Quantity = 1 }                }
            });

            _commandHandlers.Tell(new StockProduct { Quantity = 10, Id = "AppleId" });

            StockReport();

            Console.ReadLine();
        }

        private static void Bootstrap()
        {
            var aggregateContext = AggregateContext.CreateWith(new InMemoryEventStore());
            var processContext = ProcessContext.CreateWith(new InMemoryEventStore());
            _productsInStockReadModel = new List<ProductInStock>();

            var products = new ProductCommandHandler(aggregateContext);
            var orders = new OrderCommandHandler(aggregateContext);

            var sideEffects = new SideEffects();

            var readModelHandler = new ProductReadModelEventHandler(_productsInStockReadModel);
            var orderProcessRouter = new OrderProcessRouter(processContext);
            var pickingProcessRouter = new PickingProcessRouter(processContext);

            aggregateContext
                .PublishNewStateTo(readModelHandler)
                .And(orderProcessRouter)
                .And(pickingProcessRouter);
     
            processContext
                .DispatchCommandsTo(products)
                .And(sideEffects);

            _commandHandlers = Dispatch<IAmACommandMessage>
                .To(products)
                .And(orders);
        }

        private static void StockReport()
        {
            Console.WriteLine();
            Console.WriteLine("---Stockreport---");
            _productsInStockReadModel.ToList().ForEach(p => Console.WriteLine($"{p.Name}: {p.Quantity}"));
            Console.WriteLine("-----------------");
            Console.WriteLine();
        }
    }
}
