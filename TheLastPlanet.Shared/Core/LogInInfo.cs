using CitizenFX.Core;
using TheLastPlanet.Shared.Veicoli;
using System;
using System.Collections.Generic;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;

namespace TheLastPlanet.Shared
{
	public class LogInInfo
	{
		[JsonIgnore]
		private ulong CharID { set => ID = value.ToString(); }
		public string ID;
		[JsonIgnore]
		private string info
		{
			set
			{
				var p = value.FromJson<Info>();
				firstName = p.firstname;
				lastName = p.lastname;
				dateOfBirth = p.dateOfBirth;
			}
		}
		public string firstName;
		public string lastName;
		public string dateOfBirth;
		public int Money;
		public int Bank;
	}

	public class SkinAndDress
	{
		[JsonIgnore]
		private string skin { set => Skin = value.FromJson<Skin>(); }
		[JsonIgnore]
		private string dressing { set => Dressing = value.FromJson<Dressing>(); }
		[JsonIgnore]
		private string location { set => Position = value.FromJson<Position>(); }
		
		public Skin Skin;
		public Dressing Dressing;
		public Position Position;
	}
}
