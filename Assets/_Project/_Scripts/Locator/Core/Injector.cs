using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Locator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : PropertyAttribute { }

    public interface IInjector
    {
        public IInjector Inject(object obj);
        public IInjector InjectMono(object obj);
        public IInjector InjectMono(MonoBehaviour obj);
    }

    public class Injector : IInjector
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        private readonly IContainerResolver _resolver;

        public Injector(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        public IInjector InjectMono(object obj)
        {
            if (obj is MonoBehaviour monoBehaviour )
                return InjectMono(monoBehaviour);

            Inject(obj);
            return this;
        }
        public IInjector InjectMono(MonoBehaviour behavior)
        {
            var behaviours = behavior.GetComponentsInChildren<MonoBehaviour>();
            foreach (var mono in behaviours)
                Inject(mono);

            return this;
        }

        public IInjector Inject(object obj)
        {
            if (!IsInjectable(obj)) return this;

            var type = obj.GetType();
            InjectFields(type, obj);
            InjectMethods(type, obj);
            InjectProperties(type, obj);

            return this;
        }

        #region InjectionLogick
        private void InjectFields(Type type, object instance)
        {
            var injectableFields = type.GetFields(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                if (injectableField.GetValue(instance) != null)
                {
                    Debug.LogWarning($"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }

                var fieldType = injectableField.FieldType;
                var resolvedInstance = _resolver.Resolve<object>(fieldType);
                if (resolvedInstance == null)
                    throw new Exception($"Failed to inject into field '{injectableField.Name}' of class '{type.Name}'.");

                injectableField.SetValue(instance, resolvedInstance);
            }
        }

        private void InjectMethods(Type type, object instance)
        {
            var injectableMethods = type.GetMethods(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(_resolver.Resolve<object>).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                    throw new Exception($"Failed to inject into method '{injectableMethod.Name}' of class '{type.Name}'.");

                injectableMethod.Invoke(instance, resolvedInstances);
            }
        }

        private void InjectProperties(Type type, object instance)
        {
            var injectableProperties = type.GetProperties(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = _resolver.Resolve<object>(propertyType);
                if (resolvedInstance == null)
                    throw new Exception($"Failed to inject into property '{injectableProperty.Name}' of class '{type.Name}'.");

                injectableProperty.SetValue(instance, resolvedInstance);
            }
        }

        private bool IsInjectable(object obj)
        {
            var members = obj.GetType().GetMembers(BINDING_FLAGS);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
        #endregion
    }
}