using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using CitizenFX.Core;
using Newtonsoft.Json;

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
		public int PrimaryColor;
		public int SecondaryColor;
		public Color CustomPrimaryColor;
		public Color CustomSecondaryColor;
		public bool HasCustomPrimaryColor;
		public bool HasCustomSecondaryColor;
		public int PearlescentColor;
		public int WheelColor;
		public int Wheels;
		public int WindowTint;
		public bool[] NeonEnabled = new bool[4];
		public bool[] Extras = new bool[13];
		public Color NeonColor;
		public Color TyreSmokeColor;
		public List<VehMod> Mods = new List<VehMod>();
		public bool ModKitInstalled;
		public int ModLivery;
		public VehProp() { }
		public VehProp(int model, string plate, int plateIndex, float bodyHealth, float engineHealth, float dirtLevel, int color1, int color2, Color custom1, Color custom2, bool hasCustom1, bool hasCustom2, int pearlescentColor, int wheelColor, int wheels, int windowTint, bool[] neonEnabled, bool[] extras, Color neonColor, Color tyreSmokeColor, bool modkit, List<VehMod> mods, int modLivery)
		{
			Model = model;
			Plate = plate;
			PlateIndex = plateIndex;
			BodyHealth = bodyHealth;
			EngineHealth = engineHealth;
			DirtLevel = dirtLevel;
			PrimaryColor = color1; SecondaryColor = color2;
			CustomPrimaryColor = custom1; CustomSecondaryColor = custom2;
			HasCustomPrimaryColor = hasCustom1; HasCustomSecondaryColor= hasCustom2;
			PearlescentColor = pearlescentColor; WheelColor = wheelColor;
			Wheels = wheels;
			WindowTint = windowTint;
			NeonEnabled = neonEnabled;
			Extras = extras;
			NeonColor = neonColor;
			TyreSmokeColor = tyreSmokeColor;
			ModKitInstalled = modkit;
			Mods = mods;
			ModLivery = modLivery;
		}
	}

	public class VehMod
	{
		public int ModIndex;
		public int Value;
		public string ModName;
		public string modType;
		
		public VehMod(int modIndex, int value, string name, string type)
		{
			ModIndex = modIndex;
			Value = value;
			ModName = name;
			modType = type;
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
