namespace TheLastPlanet.Shared
{
    public class Employee
    {
        public string Seed { get; set; }
        public string Name { get; set; }
        public object[] Role { get; set; }
        public int Salary { get; set; }
        public int MonthlyRevenue { get; set; }
        public int TotalRevenue { get; set; }
    }

    public class Business
    {
        public string Seed { get; set; }
        public long Balance { get; set; }
        public long Registered { get; set; }
    }
}