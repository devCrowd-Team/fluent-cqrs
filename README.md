# fluent-cqrs
My TNG "The Next Generation" CQRS Framework for .Net Applications

---

Why fluent? Let's have a look at this:

    public class AwsomeCommandHandler 
    {
      Aggregates _aggregates;
    
      public AwsomeCommandHandler(Aggregates aggregates)
      {
        _aggregates = aggregates;
      }
      
      public void Handle(SuperDuperCommand command)
      {
        _aggregates.Provide<[AnAggregateYouLike]>.With(command.AggregateId)
          .Do(yourAggregate => yourAggregate.DoSomethingWith(command.Data))
          .FinallySaveIt()
          .AndPublishTheNewState();
      }
    }

Uhhh... this is a complete Handling of a Domain Command.

---

Ok, but what should I do to **publish** the new state, aka **Domain Events**?

This is simple. You assign a `Action<IEnumerable<IAmAnEventMessage>>` to the `aggregates.PublishNewState` property and consume the published events in this method. Take a look

    var _aggregates = new Aggregates(yourExtremeGoodEventStoreInstance);
    
    _aggregates.PublishNewState = yourCoolEventHandler.RecieveEvents;
    
All right... Now your cool event handler gets all changes of an aggregate.


