using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuovaGM.Shared
{
	public enum DrawableIndexes
	{
		Faccia = 0,
		Maschera = 1,
		Capelli = 2,
		Torso = 3,
		Pantaloni = 4,
		Borsa_Paracadute = 5,
		Scarpe = 6,
		Accessori = 7,
		Sottomaglia = 8,
		Kevlar = 9,
		Badge = 10,
		Torso_2 = 11,
	}

	public enum PropIndexes
	{
		Cappelli_Maschere = 0,
		Occhiali_Occhi = 1,
		Orecchie = 2,
		Unk_3 = 3,
		Unk_4 = 4,
		Unk_5 = 5,
		Orologi = 6,
		Bracciali = 7,
		Unk_8 = 8,
	}

	public class Char_data
	{
		public int id;
		public bool is_dead;
		public Info info = new Info();
		public Finance finance = new Finance();
		public Location location = new Location();
		public Job job = new Job();
		public Gang gang = new Gang();
		public Skin skin = new Skin();
		public List<Weapons> weapons = new List<Weapons>();
		public List<Licenses> licenze = new List<Licenses>();
		public List<Inventory> inventory = new List<Inventory>();
		public Dressing dressing = new Dressing();
		public Needs needs = new Needs();
		public Statistiche statistiche = new Statistiche();
		public Char_data() { }

		public Char_data(int id, Info info, Finance finance, Job job, Gang gang, Skin skin, Dressing dressing, List<Weapons> weapons, List<Inventory> inventory, Needs needs, Statistiche statistiche, bool is_dead)
		{
			this.id = id;
			this.info = info;
			this.finance = finance;
			this.job = job;
			this.gang = gang;
			this.skin = skin;
			this.dressing = dressing;
			this.weapons = weapons;
			this.inventory = inventory;
			this.needs = needs;
			this.statistiche = statistiche;
			this.is_dead = is_dead;
		}
	}

	public class Phone_data
	{
		public int id { get; set; } = 1;
		public int Theme { get; set; } = 1;
		public int Wallpaper { get; set; } = 4;
		public int Ringtone { get; set; } = 0;
		public int SleepMode { get; set; } = 0;
		public int Vibration { get; set; } = 1;
		public List<Message> messaggi = new List<Message>()
		{
			new Message("Francesco Pastrengoni", "Test", "Messaggio di prova", DateTime.Now),
			new Message("Francesco Pastrengoni", "Test", "Messaggio di prova", DateTime.Now)
		};
		public List<Contatto> contatti = new List<Contatto>()
		{
			new Contatto("Aggiungi Contatto", "CHAR_MULTIPLAYER", false, "", 0),
			new Contatto("Polizia", "CHAR_CALL911", false, "Polizia", 0),
			new Contatto("Medico", "CHAR_CALL911", false, "Medico", 0),
			new Contatto("Meccanico", "CHAR_LS_CUSTOMS", false, "Meccanico", 0),
			new Contatto("Taxi", "CHAR_TAXI", false, "Taxi", 0),
			new Contatto("Concessionario", "CHAR_CARSITE2", false, "Concessionario", 0),
			new Contatto("Agente Immobiliare", "CHAR_PEGASUS_DELIVERY", false, "Immobiliare", 0),
			new Contatto("Reporter", "CHAR_LIFEINVADER", false, "Reporter", 0),
		};

		public Phone_data() { }

		public Phone_data(dynamic result)
		{
			Theme = (int)result["Theme"].Value;
			Wallpaper = (int)result["WallPaper"].Value;
			if (result["contatti"].HasValues)
				for (int i = 0; i < result["contatti"].Count; i++)
					contatti.Add(new Contatto(result["contatti"]["Name"].Value, result["contatti"]["Icon"].Value, result["contatti"]["TelephoneNumber"].Value, result["contatti"]["IsPlayer"].Value, result["contatti"]["PlayerIndex"].Value, result["contatti"]["Player"].Value));
			if (result["messaggi"].HasValues)
				for (int i = 0; i < result["messaggi"].Count; i++)
					messaggi.Add(new Message(result["messaggi"]["From"].Value, result["messaggi"]["Title"].Value, result["messaggi"]["Messaggio"].Value, (DateTime)result["messaggi"]["Data"].Value));

		}
	}

	public class Message
	{
		public string From { get; set; }
		public string Title { get; set; }
		public string Messaggio { get; set; }
		public DateTime Data { get; set; }

		public Message(string _from, string _title, string _message, DateTime _data)
		{
			this.From = _from;
			this.Title = _title;
			this.Messaggio = _message;
			this.Data = _data;
		}
	}

	public class Contatto
	{
		public int Player;
		public string Name;
		public string Icon;
		public string TelephoneNumber;
		public bool IsPlayer;
		public int PlayerIndex;


		public Contatto(string name, string icon, bool isPlayer, string telephoneNumber, int playerIndex = 0, int player = 0)
		{
			this.Name = name;
			this.Icon = icon;
			this.IsPlayer = isPlayer;
			this.TelephoneNumber = telephoneNumber;
			this.PlayerIndex = playerIndex;
			this.Player = player;
		}
	}


	public class Info
	{
		public string firstname;
		public string lastname;
		public string dateOfBirth;
		public int height;
		public long phoneNumber;
		public long insurance;

		public Info() { }

		public Info(string firstname, string lastname, string dateOfBirth, int height, long phoneNumber, long insurance)
		{
			this.firstname = firstname;
			this.lastname = lastname;
			this.dateOfBirth = dateOfBirth;
			this.height = height;
			this.phoneNumber = phoneNumber;
			this.insurance = insurance;
		}
	}

	public class Job
	{
		public string name { get; set; } = "Disoccupato";
		public int grade { get; set; } = 0;

		public Job() { }
		public Job(string name, int grade)
		{
			this.name = name;
			this.grade = grade;
		}

	}
	public class Gang
	{
		public string name { get; set; } = "Incensurato";
		public int grade { get; set; } = 0;
		public Gang() { }
		public Gang(string name, int grade)
		{
			this.name = name;
			this.grade = grade;
		}
	}

	public class Licenses
	{
		public string name;
		public string dataDiPossesso;
		public string rilasciataDa = "Admin"; // gestire chi l'ha rilasciata!
		public string scadenza;
		public Licenses() { }
		public Licenses(string name, string possesso, string rilasciatoDa = "Admin")
		{
			this.name = name;
			dataDiPossesso = possesso;
			rilasciataDa = rilasciatoDa;
		}
	}

	public class Finance
	{
		public int money { get; set; } = 1000;
		public int bank { get; set; } = 3000;
		public int dirtyCash { get; set; } = 0;

		public Finance() { }

		public Finance(int cash, int bank, int dirtyCash)
		{
			money = cash;
			this.bank = bank;
			this.dirtyCash = dirtyCash;
		}

	}

	public class Location
	{
		public Vector3 position = new Vector3();
		public float h { get; set; }
	}

	public class Inventory
	{
		public string item { get; set; }
		public int amount { get; set; }
		public float weight { get; set; }
		public Inventory() { }
		public Inventory(string _item, int _am, float _weight)
		{
			this.item = _item;
			this.amount = _am;
			weight = _weight;
		}
	}

	public class Weapons
	{
		public string name { get; set; }
		public int ammo { get; set; }
		public int tint { get; set; }
		public List<Components> components = new List<Components>();
		public Weapons() { }
		public Weapons(string _name, int _ammo, dynamic data, int _tint)
		{
			this.name = _name;
			this.ammo = _ammo;
			this.tint = _tint;
			if (data.Count > 0)
			{
				for (int i = 0; i < data.Count; i++)
				{
					components.Add(new Components(data[i]["name"].Value, (bool)data[i]["active"].Value));
				}
			}
		}

		public Weapons(string _name, int _ammo, List<Components> data, int _tint)
		{
			this.name = _name;
			this.ammo = _ammo;
			this.tint = _tint;
			if (data.Count > 0)
			{
				foreach (Components d in data)
				{
					components.Add(new Components(d.name, d.active));
				}
			}
		}
	}

	public class Dressing
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public ComponentDrawables ComponentDrawables = new ComponentDrawables();
		public ComponentDrawables ComponentTextures = new ComponentDrawables();
		public PropIndices PropIndices = new PropIndices();
		public PropIndices PropTextures = new PropIndices();

		public Dressing() { }
		public Dressing(string name, string description, ComponentDrawables ComponentDrawables, ComponentDrawables ComponentTextures, PropIndices PropIndices, PropIndices PropTextures)
		{
			this.Name = name;
			this.Description = description;
			this.ComponentDrawables = ComponentDrawables;
			this.ComponentTextures = ComponentTextures;
			this.PropIndices = PropIndices;
			this.PropTextures = PropTextures;
		}
	}

	public class ComponentDrawables
	{
		public int Faccia;
		public int Maschera;
		public int Capelli;
		public int Torso;
		public int Pantaloni;
		public int Borsa_Paracadute;
		public int Scarpe;
		public int Accessori;
		public int Sottomaglia;
		public int Kevlar;
		public int Badge;
		public int Torso_2;
		public ComponentDrawables() { }
		public ComponentDrawables(int face, int mask, int hair, int torso, int pantaloni, int parachute_bag, int shoes, int accessory, int undershirt, int kevlar, int badge, int torso_2)
		{
			Faccia = face;
			Maschera = mask;
			Capelli = hair;
			Torso = torso;
			Pantaloni = pantaloni;
			Borsa_Paracadute = parachute_bag;
			Scarpe = shoes;
			Accessori = accessory;
			Sottomaglia = undershirt;
			Kevlar = kevlar;
			Badge = badge;
			Torso_2 = torso_2;
		}
	}

	public class PropIndices
	{
		public int Cappelli_Maschere;
		public int Orecchie;
		public int Occhiali_Occhi;
		public int Unk_3;
		public int Unk_4;
		public int Unk_5;
		public int Orologi;
		public int Bracciali;
		public int Unk_8;

		public PropIndices() { }
		public PropIndices(int cappelli_maschere, int orecchie, int occhiali_occhi, int unk_3, int unk_4, int unk_5, int orologi, int bracciali, int unk_8)
		{
			Cappelli_Maschere = cappelli_maschere;
			Orecchie = orecchie;
			Occhiali_Occhi = occhiali_occhi;
			Unk_3 = unk_3;
			Unk_4 = unk_4;
			Unk_5 = unk_5;
			Orologi = orologi;
			Bracciali = bracciali;
			Unk_8 = unk_8;
		}
	}

	public class Needs
	{
		public float fame { get; set; }
		public float sete { get; set; }
		public float stanchezza { get; set; }
		public bool malattia { get; set; } = false;
	}

	public class Statistiche
	{
		public float STAMINA { get; set; }
		public float STRENGTH { get; set; }
		public float LUNG_CAPACITY { get; set; }
		public float SHOOTING_ABILITY { get; set; }
		public float WHEELIE_ABILITY { get; set; }
		public float FLYING_ABILITY { get; set; }
		public float DRUGS { get; set; }
		public float FISHING { get; set; }
		public float HUNTING { get; set; }
	}

	public class Skin
	{
		public string sex { get; set; } = "Maschio";
		public string model { get; set; } = "mp_m_freemode_01";
		public float resemblance { get; set; } = 0f;
		public float skinmix { get; set; } = 0f;
		public Face face = new Face();
		public A2 ageing = new A2();
		public A2 makeup = new A2();
		public A2 blemishes = new A2();
		public A2 complexion = new A2();
		public A2 skinDamage = new A2();
		public A2 freckles = new A2();
		public A3 lipstick = new A3();
		public A3 blusher = new A3();
		public Facial facialHair = new Facial();
		public Hair hair = new Hair();
		public Eye eye = new Eye();
		public Ears ears = new Ears();

		public Skin() { }
		public Skin(string sex, string model, float resemblance, float skinmix, Face face, A2 ageing, A2 makeup, A2 blemishes, A2 complexion, A2 skinDamage, A2 freckles, A3 lipstick, A3 blusher, Facial facialHair, Hair hair, Eye eye, Ears ears)
		{
			this.sex = sex;
			this.model = model;
			this.resemblance = resemblance;
			this.skinmix = skinmix;
			this.face = face;
			this.ageing = ageing;
			this.makeup = makeup;
			this.blemishes = blemishes;
			this.complexion = complexion;
			this.skinDamage = skinDamage;
			this.freckles = freckles;
			this.lipstick = lipstick;
			this.blusher = blusher;
			this.facialHair = facialHair;
			this.hair = hair;
			this.eye = eye;
			this.ears = ears;
		}

	}

	public class Face
	{
		public int mom { get; set; } = 0;
		public int dad { get; set; } = 0;
		public float[] tratti = new float[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		public Face() { }
		public Face(int mom, int dad, float[] tratti)
		{
			this.mom = mom;
			this.dad = dad;
			this.tratti = tratti;
		}
	}

	public class A2
	{
		public int style { get; set; }
		public float opacity { get; set; }

		public A2() { }
		public A2(int style, float opacity)
		{
			this.style = style;
			this.opacity = opacity;
		}
	}

	public class Facial
	{
		public A3 beard = new A3();
		public A3 eyebrow = new A3();

		public Facial() { }
		public Facial(A3 beard, A3 eyebrow)
		{
			this.beard = beard;
			this.eyebrow = eyebrow;
		}
	}

	public class A3
	{
		public int style { get; set; } = 0;
		public float opacity { get; set; } = 0f;
		public int[] color = new int[2] { 0, 0 };

		public A3() { }
		public A3(int style, float opacity, int[] color)
		{
			this.style = style;
			this.opacity = opacity;
			this.color = color;
		}
	}

	public class Hair
	{
		public int style { get; set; } = 0;
		public int[] color = new int[2] { 0, 0 };
		public Hair() { }
		public Hair(int style, int[] color)
		{
			this.style = style;
			this.color = color;
		}
	}

	public class Eye
	{
		public int style { get; set; }
		public Eye() { }
		public Eye(int style)
		{
			this.style = style;
		}
	}

	public class Ears
	{
		public int style { get; set; }
		public int color { get; set; }
		public Ears() { }
		public Ears(int style, int color)
		{
			this.style = style;
			this.color = color;
		}
	}


	public class Components
	{
		public string name { get; set; }
		public bool active { get; set; }
		public Components() { }
		public Components(string _name)
		{
			name = _name;
		}
		public Components(string _name, bool _ac)
		{
			this.name = _name;
			this.active = _ac;
		}
	}

	public class Tinte
	{
		public string name { get; set; }
		public int value { get; set; }
		public Tinte(string _name, int _value)
		{
			this.name = _name;
			this.value = _value;
		}
	}

	public class Proprieta
	{

	}
}
