using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TheLastPlanet.Client.Races.Creator
{
    public class RaceTrack
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PlayerCreator { get; set; }
        public int MaxPlayers { get; set; }
        public DateTime DateCreation { get; set; }
        public int RaceType { get; set; }
        public int LapType { get; set; }
        public int Laps { get; set; }
        public List<TrackPiece> Props = new();
        public List<CheckPoint> CheckPoints = new();
        public int StartGrid { get; set; }
        public int Time { get; set; }
        public int Weather { get; set; }
        public List<VehicleClass> AllowedClasses = new();
        public VehicleClass DefaultClass { get; set; }
        public VehicleHash DefaultVehicle { get; set; }
        public List<VehicleHash> ExcludedVehicles = new();
        public int Trafffic { get; set; }
    }

    public class TrackPiece
    {
        [JsonIgnore]
        public Prop Entity { get; set; }
        public RacingProps Prop { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public int Color { get; set; }
        public float SpeedIntensity { get; set; } = 0;
        public float SlowIntensity { get; set; } = 0;
        public string SoundID { get; set; }
        public int SoundDistance { get; set; }
        public int TimeLap { get; set; }
        public bool OnePerLap { get; set; }

        public TrackPiece() { }

        public TrackPiece(Prop piece, RacingProps propHash, Vector3 pos, Vector3 rot, int color)
        {
            Entity = piece;
            Prop = propHash;
            Position = pos;
            Rotation = rot;
            Color = color;
        }

    }

    public class CheckPoint
    {
        public int Id { get; set; }
        public Vector3 Position { get; set; }
    }
}
