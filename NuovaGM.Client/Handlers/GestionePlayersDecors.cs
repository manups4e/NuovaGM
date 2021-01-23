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
					if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato)
					{
						if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato && player != Game.Player)
						{
							if (player.GetPlayerData().StatiPlayer.Istanza.Instance != string.Empty)
							{
								if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != 0 || Game.Player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != 0)
								{
									if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != Game.Player.ServerId && Game.Player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != player.ServerId)
									{
										if (!NetworkIsPlayerConcealed(player.Handle))
											NetworkConcealPlayer(player.Handle, true, true);
									}
									else
									{
										if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario == Game.Player.ServerId || Game.Player.ServerId == player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario)
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
					if (player.GetPlayerData().StatiPlayer.InPausa)
					{
						//if (player != Game.Player)
							if (player.Character.IsInRangeOf(Game.Player.GetPlayerData().posizione.ToVector3(), 30))
								HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					}
				}
			}
		}
	}
}
