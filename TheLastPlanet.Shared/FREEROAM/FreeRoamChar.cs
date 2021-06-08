using CitizenFX.Core;
using TheLastPlanet.Shared.Veicoli;
using System;
using System.Collections.Generic;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using Logger;

namespace TheLastPlanet.Shared
{
	public enum PlayerStats : int
	{
		NumberNearMisses = -1058879197,
		NumberNearMissesNoCrash = -791767133,
		FastestSpeedInCar = -2133168010,
		MostPistolHeadshots = -252472232,
		LONGEST_2WHEEL_DIST = 1061163284,
		LONGEST_2WHEEL_TIME = 1575087458,
		LongestSurvivedFreefall = 742230262,
		HighestSkittles = -1411284426,
		FarthestJumpDistance = 908833912,
		HighestJumpDistance = 1831206202,
		FlyUnderBridges = -1169860382,
		MostFlipsInOneJump = 1172390698,
		MostSpinsInOneJump = -370868036,
	}



	public enum PlayerStatType
	{
		Int,
		Float,
		String,
		CustomInt,
		CustomFloat
	}
	
	public class FreeRoamChar
	{
		public ulong CharID;
		public bool is_dead;
		public Info Info = new();
		public Finance Finance = new();
		public Position Posizione = new();
		public Gang Gang = new();
		public Skin Skin = new();
		public List<Weapons> Weapons = new();
		public List<string> Proprietà = new();
		public List<OwnedVehicle> Veicoli = new();
		public Dressing Dressing = new();
		public FreeRoamStats Statistiche = new();
		public FreeRoamChar() { }
		public int Level = 1;
		public int TotalXp;

		public FreeRoamChar(ulong id, Finance finance, Gang gang, Skin skin, Dressing dressing, List<Weapons> weapons, FreeRoamStats statistiche)
		{
			this.CharID = id;
			this.Finance = finance;
			this.Gang = gang;
			this.Skin = skin;
			this.Dressing = dressing;
			this.Weapons = weapons;
			this.Statistiche = statistiche;
		}
	}

	public class FreeRoamStats
	{
		public float STAMINA { get; set; }
		public float STRENGTH { get; set; }
		public float LUNG_CAPACITY { get; set; }
		public float SHOOTING_ABILITY { get; set; }
		public float WHEELIE_ABILITY { get; set; }
		public float FLYING_ABILITY { get; set; }
		public int Experience { get; set; }
		public int Prestige { get; set; }
		public int Kills { get; set; }
		public int Deaths { get; set; }
		public int Headshots { get; set; }
		public int MaxKillStreak { get; set; }
		public int MissionsDone { get; set; }
		public int EventsWon { get; set; }
	}

	public class PlayerScore
	{
		public int EventId { get; set; }
		public float EventXpMultiplier { get; set; } = 1.0f;
		public float CurrentAttempt { get; set; }
		public float BestAttempt { get; set; }
	}

	public class FreeRoamChar_Metadata
	{
		public int money;/*{ set => Finance.Money = value; }*/
		public int bank;/*{ set => Finance.Bank = value; }*/
		public string location;/*{ set => Posizione = value.FromJson<Position>(); }*/
		public string gang;/*{ set => Gang.Name = value; }*/
		public int gang_grade;/*{ set => Gang.Grade = value; }*/
		public string skin;/*{ set => Skin = value.FromJson<Skin>(); }*/
		public string weapons;/*{ set => Weapons = value.FromJson<List<Weapons>>(); }*/
		public string dressing;/*{ set => Dressing = value.FromJson<Dressing>(); }*/
		public string statistiche;/*{ set => Statistiche = value.FromJson<Statistiche>(); }*/
	}
}
