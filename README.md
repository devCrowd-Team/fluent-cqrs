# fluent-cqrs
My TNG "The Next Generation" CQRS Framework for .Net Applications


Why fluent? Let's have a look at this:

    public class AwsomeCommandHandler 
    {
      AggregateStore _aggregates;
    
      public AwsomeCommandHandler(AggregateStore aggregateStore)
      {
        _aggregateStore = aggregateStore;
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
