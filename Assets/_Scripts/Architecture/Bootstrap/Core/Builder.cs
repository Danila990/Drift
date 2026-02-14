using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Bootstrap
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
        private readonly IContainerRegister _register;

        public Builder(IContainerRegister register)
        {
            _register = register;
        }

        public void RegisterDontDestroyOnLoad<T>(T mono) where T : MonoBehaviour
        {
            T newMono = Object.FindFirstObjectByType<T>();
            if (newMono != null)
            {
                _register.Register<T>(newMono);
                return;
            }

            newMono = Object.Instantiate<T>(mono);
            Object.DontDestroyOnLoad(newMono);
            _register.Register<T>(newMono);
        }

        public void RegisterDontDestroyOnLoad<T>() where T : MonoBehaviour
        {
            T newMono = Object.FindFirstObjectByType<T>();
            if(newMono != null)
            {
                _register.Register<T>(newMono);
                return;
            }

            newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            Object.DontDestroyOnLoad(newMono);
            _register.Register<T>(newMono);
        }

        public void RegisterResources<T>(string path) where T : Object
        {
            T loadMono = Resources.Load<T>(path);
            if (loadMono == null)
                throw new ArgumentNullException($"Builder.RegisterResources: Ошибка загрузки путь - {path}");

            loadMono = Object.Instantiate<T>(loadMono);
            _register.Register<T>(loadMono);
        }

        public void RegisterInstantiate<T>(T mono) where T : Object
        {
            T newMono = Object.Instantiate<T>(mono);
            _register.Register<T>(newMono);
        }

        public void RegisterInstantiate<T, I>(T mono) where T : Object, I where I : class
        {
            T newMono = Object.Instantiate<T>(mono);

            if (newMono is I)
                _register.Register<I>(newMono);
            else
                Debug.LogError($"Builder.RegisterInstantiate: {typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
        }

        public void RegisteNewGameobject<T>() where T : MonoBehaviour
        {
            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();
            _register.Register<T>(newMono);
        }

        public void RegisterNewGameobject<T,I>() where T : MonoBehaviour, I where I : class
        {
            T newMono = new GameObject(typeof(T).Name).AddComponent<T>();

            if (newMono is I)
                _register.Register<I>(newMono);
            else
                Debug.LogError($"{typeof(T).Name} неудалось зарегестрировать в виде {typeof(I).Name}");
        }

        public void RegisterNewClass<T>() where T: class, new()
        {
            T newClass = new T();
            _register.Register<T>(newClass);
        }

        public void Register<T>(T register) where T : class
        {
            _register.Register<T>(register);
        }
    }
}