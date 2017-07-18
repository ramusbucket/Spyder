using System.Collections.Generic;
using EMS.Infrastructure.Common.Exceptions;
using EMS.Infrastructure.DependencyInjection.UnitTests.Helpers;
using NUnit.Framework;

namespace EMS.Infrastructure.DependencyInjection.UnitTests
{
    [TestFixture]
    public class UnityInjectorRegisterTests : UnityInjectorTests
    {
        [Test]
        public void Register_InterfaceToConcreteTypeWithoutInjectionParameters_ResolvesToTypeWithEmptyConstructor()
        {
            this.Injector.Register<IRobot, Robocop>();

            var robotInstance = this.Injector.Resolve<IRobot>();

            Assert.AreEqual(
                expected: RobotModel.NotSpecified,
                actual: robotInstance.Model);
            Assert.AreEqual(
                expected: BatteryLevel.NotSpecified,
                actual: robotInstance.Battery);
            Assert.AreEqual(
                expected: IntelligenceLevel.NotSpecified,
                actual: robotInstance.Intelligence);
        }

        [Test]
        public void Register_InterfaceToConcreteTypeWithoutInjectionParameters_ResolveMultipleTimesReturnsDifferentInstanceEachTime()
        {
            this.Injector.Register<IRobot, Robocop>();

            var firstRobot = this.Injector.Resolve<IRobot>();
            var secondRobot = this.Injector.Resolve<IRobot>();

            Assert.AreNotSame(firstRobot, secondRobot);
        }

        [TestCase(RobotModel.Sonata, BatteryLevel.Full, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Obelisk, BatteryLevel.Half, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Empty, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Quarter, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.NotSpecified, BatteryLevel.NotSpecified, IntelligenceLevel.NotSpecified)]
        public void Register_InterfaceToConcreteTypeInjectionParameters_ResolveMultipleTimesReturnsDifferentInstanceEachTime(
            RobotModel robotModel,
            BatteryLevel batteryLevel,
            IntelligenceLevel intelligenceLevel)
        {
            this.Injector.Register<IRobot, Robocop>(robotModel, batteryLevel, intelligenceLevel);

            var firstRobot = this.Injector.Resolve<IRobot>();
            var secondRobot = this.Injector.Resolve<IRobot>();

            Assert.AreNotSame(firstRobot, secondRobot);
        }

        [TestCase(RobotModel.Sonata, BatteryLevel.Full, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Obelisk, BatteryLevel.Half, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Empty, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Quarter, IntelligenceLevel.Extraordinary)]
        [TestCase(RobotModel.NotSpecified, BatteryLevel.NotSpecified, IntelligenceLevel.NotSpecified)]
        public void Register_InterfaceToConcreteTypeWithInjectionParameters_ResolvesToTypeWithExpectedConstructor(
            RobotModel robotModel,
            BatteryLevel batteryLevel,
            IntelligenceLevel intelligenceLevel)
        {
            this.Injector.Register<IRobot, Robocop>(
                robotModel,
                batteryLevel,
                intelligenceLevel);

            var robotInstance = this.Injector.Resolve<IRobot>();

            Assert.AreEqual(
                expected: robotModel,
                actual: robotInstance.Model);
            Assert.AreEqual(
                expected: batteryLevel,
                actual: robotInstance.Battery);
            Assert.AreEqual(
                expected: intelligenceLevel,
                actual: robotInstance.Intelligence);
        }

        [TestCase(RobotModel.Sonata)]
        [TestCase(RobotModel.Obelisk)]
        [TestCase(RobotModel.Excelsior)]
        public void Register_InterfaceToConcreteTypeWithOneMissingInjectionParameters_ThrowsConstructorNotFoundException(RobotModel robotModel)
        {
            var exc = Assert.Throws<ConstructorNotFoundException>(
                () => this.Injector.Register<IRobot, Robocop>(robotModel));
        }

        [TestCase(RobotModel.Sonata, BatteryLevel.Full)]
        [TestCase(RobotModel.Obelisk, BatteryLevel.Half)]
        [TestCase(RobotModel.Excelsior, BatteryLevel.Quarter)]
        public void Register_InterfaceToConcreteTypeWithTwoMissingInjectionParameters_ThrowsConstructorNotFoundException(RobotModel robotModel, BatteryLevel batteryLevel)
        {
            var exc = Assert.Throws<ConstructorNotFoundException>(
                () => this.Injector.Register<IRobot, Robocop>(robotModel, batteryLevel));
        }
    }
}
