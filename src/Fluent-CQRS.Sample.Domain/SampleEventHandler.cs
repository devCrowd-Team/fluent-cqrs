using System;
using System.Collections.Generic;
using System.Linq;
using Fluent_CQRS.Sample.Contracts;
using Fluent_CQRS.Extensions;

namespace Fluent_CQRS.Sample.Domain
{
	public class SampleEventHandler : IHandleEvents
	{
		public void Receive (IEnumerable<IAmAnEventMessage> eventMessages)
		{
			eventMessages.ToList ().ForEach (message => message.HandleMeWith (this));
		}

		public void HandleMessage (SampleEventRaised message)
		{
			Console.WriteLine ("Event {0} empfangen: {1}", message.GetType (), message.MyValue);
		}
	}
}
