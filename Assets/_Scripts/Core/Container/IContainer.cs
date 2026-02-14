using System;
using System.Collections.Generic;

namespace BootSystem
{
    public interface IContainer : IContainerGet, IContainerSet
    {
    }

    public interface IContainerGet
    {
        public IEnumerable<object> Datas { get; }
        public IContainerGet Get<T>(out T service) where T : class;
        public T Get<T>(Type type) where T : class;
        public T Get<T>() where T : class;
    }

    public interface IContainerSet
    {
        public IContainerSet Set<T>(T service) where T : class;
        public IContainerSet Set<T>(T service, Type type) where T : class;
    }
}