using System;
using EMS.Infrastructure.Common.Exceptions;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Microsoft.Practices.Unity;

namespace EMS.Infrastructure.DependencyInjection
{
    public class UnityInjector : IInjector
    {
        private static readonly Lazy<UnityInjector> injectorInstance =
            new Lazy<UnityInjector>(
                 () =>
                    new UnityInjector(
                        new UnityContainer()));

        private readonly UnityContainer container;

        private UnityInjector(UnityContainer container)
        {
            this.container = container;
        }

        public static IInjector Instance
        {
            get
            {
                return injectorInstance.Value;
            }
        }

        public IInjector Register<TFrom, TTo>()
           where TTo : TFrom
        {
            try
            {
                this.container.RegisterType<TFrom, TTo>(new InjectionConstructor());
            }
            catch (Exception exc)
            {
                throw CreateConstructorNotFoundException<TFrom, TTo>(exc);
            }

            return Instance;
        }

        public IInjector Register<TFrom, TTo>(params object[] injectionParams)
            where TTo : TFrom
        {
            try
            {
                this.container.RegisterType<TFrom, TTo>(new InjectionConstructor(injectionParams));
            }
            catch (Exception exc)
            {
                throw CreateConstructorNotFoundException<TFrom, TTo>(exc);
            }

            return Instance;
        }

        public IInjector Register<TFrom, TTo>(string key)
           where TTo : TFrom
        {
            try
            {
                this.container.RegisterType<TFrom, TTo>(key, new InjectionConstructor());
            }
            catch (Exception exc)
            {
                throw CreateConstructorNotFoundException<TFrom, TTo>(exc);
            }

            return Instance;
        }

        public IInjector Register<TFrom, TTo>(string key, params object[] injectionParams)
            where TTo : TFrom
        {
            try
            {
                this.container.RegisterType<TFrom, TTo>(key, new InjectionConstructor(injectionParams));
            }
            catch (Exception exc)
            {
                throw CreateConstructorNotFoundException<TFrom, TTo>(exc);
            }

            return Instance;
        }

        public IInjector RegisterInstance<T>(T instance)
        {
            this.container.RegisterInstance<T>(instance, new ContainerControlledLifetimeManager());
            return Instance;
        }

        public IInjector RegisterInstance<T>(string key, T instance)
        {
            this.container.RegisterInstance<T>(key, instance, new ContainerControlledLifetimeManager());
            return Instance;
        }

        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        public T Resolve<T>(string key)
        {
            return this.container.Resolve<T>(key);
        }

        private ConstructorNotFoundException CreateConstructorNotFoundException<TFrom, TTo>(Exception exc)
            where TTo : TFrom
        {
            var exceptionMessage = $"Unable to register {typeof(TFrom)} from {typeof(TTo)}";
            return new ConstructorNotFoundException(exceptionMessage, exc);
        }
    }
}
