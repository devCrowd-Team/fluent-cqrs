using System.Linq;
using System.Reflection;

namespace Fluent_CQRS.Extensions
{

	public static class EventExtension
	{
		public static void HandleMeWith (this IAmAnEventMessage @event, IHandleEvents handler)
		{
			var handlerType = handler.GetType ();
			var eventType = @event.GetType ();

			var eventHandlerMethods =
                from method in handlerType.GetTypeInfo().DeclaredMethods
				from parameter in method.GetParameters ()
				where parameter.ParameterType == eventType
				select method;

			var allMethodes = eventHandlerMethods.ToList ();
			
			if (allMethodes.Any ()) {
				if (allMethodes.Count () > 1) {
					throw new MultipleHandlerMethodesDetected (eventType.Name);
				}

				allMethodes.First ().Invoke (handler, new object[]{ @event });
			}
		}
	}
}