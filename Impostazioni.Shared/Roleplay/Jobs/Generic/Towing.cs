using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.Generics
{
    public class Towing
    {
        public Vector3 JobStart { get; set; }
        public List<Vector3> DespawnPoint { get; set; }
        public List<string> TowableVehicles { get; set; }
        public List<Vector4> SpawnVehicles { get; set; }
    }
}