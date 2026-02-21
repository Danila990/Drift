using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.UnityServiceLocator
{
    public interface IBuilder
    {
        public void RegisterDontDestroyOnLoad<T>(T mono) where T : MonoBehaviour;
        public void RegisterDontDestroyOnLoad<T>() where T : MonoBehaviour;
        public void RegisterResources<T>(string path) where T : Object;
        public void RegisterInstantiate<T>(T mono) where T : Object;
        public void RegisterInstantiate<T, I>(T mono) where T : Object, I where I : class;
        public void RegisteNewGameobject<T>() where T : MonoBehaviour;
        public void RegisterNewGameobject<T, I>() where T : MonoBehaviour, I where I : class;
        public void RegisterNewClass<T>() where T : class, new();
        public void Register<T>(T registerClass) where T : class;
    }

    public class Builder : IBuilder
    {
        private readonly IContainer _container;

        public Builder(IContainer register)
        {
            _container = register;
        }

        public void RegisterDontDestroyOnLoad<T>(T mono) where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return;

            T newMono = Object.FindFirstObjectByType<T>();
            if (newMono != null)
            {
                _container.Register<T>(newMono);
                return;
            }

            newMono = Object.Instantiate<T>(mono);
            Object.DontDestroyOnLoad(newMono);
            _container.Register<T>(newMono);
        }

        public void RegisterDontDestroyOnLoad<T>() where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return;

            T newMono = Object.FindFirstObjectByType<T>();
            if(newMono != null)
            {
                _container.Register<T>(newMono);
                return;
            }

            newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            Object.DontDestroyOnLoad(newMono);
            _container.Register<T>(newMono);
        }

        public void RegisterResources<T>(string path) where T : Object
        {
            if (!CheckAdd<T>()) return;

            T loadMono = Resources.Load<T>(path);
            if (loadMono == null)
                throw new ArgumentNullException($"Builder.RegisterResources: Ошибка загрузки путь - {path}");

            loadMono = Object.Instantiate<T>(loadMono);
            _container.Register<T>(loadMono);
        }

        public void RegisterInstantiate<T>(T mono) where T : Object
        {
            if (!CheckAdd<T>()) return;

            T newMono = Object.Instantiate<T>(mono);
            _container.Register<T>(newMono);
        }

        public void RegisterInstantiate<T, I>(T mono) where T : Object, I where I : class
        {
            if (!CheckAdd<T>()) return;

            T newMono = Object.Instantiate<T>(mono);

            if (newMono is I)
                _container.Register<I>(newMono);
            else
                Debug.LogError($"Builder.RegisterInstantiate: {typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
        }

        public void RegisteNewGameobject<T>() where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return;

            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            _container.Register<T>(newMono);
        }

        public void RegisterNewGameobject<T,I>() where T : MonoBehaviour, I where I : class
        {
            if (!CheckAdd<T>()) return;

            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();

            if (newMono is I)
                _container.Register<I>(newMono);
            else
                Debug.LogError($"{typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
        }

        public void RegisterNewClass<T>() where T: class, new()
        {
            if (!CheckAdd<T>()) return;

            T newClass = new T();
            _container.Register<T>(newClass);
        }

        public void Register<T>(T register) where T : class
        {
            if (!CheckAdd<T>()) return;

            _container.Register<T>(register);
        }

        private bool CheckAdd<T>() where T : class
        {
            if (_container.FindService<T>())
            {
                Debug.LogError($"Register: {typeof(T)} уже зарегестрировано");
                return false;
            }

            return true;
        }
    }
}