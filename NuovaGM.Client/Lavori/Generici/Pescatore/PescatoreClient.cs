using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using NuovaGM.Shared;
using Newtonsoft.Json;

namespace NuovaGM.Client.Lavori.Generici.Pescatore
{
	static class PescatoreClient
	{
		private static string scenario = "WORLD_HUMAN_STAND_FISHING";
		private static PescatoreDati Città = new PescatoreDati();
		private static PescatoreDati Paleto = new PescatoreDati();
		private static PescatoreDati Sandy = new PescatoreDati(new Vector3(3800.984f, 4441.265f, 4.198f), new Vector3(3857.235f, 4459.287f, 1.84f), new List<Vector3>());

		// oggetti: canna da pesca, esche, pesci, frutti di mare magari, gamberi.. crostacei

		public static void Init()
		{

		}

		public async static Task ControlloPesca()
		{
			
		}
	}

	internal class PescatoreDati
	{
		public Vector3 Contratto;
		public Vector3 AffittoBarca;
		public List<Vector3> PuntiPesca;
		public PescatoreDati() { }
		public PescatoreDati(Vector3 contratto, Vector3 affitto, List<Vector3> punti) 
		{
			Contratto = contratto;
			AffittoBarca = affitto;
			PuntiPesca = punti;
		}
	}
}
