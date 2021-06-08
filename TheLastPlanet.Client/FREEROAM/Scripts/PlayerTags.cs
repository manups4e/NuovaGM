using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.FreeRoam.Scripts
{
	static class PlayerTags
	{
		public static Dictionary<int, GamerTag> GamerTags = new Dictionary<int, GamerTag>();
		public static void Init()
		{
		}

		public static async Task GamerTagsHandler()
		{
			foreach (var player in Client.Instance.GetPlayers)
			{
				if (player.Character.LodDistance != 0xFFFF)
					player.Character.LodDistance = 0xFFFF;
				if (!player.Character.IsInVehicle())
				{
					if (!GamerTags.ContainsKey(player.Handle) || !IsMpGamerTagActive(GamerTags[player.Handle].Tag))
					{
						if (GamerTags.ContainsKey(player.Handle))
							RemoveMpGamerTag(GamerTags[player.Handle].Tag);
						GamerTags[player.Handle] = new GamerTag()
						{
							Tag = CreateMpGamerTag(player.Character.Handle, "", false, false, "", 0),
							Ped = player.Character
						};
					}

					int tag = GamerTags[player.Handle].Tag;
					SetMpGamerTagName(tag, player.Name);
					//SetMpGamerTagColour

					// AGGIUNGERE OPZIONE SE IL PLAYER VUOLE DISATTIVARE LA GAMERTAG
					SetMpGamerTagVisibility(tag, 0, true);
					SetMpGamerTagVisibility(tag, 4, NetworkIsPlayerTalking(player.Handle));
				}
			}
		}
	}

	public class GamerTag
	{
		public int Tag;
		public Ped Ped;
	}
}
