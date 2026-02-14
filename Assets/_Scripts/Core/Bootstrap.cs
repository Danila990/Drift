using _Project.Bootstrap;
using System;
using UnityEngine;

namespace BootSystem
{
    public abstract class Bootstrap : MonoBehaviour
    {
        private BootContainer _mainContainer;
    }

    public class BootContainer
    {
        private Cotainer _sceneContainer;
        private static Cotainer _globalContainer;

        private bool _isDebugsDataContainer = false;

        public BootContainer()
        {
            if (_globalContainer == null)
                _globalContainer = new Cotainer();

            _sceneContainer = new Cotainer();
        }

        public BootContainer Get<T>(out T service) where T : class
        {
            Type findType = typeof(T);
            service = Get<T>(findType);
            return this;
        }

        public T Get<T>(Type findType) where T : class
        {
            T returnData = _sceneContainer.Get<T>(findType, _isDebugsDataContainer);
            if (returnData == null)
                returnData = _globalContainer.Get<T>(findType, _isDebugsDataContainer);

            if(returnData == null )
                throw new ArgumentNullException($"BootContainer.Get: {findType.FullName} не зарегистрирован");

            return returnData;
        }

        public T Get<T>() where T : class
        {
            Type findType = typeof(T);
            return Get<T>(findType);
        }

        //public BootContainer Set<T>(T service) where T : class
        //{
            
        //}

        //public BootContainer Set<T>(T service, Type type) where T : class
        //{
            
        //}

        public void Clear()
        {
            _sceneContainer.Clear();
        }
    }
}