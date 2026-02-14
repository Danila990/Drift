using System;
using System.Collections.Generic;
using UnityEngine;

namespace BootSystem
{
    public class Cotainer
    {
        private readonly Dictionary<Type, object> _datas = new Dictionary<Type, object>();

        public IEnumerable<object> Services => _datas.Values;

        public T Get<T>(Type findType, bool debug = false) where T : class
        {
            if (_datas.TryGetValue(findType, out object obj))
                return obj as T;

            if(debug)
                Debug.LogError($"DataContainer.Get: {findType.FullName} не зарегистрирован");
            return null;
        }

        public Cotainer Set<T>(T service, Type type, bool debug = false) where T : class
        {
            if (!_datas.TryAdd(type, service) && debug)
                Debug.LogError($"DataContainer.Set: {type.FullName} уже зарегистрирован");

            return this;
        }

        public void Clear() => _datas?.Clear();
    }
}