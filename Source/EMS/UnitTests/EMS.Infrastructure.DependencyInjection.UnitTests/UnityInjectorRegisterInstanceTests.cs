using EMS.Infrastructure.DependencyInjection.UnitTests.Helpers;
using NUnit.Framework;

namespace EMS.Infrastructure.DependencyInjection.UnitTests
{
    [TestFixture]
    public class UnityInjectorRegisterInstanceTests : UnityInjectorTests
    {
        [Test]
        public void RegisterInstance_EmptyConstructor_ResolvesToTheSameInstance()
        {
            var robot = new Robocop();
            this.Injector.RegisterInstance<IRobot>(robot);

            var resolvedRobot = this.Injector.Resolve<IRobot>();

            Assert.AreSame(robot, resolvedRobot);
        }

        [TestCase(RobotModel.Sonata, BatteryLevel.Full, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Obelisk, BatteryLevel.Half, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Empty, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Quarter, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.NotSpecified, BatteryLevel.NotSpecified, IntelligenceLevel.NotSpecified)]
        public void RegisterInstance_ConstructorWithParameters_ResolvesToTheSameInstance(
            RobotModel robotModel,
            BatteryLevel batteryLevel,
            IntelligenceLevel intelligenceLevel)
        {
            var robot = new Robocop(robotModel, batteryLevel, intelligenceLevel);
            this.Injector.RegisterInstance<IRobot>(robot);

            var resolvedRobot = this.Injector.Resolve<IRobot>();

            Assert.AreSame(robot, resolvedRobot);
        }

        [Test]
        public void RegisterInstance_WithKeyAndEmptyConstructor_ResolvesToTheSameInstance()
        {
            var robocop = new Robocop();
            var robocopKey = nameof(Robocop);
            var ironMan = new IronMan();
            var ironManKey = nameof(IronMan);

            this.Injector.RegisterInstance<IRobot>(robocopKey, robocop);
            this.Injector.RegisterInstance<IRobot>(ironManKey, ironMan);

            var resolvedRobocop = this.Injector.Resolve<IRobot>(robocopKey);
            var resolvedIronMan = this.Injector.Resolve<IRobot>(ironManKey);

            Assert.AreSame(robocop, resolvedRobocop);
            Assert.AreSame(ironMan, resolvedIronMan);
            Assert.AreNotSame(resolvedIronMan, resolvedRobocop);
        }

        [TestCase(RobotModel.Sonata, BatteryLevel.Full, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Obelisk, BatteryLevel.Half, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Empty, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Quarter, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.NotSpecified, BatteryLevel.NotSpecified, IntelligenceLevel.NotSpecified)]
        public void RegisterInstance_WithKeyAndEmptyConstructor_ResolvesToTheSameInstance(
            RobotModel robotModel,
            BatteryLevel batteryLevel,
            IntelligenceLevel intelligenceLevel)
        {
            var robot = new Robocop(robotModel, batteryLevel, intelligenceLevel);
            this.Injector.RegisterInstance<IRobot>(robot);

            var resolvedRobot = this.Injector.Resolve<IRobot>();

            Assert.AreSame(robot, resolvedRobot);
        }
    }
}
