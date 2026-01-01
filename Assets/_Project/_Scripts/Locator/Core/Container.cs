using System;
using System.Collections.Generic;
using UnityEngine;

namespace Locator
{
    public interface IContainer : IContainerResolver, IContainerRegister
    {
    }

    public interface IContainerResolver
    {
        public IEnumerable<object> Services { get; }
        public IContainerResolver Resolve<T>(out T service) where T : class;
        public T Resolve<T>(Type type) where T : class;
        public T Resolve<T>() where T : class;
    }

    public interface IContainerRegister
    {
        public IContainerRegister Register<T>(T service) where T : class;
        public IContainerRegister Register<T>(T service, Type type) where T : class;
    }

    public class Container : IContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public IEnumerable<object> Services => _services.Values;

        public IContainerResolver Resolve<T>(out T service) where T : class
        {
            service = Resolve<T>();
            return this;
        }

        public T Resolve<T>(Type type) where T : class
        {
            if (_services.TryGetValue(type, out object obj))
                return obj as T;

            Debug.LogError($"Container resolve of type {type.FullName} - not registered");
            return null;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve<T>(typeof(T));
        }

        public IContainerRegister Register<T>(T service) where T : class
        {
            Register<T>(service, typeof(T));
            return this;
        }

        public IContainerRegister Register<T>(T service, Type type) where T : class
        {
            if (!_services.TryAdd(type, service))
                Debug.LogError($"Container register of type {type.FullName} - already registered");

            return this;
        }

        public void Clear()
        {
            _services?.Clear();
        }
    }
}