﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.MODALITA.MAINLOBBY
{
	internal static class MainChooser
	{
		public static Dictionary<ModalitaServer, int> Bucket_n_Players { get; set; }

		private static BucketMarker RP_Marker = new(new Marker(MarkerType.VerticalCylinder, new Position(-1266.863f, -3013.068f, -49.0f), new Position(10f, 10f, 1f), Colors.RoyalBlue), "", "mp_mission_name_freemode_199");
		private static BucketMarker Mini_Marker = new(new Marker(MarkerType.VerticalCylinder, new Position(-1280.206f, -3021.234f, -49.0f), new Position(10f, 10f, 1f), Colors.ForestGreen), "", "mp_mission_name_freemode_1999");
		private static BucketMarker Gare_Marker = new(new Marker(MarkerType.VerticalCylinder, new Position(-1267.147f, -3032.353f, -49.0f), new Position(10f, 10f, 1f), Colors.MediumPurple), "", "mp_mission_name_freemode_19999");
		private static BucketMarker Nego_Marker = new(new Marker(MarkerType.VerticalCylinder, new Position(-1251.566f, -3032.304f, -49.0f), new Position(10f, 10f, 1f), Colors.Orange), "", "mp_mission_name_freemode_199999");
		private static BucketMarker Roam_Marker = new(new Marker(MarkerType.VerticalCylinder, new Position(-1250.61f, -3007.73f, -49.0f), new Position(10f, 10f, 1f), Colors.Indigo), "", "mp_mission_name_freemode_1999999");

		private static ParticleEffectsAssetNetworked DespawnParticle = new("scr_powerplay");

		public static void Stop()
		{
			Client.Instance.RemoveTick(DrawMarkers);
		}

		private static Position _posRp = Position.Zero;
		private static Position _posMini = Position.Zero;
		private static Position _posGare = Position.Zero;
		private static Position _posNego = Position.Zero;
		private static Position _posRoam = Position.Zero;

		public static async Task DrawMarkers()
		{
			await Cache.PlayerCache.Loaded();

			if (_posRp == Position.Zero)
			{
				_posRp = await RP_Marker.Marker.Position.GetPositionWithGroundZ();
				RP_Marker.Marker.Position = _posRp;
			}

			if (_posMini == Position.Zero)
			{
				_posMini = await Mini_Marker.Marker.Position.GetPositionWithGroundZ();
				Mini_Marker.Marker.Position = _posMini;
			}

			if (_posGare == Position.Zero)
			{
				_posGare = await Gare_Marker.Marker.Position.GetPositionWithGroundZ();
				Gare_Marker.Marker.Position = _posGare;
			}

			if (_posNego == Position.Zero)
			{
				_posNego = await Nego_Marker.Marker.Position.GetPositionWithGroundZ();
				Nego_Marker.Marker.Position = _posNego;
			}

			if (_posRoam == Position.Zero)
			{
				_posRoam = await Roam_Marker.Marker.Position.GetPositionWithGroundZ();
				Roam_Marker.Marker.Position = _posRoam;
			}

			RP_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Immergiti nella simulazione!", "~b~Pianeta RolePlay~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Roleplay]} / 256", "", "", "");
			Mini_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Minigiochi a squadre o singoli!", "~g~Pianeta Minigiochi~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Minigiochi]} / 64", "", "", "");
			Gare_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Gareggia contro tutti!", "~p~Pianeta Gare~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.Gare]} / 64", "", "", "");
			Nego_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Non influisce sul server RolePlay!", "~o~Negozio~w~", "", "", "", "", "", "", "", "");
			Roam_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "PVP in piena libertà!", "~f~Pianeta FreeRoam~w~", "", "", "", "", $"{Bucket_n_Players[ModalitaServer.FreeRoam]} / 256", "", "", "");
			RP_Marker.Draw();
			Mini_Marker.Draw();
			Gare_Marker.Draw();
			Nego_Marker.Draw();
			Roam_Marker.Draw();

			if (RP_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo del ~b~RolePlay~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					if (Bucket_n_Players[ModalitaServer.Roleplay] == 256)
					{
						HUD.ShowNotification("Il Pianeta RolePlay è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}

					await CambiaBucket("~b~Pianeta RolePlay~w~", ModalitaServer.Roleplay);
					await RolePlay.Initializer.Init();
					Stop();
				}
			}

			if (Mini_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo dei ~g~Minigiochi~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					/*
					if (Bucket_n_Players[2] == 64)
					{
						HUD.ShowNotification("Il mondo dei Minigiochi è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}
					*/
					await CambiaBucket("~g~Server Minigiochi~w~", ModalitaServer.Minigiochi);
					Screen.Fading.FadeIn(1000);
				}
			}

			if (Gare_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo delle ~p~Gare~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					/*
					if (Bucket_n_Players[3] == 64)
					{
						HUD.ShowNotification("Il mondo delle Gare è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}
					*/
					await CambiaBucket("~p~Server Gare~w~", ModalitaServer.Gare);
					Races.Creator.RaceCreator.CreatorPreparation();
					await Task.FromResult(0);
				}
			}

			if (Nego_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~o~Negozio~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					await CambiaBucket("~o~Negozio~w~", ModalitaServer.Negozio);
					Screen.Fading.FadeIn(1000);
				}
			}

			if (Roam_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~f~Pianeta FreeRoam~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					if (Bucket_n_Players[ModalitaServer.FreeRoam] == 256)
					{
						HUD.ShowNotification("Il pianeta FreeRoam è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}

					await CambiaBucket("~f~Pianeta FreeRoam~w~", ModalitaServer.FreeRoam);
					Screen.Fading.FadeIn(1000);
					await FreeRoam.Initializer.Init();
					Cache.PlayerCache.MyPlayer.User.FreeRoamChar = await Client.Instance.Events.Get<FreeRoamChar>("lprp:Select_FreeRoamChar", Cache.PlayerCache.MyPlayer.User.ID);
					BaseScript.TriggerServerEvent("worldEventsManage.Server:AddParticipant");
					Stop();
				}
			}
		}

		private static async Task CambiaBucket(string nome, ModalitaServer modalita)
		{
			Screen.Fading.FadeOut(500);
			await BaseScript.Delay(500);
			PopupWarningThread.Warning.ShowWarning(nome, "Ingresso nella sezione in corso...", "Attendi...");
			await BaseScript.Delay(10);
			Screen.Fading.FadeIn(1);
			await BaseScript.Delay(3000);
			bool dentro = await Client.Instance.Events.Get<bool>("lprp:checkSeGiaDentro", modalita);

			if (dentro)
			{
				PopupWarningThread.Warning.UpdateWarning(nome, "Errore nel caricamento...", "Ritorno alla lobby!");
				ServerJoining.PlayerSpawned();
				await BaseScript.Delay(3000);
				PopupWarningThread.Warning.Dispose();

				return;
			}

			string settings = await Client.Instance.Events.Get<string>("Config.CallClientConfig", modalita);
			Client.Impostazioni.LoadConfig(modalita, settings);
			PopupWarningThread.Warning.UpdateWarning(nome, "Caricamento completato!");
			Cache.PlayerCache.MyPlayer.User.StatiPlayer.PlayerStates.Modalita = modalita;
			Bucket_n_Players = await Client.Instance.Events.Get<Dictionary<ModalitaServer, int>>("lprp:richiediContoBuckets");
			await BaseScript.Delay(2000);
			Screen.Fading.FadeOut(1);
			PopupWarningThread.Warning.Dispose();
			Cache.PlayerCache.ModalitàAttuale = modalita;
		}
	}
}