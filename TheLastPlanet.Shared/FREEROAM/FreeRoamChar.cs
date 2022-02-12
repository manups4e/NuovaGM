using System.Collections.Generic;
using TheLastPlanet.Shared.Internal.Events.Attributes;
using TheLastPlanet.Shared.Veicoli;


// TODO: DEVO AGGIUNGERE IL SAlVATAGGIO NEL DB (guardare tickHandler.cs)
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

    [Serialization]
    public partial class FreeRoamChar
    {
        public ulong CharID { get; set; }
        public bool is_dead { get; set; }
        public Finance Finance { get; set; }
        public Position Posizione { get; set; }
        public Gang Gang { get; set; }
        public Skin Skin { get; set; }
        public List<Weapons> Weapons { get; set; }
        public List<string> Proprietà { get; set; } // da aggiungere
        public List<OwnedVehicle> Veicoli { get; set; } // da aggiungere
        public Dressing Dressing { get; set; }
        public FreeRoamStats Statistiche { get; set; }
        public int Level { get; set; } = 1;
        public int TotalXp { get; set; }

        public FreeRoamChar() { }

        public FreeRoamChar(ulong id)
        {
            CharID = id;
            Finance = new();
            Gang = new();
            Skin = new();
            Dressing = new();
            Weapons = new();
            Statistiche = new();
        }

        public FreeRoamChar(ulong id, Finance finance, Gang gang, Skin skin, Dressing dressing, List<Weapons> weapons, FreeRoamStats statistiche)
        {
            CharID = id;
            Finance = finance;
            Gang = gang;
            Skin = skin;
            Dressing = dressing;
            Weapons = weapons;
            Statistiche = statistiche;
        }
    }

    [Serialization]
    public partial class FreeRoamStats
    {
        public float STAMINA { get; set; } = 0;
        public float STRENGTH { get; set; } = 0;
        public float LUNG_CAPACITY { get; set; } = 0;
        public float SHOOTING_ABILITY { get; set; } = 0;
        public float WHEELIE_ABILITY { get; set; } = 0;
        public float FLYING_ABILITY { get; set; } = 0;
        public float STEALTH_ABILITY { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public int Prestige { get; set; } = 0;
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;
        public int Headshots { get; set; } = 0;
        public int MaxKillStreak { get; set; } = 0;
        public int MissionsDone { get; set; } = 0;
        public int EventsWon { get; set; } = 0;
    }

    [Serialization]
    public partial class PlayerScore
    {
        public int EventId { get; set; }
        public float EventXpMultiplier { get; set; } = 1.0f;
        public float CurrentAttempt { get; set; }
        public float BestAttempt { get; set; }
    }

    [Serialization]
    public partial class FreeRoamChar_Metadata
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
        public int level;
        public int totalXp;
    }
}
