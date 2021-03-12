using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastPlanet.Server.Proprietà
{
	public class SoldProperty
	{
		public string DiscordId;
		public string Personaggio;
		public string Tipo;
		public bool InAffitto;
		[JsonProperty("Prezzo_acquisto_affitto")]
		public int Prezzo;
		public string Name;
		public string Label;
		public string Garage;
		public string Guardaroba;
		public string Inventario;
		public string Armeria;
		[JsonProperty("last_bollette")] public DateTime Bollette;
		[JsonProperty("data_acquisto")] public DateTime Acquisto;

		public SoldProperty() { }

		public SoldProperty(string discord, string personaggio, string tipo, bool affitto, int prezzo, string nome, string label, string garage, string guardaroba, string inventario, string armeria, DateTime bollette, DateTime acquisto)
		{
			DiscordId = discord;
			Personaggio = personaggio;
			Tipo = tipo;
			InAffitto = affitto;
			Prezzo = prezzo;
			Name = nome;
			Label = label;
			Garage = garage;
			Guardaroba = guardaroba;
			Inventario = inventario;
			Armeria = armeria;
			Bollette = bollette;
			Acquisto = acquisto;
		}

		public SoldProperty(dynamic data)
		{
			DiscordId = data.DiscordId;
			Personaggio = data.Personaggio;
			Tipo = data.Tipo;
			InAffitto = (bool)data.InAffitto;
			Name = data.Name;
			Label = data.Label;
			Guardaroba = data.Guardaroba;
			Inventario = data.Inventario;
			Armeria = data.Armeria;
			Bollette = Convert.ToDateTime(data.last_bollette);
			Acquisto = Convert.ToDateTime(data.data_acquisto);
		}
	}
}