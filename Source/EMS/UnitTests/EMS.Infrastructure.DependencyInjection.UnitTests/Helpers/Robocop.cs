namespace EMS.Infrastructure.DependencyInjection.UnitTests.Helpers
{
    public class Robocop : BaseRobot
    {
        public Robocop()
            : base()
        {
        }

        public Robocop(RobotModel model, BatteryLevel battery, IntelligenceLevel intelligence) 
            : base(model, battery, intelligence)
        {
        }
    }
}
