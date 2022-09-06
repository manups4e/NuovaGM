using Impostazioni.Shared.Configurazione.Generici;
using System.Collections.Generic;
using FxEvents.Shared.Attributes;

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
                        if (com.name == component)
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
        public string type { get; set; }
        public int id { get; set; }
        public ObjectHash obj { get; set; }
        public int propObj { get; set; }
        public string label { get; set; }
        public bool inRange { get; set; } = false;
        public Position coords { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public List<Components> componenti { get; set; }
        public int tintIndex { get; set; }


        public OggettoRaccoglibile(int _id, string _name, int _amount, ObjectHash _obj, int _propObj, string _label, Position _coords, string _type = "item", List<Components> _components = null, int _tintIndex = 0)
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
