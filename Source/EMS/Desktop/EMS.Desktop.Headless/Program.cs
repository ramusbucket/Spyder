using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Desktop.Headless
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonConfig = string.Empty;
            var dependenciesRegister = new DependenciesRegister();
            var injector = dependenciesRegister.RegisterDependencies(jsonConfig);

            var keyboardListener = injector.Resolve<IListener>(nameof(KeyboardListener));

            var task = Task.Run(() => keyboardListener.Start());

            Task.WaitAll(task);
        }
    }
}
