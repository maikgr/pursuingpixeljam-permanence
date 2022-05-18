using System.Collections.Generic;
using System;
using UnityEngine;

namespace Permanence.Scripts.Cores
{
    public abstract class EventBusBehaviour : MonoBehaviour
    {
        protected IDictionary<string, List<Action>> eventBus;

        protected virtual void Awake()
        {
            eventBus = new Dictionary<string, List<Action>>();
        }

        public virtual void AddEventListener(string eventName, Action action)
        {
            if (!eventBus.ContainsKey(eventName)) {
                eventBus.Add(eventName, new List<Action>());
            }
            eventBus[eventName].Add(action);
        }

        public virtual void RemoveEventListener(string eventName, Action action)
        {
            if (!eventBus.ContainsKey(eventName)) return;
            eventBus[eventName].Remove(action);
        }

        public virtual void DispatchEvent(string eventName)
        {
            if (!eventBus.ContainsKey(eventName)) return;
            eventBus[eventName].ForEach(action => action.Invoke());
        }
    }

    public abstract class EventBusBehaviour<T> : MonoBehaviour
    {
        private IDictionary<string, List<Action<T>>> eventBus;

        protected virtual void Awake()
        {
            eventBus = new Dictionary<string, List<Action<T>>>();
        }

        public virtual void AddEventListener(string eventName, Action<T> action)
        {
            if (!eventBus.ContainsKey(eventName)) {
                eventBus.Add(eventName, new List<Action<T>>());
            }
            eventBus[eventName].Add(action);
        }

        public virtual void RemoveEventListener(string eventName, Action<T> action)
        {
            if (!eventBus.ContainsKey(eventName)) return;
            eventBus[eventName].Remove(action);
        }

        public virtual void DispatchEvent(string eventName, T value)
        {
            if (!eventBus.ContainsKey(eventName)) return;
            eventBus[eventName].ForEach(action => action.Invoke(value));
        }
    }
}