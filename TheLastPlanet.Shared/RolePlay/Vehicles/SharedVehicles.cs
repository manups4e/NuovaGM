using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace TheLastPlanet.Shared.Vehicles
{

    public class OwnedVehicle
    {

        [JsonIgnore]
        Vehicle Vehicle = null;
        public string Plate { get; set; }
        public VehicleData VehData { get; set; }
        public VehGarage Garage { get; set; }
        public string State { get; set; }

        public OwnedVehicle() { }
        public OwnedVehicle(Vehicle veh, string targa, VehicleData data, VehGarage garage, string stato)
        {
            Vehicle = veh;
            Plate = targa;
            VehData = data;
            Garage = garage;
            State = stato;
        }
        public OwnedVehicle(string targa, VehicleData data, VehGarage garage, string stato)
        {
            Plate = targa;
            VehData = data;
            Garage = garage;
            State = stato;
        }
        public OwnedVehicle(string targa, string veh_data, string garage, string stato)
        {
            Plate = targa;
            VehData = veh_data.FromJson<VehicleData>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            Garage = garage.FromJson<VehGarage>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            State = stato;
        }

        public OwnedVehicle(dynamic data)
        {
            Plate = data.targa;
            VehData = (data.vehicle_data as string).FromJson<VehicleData>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            Garage = (data.garage as string).FromJson<VehGarage>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            State = data.stato;
        }
    }


    public class VehGarage
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

    public class VehicleData
    {

        [JsonIgnore]
        public long Insurance;

        [JsonIgnore]
        public VehProp Props = new();
        public bool Stolen { get; set; }
        public VehicleData() { }
        public VehicleData(long insurance, VehProp dati, bool stolen)
        {
            Insurance = insurance;
            Props = dati;
            Stolen = stolen;
        }
    }

    public class VehProp
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
        public Color CustomPrimaryColor { get; set; }
        public Color CustomSecondaryColor { get; set; }
        public bool HasCustomPrimaryColor { get; set; }
        public bool HasCustomSecondaryColor { get; set; }
        public int PearlescentColor { get; set; }
        public int WheelColor { get; set; }
        public int Wheels { get; set; }
        public int WindowTint { get; set; }
        public bool[] NeonEnabled = new bool[4];
        public bool[] Extras = new bool[13];
        public Color NeonColor { get; set; }
        public Color TireSmokeColor { get; set; }
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

    public class VehMod
    {
        public int ModIndex { get; set; }
        public int Value { get; set; }
        public string ModName { get; set; }
        public string ModType { get; set; }

        public VehMod(int modIndex, int value, string name, string type)
        {
            ModIndex = modIndex;
            Value = value;
            ModName = name;
            ModType = type;
        }
    }


    public class RentVehicle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Model { get; set; }
        public RentVehicle() { }
        public RentVehicle(string _name, string _desc, int _price, string _model)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Price = _price;
            this.Model = _model;
        }
        public RentVehicle(string _name, int _price, string _desc, string _model)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Price = _price;
            this.Model = _model;
        }
    }


    public class Train
    {
        public int Entity { get; set; }
        public int Spawnidx { get; set; }

        public Train() { }
        public Train(int _ent, int sp)
        {
            this.Entity = _ent;
            this.Spawnidx = sp;
        }
    }
}
