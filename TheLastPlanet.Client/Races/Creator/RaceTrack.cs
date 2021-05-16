using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.RolePlay.TimeWeather;
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
		public List<TrackPieces> Props = new();
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

	public class TrackPieces
	{
		public RacingProps Prop { get; set; }
		public Vector3 Position { get; set; }
	}

	public class CheckPoint
	{
		public int Id { get; set; }
		public Vector3 Position { get; set; }
	}
}
