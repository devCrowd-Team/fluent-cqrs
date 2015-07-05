namespace Fluent_CQRS
{
	public interface IConcatenateEventHandler
	{
		IConcatenateEventHandler And (IHandleEvents eventHandler);
	}
}