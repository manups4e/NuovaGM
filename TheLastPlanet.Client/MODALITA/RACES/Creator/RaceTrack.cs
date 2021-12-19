using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.TimeWeather;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Races.Creator
{
	public class RaceTrack
	{
		public string Titolo { get; set; }
		public string Descrizione { get; set; }
		public string PlayerCreatore { get; set; }
		public int MaxPlayers { get; set; }
		public DateTime DataCreazione { get; set; }
		public int TipoGara { get; set; }
		public int TipoGiri { get; set; }
		public int Laps { get; set; }
		public List<TrackPiece> Props = new();
		public List<CheckPoint> CheckPoints = new();
		public int GrigliaIniziale { get; set; }
		public int Orario { get; set; }
		public int Meteo { get; set; }
		public List<VehicleClass> ClassiPermesse = new();
		public VehicleClass ClasseDefault { get; set; }
		public VehicleHash VeicoloDefault { get; set; }
		public List<VehicleHash> VeicoliEsclusi = new();
		public int Traffico { get; set; }
	}

	public class TrackPiece
	{
		[JsonIgnore]
		public Prop Entity { get; set; }
		public RacingProps Prop { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public int Color { get;  set; }
		public float SpeedIntensity { get; set; } = 0;
		public float SlowIntensity { get; set; } = 0;
		public string SoundID { get; set; }
		public int SoundDistance { get; set; }
		public int VolteGiro { get; set; }
		public bool UnoPerGiro { get;  set; }

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
