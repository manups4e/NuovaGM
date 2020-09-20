using NuovaGM.Shared.Meteo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Shared
{
	public class ConfigShared
	{
		public static SharedConfig SharedConfig = new SharedConfig();
	}

	public class SharedConfig
	{
		public MainShared Main = new MainShared();
	}

	public class MainShared
	{
		public SharedConfigVeicoli Veicoli = new SharedConfigVeicoli();
		public SharedMeteo Meteo = new SharedMeteo();
		public SharedGenerici Generici = new SharedGenerici();
	}
	public class SharedConfigVeicoli
	{
		public List<GasStation> gasstations = new List<GasStation>();
	}

	public class SharedGenerici
	{
		public Dictionary<string, Item> ItemList = new Dictionary<string, Item>();
		public Dictionary<uint, string> DeathReasons = new Dictionary<uint, string>();
		public Dictionary<string, Arma> Armi = new Dictionary<string, Arma>();
	}

	public class Arma
	{
		public string name;
		public List<Components> components = new List<Components>();
		public List<Tinte> tints = new List<Tinte>();
		public Arma() { }
		public Arma(string _name, List<Components> _comp, List<Tinte> _tints)
		{
			name = _name;
			components = _comp;
			tints = _tints;
		}
	}

	public class Item
	{
		public string label;
		public string description;
		public float peso;
		public int sellPrice;
		public int max;
		public ObjectHash prop;
		public Use use = new Use();
		public Give give = new Give();
		public Drop drop = new Drop();
		public Sell sell = new Sell();
		public Buy buy = new Buy();


		public event UsaOggetto Usa;
		public event DaiOggetto Dai;
		public event ButtaOggetto Butta;
		public event VendiOggetto Vendi;
		public event CompraOggetto Compra;

		public void UsaOggettoEvent(int quantity)
		{
			Usa.Invoke(this, quantity);
		}

		public void DaiOggettoEvent(int quantity)
		{
			Dai.Invoke(this, quantity);
		}

		public void ButtaOggettoEvent(int quantity)
		{
			Butta.Invoke(this, quantity);
		}

		public void VendiOggettoEvent(int quantity)
		{
			Vendi.Invoke(this, quantity);
		}

		public void CompraOggettoEvent(int quantity)
		{
			Compra.Invoke(this, quantity);
		}
		public Item() { }
		public Item(string _label, string _desc, float _peso, int _sellpr, int _max, ObjectHash _prop, Use _use, Give _give, Drop _drop, Sell _sell, Buy _buy)
		{
			label = _label;
			description = _desc;
			peso = _peso;
			sellPrice = _sellpr;
			max = _max;
			prop = _prop;
			use = _use;
			give = _give;
			drop = _drop;
			sell = _sell;
			buy = _buy;
		}
	}

	public class Use
	{
		public string label;
		public string description;
		public bool use;

		public Use() { }
		public Use(string _label, string _description, bool _use)
		{
			this.label = _label;
			this.description = _description;
			this.use = _use;
		}
	}

	public class Give
	{
		public string label;
		public string description;
		public bool give;
		public Give() { }
		public Give(string _label, string _description, bool _give)
		{
			this.label = _label;
			this.description = _description;
			this.give = _give;
		}
	}

	public class Drop
	{
		public string label;
		public string description;
		public bool drop;

		public Drop() { }
		public Drop(string _label, string _description, bool _drop)
		{
			this.label = _label;
			this.description = _description;
			this.drop = _drop;
		}
	}

	public class Sell
	{
		public string label;
		public string description;
		public bool sell;

		public Sell() { }
		public Sell(string _label, string _description, bool _sell)
		{
			this.label = _label;
			this.description = _description;
			this.sell = _sell;
		}
	}

	public class Buy
	{
		public string label;
		public string description;
		public bool buy;

		public Buy() { }
		public Buy(string _label, string _description, bool _buy)
		{
			this.label = _label;
			this.description = _description;
			this.buy = _buy;
		}
	}

}