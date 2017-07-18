namespace EMS.Infrastructure.DependencyInjection.UnitTests.Helpers
{
    public class IronMan : BaseRobot
    {
        public IronMan()
            : base()
        {
        }

        public IronMan(RobotModel model, BatteryLevel battery, IntelligenceLevel intelligence) 
            : base(model, battery, intelligence)
        {
        }
    }
}
