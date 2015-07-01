using System;
using System.Collections.Generic;

namespace Fluent_CQRS.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<tValue> Flatten<tValue>(this IObservable<IEnumerable<tValue>> source)
        {
            return new FlattenConnection<tValue>(source);

        }

        public static IObservable<tTarget> OfType<tTarget>(this IObservable<object> source)
            where tTarget : class
        {
            return new OfTypeConnection<tTarget>(source);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> action)
        {
            return source.Subscribe(new DelegateObserver<T>(action));
        }

        public static IObservable<object> SentEventsTo<tTarget>(this IObservable<object> source, Action<tTarget> handler)
            where tTarget : class
        {
            source.OfType<tTarget>().Subscribe(handler);
            return source;
        }

        public static IObservable<object> AndTo<tTarget>(this IObservable<object> source, Action<tTarget> handler)
            where tTarget : class
        {
            return source.SentEventsTo(handler);
        }


    }

    class OfTypeConnection<tTarget> : IObservable<tTarget>, IObserver<object>, IDisposable
        where tTarget : class
    {
        private readonly Subject<tTarget> _subject;
        private readonly IDisposable _subscription;

        public OfTypeConnection(IObservable<object> source)
        {
            _subject = new Subject<tTarget>();
            _subscription = source.Subscribe(this);
        }

        public IDisposable Subscribe(IObserver<tTarget> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void OnNext(object value)
        {
            var target = value as tTarget;
            if (target != null)
                _subject.OnNext(target);
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

    internal class Subject<T> : IObservable<T>, IDisposable
    {
        private readonly List<IObserver<T>> _observer;

        public Subject()
        {
            _observer = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            lock (_observer)
            {
                _observer.Add(observer);
            }
            return new DelegateDispose(() => Remove(observer));
        }

        private void Remove(IObserver<T> observer)
        {
            lock (_observer)
            {
                _observer.Remove(observer);
            }
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
                lock (_observer)
                {
                    OnCompleted();
                    _observer.Clear();
                }
            }
        }

        public void OnNext(T value)
        {
            IObserver<T>[] subscribtors;
            lock (_observer) subscribtors = _observer.ToArray();
            foreach (var observer in subscribtors)
                observer.OnNext(value);
        }

        public void OnError(Exception error)
        {
            IObserver<T>[] subscribtors;
            lock (_observer) subscribtors = _observer.ToArray();
            foreach (var observer in subscribtors)
                observer.OnError(error);
        }

        public void OnCompleted()
        {
            IObserver<T>[] subscribtors;
            lock (_observer) subscribtors = _observer.ToArray();
            foreach (var observer in subscribtors)
                observer.OnCompleted();
        }
    }

    class DelegateDispose : IDisposable
    {
        private Action _onDispose;

        public DelegateDispose(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (_onDispose != null)
                _onDispose();
            _onDispose = null;
        }
    }

    class DelegateObserver<T> : IObserver<T>
    {
        private readonly Action<T> _action;

        public DelegateObserver(Action<T> action)
        {
            _action = action;
        }

        public void OnNext(T value)
        {
            _action(value);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
