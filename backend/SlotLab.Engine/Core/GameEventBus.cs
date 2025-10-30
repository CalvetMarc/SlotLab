using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SlotLab.Engine.Core.Events;

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
    /// - Lock-free using ConcurrentDictionary / ConcurrentBag
    /// - Constant-time invocation path
    /// </summary>
    public class GameEventBus
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<(Delegate raw, Action<AbstractEvent> compiled)>> _handlersByType
            = new();

        // ------------------------------------------------------------------
        // SUBSCRIBE
        // ------------------------------------------------------------------
        public void Subscribe<T>(Action<T> handler)
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

            var bag = _handlersByType.GetOrAdd(eventType, _ => new ConcurrentBag<(Delegate, Action<AbstractEvent>)>());

            // Avoid duplicates manually (ConcurrentBag has no Remove or Contains)
            if (bag.Any(x => x.raw.Equals(handler)))
                return;

            bag.Add((handler, compiled));
        }

        // ------------------------------------------------------------------
        // PUBLISH
        // ------------------------------------------------------------------
        public void Publish<T>(T @event) where T : AbstractEvent
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            var concreteType = @event.GetType();

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

        // ------------------------------------------------------------------
        // INTERNAL INVOKER
        // ------------------------------------------------------------------
        private void InvokeHandlers(Type type, AbstractEvent evt)
        {
            if (!_handlersByType.TryGetValue(type, out var handlers))
                return;

            foreach (var (_, compiled) in handlers.ToArray()) // snapshot
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
        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(T);

            if (!_handlersByType.TryGetValue(eventType, out var bag))
                return;

            // Rebuild bag without the handler (ConcurrentBag doesn't support Remove)
            var remaining = bag.Where(x => !x.raw.Equals(handler)).ToList();
            if (remaining.Count == 0)
            {
                _handlersByType.TryRemove(eventType, out _);
                return;
            }

            // Replace with new bag
            var newBag = new ConcurrentBag<(Delegate, Action<AbstractEvent>)>(remaining);
            _handlersByType[eventType] = newBag;
        }
    }
}
