using CitizenFX.Core;
using TheLastPlanet.Shared.Veicoli;
using System;
using System.Collections.Generic;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class LogInInfo
	{
		public string ID { get; set; }
		public string firstName { get; set; }
		public string lastName{ get; set; }
		public string dateOfBirth{ get; set; }
		public int Money{ get; set; }
		public int Bank{ get; set; }
	}

	[Serialization]
	public partial class SkinAndDress
	{
		public Skin Skin { get; set; }
		public Dressing Dressing { get; set; }
		public Position Position { get; set; }
	}
}
