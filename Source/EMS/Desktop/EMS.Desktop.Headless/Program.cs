using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace EMS.Desktop.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonConfig = string.Empty;
            var dependenciesRegister = new DependenciesRegister();
            var injector = dependenciesRegister.RegisterDependencies(jsonConfig);

            var type = typeof(IListener);
            var listenerTypes = Assembly.Load("EMS.Desktop.Headless")
                .GetTypes()
                .Where(
                    x =>
                        !x.IsAbstract &&
                        !x.IsInterface &&
                        x.IsClass &&
                        type.IsAssignableFrom(x));

            var listeners = listenerTypes.Select(x => injector.Resolve<IListener>(x.Name));
            var listenerTasks = new List<Task>(listeners.Count());
            foreach(var listener in listeners)
            {
                listenerTasks.Add(
                    Task.Run(
                        async () => await listener.Start()));
            }

            Task.WaitAll(listenerTasks.ToArray());
        }
    }
}
