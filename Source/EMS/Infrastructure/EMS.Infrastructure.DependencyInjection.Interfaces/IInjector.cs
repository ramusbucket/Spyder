using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Infrastructure.DependencyInjection.Interfaces
{
    public interface IInjector
    {
        IInjector Register<TFrom, TTo>()
            where TTo : TFrom;

        IInjector Register<TFrom, TTo>(params object[] injectionParams)
            where TTo : TFrom;

        IInjector Register<TFrom, TTo>(string key)
           where TTo : TFrom;

        IInjector Register<TFrom, TTo>(string key, params object[] injectionParams)
            where TTo : TFrom;

        IInjector RegisterInstance<T>(T instance);

        IInjector RegisterInstance<T>(string key, T instance);

        T Resolve<T>();

        T Resolve<T>(string key);
    }
}
