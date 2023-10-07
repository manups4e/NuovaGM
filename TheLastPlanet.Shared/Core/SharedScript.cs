using Settings.Shared.Config.Generic;
using System.Collections.Generic;

// ReSharper disable All

namespace TheLastPlanet.Shared
{
    public enum UserGroup
    {
        User = 0,
        Helper,
        Moderator,
        Admin,
        Founder,
        Developer
    }

    public class SharedScript
    {

        public static bool hasWeaponComponent(string weapon, string component)
        {

/* Modifica senza merge dal progetto 'TheLastPlanet.Server'
Prima:
            foreach (KeyValuePair<string, Arma> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
Dopo:
            foreach (KeyValuePair<string, Weapon> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
*/
            foreach (KeyValuePair<string, Settings.Shared.Config.Generic.Weapon> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
                if (weap.Key == weapon)
                    foreach (Components com in weap.Value.components)
                        if (com.name == component)
                            return true;
            return false;
        }
        public static bool hasWeaponTint(string weapon, int tint)
        {

/* Modifica senza merge dal progetto 'TheLastPlanet.Server'
Prima:
            foreach (KeyValuePair<string, Arma> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
Dopo:
            foreach (KeyValuePair<string, Weapon> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
*/
            foreach (KeyValuePair<string, Settings.Shared.Config.Generic.Weapon> weap in ConfigShared.SharedConfig.Main.Generics.Weapons)
                if (weap.Key == weapon)
                    foreach (Tints tin in weap.Value.tints)
                        if (tin.value == tint) return true;
            return false;
        }
        public static bool hasComponents(string weapon)
        {

/* Modifica senza merge dal progetto 'TheLastPlanet.Server'
Prima:
            foreach (KeyValuePair<string, Arma> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
Dopo:
            foreach (KeyValuePair<string, Weapon> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
*/
            foreach (KeyValuePair<string, Settings.Shared.Config.Generic.Weapon> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
                if (arma.Key == weapon)
                    if (arma.Value.components.Count > 0) return true;
            return false;
        }

        public static bool hasTints(string weapon)
        {

/* Modifica senza merge dal progetto 'TheLastPlanet.Server'
Prima:
            foreach (KeyValuePair<string, Arma> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
Dopo:
            foreach (KeyValuePair<string, Weapon> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
*/
            foreach (KeyValuePair<string, Settings.Shared.Config.Generic.Weapon> arma in ConfigShared.SharedConfig.Main.Generics.Weapons)
                if (arma.Key == weapon)
                    if (arma.Value.tints.Count > 0) return true;
            return false;
        }
    }


    public class PickupObject
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public ObjectHash ObjectHash { get; set; }
        public int PropObj { get; set; }
        public string Label { get; set; }
        public bool InRange { get; set; } = false;
        public Position Coords { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public List<Components> Components { get; set; }
        public int TintIndex { get; set; }


        public PickupObject(int _id, string _name, int _amount, ObjectHash _obj, int _propObj, string _label, Position _coords, string _type = "item", List<Components> _components = null, int _tintIndex = 0)
        {
            Id = _id;
            Name = _name;
            Type = _type;
            Amount = _amount;
            ObjectHash = _obj;
            PropObj = _propObj;
            Label = _label;
            Coords = _coords;
            Components = _components;
            TintIndex = _tintIndex;
        }
    }
}
