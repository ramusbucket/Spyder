namespace EMS.Infrastructure.DependencyInjection.UnitTests.Helpers
{
    public interface IRobot
    {
        RobotModel Model { get; set; }

        BatteryLevel Battery { get; set; }

        IntelligenceLevel Intelligence { get; set; }
    }
}
