using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
	class EventHandlers : IConcatenateEventHandler
	{
		readonly IList<IHandleEvents> _receiver;

		public EventHandlers ()
		{
			_receiver = new List<IHandleEvents> ();
		}

		public void Add (IHandleEvents eventHandler)
		{
			_receiver.Add (eventHandler);
		}

		public void Receive (IEnumerable<IAmAnEventMessage> events)
		{
		    foreach (var receiver in _receiver)
		    {
		        receiver.Receive (events);
		    }
		}

		public IConcatenateEventHandler And (IHandleEvents eventHandler)
		{
			_receiver.Add (eventHandler);

			return this;
		}
	}
}