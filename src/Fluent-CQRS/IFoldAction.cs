using System;

namespace Fluent_CQRS
{
    internal interface IFoldAction<TResult> 
    {
        Func<IAmAnEventMessage, TResult, TResult> Apply { get; }
        Func<IAmAnEventMessage, bool> IsActionFor { get; }
    }
}