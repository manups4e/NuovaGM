using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Lavori.Generici.Pescatore
{
	class PescatoreJob : LavoroBase
	{
		public override Employment Lavoro { get; set; } = Employment.Pescatore;
		public override string Label { get; set; } = "Pescatore";

		public override void OnJobRemoved()
		{
			throw new NotImplementedException();
		}
		public override void OnJobSet()
		{
			throw new NotImplementedException();
		}

		public override JobProfile[] Profiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override Dictionary<int, string> Roles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); 
	}
}
