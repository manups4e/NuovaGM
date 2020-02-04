using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.Proprietà.Hotel;
using NuovaGM.Shared;
using NuovaGM.Shared.Veicoli;
using System;
using System.Collections.Generic;


namespace NuovaGM.Client
{
	static class ConfigClient
	{
		public static Configurazione Conf = null;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:ConfigurazioneClient", new Action<string, string>(Configurazione));
			BaseScript.TriggerServerEvent("lprp:riavvioApp");
		}

		public static void Configurazione(string JsonMain, string JsonShared)
		{
			Conf = JsonConvert.DeserializeObject<Configurazione>(JsonMain);
			ConfigShared.SharedConfig = JsonConvert.DeserializeObject<SharedConfig>(JsonShared);
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigVeicoli Veicoli = new ConfigVeicoli();
		public ConfigPolizia Polizia = new ConfigPolizia();
		public ConfigNegozi Negozi = new ConfigNegozi();
		public ConfigProprieta Proprieta = new ConfigProprieta();

	}

	public class ConfPrincipale
	{
		public string NomeServer;
		public string DiscordAppId;
		public string DiscordRichPresenceAsset;
		public bool PassengerDriveBy;
		public bool KickWarning;
		public int AFKCheckTime;
		public float[] Firstcoords = new float[4];
		public int ReviveReward;
		public bool EarlyRespawn;
		public bool EarlyRespawnFine;
		public int EarlyRespawnFineAmount;
		public int EarlySpawnTimer;
		public int BleedoutTimer;
		public bool EscludiMirino;
		public List<uint> ScopedWeapons = new List<uint>();

		public List<GenericPeds> stripClub = new List<GenericPeds>();
		public List<GenericPeds> blackMarket = new List<GenericPeds>();
		public List<GenericPeds> illegal_weapon_extra_shop = new List<GenericPeds>();
		public float baseTraffic;
		public float divMultiplier;
	}

	public class ConfVeicoli
	{
		public float FuelCapacity;
		public float FuelRpmImpact;
		public float FuelAccelImpact;
		public float FuelTractionImpact;
		public float refuelCost;
		public int maxtankerfuel;
		public List<string> trucks = new List<string>();
		public string tanker;
		public int deformationMultiplier;
		public float deformationExponent;
		public float collisionDamageExponent;
		public float damageFactorEngine;
		public float damageFactorBody;
		public float damageFactorPetrolTank;
		public float engineDamageExponent;
		public float weaponsDamageMultiplier;
		public int degradingHealthSpeedFactor;
		public float cascadingFailureSpeedFactor;
		public float degradingFailureThreshold;
		public float cascadingFailureThreshold;
		public float engineSafeGuard;
		public bool torqueMultiplierEnabled;
		public bool limpMode = false;
		public float limpModeMultiplier;
		public bool preventVehicleFlip;
		public bool sundayDriver;
		public bool displayMessage;
		public bool compatibilityMode;
		public int randomTireBurstInterval;
		public List<float> classDamageMultiplier = new List<float>();
	}

	public class ConfigLavori
	{
		public ConfigPolizia Polizia = new ConfigPolizia();
	}
	public class ConfigVeicoli
	{
		public VeicoliAffitto veicoliAff = new VeicoliAffitto();
		public ConfVeicoli DanniVeicoli = new ConfVeicoli();
	}
	public class ConfigNegozi
	{
		public ConfigNegoziAbiti Abiti = new ConfigNegoziAbiti();
		public ConfigNegoziBarbieri Barbieri = new ConfigNegoziBarbieri();
	}
	public class ConfigNegoziAbiti
	{
		public Abiti Femmina = new Abiti();
		public Abiti Maschio = new Abiti();
	}
	public class ConfigNegoziBarbieri
	{
		public BarbieriTesta Femmina = new BarbieriTesta();
		public BarbieriTesta Maschio = new BarbieriTesta();
	}
	public class ConfigProprieta
	{
		public List<Hotel> hotels = new List<Hotel>();
	}

	public class BarbieriTesta
	{
		public Capelli capelli = new Capelli();
		public List<Capigliature> trucco = new List<Capigliature>();
		public List<Capigliature> sopr = new List<Capigliature>();
		public List<Capigliature> barba = new List<Capigliature>();
		public List<Capigliature> ross = new List<Capigliature>();
	}

	public class Capelli
	{
		public List<Capigliature> kuts = new List<Capigliature>();
		public List<Capigliature> osheas = new List<Capigliature>();
		public List<Capigliature> hawick = new List<Capigliature>();
		public List<Capigliature> beach = new List<Capigliature>();
		public List<Capigliature> mulet = new List<Capigliature>();
	}

	public class Abiti
	{
		public List<Completo> BincoVest = new List<Completo>();
		public List<Completo> DiscVest = new List<Completo>();
		public List<Completo> SubVest = new List<Completo>();
		public List<Completo> PonsVest = new List<Completo>();
		public List<Singolo> BincoScarpe = new List<Singolo>();
		public List<Singolo> DiscScarpe = new List<Singolo>();
		public List<Singolo> SubScarpe = new List<Singolo>();
		public List<Singolo> PonsScarpe = new List<Singolo>();
		public List<Singolo> BincoPant = new List<Singolo>();
		public List<Singolo> DiscPant = new List<Singolo>();
		public List<Singolo> SubPant = new List<Singolo>();
		public List<Singolo> PonsPant = new List<Singolo>();
		public List<Singolo> Occhiali = new List<Singolo>();
		public Accessori Accessori = new Accessori();
	}

	public class Completo
	{
		public string Name;
		public string Description;
		public int Price;
		public ComponentDrawables ComponentDrawables = new ComponentDrawables();
		public ComponentDrawables ComponentTextures = new ComponentDrawables();
		public PropIndices PropIndices = new PropIndices();
		public PropIndices PropTextures = new PropIndices();

		public Completo(string name, string desc, int price, ComponentDrawables componentDrawables, ComponentDrawables componentTextures, PropIndices propIndices, PropIndices propTextures)
		{
			Name = name;
			Description = desc;
			Price = price;
			ComponentDrawables = componentDrawables;
			ComponentTextures = componentTextures;
			PropIndices = propIndices;
			PropTextures = propTextures;
		}
	}

	public class Singolo
	{
		public string Title;
		public string Description;
		public int Modello;
		public int Price;
		public List<int> Text = new List<int>();
	}

	public class Accessori
	{
		public List<Singolo> Borse = new List<Singolo>();
		public Testa Testa = new Testa();
		public List<Singolo> Orologi = new List<Singolo>();
		public List<Singolo> Bracciali = new List<Singolo>();
	}

	public class Testa
	{
		public List<Singolo> Orecchini = new List<Singolo>();
		public List<Singolo> Auricolari = new List<Singolo>();
		public List<Singolo> Cappellini = new List<Singolo>();
	}

	public class Capigliature
	{
		public string Name;
		public string Description;
		public int var;
		public int price;
	}

}
