using FxEvents.Shared.Snowflakes;

namespace TheLastPlanet.Shared
{
    public class Employee
    {
        public Snowflake Seed { get; set; }
        public string Name { get; set; }
        public object[] Role { get; set; }
        public int Salary { get; set; }
        public int MonthlyRevenue { get; set; }
        public int TotalRevenue { get; set; }
    }
}