namespace EMS.Infrastructure.DependencyInjection.UnitTests.Helpers
{
    public abstract class BaseRobot : IRobot
    {
        public BaseRobot()
        {
            this.Model = RobotModel.NotSpecified;
            this.Battery = BatteryLevel.NotSpecified;
            this.Intelligence = IntelligenceLevel.NotSpecified;
        }

        public BaseRobot(RobotModel model, BatteryLevel battery, IntelligenceLevel intelligence)
        {
            this.Model = model;
            this.Battery = battery;
            this.Intelligence = intelligence;
        }

        public BatteryLevel Battery { get; set; }

        public IntelligenceLevel Intelligence { get; set; }

        public RobotModel Model { get; set; }
    }
}
