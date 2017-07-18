using EMS.Infrastructure.DependencyInjection.Interfaces;
using NUnit.Framework;

namespace EMS.Infrastructure.DependencyInjection.UnitTests
{
    [TestFixture]
    public class UnityInjectorTests
    {
        protected IInjector Injector { get; set; }

        [SetUp]
        public virtual void TestSetup()
        {
            this.Injector = UnityInjector.Instance;
        }
    }
}
