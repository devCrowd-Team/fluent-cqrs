# fluent-cqrs
"The Next Generation" CQRS Framework for .Net applications

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
			.Do(yourAggregate => yourAggregate.DoSomethingWith(command.Data))
			.OnError(exception=> handleThis(exception));
      }
    }

Uhhh... this is the *complete handling* of a Domain Command.

---

Ok, but what do I have to do to **publish** the new state, aka **Domain Events**?

This is simple. You assign a `Action<IEnumerable<IAmAnEventMessage>>` to the `aggregates.PublishNewState` property and consume the published events in this method. For example:

    var _aggregates = new Aggregates(yourExtremeGoodEventStoreInstance);
    
    _aggregates.PublishNewState = yourCoolEventHandler.RecieveEvents;
    
now your cool event handler receives all changes of an aggregate.


