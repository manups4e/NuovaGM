using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;

namespace NuovaGM.Client.gmPrincipale.Personaggio
{
	public class PlayerChar
	{
		public int source;
		private int playerId;
		public string group;
		public int group_level;
		public int char_current;
		public long playTime;
		public Identifiers identifiers;
		public bool ammanettato;
		public string lastConnection;
		public Status status = new Status();
		public List<Char_data> char_data = new List<Char_data>();
		public dynamic data;
		public PlayerChar() { }
		public PlayerChar(JObject result)
		{
			char_current = result.Value<int>("char_current");
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = 0;
			group = result.Value<string>("group");
			group_level = result.Value<int>("group_level");
			playTime = result.Value<long>("playTime");
			data = JsonConvert.DeserializeObject<JContainer>(result.Value<string>("char_data"));
			status = new Status();
			ammanettato = false;
			playerId = Game.Player.Handle;
			for (int i = 0; i < data.Count; i++)
			{
				char_data.Add(new Char_data(data[i] as JContainer));
			}
		}
		public PlayerChar(dynamic result)
		{
			char_current = result.char_current;
			lastConnection = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
			source = Game.Player.ServerId;
			group = result.group;
			group_level = result.group_level;
			playTime = result.playTime;
			data = JsonConvert.DeserializeObject<JContainer>(result.char_data);
			status = new Status();

			for (int i = 0; i < data.Count; i++)
			{
				char_data.Add(new Char_data(data[i] as JContainer));
			}
		}

		public Char_data CurrentChar
		{
			get
			{
				for (int i = 0; i < char_data.Count; i++)
				{
					if ((char_current - 1) == char_data[i].id - 1)
					{
						return char_data[i];
					}
				}
				return null;
			}
		}

		[JsonIgnore]
		public string FullName { get { return CurrentChar.info.firstname + " " + CurrentChar.info.lastname; } }

		[JsonIgnore]
		public string DOB { get { return CurrentChar.info.dateOfBirth; } }

		[JsonIgnore]
		public bool DeathStatus { get { return CurrentChar.is_dead; } }

		[JsonIgnore]
		public int Money { get { return CurrentChar.finance.cash; } }

		[JsonIgnore]
		public int Bank { get { return CurrentChar.finance.bank; } }

		[JsonIgnore]
		public int DirtyMoney { get { return CurrentChar.finance.dirtyCash; } }

		public Tuple<bool, Inventory> getInventoryItem(string item)
		{
			for (int i = 0; i < CurrentChar.inventory.Count; i++)
			{
				if (CurrentChar.inventory[i].item == item)
				{
					return new Tuple<bool, Inventory>(true, CurrentChar.inventory[i]);
				}
			}
			return new Tuple<bool, Inventory>(false, null);
		}

		public List<Inventory> getCharInventory(int charId)
		{
			for (int i = 0; i < char_data.Count; i++)
			{
				if (char_data[i].id == charId)
				{
					return char_data[i].inventory;
				}
			}
			return null;
		}

		public List<Weapons> getCharWeapons(int charId)
		{
			for (int i = 0; i < char_data.Count; i++)
			{
				if (char_data[i].id == charId)
				{
					return char_data[i].weapons;
				}
			}
			return null;
		}


		public bool hasWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
			{
				if (CurrentChar.weapons[i].name == weaponName)
				{
					return true;
				}
			}
			return false;
		}

		public Tuple<int, Weapons> getWeapon(string weaponName)
		{
			for (int i = 0; i < CurrentChar.weapons.Count; i++)
			{
				if (CurrentChar.weapons[i].name == weaponName)
				{
					return new Tuple<int, Weapons>(i, CurrentChar.weapons[i]);
				}
			}
			return new Tuple<int, Weapons>(0, null);
		}

		public bool hasWeaponTint(string weaponName, int tint)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
			{
				return false;
			}

			if (weapon.tint == tint)
			{
				return true;
			}

			return false;
		}

		public bool hasWeaponComponent(string weaponName, string weaponComponent)
		{
			Weapons weapon = getWeapon(weaponName).Item2;
			if (weapon == null)
			{
				return false;
			}

			for (int i = 0; i < weapon.components.Count; i++)
			{
				if (weapon.components[i].name == weaponComponent)
				{
					return true;
				}
			}
			return false;
		}
	}

	public class Identifiers
	{
		public string steam;
		public string license;
		public string discord;
	}

	public class Status
	{
		public bool connected = true;
		public bool spawned = false;
	}

	public class BarbieriEAccessori
	{
		public static BarbieriTesta Femmina = new BarbieriTesta();
		public static BarbieriTesta Maschio = new BarbieriTesta();

		public static void CaricaCapigliature(string JsonMaschio, string JsonFemmina)
		{
			Maschio = JsonConvert.DeserializeObject<BarbieriTesta>(JsonMaschio);
			Femmina = JsonConvert.DeserializeObject<BarbieriTesta>(JsonFemmina);
		}

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