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
		public string info;
		public int Money;
		public int Bank;
	}

	public class SkinAndDress
	{
		[JsonIgnore]
		private string skin { set => Skin = value.FromJson<Skin>(); }
		[JsonIgnore]
		private string dressing { set => Dressing = value.FromJson<Dressing>(); }
		public Skin Skin;
		public Dressing Dressing;
	}
}
