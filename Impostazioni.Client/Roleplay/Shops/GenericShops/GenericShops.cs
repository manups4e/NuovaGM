using System.Collections.Generic;

namespace Settings.Client.Configuration.Negozi.Generici
{
    public class ConfigGenericShops
    {
        public List<Vector3> Tfs { get; set; }
        public List<Vector3> Rq { get; set; }
        public List<Vector3> Ltd { get; set; }
        public List<Vector3> WeaponShops { get; set; }
        public ItemsToSell ItemsToSell { get; set; }
    }

    public class ItemsToSell
    {
        public List<ItemSell> Shared { get; set; }
        public List<ItemSell> Tfs { get; set; }
        public List<ItemSell> Rq { get; set; }
        public List<ItemSell> Ltd { get; set; }
    }

    public class ItemSell
    {
        public string Object { get; set; }
        public int Price { get; set; }
    }

}