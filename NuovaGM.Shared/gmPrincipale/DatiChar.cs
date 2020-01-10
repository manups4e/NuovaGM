using Newtonsoft.Json.Linq;
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
		public List<Inventory> inventory = new List<Inventory>();
		public Dressing dressing = new Dressing();
		public Needs needs = new Needs();
		public Statistiche statistiche = new Statistiche();
		public Char_data() { }
		public Char_data(JContainer data)
		{
			id = data.Value<int>("id");
			is_dead = data.Value<bool>("is_dead");
			info.firstname = data["info"].Value<string>("firstname");
			info.lastname = data["info"].Value<string>("lastname");
			info.dateOfBirth = data["info"].Value<string>("dateOfBirth");
			info.height = data["info"].Value<int>("height");
			info.phoneNumber = data["info"].Value<long>("phoneNumber");
			info.insurance = data["info"].Value<long>("insurance");
			finance.cash = data["finance"].Value<int>("cash");
			finance.bank = data["finance"].Value<int>("bank");
			finance.dirtyCash = data["finance"].Value<int>("dirtyCash");
			job.name = data["job"].Value<string>("name");
			job.grade = data["job"].Value<int>("grade");
			gang.name = data["gang"].Value<string>("name");
			gang.grade = data["gang"].Value<int>("grade");
			needs.fame = data["needs"].Value<float>("fame");
			needs.sete = data["needs"].Value<float>("sete");
			needs.stanchezza = data["needs"].Value<float>("stanchezza");
			needs.malattia = data["needs"].Value<bool>("malattia");
			location.x = data["location"].Value<float>("x");
			location.y = data["location"].Value<float>("y");
			location.z = data["location"].Value<float>("z");
			location.h = data["location"].Value<float>("h");

			dressing.ComponentDrawables.Faccia = data["dressing"]["ComponentDrawables"].Value<int>("Faccia");
			dressing.ComponentDrawables.Maschera = data["dressing"]["ComponentDrawables"].Value<int>("Maschera");
			dressing.ComponentDrawables.Torso = data["dressing"]["ComponentDrawables"].Value<int>("Torso");
			dressing.ComponentDrawables.Pantaloni = data["dressing"]["ComponentDrawables"].Value<int>("Pantaloni");
			dressing.ComponentDrawables.Borsa_Paracadute = data["dressing"]["ComponentDrawables"].Value<int>("Borsa_Paracadute");
			dressing.ComponentDrawables.Scarpe = data["dressing"]["ComponentDrawables"].Value<int>("Scarpe");
			dressing.ComponentDrawables.Accessori = data["dressing"]["ComponentDrawables"].Value<int>("Accessori");
			dressing.ComponentDrawables.Sottomaglia = data["dressing"]["ComponentDrawables"].Value<int>("Sottomaglia");
			dressing.ComponentDrawables.Kevlar = data["dressing"]["ComponentDrawables"].Value<int>("Kevlar");
			dressing.ComponentDrawables.Badge = data["dressing"]["ComponentDrawables"].Value<int>("Badge");
			dressing.ComponentDrawables.Torso_2 = data["dressing"]["ComponentDrawables"].Value<int>("Torso_2");

			dressing.ComponentTextures.Faccia = data["dressing"]["ComponentTextures"].Value<int>("Faccia");
			dressing.ComponentTextures.Maschera = data["dressing"]["ComponentTextures"].Value<int>("Maschera");
			dressing.ComponentTextures.Torso = data["dressing"]["ComponentTextures"].Value<int>("Torso");
			dressing.ComponentTextures.Pantaloni = data["dressing"]["ComponentTextures"].Value<int>("Pantaloni");
			dressing.ComponentTextures.Borsa_Paracadute = data["dressing"]["ComponentTextures"].Value<int>("Borsa_Paracadute");
			dressing.ComponentTextures.Scarpe = data["dressing"]["ComponentTextures"].Value<int>("Scarpe");
			dressing.ComponentTextures.Accessori = data["dressing"]["ComponentTextures"].Value<int>("Accessori");
			dressing.ComponentTextures.Sottomaglia = data["dressing"]["ComponentTextures"].Value<int>("Sottomaglia");
			dressing.ComponentTextures.Kevlar = data["dressing"]["ComponentTextures"].Value<int>("Kevlar");
			dressing.ComponentTextures.Badge = data["dressing"]["ComponentTextures"].Value<int>("Badge");
			dressing.ComponentTextures.Torso_2 = data["dressing"]["ComponentTextures"].Value<int>("Torso_2");

			dressing.PropIndices.Cappelli_Maschere = data["dressing"]["PropIndices"].Value<int>("Cappelli_Maschere");
			dressing.PropIndices.Orecchie = data["dressing"]["PropIndices"].Value<int>("Orecchie");
			dressing.PropIndices.Occhiali_Occhi = data["dressing"]["PropIndices"].Value<int>("Occhiali_Occhi");
			dressing.PropIndices.Unk_3 = data["dressing"]["PropIndices"].Value<int>("Unk_3");
			dressing.PropIndices.Unk_4 = data["dressing"]["PropIndices"].Value<int>("Unk_4");
			dressing.PropIndices.Unk_5 = data["dressing"]["PropIndices"].Value<int>("Unk_5");
			dressing.PropIndices.Orologi = data["dressing"]["PropIndices"].Value<int>("Orologi");
			dressing.PropIndices.Bracciali = data["dressing"]["PropIndices"].Value<int>("Bracciali");
			dressing.PropIndices.Unk_8 = data["dressing"]["PropIndices"].Value<int>("Unk_8");

			dressing.PropTextures.Cappelli_Maschere = data["dressing"]["PropTextures"].Value<int>("Cappelli_Maschere");
			dressing.PropTextures.Orecchie = data["dressing"]["PropTextures"].Value<int>("Orecchie");
			dressing.PropTextures.Occhiali_Occhi = data["dressing"]["PropTextures"].Value<int>("Occhiali_Occhi");
			dressing.PropTextures.Unk_3 = data["dressing"]["PropTextures"].Value<int>("Unk_3");
			dressing.PropTextures.Unk_4 = data["dressing"]["PropTextures"].Value<int>("Unk_4");
			dressing.PropTextures.Unk_5 = data["dressing"]["PropTextures"].Value<int>("Unk_5");
			dressing.PropTextures.Orologi = data["dressing"]["PropTextures"].Value<int>("Orologi");
			dressing.PropTextures.Bracciali = data["dressing"]["PropTextures"].Value<int>("Bracciali");
			dressing.PropTextures.Unk_8 = data["dressing"]["PropTextures"].Value<int>("Unk_8");

			statistiche.STAMINA = data["statistiche"].Value<float>("STAMINA");
			statistiche.STRENGTH = data["statistiche"].Value<float>("STRENGTH");
			statistiche.LUNG_CAPACITY = data["statistiche"].Value<float>("LUNG_CAPACITY");
			statistiche.FLYING_ABILITY = data["statistiche"].Value<float>("FLYING_ABILITY");
			statistiche.SHOOTING_ABILITY = data["statistiche"].Value<float>("SHOOTING_ABILITY");
			statistiche.WHEELIE_ABILITY = data["statistiche"].Value<float>("WHEELIE_ABILITY");
			statistiche.drugs = data["statistiche"].Value<float>("drugs");
			statistiche.fishing = data["statistiche"].Value<float>("fishing");
			if (data["inventory"].HasValues)
			{
				for (int i = 0; i < data["inventory"].Count(); i++)
				{
					inventory.Add(new Inventory(data["inventory"][i].Value<string>("item"), data["inventory"][i].Value<int>("amount")));
				}
			}

			if (data["weapons"].HasValues)
			{
				for (int i = 0; i < data["weapons"].Count(); i++)
				{
					weapons.Add(new Weapons(data["weapons"][i].Value<string>("name"), data["weapons"][i].Value<int>("ammo"), data["weapons"][i]["components"], data["weapons"][i].Value<int>("tint")));
				}
			}

			skin.sex = data["skin"].Value<string>("sex");
			skin.model = data["skin"].Value<string>("model");
			skin.resemblance = data["skin"].Value<float>("resemblance");
			skin.skinmix = data["skin"].Value<float>("skinmix");
			skin.face.mom = data["skin"]["face"].Value<int>("mom");
			skin.face.dad = data["skin"]["face"].Value<int>("dad");
			for (int i = 0; i < 20; i++)
			{
				skin.face.tratti[i] = data["skin"]["face"]["tratti"].Value<float>(i);
			}

			skin.ageing.style = data["skin"]["ageing"].Value<int>("style");
			skin.ageing.opacity = data["skin"]["ageing"].Value<float>("opacity");
			skin.makeup.style = data["skin"]["makeup"].Value<int>("style");
			skin.makeup.opacity = data["skin"]["makeup"].Value<float>("opacity");
			skin.blemishes.style = data["skin"]["blemishes"].Value<int>("style");
			skin.blemishes.opacity = data["skin"]["blemishes"].Value<float>("opacity");
			skin.complexion.style = data["skin"]["complexion"].Value<int>("style");
			skin.complexion.opacity = data["skin"]["complexion"].Value<float>("opacity");
			skin.skinDamage.style = data["skin"]["skinDamage"].Value<int>("style");
			skin.skinDamage.opacity = data["skin"]["skinDamage"].Value<float>("opacity");
			skin.freckles.style = data["skin"]["freckles"].Value<int>("style");
			skin.freckles.opacity = data["skin"]["freckles"].Value<float>("opacity");
			skin.eye.style = data["skin"]["eye"].Value<int>("style");
			skin.ears.style = data["skin"]["ears"].Value<int>("style");
			skin.ears.color = data["skin"]["ears"].Value<int>("color");
			skin.facialHair.beard.style = data["skin"]["facialHair"]["beard"].Value<int>("style");
			skin.facialHair.beard.opacity = data["skin"]["facialHair"]["beard"].Value<float>("opacity");
			skin.facialHair.beard.color[0] = data["skin"]["facialHair"]["beard"]["color"].Value<int>(0);
			skin.facialHair.beard.color[1] = data["skin"]["facialHair"]["beard"]["color"].Value<int>(1);
			skin.facialHair.eyebrow.style = data["skin"]["facialHair"]["eyebrow"].Value<int>("style");
			skin.facialHair.eyebrow.opacity = data["skin"]["facialHair"]["eyebrow"].Value<float>("opacity");
			skin.facialHair.eyebrow.color[0] = data["skin"]["facialHair"]["eyebrow"]["color"].Value<int>(0);
			skin.facialHair.eyebrow.color[1] = data["skin"]["facialHair"]["eyebrow"]["color"].Value<int>(1);
			skin.hair.style = data["skin"]["hair"].Value<int>("style");
			skin.hair.color[0] = data["skin"]["hair"]["color"].Value<int>(0);
			skin.hair.color[1] = data["skin"]["hair"]["color"].Value<int>(1);
			skin.lipstick.style = data["skin"]["lipstick"].Value<int>("style");
			skin.lipstick.opacity = data["skin"]["lipstick"].Value<float>("opacity");
			skin.lipstick.color[0] = data["skin"]["lipstick"]["color"].Value<int>(0);
			skin.lipstick.color[1] = data["skin"]["lipstick"]["color"].Value<int>(1);
			skin.blusher.style = data["skin"]["blusher"].Value<int>("style");
			skin.blusher.opacity = data["skin"]["blusher"].Value<float>("opacity");
			skin.blusher.color[0] = data["skin"]["blusher"]["color"].Value<int>(0);
			skin.blusher.color[1] = data["skin"]["blusher"]["color"].Value<int>(1);
		}

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

	public class Finance
	{
		public int cash { get; set; } = 1000;
		public int bank { get; set; } = 3000;
		public int dirtyCash { get; set; } = 0;

		public Finance() { }

		public Finance(int cash, int bank, int dirtyCash)
		{
			this.cash = cash;
			this.bank = bank;
			this.dirtyCash = dirtyCash;
		}

	}

	public class Location
	{
		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }
		public float h { get; set; }
	}

	public class Inventory
	{
		public string item { get; set; }
		public int amount { get; set; }
		public Inventory() { }
		public Inventory(string _item, int _am)
		{
			this.item = _item;
			this.amount = _am;
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
		public float fame { get; set; } = 0f;
		public float sete { get; set; } = 0f;
		public float stanchezza { get; set; } = 0f;
		public bool malattia { get; set; } = false;
	}

	public class Statistiche
	{
		public float STAMINA { get; set; } = 0f;
		public float STRENGTH { get; set; } = 0f;
		public float LUNG_CAPACITY { get; set; } = 0f;
		public float SHOOTING_ABILITY { get; set; } = 0f;
		public float WHEELIE_ABILITY { get; set; } = 0f;
		public float FLYING_ABILITY { get; set; } = 0f;
		public float drugs { get; set; } = 0f;
		public float fishing { get; set; } = 0f;
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
