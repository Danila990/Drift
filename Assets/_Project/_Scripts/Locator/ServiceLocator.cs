using System;
using UnityEngine;

namespace UnityServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        protected static Container _container;

        protected virtual void Awake()
        {
            InitServiceLocator();
        }

        private void InitServiceLocator()
        {
            _container = new Container();
        }

        public static IContainerResolver Resolve<T>(out T service) where T : class => _container.Resolve(out service);
        public static T Resolve<T>(Type type) where T : class => _container.Resolve<T>();
        public static T Resolve<T>() where T : class => _container.Resolve<T>();
        public static IContainerRegister Register<T>(T service) where T : class => _container.Register<T>(service);
        public static IContainerRegister Register<T>(T service, Type type) where T : class => _container.Register<T>(service, type);

        protected virtual void OnDestroy()
        {
            _container.Clear();
            _container = null;
        }
    }
}