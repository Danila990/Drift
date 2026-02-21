using System;
using UnityEngine;

namespace _Project.UnityServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        [Header("Service Settings")]
        [SerializeField] private bool _isAutoBuild = true;
        [SerializeField] private BootstrapInstaller[] _installers;

        private Builder _builder;
        private static Injector _injector;
        private static Container _container;

        protected virtual void Awake()
        {
            if (!_isAutoBuild) return;

            Setup();
            SetupInstallers();
            Configurate(_builder);
            InjectContainer();
        }

        private void SetupInstallers()
        {
            if (_injector == null) return;

            foreach (var installer in _installers)
                installer.Install(_builder);
        }

        private void Setup()
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

        public static IInjector Inject(object obj) => _injector.Inject(obj);
        public static IInjector InjectMono(object obj) => _injector.InjectMono(obj);
        public static IInjector InjectMono(MonoBehaviour obj) => _injector.InjectMono(obj);

        public static IContainerResolver Get<T>(out T service) where T : class => _container.Resolve(out service);
        public static T Get<T>(Type type) where T : class => _container.Resolve<T>();
        public static T Get<T>() where T : class => _container.Resolve<T>();

        private void OnDestroy()
        {
            _container.Clear();
            _container = null;
            _injector = null;
            _builder = null;
        }
    }
}