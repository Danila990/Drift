using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project
{
    [CreateAssetMenu(fileName = "ServiceResources", menuName = "MySo/ServiceResources")]
    public class ServiceResources : ScriptableObject
    {
        [SerializeField] private Object[] _resources;

        public T Get<T>() where T : Object
        {
            Type type = typeof(T);
            return _resources.FirstOrDefault(_ => _.GetType() == type) as T;
        }
    }
}