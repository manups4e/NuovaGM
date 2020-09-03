﻿using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.Lavori;
using NuovaGM.Client.Proprietà.Hotel;
using NuovaGM.Shared;
using NuovaGM.Shared.Veicoli;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	static class ConfigClient
	{
		public static async Task Init()
		{
			Client.Instance.AddEventHandler("lprp:ConfigurazioneClient", new Action<string, string>(Configurazione));
			BaseScript.TriggerServerEvent("lprp:riavvioApp");
			while (Client.Impostazioni == null) await BaseScript.Delay(0);
			await Task.FromResult(0);
		}

		public static void Configurazione(string JsonMain, string JsonShared)
		{
			Client.Impostazioni = JsonConvert.DeserializeObject<Configurazione>(JsonMain);
			ConfigShared.SharedConfig = JsonConvert.DeserializeObject<SharedConfig>(JsonShared);
		}
	}

	public class Configurazione
	{
		public ConfPrincipale Main = new ConfPrincipale();
		public ConfigVeicoli Veicoli = new ConfigVeicoli();
		public ConfigLavori Lavori = new ConfigLavori();
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
		public Vector4 Firstcoords;
		public int ReviveReward;
		public bool EarlyRespawn;
		public bool EarlyRespawnFine;
		public int EarlyRespawnFineAmount;
		public int EarlySpawnTimer;
		public int BleedoutTimer;
		public bool EscludiMirino;
		public List<uint> ScopedWeapons = new List<uint>();
		public Dictionary<long, float> recoils = new Dictionary<long, float>();
		public List<string> pickupList = new List<string>();

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
		public ConfigMedici Medici = new ConfigMedici();
		public LavoriGenerici Generici = new LavoriGenerici();
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
		public ConfigNegoziGenerici NegoziGenerici = new ConfigNegoziGenerici();
	}

	public class ConfigNegoziGenerici
	{
		public List<Vector3> tfs = new List<Vector3>();
		public List<Vector3> rq = new List<Vector3>();
		public List<Vector3> ltd = new List<Vector3>();
		public List<Vector3> armerie = new List<Vector3>();
		public OggettiDaVendere OggettiDaVendere;
	}
	public class OggettiDaVendere
	{
		public List<OggettoVendita> shared = new List<OggettoVendita>();
		public List<OggettoVendita> tfs = new List<OggettoVendita>();
		public List<OggettoVendita> rq = new List<OggettoVendita>();
		public List<OggettoVendita> ltd = new List<OggettoVendita>();
	}
	public class OggettoVendita
	{
		public string oggetto;
		public int prezzo;
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
		public ConfigAppartamenti Appartamenti = new ConfigAppartamenti();
		public ConfigGarages Garages = new ConfigGarages();
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

	public class ConfigAppartamenti
	{
		public Dictionary<string, ConfigCase> LowEnd = new Dictionary<string, ConfigCase>();
		public Dictionary<string, ConfigCase> MidEnd = new Dictionary<string, ConfigCase>();
		public Dictionary<string, ConfigCase> HighEnd = new Dictionary<string, ConfigCase>();
	}

	public class ConfigGarages
	{
		public ConfigGarage LowEnd = new ConfigGarage();
		public ConfigGarage MidEnd4 = new ConfigGarage();
		public ConfigGarage MidEnd6 = new ConfigGarage();
		public ConfigGarage HighEnd = new ConfigGarage();
		// aggiungere uffici
	}

	public class ConfigGarage
	{
		public Vector3 Pos;
		public int NVehs;
		public Vector4 OutMarker;
		public Vector4 ModifyMarker;
		public Vector3[] ModifyCam = new Vector3[2];
		public Vector4 SpawnInLocation;
		public List<Vector4> PosVehs = new List<Vector4>();
	}

	public class ConfigCase
	{
		public string Label;
		public int VehCapacity;
		public Vector3 MarkerEntrata;
		public Vector3 MarkerUscita;
		public Vector3 SpawnDentro;
		public Vector3 SpawnFuori;
		public ConfigCaseCamExt TelecameraFuori = new ConfigCaseCamExt();
		public List<string> Ipls = new List<string>();
		public string Gateway;
		public bool Is_single;
		public bool Is_room;
		public bool Is_gateway;
		public bool GarageIncluso;
		public Vector3 GarageMarker;
		[JsonIgnore]
		public int Price;
	}
	public class ConfigCaseCamExt
	{
		public Vector3 pos;
		public Vector3 guarda;
	}
}
