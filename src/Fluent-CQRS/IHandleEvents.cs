using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
	public interface IHandleEvents
	{
		void Receive (IEnumerable<IAmAnEventMessage> events);
	}
}

