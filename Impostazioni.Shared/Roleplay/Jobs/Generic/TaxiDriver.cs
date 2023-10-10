using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.Generics
{
    public class Tassisti
    {
        public Vector3 PosAcceptance { get; set; }
        public Vector3 PosVehicleCollection { get; set; }
        public Vector3 PosVehicleReturn { get; set; }
        public Vector4 PosSpawnVeicolo { get; set; }
        public float PriceModifier { get; set; }
        public float pickupRange { get; set; }
        public List<Vector3> jobCoords { get; set; }
    }
}