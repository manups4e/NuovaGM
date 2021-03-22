using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client
{
	internal static class GestionePlayersDecors
	{
		private static int checkTimer1 = 0;
		public static async Task GestioneDecors()
		{
			if (Cache.GiocatoriOnline.Count < 2) return;

			if(Game.GameTime - checkTimer1 > 250)
			{
				foreach (Player player in ClientSession.Instance.GetPlayers)
				{
					if (player == Cache.MyPlayer.Player) return;
					if (player.GetPlayerData() == null) continue;
					if (!player.GetPlayerData().StatiPlayer.Istanza.Stanziato) continue;
					if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato)
					{
						if (player.GetPlayerData().StatiPlayer.Istanza.Instance != string.Empty)
						{
							if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != 0 || Cache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != 0)
							{
								if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != Cache.MyPlayer.Player.ServerId && Cache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != player.ServerId)
								{
									if (!NetworkIsPlayerConcealed(player.Handle)) 
										NetworkConcealPlayer(player.Handle, true, true);
								}
								else
								{
									if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario == Cache.MyPlayer.Player.ServerId || Cache.MyPlayer.Player.ServerId == player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario)
										if (NetworkIsPlayerConcealed(player.Handle))
											NetworkConcealPlayer(player.Handle, false, false);
								}
							}
							else
								if (NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, false, false);
						}
						else
							if (!NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, true, true);
					}
					else
						if (NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, false, false);
					/*
					if (!player.GetPlayerData().StatiPlayer.InPausa) continue;
					if (player.Character.IsInRangeOf(Cache.MyPlayer.User.posizione.ToVector3, 30))
						HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					*/
					checkTimer1 = Game.GameTime;
				}
			}
		}
	}
}