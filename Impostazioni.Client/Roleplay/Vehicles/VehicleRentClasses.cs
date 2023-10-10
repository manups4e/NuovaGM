using System.Collections.Generic;
using TheLastPlanet.Shared.Vehicles;

namespace Settings.Client.Configuration.Vehicles
{
    public class VehicleRentClasses
    {
        public List<RentVehicle> Bycicles { get; set; }
        public List<RentVehicle> CarsGeneric { get; set; }
        public List<RentVehicle> CarsMedium { get; set; }
        public List<RentVehicle> CarsSuper { get; set; }
        public List<RentVehicle> BykesGeneric { get; set; }
        public List<RentVehicle> BykesMedium { get; set; }
        public List<RentVehicle> BykesSuper { get; set; }
    }
}