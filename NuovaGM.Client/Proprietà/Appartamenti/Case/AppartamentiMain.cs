using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;

namespace NuovaGM.Client.Proprietà.Appartamenti.Case
{
	static class AppartamentiMain
	{
		public static void Init()
		{

		}

		public static async void EntraMenu(KeyValuePair<string, ConfigCase> app)
		{
			if (Game.PlayerPed.IsVisible)
				NetworkFadeOutEntity(PlayerPedId(), true, false);
			var cam = World.CreateCamera(app.Value.TelecameraFuori.pos, new Vector3(0), 45f);
			cam.PointAt(app.Value.TelecameraFuori.guarda);
			RenderScriptCams(true, true, 1500, true, false);

			UIMenu casa = new UIMenu(app.Value.Label, "Citofono");
			HUD.MenuPool.Add(casa);
			UIMenu Citofona = HUD.MenuPool.AddSubMenu(casa, "Citofona");
			UIMenu entra;
			if (Game.Player.GetPlayerData().CurrentChar.Proprietà.Contains(app.Key))
			{
				entra = HUD.MenuPool.AddSubMenu(casa, "Entra in casa");
			}
			Citofona.OnMenuOpen += async (_menu) =>
			{
				foreach(var p in Client.Instance.GetPlayers.ToList())
				{
					var pl = p.GetPlayerData();
					if (pl.Istanza.Stanziato)
					{
						if (pl.Istanza.IsProprietario)
						{
							if (pl.Istanza.Instance == app.Key)
							{

							}
						}
					}
				}
			};
			casa.Visible = true;
			//await BaseScript.Delay(5000);
			//RenderScriptCams(false, true, 1500, true, false);
			//NetworkFadeInEntity(PlayerPedId(), true);
		}
		public static async void EsciMenu(KeyValuePair<string, ConfigCase> app)
		{

		}
	}
}
