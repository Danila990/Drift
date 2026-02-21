using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.UnityServiceLocator
{
    public interface IBuilder
    {
        public T RegisterDontDestroyOnLoad<T>(T mono) where T : MonoBehaviour;
        public T RegisterDontDestroyOnLoad<T>() where T : MonoBehaviour;
        public T RegisterResources<T>(string path) where T : Object;
        public T RegisterInstantiate<T>(T mono) where T : Object;
        public T RegisterInstantiate<T, I>(T mono) where T : Object, I where I : class;
        public T RegisteNewGameobject<T>() where T : MonoBehaviour;
        public T RegisterNewGameobject<T, I>() where T : MonoBehaviour, I where I : class;
        public T RegisterNewClass<T>() where T : class, new();
        public void Register<T>(T registerClass) where T : class;
    }

    public class Builder : IBuilder
    {
        private readonly IContainer _container;

        public Builder(IContainer register)
        {
            _container = register;
        }

        public T RegisterDontDestroyOnLoad<T>(T mono) where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return null;

            T newMono = Object.FindFirstObjectByType<T>();
            if (newMono != null)
            {
                _container.Register<T>(newMono);
                return newMono;
            }

            newMono = Object.Instantiate<T>(mono);
            Object.DontDestroyOnLoad(newMono);
            _container.Register<T>(newMono);
            return newMono;
        }

        public T RegisterDontDestroyOnLoad<T>() where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return null;

            T newMono = Object.FindFirstObjectByType<T>();
            if(newMono != null)
            {
                _container.Register<T>(newMono);
                return newMono;
            }

            newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            Object.DontDestroyOnLoad(newMono);
            _container.Register<T>(newMono);
            return newMono;
        }

        public T RegisterResources<T>(string path) where T : Object
        {
            if (!CheckAdd<T>()) return null;

            T loadMono = Resources.Load<T>(path);
            if (loadMono == null)
                throw new ArgumentNullException($"Builder.RegisterResources: Ошибка загрузки путь - {path}");

            loadMono = Object.Instantiate<T>(loadMono);
            _container.Register<T>(loadMono);
            return loadMono;
        }

        public T RegisterInstantiate<T>(T mono) where T : Object
        {
            if (!CheckAdd<T>()) return null;

            T newMono = Object.Instantiate<T>(mono);
            _container.Register<T>(newMono);
            return newMono;
        }

        public T RegisterInstantiate<T, I>(T mono) where T : Object, I where I : class
        {
            if (!CheckAdd<T>()) return null;

            T newMono = Object.Instantiate<T>(mono);

            if (newMono is I)
            {
                _container.Register<I>(newMono);
                return newMono;
            }
            else
            {
                Debug.LogError($"Builder.RegisterInstantiate: {typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
                return null;
            }
        }

        public T RegisteNewGameobject<T>() where T : MonoBehaviour
        {
            if (!CheckAdd<T>()) return null;

            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            _container.Register<T>(newMono);

            return newMono;
        }

        public T RegisterNewGameobject<T,I>() where T : MonoBehaviour, I where I : class
        {
            if (!CheckAdd<T>()) return null;

            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();

            if (newMono is I)
            {
                _container.Register<I>(newMono);
                return newMono;
            }
            else
            {
                Debug.LogError($"{typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
                return null;
            }
        }

        public T RegisterNewClass<T>() where T: class, new()
        {
            if (!CheckAdd<T>()) return null;

            T newClass = new T();
            _container.Register<T>(newClass);
            return newClass;
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