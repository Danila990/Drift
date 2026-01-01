using System;
using UnityEngine;

namespace Locator
{
    public class LocatorScope : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] private bool _isAutoBuild = true;

        private bool _isBuilded = false;
        private Injector _injector;
        private Builder _builder;
        private static Container _container;

        protected virtual void Awake()
        {
            if (_isAutoBuild)
                Build();
        }

        private void Build()
        {
            if (_isBuilded) return;

            _isBuilded = true;
            SetupLocator();
            Configurate(_builder);
            InjectContainer();
        }

        private void SetupLocator()
        {
            _container = new Container();
            _builder = new Builder(_container);
            _injector = new Injector(_container);
        }

        private void InjectContainer()
        {
            foreach (var service in _container.Services)
                _injector.InjectMono(service);
        }

        public virtual void Configurate(IBuilder builder) { }

        public static IContainerResolver Resolve<T>(out T service) where T : class => _container.Resolve(out service);
        public static T Resolve<T>(Type type) where T : class => _container.Resolve<T>();
        public static T Resolve<T>() where T : class => _container.Resolve<T>();

        private void OnDestroy()
        {
            _container.Clear();
            _injector = null;
            _builder = null;
            _container = null;
        }
    }
}