using System;
using UnityEngine;

namespace UnityServiceLocator
{
    public class ServiceScope : ServiceLocator
    {
        [Header("Scope Settings")]
        [SerializeField] private bool _isAutoBuild = true;
        [SerializeField] private ServiceInstaller[] _installers;

        private bool _isBuilded = false;
        private Builder _builder;
        private static Injector _injector;

        protected override void Awake()
        {
            base.Awake();

            if (_isAutoBuild)
                BuildScope();
        }

        protected virtual void BuildScope()
        {
            if (_isBuilded) return;

            _isBuilded = true;
            InitScope();
            Installers();
            Configurate(_builder);
            InjectContainer();
        }

        private void Installers()
        {
            if (_injector == null) return;

            foreach (var installer in _installers)
            {
                try
                {
                    installer.Install(_builder);
                }
                catch (Exception)
                {
                    Debug.LogError($"ServiceScope.Install: ошибка инсталлера - {installer.GetType().Name}");
                    throw;
                }
            }
        }

        private void InitScope()
        {
            _container = new Container();
            _builder = new Builder(_container);
            _injector = new Injector(_container);
        }

        private void InjectContainer()
        {
            foreach (var service in _container.Services)
            {
                try
                {
                    _injector.InjectMono(service);
                }
                catch (Exception)
                {
                    Debug.LogError($"ServiceScope.InjectContainer: ошибка иньекции класса - {service.GetType().Name}");
                    throw;
                }
            }
        }

        public virtual void Configurate(IBuilder builder) { }

        public static IInjector Inject(object obj) => _injector.Inject(obj);
        public static IInjector InjectMono(object obj) => _injector.InjectMono(obj);
        public static IInjector InjectMono(MonoBehaviour obj) => _injector.InjectMono(obj);


        protected override void OnDestroy()
        {
            base.OnDestroy();

            _injector = null;
            _builder = null;
        }
    }
}