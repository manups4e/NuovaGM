using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using CitizenFX.Core;
using Newtonsoft.Json;
#if CLIENT
#elif SERVER
#endif
namespace NuovaGM.Shared.Veicoli
{
	public class OwnedVehicle
	{
		[JsonIgnore]
		Vehicle Vehicle = null;
		[JsonProperty("targa")]
		public string Targa;
		[JsonProperty("vehicle_data")]
		public VehicleData DatiVeicolo = new VehicleData();
		[JsonProperty("in_garage")]
		public bool InGarage;
		[JsonProperty("stato")]
		public string Stato;

		public OwnedVehicle() { }
		public OwnedVehicle(string targa, VehicleData data, bool garage, string stato) 
		{
			Targa = targa;
			DatiVeicolo = data;
			InGarage = garage;
			Stato = stato;
		}
	}

	public class VehicleData
	{
		public long Assicurazione;
		public VehProp props = new VehProp();
		public bool Rubato;
	}

	public class VehProp
	{
		public int Model;
		public string Plate;
		public int PlateIndex;
		public float BodyHealth;
		public float EngineHealth;
		public float DirtLevel;
		public int Color1;
		public int Color2;
		public int PearlescentColor;
		public int WheelColor;
		public int Wheels;
		public int WindowTint;
		public List<bool> NeonEnabled = new List<bool>();
		public List<bool> Extras = new List<bool>();
		public int NeonColorR;
		public int NeonColorG;
		public int NeonColorB;
		public int TyreSmokeColorR;
		public int TyreSmokeColorG;
		public int TyreSmokeColorB;
		public int ModSpoilers;
		public int ModFrontBumper;
		public int ModRearBumper;
		public int ModSideSkirt;
		public int ModExhaust;
		public int ModFrame;
		public int ModGrille;
		public int ModHood;
		public int ModFender;
		public int ModRightFender;
		public int ModRoof;
		public int ModEngine;
		public int ModBrakes;
		public int ModTransmission;
		public int ModHorns;
		public int ModSuspension;
		public int ModArmor;
		public bool ModTurbo;
		public bool ModSmokeEnabled;
		public bool ModXenon;
		public int ModFrontWheels;
		public int ModBackWheels;
		public int ModPlateHolder;
		public int ModVanityPlate;
		public int ModTrimA;
		public int ModOrnaments;
		public int ModDashboard;
		public int ModDial;
		public int ModDoorSpeaker;
		public int ModSeats;
		public int ModSteeringWheel;
		public int ModShifterLeaver;
		public int ModAPlate;
		public int ModSpeakers;
		public int ModTrunk;
		public int ModHydrolic;
		public int ModEngineBlock;
		public int ModAirFilter;
		public int ModStruts;
		public int ModArchCover;
		public int ModAerials;
		public int ModTrimB;
		public int ModTank;
		public int ModWindows;
		public int ModLivery;
		public VehProp() { }
		public VehProp(int model, string plate, int plateIndex, float bodyHealth, float engineHealth, float dirtLevel, int color1, int color2, int pearlescentColor, int wheelColor, int wheels, int windowTint, List<bool> neonEnabled, List<bool> extras, int neonColorR, int neonColorG, int neonColorB, int tyreSmokeColorR, int tyreSmokeColorG, int tyreSmokeColorB, int modSpoilers, int modFrontBumper, int modRearBumper, int modSideSkirt, int modExhaust, int modFrame, int modGrille, int modHood, int modFender, int modRightFender, int modRoof, int modEngine, int modBrakes, int modTransmission, int modHorns, int modSuspension, int modArmor, bool modTurbo, bool modSmokeEnabled, bool modXenon, int modFrontWheels, int modBackWheels, int modPlateHolder, int modVanityPlate, int modTrimA, int modOrnaments, int modDashboard, int modDial, int modDoorSpeaker, int modSeats, int modSteeringWheel, int modShifterLeaver, int modAPlate, int modSpeakers, int modTrunk, int modHydrolic, int modEngineBlock, int modAirFilter, int modStruts, int modArchCover, int modAerials, int modTrimB, int modTank, int modWindows, int modLivery)
		{
			Model = model;
			Plate = plate;
			PlateIndex = plateIndex;
			BodyHealth = bodyHealth;
			EngineHealth = engineHealth;
			DirtLevel = dirtLevel;
			Color1 = color1; Color2 = color2;
			PearlescentColor = pearlescentColor; WheelColor = wheelColor;
			Wheels = wheels;
			WindowTint = windowTint;
			NeonEnabled = neonEnabled;
			Extras = extras;
			NeonColorR = neonColorR; NeonColorG = neonColorG; NeonColorB = neonColorB;
			TyreSmokeColorR = tyreSmokeColorR; TyreSmokeColorG = tyreSmokeColorG; TyreSmokeColorB = tyreSmokeColorB;
			ModSpoilers = modSpoilers;
			ModFrontBumper = modFrontBumper;
			ModRearBumper = modRearBumper;
			ModSideSkirt = modSideSkirt;
			ModExhaust = modExhaust;
			ModFrame = modFrame;
			ModGrille = modGrille;
			ModHood = modHood;
			ModFender = modFender;
			ModRightFender = modRightFender;
			ModRoof = modRoof;
			ModEngine = modEngine;
			ModBrakes = modBrakes;
			ModTransmission = modTransmission;
			ModHorns = modHorns;
			ModSuspension = modSuspension;
			ModArmor = modArmor;
			ModTurbo = modTurbo;
			ModSmokeEnabled = modSmokeEnabled;
			ModXenon = modXenon;
			ModFrontWheels = modFrontWheels;
			ModBackWheels = modBackWheels;
			ModPlateHolder = modPlateHolder;
			ModVanityPlate = modVanityPlate;
			ModTrimA = modTrimA;
			ModOrnaments = modOrnaments;
			ModDashboard = modDashboard;
			ModDial = modDial;
			ModDoorSpeaker = modDoorSpeaker;
			ModSeats = modSeats;
			ModSteeringWheel = modSteeringWheel;
			ModShifterLeaver = modShifterLeaver;
			ModAPlate = modAPlate;
			ModSpeakers = modSpeakers;
			ModTrunk = modTrunk;
			ModHydrolic = modHydrolic;
			ModEngineBlock = modEngineBlock;
			ModAirFilter = modAirFilter;
			ModStruts = modStruts;
			ModArchCover = modArchCover;
			ModAerials = modAerials;
			ModTrimB = modTrimB;
			ModTank = modTank;
			ModWindows = modWindows;
			ModLivery = modLivery;
		}
	}

	public class VeicoliAffitto
	{
		public List<Veicoloaff> biciclette = new List<Veicoloaff>();
		public List<Veicoloaff> macchineGeneric = new List<Veicoloaff>();
		public List<Veicoloaff> macchineMedium = new List<Veicoloaff>();
		public List<Veicoloaff> macchineSuper = new List<Veicoloaff>();
		public List<Veicoloaff> motoGeneric = new List<Veicoloaff>();
		public List<Veicoloaff> motoMedium = new List<Veicoloaff>();
		public List<Veicoloaff> motoSuper = new List<Veicoloaff>();
	}

	public class Veicoloaff
	{
		public string name;
		public string description;
		public int price;
		public string model;
		public Veicoloaff(string _name, string _desc, int _price, string _model)
		{
			this.name = _name;
			this.description = _desc;
			this.price = _price;
			this.model = _model;
		}
	}

	public class Treno
	{
		public int entity;
		public int spawnidx;

		public Treno() { }
		public Treno(int _ent, int sp)
		{
			this.entity = _ent;
			this.spawnidx = sp;
		}
	}
}
