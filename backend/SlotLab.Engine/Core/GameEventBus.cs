using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Thread-safe, high-performance in-process event bus.
    /// 
    /// Supports:
    /// 1. Subscribing to a concrete event type (e.g., PlayerSpinEvent)
    /// 2. Subscribing to interfaces (e.g., IGameplayEvent, IBonusEvent)
    /// 3. Subscribing to AbstractEvent (global listener)
    /// 
    /// Optimized:
    /// - No DynamicInvoke (precompiled Action<AbstractEvent> dispatchers)
    /// - Concurrent-safe via ReaderWriterLockSlim
    /// - Constant-time invocation path
    /// </summary>
    public static class GameEventBus
    {
        private static readonly Dictionary<Type, List<(Delegate raw, Action<AbstractEvent> compiled)>> handlersByType = new();
        private static readonly ReaderWriterLockSlim rwLock = new(LockRecursionPolicy.SupportsRecursion);

        // ------------------------------------------------------------------
        // SUBSCRIBE
        // ------------------------------------------------------------------
        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(T);

            // Precompile safe dispatcher
            Action<AbstractEvent> compiled = evt =>
            {
                if (evt is T typed)
                    handler(typed);
            };

            rwLock.EnterWriteLock();
            try
            {
                if (!handlersByType.TryGetValue(eventType, out var list))
                {
                    list = new List<(Delegate, Action<AbstractEvent>)>();
                    handlersByType[eventType] = list;
                }

                // Avoid duplicates
                if (list.Any(x => x.raw.Equals(handler)))
                    return;

                list.Add((handler, compiled));
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        // ------------------------------------------------------------------
        // PUBLISH
        // ------------------------------------------------------------------
        public static void Publish<T>(T @event) where T : AbstractEvent
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            var concreteType = @event.GetType();

            rwLock.EnterReadLock();
            try
            {
                // 1️⃣ Exact type
                InvokeHandlers(concreteType, @event);

                // 2️⃣ Interfaces (e.g. IGameplayEvent, IBonusEvent)
                foreach (var iface in concreteType.GetInterfaces())
                    InvokeHandlers(iface, @event);

                // 3️⃣ Base class chain (e.g. AbstractEvent)
                var baseType = concreteType.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    InvokeHandlers(baseType, @event);
                    baseType = baseType.BaseType;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        // ------------------------------------------------------------------
        // INTERNAL INVOKER
        // ------------------------------------------------------------------
        private static void InvokeHandlers(Type type, AbstractEvent evt)
        {
            if (!handlersByType.TryGetValue(type, out var handlers))
                return;

            // Snapshot per evitar modificacions durant iteració
            foreach (var (_, compiled) in handlers.ToList())
            {
                try
                {
                    compiled(evt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GameEventBus] Error invoking handler for {type.Name}: {ex.Message}");
                }
            }
        }

        // ------------------------------------------------------------------
        // UNSUBSCRIBE
        // ------------------------------------------------------------------
        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(T);

            rwLock.EnterWriteLock();
            try
            {
                if (handlersByType.TryGetValue(eventType, out var handlers))
                {
                    handlers.RemoveAll(x => x.raw.Equals(handler));
                    if (handlers.Count == 0)
                        handlersByType.Remove(eventType);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
