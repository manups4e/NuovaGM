using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	static class GestionePlayersDecors
	{
		public async static Task GestioneDecors()
		{
			foreach (var player in Client.Instance.GetPlayers)
			{
				if (player.Character.HasDecor("PlayerStanziato"))
				{
					if (player.Character.GetDecor<bool>("PlayerStanziato") && player != Game.Player)
					{
						if (player.Character.HasDecor("PlayerStanziatoInIstanza"))
						{
							if (player.Character.GetDecor<int>("PlayerStanziatoInIstanza") != 0 || Game.PlayerPed.GetDecor<int>("PlayerStanziatoInIstanza") != 0)
							{
								if ((player.Character.GetDecor<int>("PlayerStanziatoInIstanza") != Game.PlayerPed.Handle && Game.PlayerPed.GetDecor<int>("PlayerStanziatoInIstanza") != player.Character.Handle))
								{
									if (!NetworkIsPlayerConcealed(player.Handle))
										NetworkConcealPlayer(player.Handle, true, true);
								}
								else
								{
									if (player.Character.GetDecor<int>("PlayerStanziatoInIstanza") == Game.PlayerPed.Handle || Game.PlayerPed.GetDecor<int>("PlayerStanziatoInIstanza") == player.Character.Handle)
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
				if (player.Character.HasDecor("PlayerInPausa"))
				{
					if (player.Character.GetDecor<bool>("PlayerInPausa") && player != Game.Player)
						if (player.Character.IsInRangeOf(Game.PlayerPed.Position, 30))
							HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
				}
			}
		}
	}
}
