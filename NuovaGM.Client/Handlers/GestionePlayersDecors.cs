using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	static class GestionePlayersDecors
	{
		public async static Task GestioneDecors()
		{
			foreach (var player in Client.Instance.GetPlayers)
			{
				if (player.GetPlayerData() != null)
				{
					if (player.State["Istanza"].Stanziato)
					{
						if (player.GetPlayerData().Istanza.Stanziato && player != Game.Player)
						{
							if (player.GetPlayerData().Istanza.Instance != string.Empty)
							{
								if (player.GetPlayerData().Istanza.ServerIdProprietario != 0 || Game.Player.GetPlayerData().Istanza.ServerIdProprietario != 0)
								{
									if (player.GetPlayerData().Istanza.ServerIdProprietario != Game.Player.ServerId && Game.Player.GetPlayerData().Istanza.ServerIdProprietario != player.ServerId)
									{
										if (!NetworkIsPlayerConcealed(player.Handle))
											NetworkConcealPlayer(player.Handle, true, true);
									}
									else
									{
										if (player.GetPlayerData().Istanza.ServerIdProprietario == Game.Player.ServerId || Game.Player.ServerId == player.GetPlayerData().Istanza.ServerIdProprietario)
										{
											if (NetworkIsPlayerConcealed(player.Handle))
												NetworkConcealPlayer(player.Handle, false, false);
										}
									}
								}
								else
								{
									if (NetworkIsPlayerConcealed(player.Handle))
										NetworkConcealPlayer(player.Handle, false, false);
								}
							}
							else
							{
								if (!NetworkIsPlayerConcealed(player.Handle))
									NetworkConcealPlayer(player.Handle, true, true);
							}
						}
						else
						{
							if (NetworkIsPlayerConcealed(player.Handle))
								NetworkConcealPlayer(player.Handle, false, false);
						}
					}
					if (player.State["Pausa"].Attivo)
					{
						if (player != Game.Player)
							if (player.Character.IsInRangeOf(Game.Player.GetPlayerData().posizione.ToVector3(), 30))
								HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					}
				}
			}
		}
	}
}
