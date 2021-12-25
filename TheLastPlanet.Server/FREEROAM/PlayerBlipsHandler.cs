using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server.FREEROAM
{
	static class PlayerBlipsHandler
	{
		private static List<FRBlipsInfo> _blipsInfos = new();

		private static readonly List<int> HeliHashes = new() { 837858166, 788747387, 745926877, -50547061, 1621617168, 1394036463, 2025593404, 744705981, 1949211328, -1660661558, -82626025, 1044954915, 710198397, -1671539132, -339587598, 1075432268, -1600252419, 1543134283, -1845487887, };
		private static readonly List<int> PlaneHashes = new() { -150975354, -613725916, 368211810, -644710429, -901163259, 970356638, 1058115860, 621481054, -1214293858, -1746576111, 165154707, -1295027632, -1214505995, -2122757008, 1981688531, -1673356438, 1077420264, 1341619767, };
		private static readonly List<int> BoatHashes = new() { 1033245328, 276773164, 509498602, 867467158, 861409633, -1043459709, -1030275036, -616331036, -311022263, 231083307, 437538602, 400514754, 771711535, -1066334226, -282946103, 1070967343, 908897389, 290013743, 1448677353, -2100640717, };
		private static readonly List<int> JetHashes = new() { 970385471, -1281684762, 1824333165 };

		public static void Init()
		{
			Server.Instance.AddTick(UpdatePlayersBlips);
		}

		private static async Task UpdatePlayersBlips()
		{
			try
			{
				await BaseScript.Delay(200); // per ora mezzo secondo.. non so se cambierò in futuro.. vedremo le performances..
				if (BucketsHandler.FreeRoam.GetTotalPlayers() < 1) return; // cambiare con 2... se sono da solo non ha senso (se non per testare)

				var daRim = _blipsInfos.Where(x => !BucketsHandler.FreeRoam.Bucket.Players.Any(y => y.Handle == x.ServerId)).ToList();
				_blipsInfos.RemoveAll(x => daRim.Contains(x));

				foreach (var client in BucketsHandler.FreeRoam.Bucket.Players)
				{
					if (!client.User.Status.Spawned) continue;
					int hand = client.Handle;
					// GESTIRE QUANDO IL PLAYER ESCE DAL SERVER
					var serverId = Convert.ToInt32(client.Player.Handle);
					var pos = client.Ped.Position;
					var rot = client.Ped.Rotation;
					var netId = client.Ped.NetworkId;
					Ped p = client.Ped;
					int sprite = 0;
					if (_blipsInfos.Any(x => x.ServerId == serverId))
					{
						var blip = _blipsInfos.SingleOrDefault(x => x.ServerId == serverId);
						blip.Pos = new(pos, rot);
						var veh = GetVehiclePedIsIn(p.Handle, false);
						if (veh != 0)
						{
							var model = GetEntityModel(veh);
							if (HeliHashes.Contains(model))
							{
								sprite = 422;
							}
							else if (PlaneHashes.Contains(model))
							{
								sprite = 423;
								if (JetHashes.Contains(model))
									sprite = 424;
							}
							else if (BoatHashes.Contains(model))
								sprite = 427;
						}
						else sprite = 1;
						blip.Sprite = sprite;
					}
					else
					{
						FRBlipsInfo user = new(client.Player.Name, new Position(pos, rot), netId, serverId);
						_blipsInfos.Add(user);
					}
					client.User.FreeRoamChar.Posizione = new(pos, rot);
				}
				Server.Instance.Events.Send(BucketsHandler.FreeRoam.Bucket.Players, "freeroam.UpdatePlayerBlipInfos", _blipsInfos);
			}
			catch(Exception e)
			{
				Server.Logger.Error(e.ToString());
			}
		}
	}
}
