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

This is simple. You assign any Event Handler you like by chaining it by the `And` method after `PublishNewStateTo`. For example you have three Event Handlers:

    var _aggregates = Aggregates.CreateWith(yourExtremeGoodEventStoreInstance);
    
    var firstEventHandler = new SampleEventHandler();
    var secondEventHandler = new ReportingHandler();
    var thirdEventHandler = new LoggingHandler();

    _aggregates
      .PublishNewStateTo(firstEventHandler)
      .And(secondEventHandler)
      .And(thirdEventHandler);
    
now all your cool Event Handlers receiving all changes of an aggregate.


