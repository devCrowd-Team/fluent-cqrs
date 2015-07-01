using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Fluent_CQRS.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<tValue> Flatten<tValue>(this IObservable<IEnumerable<tValue>> source)
        {
            return new FlattenConnection<tValue>(source);

        }

    }

    class FlattenConnection<tValue> : IObservable<tValue>, IObserver<IEnumerable<tValue>>, IDisposable
    {
        private readonly Subject<tValue> _subject;
        private readonly IDisposable _subscription;

        public FlattenConnection(IObservable<IEnumerable<tValue>> source)
        {
            _subject = new Subject<tValue>();
            _subscription = source.Subscribe(this);
        }

        public IDisposable Subscribe(IObserver<tValue> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void OnNext(IEnumerable<tValue> values)
        {
            foreach (var value in values)
                _subject.OnNext(value);
        }

        public void OnError(Exception error)
        {
            _subject.OnError(error);
        }

        public void OnCompleted()
        {
            _subject.OnCompleted();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription.Dispose();
                _subject.Dispose();
            }
        }
    }
}
