namespace TheLastPlanet.Shared
{
    public class JobVeh_Rent
    {
        public Vehicle Vehicle { get; set; }
        public string Owner { get; set; }

        public JobVeh_Rent(Vehicle veh, string proprietario)
        {
            Vehicle = veh;
            Owner = proprietario;
        }
    }

    public class VehiclePolice
    {
        public string Plate { get; set; }
        public int Model { get; set; }
        public int Handle { get; set; }

        public VehiclePolice(string plate, int model, int handle)
        {
            Plate = plate;
            Model = model;
            Handle = handle;
        }
    }
}