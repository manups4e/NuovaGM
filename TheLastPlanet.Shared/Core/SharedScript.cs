using CitizenFX.Core;
using System.Collections.Generic;
using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared.Internal.Events.Attributes;

// ReSharper disable All

namespace TheLastPlanet.Shared
{
	public enum UserGroup
	{
		User = 0,
		Helper,
		Moderatore,
		Admin,
		Founder,
		Sviluppatore
	}

	public class SharedScript
	{

		public static bool hasWeaponComponent(string weapon, string component)
		{
			foreach (KeyValuePair<string, Arma> weap in ConfigShared.SharedConfig.Main.Generici.Armi)
				if (weap.Key == weapon)
					foreach (Components com in weap.Value.components)
						if(com.name == component)
						return true;
			return false;
		}
		public static bool hasWeaponTint(string weapon, int tint)
		{
			foreach (KeyValuePair<string, Arma> weap in ConfigShared.SharedConfig.Main.Generici.Armi)
				if (weap.Key == weapon)
					foreach (Tinte tin in weap.Value.tints)
						if (tin.value == tint) return true;
			return false;
		}
		public static bool hasComponents(string weapon)
		{
			foreach (KeyValuePair<string, Arma> arma in ConfigShared.SharedConfig.Main.Generici.Armi)
				if (arma.Key == weapon)
					if (arma.Value.components.Count > 0) return true;
			return false;
		}

		public static bool hasTints(string weapon)
		{
			foreach (KeyValuePair<string, Arma> arma in ConfigShared.SharedConfig.Main.Generici.Armi)
				if (arma.Key == weapon)
					if (arma.Value.tints.Count > 0) return true;
			return false;
		}
	}

	[Serialization]
	public partial class OggettoRaccoglibile
	{
		public string type;
		public int id;
		public ObjectHash obj;
		public int propObj;
		public string label;
		public bool inRange = false;
		public Vector3 coords;
		public string name;
		public int amount; 
		public List<Components> componenti = new List<Components>();
		public int tintIndex;


		public OggettoRaccoglibile(int _id, string _name, int _amount, ObjectHash _obj, int _propObj, string _label, Vector3 _coords, string _type = "item", List<Components> _components = null, int _tintIndex = 0)
		{
			id = _id;
			name = _name;
			type = _type;
			amount = _amount;
			obj = _obj;
			propObj = _propObj;
			label = _label;
			coords = _coords;
			componenti = _components;
			tintIndex = _tintIndex;
		}
	}
}
