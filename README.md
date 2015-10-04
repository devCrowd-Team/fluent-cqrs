# fluent-cqrs
"The Next Generation" CQRS Framework for .Net applications

### Attenzione Attenzione:
There is an **Api break**. The method `OnError` was splitted into `CatchException`
(raised by unexpected System Exceptions) and `CatchFault` (raised by Business Faults)
By default 'Do' throws all exceptions directly. Use 'Try' for catching errors.

---

Why fluent? Just look at this:

    public class AwsomeCommandHandler
    {
        Aggregates _aggregates;

        public AwsomeCommandHandler(Aggregates aggregates)
        {
            _aggregates = aggregates;
        }

        public void Handle(SuperDuperCommand command)
        {
            _aggregates
                .Provide<[AnAggregateYouLike]>
                .With(command.AggregateId)
                .Do(yourAggregate => yourAggregate.DoSomethingWith(command.Data));

		    // You want it with exception handling?
		    // Lets do it

		    _aggregates
			    .Provide<[AnAggregateYouLike]>
			    .With(command.AggregateId)
			    .Try(yourAggregate => yourAggregate.DoSomethingWith(command.Data))
			    .CatchException(exception=> handleThis(exception));

			// And here a very simple way to catch business faults which thrown within the Aggregate

			_aggregates
			    .Provide<[AnAggregateYouLike]>
			    .With(command.AggregateId)
			    .Try(yourAggregate => yourAggregate.DoSomethingWith(command.Data))
			    .CatchFault(fault=> handleThis(fault))
				  .CatchException(exception => handleThis(exception));
        }
    }

Uhhh... this is the **complete handling** of a Domain Command.

---

Ok, but what do I have to do to **publish** the new state, aka **Domain Events**?

This is simple. You assign any Event Handler you like by chaining it by the `And` method after `PublishNewStateTo`.
For example you have three Event Handlers:

    var _aggregates = Aggregates.CreateWith(yourExtremeGoodEventStoreInstance);
    var firstEventHandler = new SampleEventHandler();
    var secondEventHandler = new ReportingHandler();
    var thirdEventHandler = new LoggingHandler();

    _aggregates
        .PublishNewStateTo(firstEventHandler)
        .And(secondEventHandler)
        .And(thirdEventHandler);

now all your cool Event Handlers receiving all changes of an aggregate.

---

All right... In some cases you want to save an event only once, but if you add the event into the list of Changes like this:

    class CoolAggregate() : Aggregate
    {
        ...
        public void DoSomethingHelpful()
        {
            Changes.Add(new SomethingHappend());
        }
        ...
    }

the event will save everytime. Bad, very bad. Here comes the hero 'Replay' ...

    class CoolAggregate() : Aggregate
    {
        ...
        public void DoSomethingHelpful()
        {
            if(MessagesOfType<SomethingHappend>().Any)
            {
                Replay(new SomethingHappend());
            }
           else
           {
               Changes.Add(new SomethingHappend());
           }
        }
        ...
    }

The `Replay` method prevents you for multiple equals events.

Nice, realy nice... But what if you want to replay all Events of an Aggregate? Yes... you can't... still now.

To replay all Events of an Aggregate code this:

    _aggregates
        .ReplayFor<[AnAggregateYouLike]>()
        .EventsWithAggregateId(aggrId)
        .ToAllEventHandlers();

This published all Events of the Aggregate with the given ID to all registered Event Handler.
If you want to publish to only one special Event Handler change your Code to:

    _aggregates
        .ReplayFor<[AnAggregateYouLike]>()
        .EventsWithAggregateId(aggrId)
        .To([OneOfYourEventHandler]);

This ist simple, as well.


You can also replay events of an aggregate type:

    _aggregates
        .ReplayFor<[AnAggregateYouLike]>()
        .AllEvents()
        .ToAllEventHandlers();


You can also filter for certain event messages:

    _aggregates
        .ReplayFor<[AnAggregateYouLike]>()
        .AllEvents()
		    .OfType<[AnEvent]>()
        .ToAllEventHandlers();


---
~tbc
