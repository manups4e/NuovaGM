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
			Ped playerPed = Game.PlayerPed;
			foreach (var player in Client.Instance.GetPlayers)
			{
				if (player.GetPlayerData() != null)
				{
					if (player.Character.HasDecor("PlayerStanziato"))
					{
						if (player.Character.GetDecor<bool>("PlayerStanziato") || player.GetPlayerData().Istanza.Stanziato && player != Game.Player)
						{
							if (player.Character.HasDecor("PlayerStanziatoInIstanza") || player.GetPlayerData().Istanza.Instance != null)
							{
								if (player.Character.GetDecor<int>("PlayerStanziatoInIstanza") != 0 || player.GetPlayerData().Istanza.ServerId != 0 || playerPed.GetDecor<int>("PlayerStanziatoInIstanza") != 0 || Game.Player.GetPlayerData().Istanza.ServerId != 0)
								{
									if (player.Character.GetDecor<int>("PlayerStanziatoInIstanza") != Game.Player.ServerId && playerPed.GetDecor<int>("PlayerStanziatoInIstanza") != player.ServerId || player.GetPlayerData().Istanza.ServerId != Game.Player.ServerId && Game.Player.GetPlayerData().Istanza.ServerId != player.ServerId)
									{
										if (!NetworkIsPlayerConcealed(player.Handle))
											NetworkConcealPlayer(player.Handle, true, true);
									}
									else
									{
										if (player.Character.GetDecor<int>("PlayerStanziatoInIstanza") == Game.Player.ServerId || playerPed.GetDecor<int>("PlayerStanziatoInIstanza") == player.ServerId || player.GetPlayerData().Istanza.ServerId == Game.Player.ServerId || Game.Player.ServerId == player.GetPlayerData().Istanza.ServerId)
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
				}
				if (player.Character.HasDecor("PlayerInPausa"))
				{
					if (player.Character.GetDecor<bool>("PlayerInPausa") && player != Game.Player)
						if (player.Character.IsInRangeOf(Game.Player.GetPlayerData().posizione.ToVector3(), 30))
							HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
				}
			}
		}
	}
}
