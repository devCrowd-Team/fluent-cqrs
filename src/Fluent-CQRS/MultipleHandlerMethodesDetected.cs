using System;

namespace Fluent_CQRS
{

	public class MultipleHandlerMethodesDetected : Exception
	{
		public MultipleHandlerMethodesDetected (string eventType) : base (
				"There are to many methodes to handle the Event " + eventType) { }
	}
}