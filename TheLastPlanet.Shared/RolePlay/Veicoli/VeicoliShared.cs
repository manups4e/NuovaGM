using System.Collections.Generic;
using System.Drawing;
using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared.Veicoli
{
	[Serialization]
	public partial class OwnedVehicle
	{
		[Ignore]
		[JsonIgnore]
		Vehicle Vehicle = null;
		[JsonProperty("targa")]
		public string Targa { get; set; }
		[JsonProperty("vehicle_data")]
		public VehicleData DatiVeicolo { get; set; }
		[JsonProperty("garage")]
		public VehGarage Garage { get; set; }
		[JsonProperty("stato")]
		public string Stato { get; set; }

		public OwnedVehicle() { }
		public OwnedVehicle(Vehicle veh, string targa, VehicleData data, VehGarage garage, string stato)
		{
			Vehicle = veh;
			Targa = targa;
			DatiVeicolo = data;
			Garage = garage;
			Stato = stato;
		}
		public OwnedVehicle(string targa, VehicleData data, VehGarage garage, string stato)
		{
			Targa = targa;
			DatiVeicolo = data;
			Garage = garage;
			Stato = stato;
		}
		public OwnedVehicle(string targa, string data, string garage, string stato)
		{
			Targa = targa;
			DatiVeicolo = data.FromJson<VehicleData>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			Garage = garage.FromJson<VehGarage>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			Stato = stato;
		}

		public OwnedVehicle(dynamic data)
		{
			Targa = data.targa;
			DatiVeicolo = (data.vehicle_data as string).FromJson<VehicleData>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			Garage = (data.garage as string).FromJson<VehGarage>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			Stato = data.stato;
		}
	}

	[Serialization]
	public partial class VehGarage
	{
		public bool InGarage { get; set; }
		public string Garage { get; set; }
		public int Posto { get; set; }
		public VehGarage() { }
		public VehGarage(bool ingarage, string garageName, int posto)
		{
			InGarage = ingarage;
			Garage = garageName;
			Posto = posto;
		}
		public VehGarage(dynamic data)
		{
			InGarage = data.ingarage;
			Garage = data.garageName;
			Posto = data.posto;
		}
	}

	[Serialization]
	public partial class VehicleData
	{
		[Ignore]
		[JsonIgnore]
		public long Assicurazione;
		[Ignore]
		[JsonIgnore]
		public VehProp props = new VehProp();
		public bool Rubato { get; set; }
		public VehicleData() { }
		public VehicleData(long insurance, VehProp dati, bool stolen)
		{
			Assicurazione = insurance;
			props = dati;
			Rubato = stolen;
		}
	}

	[Serialization]
	public partial class VehProp
	{
		public int Model { get; set; }
		public string Name { get; set; }
		public string Plate { get; set; }
		public int PlateIndex { get; set; }
		public float BodyHealth { get; set; }
		public float EngineHealth { get; set; }
		public float FuelLevel { get; set; }
		public float DirtLevel { get; set; }
		public int PrimaryColor { get; set; }
		public int SecondaryColor { get; set; }
		[Force] public int CustomPrimaryColorInt { get => CustomPrimaryColor.ToArgb(); set { CustomPrimaryColor = Color.FromArgb(value); } }
		[Force] public int CustomSecondaryColorInt { get => CustomSecondaryColor.ToArgb(); set { CustomSecondaryColor = Color.FromArgb(value); } }
		[Ignore] public Color CustomPrimaryColor { get; set; }
		[Ignore] public Color CustomSecondaryColor { get; set; }
		public bool HasCustomPrimaryColor { get; set; }
		public bool HasCustomSecondaryColor { get; set; }
		public int PearlescentColor { get; set; }
		public int WheelColor { get; set; }
		public int Wheels { get; set; }
		public int WindowTint { get; set; }
		public bool[] NeonEnabled = new bool[4];
		public bool[] Extras = new bool[13];
		[Force] public int NeonColorInt { get => NeonColor.ToArgb(); set { NeonColor = Color.FromArgb(value); } }
		[Force] public int TireSmokeColorInt { get => TireSmokeColor.ToArgb(); set { TireSmokeColor = Color.FromArgb(value); } }
		[Ignore] public Color NeonColor { get; set; }
		[Ignore] public Color TireSmokeColor { get; set; }
		public List<VehMod> Mods { get; set; }
		public bool ModKitInstalled { get; set; }
		public int ModLivery { get; set; }
		public VehProp() { }
		public VehProp(int model, string name, string plate, int plateIndex, float bodyHealth, float engineHealth, float fuelLevel, float dirtLevel, int color1, int color2, Color custom1, Color custom2, bool hasCustom1, bool hasCustom2, int pearlescentColor, int wheelColor, int wheels, int windowTint, bool[] neonEnabled, bool[] extras, Color neonColor, Color tyreSmokeColor, bool modkit, List<VehMod> mods, int modLivery)
		{
			Model = model;
			Name = name;
			Plate = plate;
			PlateIndex = plateIndex;
			BodyHealth = bodyHealth;
			EngineHealth = engineHealth;
			FuelLevel = fuelLevel;
			DirtLevel = dirtLevel;
			PrimaryColor = color1; SecondaryColor = color2;
			CustomPrimaryColor = custom1; CustomSecondaryColor = custom2;
			HasCustomPrimaryColor = hasCustom1; HasCustomSecondaryColor = hasCustom2;
			PearlescentColor = pearlescentColor; WheelColor = wheelColor;
			Wheels = wheels;
			WindowTint = windowTint;
			NeonEnabled = neonEnabled;
			Extras = extras;
			NeonColor = neonColor;
			TireSmokeColor = tyreSmokeColor;
			ModKitInstalled = modkit;
			Mods = mods;
			ModLivery = modLivery;
		}
	}

	[Serialization]
	public partial class VehMod
	{
		public int ModIndex { get; set; }
		public int Value { get; set; }
		public string ModName { get; set; }
		public string modType { get; set; }

		public VehMod(int modIndex, int value, string name, string type)
		{
			ModIndex = modIndex;
			Value = value;
			ModName = name;
			modType = type;
		}
	}

	[Serialization]
	public partial class _VeicoliAffitto
	{
		public List<Veicoloaff> biciclette { get; set; }
		public List<Veicoloaff> macchineGeneric { get; set; }
		public List<Veicoloaff> macchineMedium { get; set; }
		public List<Veicoloaff> macchineSuper { get; set; }
		public List<Veicoloaff> motoGeneric { get; set; }
		public List<Veicoloaff> motoMedium { get; set; }
		public List<Veicoloaff> motoSuper { get; set; }
	}

	[Serialization]
	public partial class Veicoloaff
	{
		public string name { get; set; }
		public string description { get; set; }
		public int price { get; set; }
		public string model { get; set; }
		public Veicoloaff() { }
		public Veicoloaff(string _name, string _desc, int _price, string _model)
		{
			this.name = _name;
			this.description = _desc;
			this.price = _price;
			this.model = _model;
		}
		public Veicoloaff(string _name, int _price, string _desc, string _model)
		{
			this.name = _name;
			this.description = _desc;
			this.price = _price;
			this.model = _model;
		}
	}

	[Serialization]
	public partial class Treno
	{
		public int entity { get; set; }
		public int spawnidx { get; set; }

		public Treno() { }
		public Treno(int _ent, int sp)
		{
			this.entity = _ent;
			this.spawnidx = sp;
		}
	}
}
