using System.Collections.Generic;

namespace TheLastPlanet.Shared
{

    public class PoliceSettings
    {
        public bool EnableBlipsPolice { get; set; }
        public bool EnableCuffTimer { get; set; }
        public int CuffTimer { get; set; }
        public int MaxOnService { get; set; }
        public bool CanTackle { get; set; }
        public bool EnableKNine { get; set; }
        public List<PoliceDog> Dogs { get; set; }
        public List<string> AuthorizedModifications { get; set; }
        public List<PoliceStation> Stations { get; set; }
    }


    public class MedicsSettings
    {
        public bool EnableBlipCars { get; set; }
        public List<Hospital> Hospitals { get; set; }
    }


    public class CarDealerSettings
    {
        public Position BossActions { get; set; }
        public Position SellMenu { get; set; }
    }

    public class RealEstateSettings
    {
        public Position BossActions { get; set; }
        public Position Entrance { get; set; }
        public Position Exit { get; set; }
        public Position Inside { get; set; }
        public Position Outside { get; set; }
        public Position Actions { get; set; }
    }

    public class JobGrade
    {
        public int Id { get; set; }
        public int PayCheck { get; set; }
        public WorkUniform WorkUniforms { get; set; }
    }

    public class WorkUniform
    {
        public Uniform Male { get; set; }
        public Uniform Female { get; set; }
    }

    public class Uniform
    {
        public ComponentDrawables Dress { get; set; }
        public ComponentDrawables TextureDress { get; set; }
        public PropIndices Accessories { get; set; }
        public PropIndices TexturesAccessories { get; set; }
    }


    public class PoliceDog
    {
        public string Name { get; set; }
        public string Model { get; set; }
    }


    public class Authorized
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public List<int> AuthorizedGrades { get; set; }
    }



    public class PoliceStation
    {
        public JobBlip Blip { get; set; }
        public List<Position> LockerRoom { get; set; }
        public List<Position> Armouries { get; set; }
        public List<Authorized> AuthorizedVehicles { get; set; }
        public List<Authorized> AuthorizedHelicopters { get; set; }
        public List<Authorized> AuthorizedWeapons { get; set; }
        public List<SpawnerSpawn> Vehicles { get; set; }
        public List<SpawnerSpawn> Copters { get; set; }
        public List<Position> BossActions { get; set; }
    }


    public class Hospital
    {
        public JobBlip Blip { get; set; }
        public List<Position> LockerRooms { get; set; }
        public List<Position> Pharmacies { get; set; }
        public List<Position> VisitorsEntrances { get; set; }
        public List<Position> VisitorsExits { get; set; }
        public List<Authorized> AuthorizedVehicles { get; set; }
        public List<Authorized> AutorizedHelicopters { get; set; }
        public List<SpawnerSpawn> Vehicles { get; set; }
        public List<SpawnerSpawn> Helicopters { get; set; }
        public List<Position> BossActions { get; set; }
    }


    public class JobBlip
    {
        public Position Coords { get; set; }
        public int Sprite { get; set; }
        public int Display { get; set; }
        public float Scale { get; set; }
        public int Color { get; set; }
        public string Name { get; set; }
    }

    public class SpawnerSpawn
    {
        public Position SpawnerMenu { get; set; }
        public List<SpawnPoints> SpawnPoints { get; set; }
        public List<Position> Deleters { get; set; }
    }


    public class SpawnPoints
    {
        public Position Coords { get; set; }
        public float Heading { get; set; }
        public float Radius { get; set; }
    }
}