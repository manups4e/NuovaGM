using System.Collections.Generic;

namespace Settings.Shared.Roleplay.Jobs.Generics
{
    public class Fishermen
    {
        public bool DynamicFishingTime { get; set; }
        public int FixedTime { get; set; }
        public int SellingFishPrice { get; set; }
        public int SellingOthersPrice { get; set; }
        public List<Vector3> SellingPoints { get; set; }
        public Vector4 BoatSpawn { get; set; }
        public List<string> Boats { get; set; }
        public CoughtFish Fishes { get; set; }
    }

    public class CoughtFish
    {
        public List<string> Easy = new List<string>();
        public List<string> Intermediate = new List<string>();
        public List<string> Advanced = new List<string>();
    }

}