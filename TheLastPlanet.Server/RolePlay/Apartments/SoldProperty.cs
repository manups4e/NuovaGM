using System;

namespace TheLastPlanet.Server.Proprietà
{
    public class SoldProperty
    {
        public string DiscordId;
        public string Character;
        public string Type;
        public bool Rent;
        public int Price;
        public string Name;
        public string Label;
        public string Garage;
        public string Wardrobe;
        public string Inventory;
        public string Armory;
        public DateTime Bills;
        public DateTime Purchase;

        public SoldProperty() { }

        public SoldProperty(string discord, string personaggio, string tipo, bool affitto, int prezzo, string nome, string label, string garage, string guardaroba, string inventario, string armeria, DateTime bollette, DateTime acquisto)
        {
            DiscordId = discord;
            Character = personaggio;
            Type = tipo;
            Rent = affitto;
            Price = prezzo;
            Name = nome;
            Label = label;
            Garage = garage;
            Wardrobe = guardaroba;
            Inventory = inventario;
            Armory = armeria;
            Bills = bollette;
            Purchase = acquisto;
        }

        public SoldProperty(dynamic data)
        {
            DiscordId = data.DiscordId;
            Character = data.Personaggio;
            Type = data.Tipo;
            Rent = (bool)data.InAffitto;
            Name = data.Name;
            Label = data.Label;
            Wardrobe = data.Guardaroba;
            Inventory = data.Inventario;
            Armory = data.Armeria;
            Bills = Convert.ToDateTime(data.last_bollette);
            Purchase = Convert.ToDateTime(data.data_acquisto);
        }
    }
}