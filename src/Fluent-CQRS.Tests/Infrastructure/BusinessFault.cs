namespace Fluent_CQRS.Tests.Infrastructure
{
    public class BusinessFault : Fault
    {
        public BusinessFault() : base("My BusinessFault")
        {
        }
    }
}