using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Shared.Veicoli;
using TheLastPlanet.Client.Veicoli;

namespace TheLastPlanet.Client.Core.Utility
{
	internal enum PedTypes
	{
		Player0,  // michael  
		Player1,  // franklin  
		MpPlayer, // mp character  
		Player2,  // trevor  
		CivMale,
		CivFemale,
		Cop,
		GangAlbanian,
		GangBiker,
		GangBiker2,
		GangItalian,
		GangRussian,
		GangRussian2,
		GangIrish,
		GangJamaican,
		GangAfricanAmerican,
		GangKorean,
		GangChineseJapanese,
		GangPuertorican,
		Dealer,
		Medic,
		Fireman,
		Criminal,
		Bum,
		Prostitute,
		Special,
		Mission,
		Swat,
		Animal,
		Army
	};

	internal static class Funzioni
	{
		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKvpString(string key, string value) { SetResourceKvp(key, value); }

		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		private static void SalvaKvp(string key, object value) { SetResourceKvp(key, JsonConvert.SerializeObject(value)); }

		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKvpInt(string key, int value) { SetResourceKvpInt(key, value); }

		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKvpFloat(string key, float value) { SetResourceKvpFloat(key, value); }

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static int CaricaKvpInt(string key) { return GetResourceKvpInt(key); }

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static float CaricaKvpFloat(string key) { return GetResourceKvpFloat(key); }

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static string CaricaKvpString(string key) { return GetResourceKvpString(key); }

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static T CaricaKvp<T>(string key) { return JsonConvert.DeserializeObject<T>(GetResourceKvpString(key)); }

		public static User GetPlayerCharFromPlayerId(int id)
		{
			foreach (KeyValuePair<string, User> p in CachePlayer.Cache.GiocatoriOnline)
				if (GetPlayerFromServerId(Convert.ToInt32(p.Key)) == id)
					return p.Value;

			return null;
		}

		public static User GetPlayerCharFromServerId(int id)
		{
			foreach (KeyValuePair<string, User> p in CachePlayer.Cache.GiocatoriOnline)
				if (Convert.ToInt32(p.Key) == id)
					return p.Value;

			return null;
		}

		public static User GetPlayerData(this Player player)
		{
			return player == CachePlayer.Cache.MyPlayer.Player ? CachePlayer.Cache.MyPlayer.User : GetPlayerCharFromServerId(player.ServerId);
		}

		public static void SendNuiMessage(object message)
		{
			API.SendNuiMessage(message.SerializeToJson());
		}

		public static void ConcealPlayersNearby(Vector3 coord, float radius)
		{
			List<Player> players = GetPlayersInArea(coord, radius);
			foreach (Player pl in players)
				if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != CachePlayer.Cache.MyPlayer.Player.Handle)
					NetworkConcealPlayer(pl.Handle, true, true);
		}

		public static void ConcealAllPlayers()
		{
			Client.Instance.GetPlayers.ToList().ForEach(pl =>
			{
				if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != CachePlayer.Cache.MyPlayer.Player.Handle) NetworkConcealPlayer(pl.Handle, true, true);
			});
		}

		public static void RevealPlayersNearby(Vector3 coord, float radius)
		{
			List<Player> players = GetPlayersInArea(coord, radius);
			foreach (Player pl in players)
				if (NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != CachePlayer.Cache.MyPlayer.Player.Handle)
					NetworkConcealPlayer(pl.Handle, false, false);
		}

		public static void RevealAllPlayers()
		{
			Client.Instance.GetPlayers.ToList().ForEach(pl =>
			{
				if (NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != CachePlayer.Cache.MyPlayer.Player.Handle) NetworkConcealPlayer(pl.Handle, false, false);
			});
		}

		public static async Task UpdateFace(Skin skin)
		{
			var id = CachePlayer.Cache.MyPlayer.Ped.Handle;
			SetPedHeadBlendData(id, skin.face.mom, skin.face.dad, 0, skin.face.mom, skin.face.dad, 0, skin.resemblance, skin.skinmix, 0f, false);
			SetPedHeadOverlay(id, 0, skin.blemishes.style, skin.blemishes.opacity);
			SetPedHeadOverlay(id, 1, skin.facialHair.beard.style, skin.facialHair.beard.opacity);
			SetPedHeadOverlayColor(id, 1, 1, skin.facialHair.beard.color[0], skin.facialHair.beard.color[1]);
			SetPedHeadOverlay(id, 2, skin.facialHair.eyebrow.style, skin.facialHair.eyebrow.opacity);
			SetPedHeadOverlayColor(id, 2, 1, skin.facialHair.eyebrow.color[0], skin.facialHair.eyebrow.color[1]);
			SetPedHeadOverlay(id, 3, skin.ageing.style, skin.ageing.opacity);
			SetPedHeadOverlay(id, 4, skin.makeup.style, skin.makeup.opacity);
			SetPedHeadOverlay(id, 5, skin.blusher.style, skin.blusher.opacity);
			SetPedHeadOverlay(id, 6, skin.complexion.style, skin.complexion.opacity);
			SetPedHeadOverlay(id, 7, skin.skinDamage.style, skin.skinDamage.opacity);
			SetPedHeadOverlay(id, 8, skin.lipstick.style, skin.lipstick.opacity);
			SetPedHeadOverlayColor(id, 8, 1, skin.lipstick.color[0], skin.lipstick.color[1]);
			SetPedHeadOverlay(id, 9, skin.freckles.style, skin.freckles.opacity);
			SetPedEyeColor(id, skin.eye.style);
			SetPedComponentVariation(id, 2, skin.hair.style, 0, 0);
			SetPedHairColor(id, skin.hair.color[0], skin.hair.color[1]);
			SetPedPropIndex(id, 2, skin.ears.style, skin.ears.color, true);
			for (var i = 0; i < skin.face.tratti.Length; i++) SetPedFaceFeature(id, i, skin.face.tratti[i]);
			await Task.FromResult(0);
		}

		public static async Task UpdateDress(Dressing dress)
		{
			var id = CachePlayer.Cache.MyPlayer.Ped.Handle;
			SetPedComponentVariation(id, (int)DrawableIndexes.Faccia, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Maschera, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Pantaloni, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Borsa_Paracadute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Scarpe, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Accessori, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Sottomaglia, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);
			if (dress.PropIndices.Cappelli_Maschere == -1) ClearPedProp(id, 0);
			SetPedPropIndex(id, (int)PropIndexes.Cappelli_Maschere, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, false);
			if (dress.PropIndices.Orecchie == -1) ClearPedProp(id, 2);
			SetPedPropIndex(id, (int)PropIndexes.Orecchie, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, false);
			if (dress.PropIndices.Occhiali_Occhi == -1) ClearPedProp(id, 1);
			SetPedPropIndex(id, (int)PropIndexes.Occhiali_Occhi, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
			if (dress.PropIndices.Unk_3 == -1) ClearPedProp(id, 3);
			SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
			if (dress.PropIndices.Unk_4 == -1) ClearPedProp(id, 4);
			SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
			if (dress.PropIndices.Unk_5 == -1) ClearPedProp(id, 5);
			SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
			if (dress.PropIndices.Orologi == -1) ClearPedProp(id, 6);
			SetPedPropIndex(id, (int)PropIndexes.Orologi, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
			if (dress.PropIndices.Bracciali == -1) ClearPedProp(id, 7);
			SetPedPropIndex(id, (int)PropIndexes.Bracciali, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
			if (dress.PropIndices.Unk_8 == -1) ClearPedProp(id, 8);
			SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.PropIndices.Unk_8, dress.PropTextures.Unk_8, true);
			await Task.FromResult(0);
		}

		/// <summary>
		/// Prende il primo piano di un ped
		/// </summary>
		/// <param name="ped"></param>
		/// <param name="transparent"></param>
		/// <returns></returns>
		public static async Task<Tuple<int, string>> GetPedMugshotAsync(Ped ped, bool transparent = false)
		{
			var mugshot = RegisterPedheadshot(ped.Handle);
			if (transparent) mugshot = RegisterPedheadshotTransparent(ped.Handle);
			while (!IsPedheadshotReady(mugshot)) await BaseScript.Delay(1);
			var txd = GetPedheadshotTxdString(mugshot);

			return new Tuple<int, string>(mugshot, txd);
		}

		public static string GetRandomString(int size, bool lowerCase = false)
		{
			var builder = new StringBuilder(size);
			var random = new Random(Game.GameTime);
			// Unicode/ASCII Letters are divided into two blocks
			// (Letters 65–90 / 97–122):
			// The first group containing the uppercase letters and
			// the second group containing the lowercase.  

			// char is a single Unicode character  
			char offset = lowerCase ? 'a' : 'A';  
			const int lettersOffset = 26; // A...Z or a..z: length=26  
  
			for (var i = 0; i < size; i++)  
			{  
				var @char = (char)random.Next(offset, offset + lettersOffset);  
				builder.Append(@char);  
			}  
  
			return lowerCase ? builder.ToString().ToLower() : builder.ToString();  
		}

		public static bool IsAnyControlJustPressed() { return Enum.GetValues(typeof(Control)).Cast<Control>().Any(value => Input.IsControlJustPressed(value)); }

		public static bool IsAnyControlPressed() { return Enum.GetValues(typeof(Control)).Cast<Control>().Any(value => Input.IsControlPressed(value)); }

		public static async Task<VehProp> GetVehicleProperties(this Vehicle veh)
		{
			bool[] extras = new bool[13];
			for (int i = 0; i < 13; i++) extras[i] = veh.IsExtraOn(i);
			List<VehMod> mods = veh.Mods.GetAllMods().Select(mod => new VehMod((int)mod.ModType, mod.Index, mod.LocalizedModName, mod.LocalizedModTypeName)).ToList();
			VehProp vehi = new VehProp(veh.Model, veh.LocalizedName, veh.Mods.LicensePlate, (int)veh.Mods.LicensePlateStyle, veh.BodyHealth, veh.EngineHealth, veh.FuelLevel, veh.DirtLevel, (int)veh.Mods.PrimaryColor, (int)veh.Mods.SecondaryColor, veh.Mods.CustomPrimaryColor, veh.Mods.CustomSecondaryColor, veh.Mods.IsPrimaryColorCustom, veh.Mods.IsSecondaryColorCustom, (int)veh.Mods.PearlescentColor, (int)veh.Mods.RimColor, (int)veh.Mods.WheelType, (int)veh.Mods.WindowTint, new bool[4] { veh.Mods.HasNeonLight(VehicleNeonLight.Left), veh.Mods.HasNeonLight(VehicleNeonLight.Right), veh.Mods.HasNeonLight(VehicleNeonLight.Front), veh.Mods.HasNeonLight(VehicleNeonLight.Back) }, extras, veh.Mods.NeonLightsColor, veh.Mods.TireSmokeColor, !(GetVehicleModKit(veh.Handle) == 65535), mods, veh.Mods.Livery);
			await Task.FromResult(0);

			return vehi;
		}

		public static async Task SetVehicleProperties(this Vehicle veh, VehProp props)
		{
			veh.Mods.LicensePlate = props.Plate;
			if (props.ModKitInstalled) veh.Mods.InstallModKit();
			veh.Mods.LicensePlateStyle = (LicensePlateStyle)props.PlateIndex;
			veh.BodyHealth = props.BodyHealth;
			veh.EngineHealth = props.EngineHealth;
			veh.SetVehicleFuelLevel(props.FuelLevel);
			veh.DirtLevel = props.DirtLevel;
			veh.Mods.PrimaryColor = (VehicleColor)props.PrimaryColor;
			veh.Mods.SecondaryColor = (VehicleColor)props.SecondaryColor;
			veh.Mods.CustomPrimaryColor = props.CustomPrimaryColor;
			veh.Mods.CustomSecondaryColor = props.CustomSecondaryColor;
			//veh.Mods.IsPrimaryColorCustom = props.HasCustomPrimaryColor;
			//veh.Mods.IsSecondaryColorCustom = props.HasCustomSecondaryColor;
			veh.Mods.PearlescentColor = (VehicleColor)props.PearlescentColor;
			veh.Mods.RimColor = (VehicleColor)props.WheelColor;
			veh.Mods.WheelType = (VehicleWheelType)props.Wheels;
			veh.Mods.WindowTint = (VehicleWindowTint)props.WindowTint;
			for (int i = 0; i < props.NeonEnabled.Length; i++) veh.Mods.SetNeonLightsOn((VehicleNeonLight)i, props.NeonEnabled[i]);
			for (int i = 0; i < 13; i++) veh.ToggleExtra(i, props.Extras[i]);
			veh.Mods.NeonLightsColor = props.NeonColor;
			veh.Mods.TireSmokeColor = props.TireSmokeColor;
			VehicleMod[] mods = veh.Mods.GetAllMods();
			foreach (VehMod mod in props.Mods) SetVehicleMod(veh.Handle, mod.ModIndex, mod.Value, mods.ToList().FirstOrDefault(x => (int)x.ModType == mod.ModIndex).Variation);
			veh.Mods.Livery = props.ModLivery;
		}

		// Apparently there's a built-in native for this...
		// TODO: Replace with that
		public static string GetEntityType(int entityHandle)
		{
			try
			{
				if (IsEntityAPed(entityHandle)) return "PED";
				if (IsEntityAVehicle(entityHandle)) return "VEH";
				if (IsEntityAnObject(entityHandle)) return "OBJ";
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GetEntityType Error: {ex.Message}");
			}

			return "UNK";
		}

		[Obsolete("Obsoleto.. usare la versione in estensione al vettore stesso.. Vector().FindGroundZ()")]
		public static async Task<float> FindGroundZ(Vector2 position)
		{
			float result = -199f;

			try
			{
				float[] groundCheckHeight = new float[] { -100.0f, -50.0f, 0.0f, 50.0f, 100.0f, 150.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f, 450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f };

				foreach (float h in groundCheckHeight)
				{
					await BaseScript.Delay(1);
					float z = 0;
					if (GetGroundZFor_3dCoord(position.X, position.Y, h, ref z, false)) result = z;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe FindGroundZ Error: {ex.Message}");
			}

			await Task.FromResult(0);

			return result;
		}

		public static async void Teleport(Vector3 coords)
		{
			Ped playerPed = CachePlayer.Cache.MyPlayer.Ped;
			ClearPedTasksImmediately(playerPed.Handle);
			playerPed.IsPositionFrozen = true;
			if (playerPed.IsVisible) NetworkFadeOutEntity(playerPed.Handle, true, false);
			DoScreenFadeOut(500);
			while (!IsScreenFadedOut()) await BaseScript.Delay(0);
			RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);
			NewLoadSceneStart(coords.X, coords.Y, coords.Z, coords.X, coords.Y, coords.Z, 50f, 0);
			int tempTimer = GetGameTimer();

			// Wait for the new scene to be loaded.
			while (IsNetworkLoadingScene())
			{
				// If this takes longer than 1 second, just abort. It's not worth waiting that long.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			SetEntityCoords(playerPed.Handle, coords.X, coords.Y, coords.Z, false, false, false, false);
			tempTimer = GetGameTimer();

			// Wait for the collision to be loaded around the entity in this new location.
			while (!HasCollisionLoadedAroundEntity(playerPed.Handle))
			{
				// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			NetworkFadeInEntity(playerPed.Handle, true);
			playerPed.IsPositionFrozen = false;
			DoScreenFadeIn(500);
			SetGameplayCamRelativePitch(0.0f, 1.0f);
		}

		public static async void TeleportConVeh(Vector3 coords)
		{
			Ped playerPed = CachePlayer.Cache.MyPlayer.Ped;
			ClearPedTasksImmediately(playerPed.Handle);
			playerPed.IsPositionFrozen = true;
			if (playerPed.IsVisible) NetworkFadeOutEntity(playerPed.Handle, true, false);
			DoScreenFadeOut(500);
			while (!IsScreenFadedOut()) await BaseScript.Delay(0);
			RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);
			NewLoadSceneStart(coords.X, coords.Y, coords.Z, coords.X, coords.Y, coords.Z, 50f, 0);
			int tempTimer = GetGameTimer();

			// Wait for the new scene to be loaded.
			while (IsNetworkLoadingScene())
			{
				// If this takes longer than 1 second, just abort. It's not worth waiting that long.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			SetPedCoordsKeepVehicle(playerPed.Handle, coords.X, coords.Y, coords.Z);
			tempTimer = GetGameTimer();

			// Wait for the collision to be loaded around the entity in this new location.
			while (!HasCollisionLoadedAroundEntity(playerPed.Handle))
			{
				// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			NetworkFadeInEntity(playerPed.Handle, true);
			playerPed.IsPositionFrozen = false;
			DoScreenFadeIn(500);
			SetGameplayCamRelativePitch(0.0f, 1.0f);
		}

		public static void DeleteObject(int oggetto)
		{
			SetEntityAsMissionEntity(oggetto, false, true);
			DeleteObject(oggetto);
		}

		public static int GetVehicleInDirection()
		{
			int ped = CachePlayer.Cache.MyPlayer.Ped.Handle;
			Vector3 coords = CachePlayer.Cache.MyPlayer.Ped.Position;
			Vector3 inDirection = GetOffsetFromEntityInWorldCoords(ped, 0.0f, 5.0f, 0.0f);
			int rayHandle = CastRayPointToPoint(coords.X, coords.Y, coords.Z, inDirection.X, inDirection.Y, inDirection.Z, 10, ped, 0);
			bool a = false;
			Vector3 b = new Vector3();
			Vector3 c = new Vector3();
			int vehicle = 0;
			GetRaycastResult(rayHandle, ref a, ref b, ref c, ref vehicle);

			return vehicle;
		}

		#region SpawnaVehicle
	
		#region spawnaDentro
		

		public static async Task<Vehicle> SpawnVehicle(string modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicle(a, coords, heading);
		}

		public static async Task<Vehicle> SpawnVehicle(int modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicle(a, coords, heading);
		
		}

		public static async Task<Vehicle> SpawnVehicle(VehicleHash modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicle(a, coords, heading);
		}

		private static async Task<Vehicle> SpawnVehicle(Model vehicleModel, Vector3 coords, float heading)
		{
			if (vehicleModel.IsValid)
			{
				if (!vehicleModel.IsLoaded) await vehicleModel.Request(3000); // for when you stream resources.

				if (!IsSpawnPointClear(coords, 2f))
				{
					Vehicle[] vehs = GetVehiclesInArea(coords, 2f);
					foreach (Vehicle v in vehs) v.Delete();
				}

				int callback =
					await Client.Instance.Eventi.Request<int>("lprp:entity:spawnVehicle", (uint)vehicleModel.Hash, coords, heading);
				var result = (Vehicle)Entity.FromNetworkId(callback);
				while (result == null || !result.Exists()) await BaseScript.Delay(50);

				result.NeedsToBeHotwired = false;
				result.RadioStation = RadioStation.RadioOff;
				result.IsEngineStarting = false;
				result.IsEngineRunning = false;
				result.IsDriveable = false;
				result.IsPersistent = true;
				result.PreviouslyOwnedByPlayer = true;

				result.PlaceOnGround();
				CachePlayer.Cache.MyPlayer.Ped.SetIntoVehicle(result, VehicleSeat.Driver);
				result.SetDecor(Main.decorName, Main.decorInt);
				vehicleModel.MarkAsNoLongerNeeded();
				return result;
			}
			else
			{
				BaseScript.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO car] = ", "nome modello non corretto!" }, color = new[] { 255, 0, 0 } });

				return null;
			}
		}
		#endregion

		#region spawnaFuori
		public static async Task<Vehicle> SpawnVehicleNoPlayerInside(int modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicleNoPlayerInside(a, coords, heading);
		}

		public static async Task<Vehicle> SpawnVehicleNoPlayerInside(string modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicleNoPlayerInside(a, coords, heading);
		}
		
		public static async Task<Vehicle> SpawnVehicleNoPlayerInside(VehicleHash modelName, Vector3 coords, float heading)
		{
			var a = new Model(modelName);
			return await SpawnVehicleNoPlayerInside(a, coords, heading);
		}
		
		private static async Task<Vehicle> SpawnVehicleNoPlayerInside(Model vehicleModel, Vector3 coords, float heading)
		{
			if (vehicleModel.IsValid)
			{
				if (!vehicleModel.IsLoaded) await vehicleModel.Request(3000); //for when you stream resources.

				if (!IsSpawnPointClear(coords, 2f))
				{
					Vehicle[] vehs = GetVehiclesInArea(coords, 2f);
					foreach (Vehicle v in vehs) v.Delete();
				}

				int callback =
					await Client.Instance.Eventi.Request<int>("lprp:entity:spawnVehicle", (uint)vehicleModel.Hash, coords, heading);
				var result = (Vehicle)Entity.FromNetworkId(callback);
				while (result == null || !result.Exists()) await BaseScript.Delay(50);

				result.NeedsToBeHotwired = false;
				result.RadioStation = RadioStation.RadioOff;
				result.IsEngineStarting = false;
				result.IsEngineRunning = false;
				result.IsDriveable = false;
				result.IsPersistent = true;
				result.PreviouslyOwnedByPlayer = true;

				result.PlaceOnGround();
				result.SetDecor(Main.decorName, Main.decorInt);
				vehicleModel.MarkAsNoLongerNeeded();
				//var ready = await Client.Instance.Eventi.Request<bool>("cullingEntity", result.NetworkId);
				return result;
			}

			BaseScript.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO car] = ", "nome modello non corretto!" }, color = new[] { 255, 0, 0 } });

			return null;
		}
		#endregion

		#region spawnaLocal
		public static async Task<Vehicle> SpawnLocalVehicle(int vehicleModel, Vector3 coords, float heading)
		{
			var a = new Model(vehicleModel);
			return await SpawnLocalVehicle(a, coords, heading);
		}

		public static async Task<Vehicle> SpawnLocalVehicle(string vehicleModel, Vector3 coords, float heading)
		{
			var a = new Model(vehicleModel);
			return await SpawnLocalVehicle(a, coords, heading);
		}

		public static async Task<Vehicle> SpawnLocalVehicle(VehicleHash vehicleModel, Vector3 coords, float heading)
		{
			var a = new Model(vehicleModel);
			return await SpawnLocalVehicle(a, coords, heading);
		}

		private static async Task<Vehicle> SpawnLocalVehicle(Model vehicleModel, Vector3 coords, float heading)
		{
			if (!vehicleModel.IsValid) return null;
			if (!vehicleModel.IsLoaded) await vehicleModel.Request(3000); // for when you stream resources.

			if (!IsSpawnPointClear(coords, 2f))
			{
				Vehicle[] vehs = GetVehiclesInArea(coords, 2f);
				foreach (Vehicle v in vehs) v.Delete();
			}

			Vehicle vehicle = new Vehicle(CreateVehicle((uint)vehicleModel.Hash, coords.X, coords.Y, coords.Z, heading, false, false))
			{
				IsPersistent = true,
				NeedsToBeHotwired = false,
				RadioStation = RadioStation.RadioOff,
				IsEngineStarting = false,
				IsEngineRunning = false,
				IsDriveable = false
			};
			while (!vehicle.Exists()) await BaseScript.Delay(0);
			vehicle.PlaceOnGround();
			EntityDecoration.SetDecor(vehicle, Main.decorName, Main.decorInt);
			vehicleModel.MarkAsNoLongerNeeded();

			//SetVehicleEngineOn(vehicle.Handle, false, true, true);
			return vehicle;

		}
		#endregion
		
		#endregion
		
		public static async Task<Prop> SpawnLocalProp(dynamic modelName, Vector3 coords, bool dynamic, bool placeOnGround)
		{
			Model model = new Model(modelName);

			if (!await model.Request(1000)) return null;
			Prop p = new Prop(CreateObject(model.Hash, coords.X, coords.Y, coords.Z, false, false, dynamic));
			if (placeOnGround) PlaceObjectOnGroundProperly(p.Handle);

			return p;
		}

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Ped"/> could not be spawned</remarks>
		public static async Task<Ped> CreatePedLocally(dynamic model, Vector3 position, float heading = 0f, PedTypes PedType = PedTypes.Mission)
		{
			Model mod = new Model(model);

			if (!mod.IsPed || !await mod.Request(3000)) return null;
			Ped p = new Ped(CreatePed((int)PedType, (uint)mod.Hash, position.X, position.Y, position.Z, heading, false, false));
			while (!p.Exists()) await BaseScript.Delay(0);
			EntityDecoration.SetDecor(p, Main.decorName, Main.decorInt);

			return p;
		}

		#region SpawnPed

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <param name="pedType"></param>
		/// <remarks>returns <c>null</c> if the <see cref="Ped"/> could not be spawned</remarks>
		public static async Task<Ped> SpawnPed(int model, Vector3 position, float heading = 0f,
			PedTypes pedType = PedTypes.Mission)
		{
			var a = new Model(model);
			return await SpawnPed(a, position, heading, pedType);

		}

		public static async Task<Ped> SpawnPed(string model, Vector3 position, float heading = 0f,
			PedTypes pedType = PedTypes.Mission)
		{
			var a = new Model(model);
			return await SpawnPed(a, position, heading, pedType);
	
		}

		public static async Task<Ped> SpawnPed(PedHash model, Vector3 position, float heading = 0f,
			PedTypes pedType = PedTypes.Mission)
		{
			var a = new Model(model);
			return await SpawnPed(a, position, heading, pedType);
		}
		private static async Task<Ped> SpawnPed(Model pedModel, Vector3 position, float heading = 0f, PedTypes pedType = PedTypes.Mission)
		{
			if (pedModel.IsValid)
			{
				if (!pedModel.IsLoaded) await pedModel.Request(3000); // for when you stream resources.

				if (!IsSpawnPedPointClear(position, 2f))
				{
					var Peds = GetPedsInArea(position, 2f);
					foreach (var v in Peds)
						if (!v.IsPlayer)
							v.Delete();
				}
			}

			int callback =
				await Client.Instance.Eventi.Request<int>("lprp:entity:spawnPed", (uint) pedModel.Hash, position,
					heading, (int)pedType);

			var ped = (Ped) Entity.FromNetworkId(callback);
			while (!ped.Exists()) await BaseScript.Delay(50);
			ped.SetDecor(Main.decorName, Main.decorInt);
			ped.IsPersistent = true;
			return ped;
		}
	
		/// <summary>
		/// Spawns a <see cref="Ped"/> of a random <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		public static async Task<Ped> SpawnRandomPed(Vector3 position)
		{
			var ped = new Ped(CreateRandomPed(position.X, position.Y, position.Z));
			while (!ped.Exists()) await BaseScript.Delay(0);
			ped.SetDecor(Main.decorName, Main.decorInt);
			return ped;
		}
		#endregion

		public static void spectatePlayer(int targetPed, int targetId, string name, bool enableSpectate)
		{
			int mio = PlayerPedId();
			enableSpectate = true;

			if (targetId == mio)
			{
				enableSpectate = false;
				HUD.HUD.ShowNotification("~r~Non puoi spectare te stesso!!");
			}

			if (enableSpectate)
			{
				Vector3 coords = GetEntityCoords(PlayerPedId(), true);
				RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);
				NetworkSetInSpectatorMode(true, targetPed);
			}
			else
			{
				Vector3 coords = GetEntityCoords(PlayerPedId(), true);
				RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);
				NetworkSetInSpectatorMode(false, targetPed);
			}
		}

		/// <summary>
		/// Ritorna i Player nell'area delle coordinate inserite
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static List<Player> GetPlayersInArea(Vector3 coords, float area, bool ignoreCallerPlayer = true)
		{
			List<Player> playersInArea = ignoreCallerPlayer ? Client.Instance.GetPlayers.ToList().FindAll(p => Vector3.Distance(p.Character.Position, coords) < area && p != CachePlayer.Cache.MyPlayer.Player) : Client.Instance.GetPlayers.ToList().FindAll(p => Vector3.Distance(p.Character.Position, coords) < area);

			return playersInArea;
		}

		/// <summary>
		/// Ritorna i Ped dei Player nell'area delle coordinate inserite
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static List<Ped> GetPlayersPedsInArea(Vector3 coords, float area)
		{
			return Client.Instance.GetPlayers.ToList().Select(p => p.Character).Where(target => target.IsInRangeOf(coords, area)).ToList();
		}

		#region Closest Methodi

		#region Veicoli

		public static Vehicle GetClosestVehicle(this Entity entity) { return World.GetClosest(entity.Position, World.GetAllVehicles()); }

		public static Vehicle GetClosestVehicle(this Entity entity, string model) { return GetClosestVehicle(entity, new Model(model)); }

		public static Vehicle GetClosestVehicle(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsVehicle) return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => x.Model.Hash == model.Hash).ToArray());

			return null;
		}

		public static Vehicle GetClosestVehicle(this Entity entity, VehicleHash hash) { return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => x.Model.Hash == (int)hash).ToArray()); }

		public static Vehicle GetClosestVehicle(this Entity entity, List<VehicleHash> hashes) { return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => hashes.Contains((VehicleHash)x.Model.Hash)).ToArray()); }

		public static Vehicle GetClosestVehicle(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));

			return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Vehicle GetClosestVehicle(this Entity entity, List<Model> models) { return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => models.Contains(x.Model)).ToArray()); }

		public static Tuple<Vehicle, float> GetClosestVehicleWithDistance(this Ped entity)
		{
			Vehicle veh = World.GetClosest(entity.Position, World.GetAllVehicles());
			float dist = Vector3.Distance(entity.Position, veh.Position);

			return new Tuple<Vehicle, float>(veh, dist);
		}

		#endregion

		#region Peds

		public static Ped GetClosestPed(this Entity entity) { return World.GetClosest(entity.Position, World.GetAllPeds()); }

		public static Ped GetClosestPed(this Entity entity, string model) { return GetClosestPed(entity, new Model(model)); }

		public static Ped GetClosestPed(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsPed) return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => x.Model.Hash == model.Hash).ToArray());

			return null;
		}

		public static Ped GetClosestPed(this Entity entity, PedHash hash) { return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => (PedHash)x.Model.Hash == hash).ToArray()); }

		public static Ped GetClosestPed(this Entity entity, List<PedHash> hashes) { return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => hashes.Contains((PedHash)x.Model.Hash)).ToArray()); }

		public static Ped GetClosestPed(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));

			return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Ped GetClosestPed(this Entity entity, List<Model> models) { return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => models.Contains(x.Model)).ToArray()); }

		public static Tuple<Ped, float> GetClosestPedWithDistance(this Ped entity)
		{
			Ped ped = World.GetClosest(entity.Position, World.GetAllPeds());
			float dist = Vector3.Distance(entity.Position, ped.Position);

			return new Tuple<Ped, float>(ped, dist);
		}

		#endregion

		#region Props

		public static Prop GetClosestProp(this Entity entity) { return World.GetClosest(entity.Position, World.GetAllProps()); }

		public static Prop GetClosestProp(this Entity entity, string model) { return GetClosestProp(entity, new Model(model)); }

		public static Prop GetClosestProp(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsProp) return World.GetClosest(entity.Position, World.GetAllProps().Where(x => x.Model.Hash == model.Hash).ToArray());

			return null;
		}

		public static Prop GetClosestProp(this Entity entity, ObjectHash hash) { return World.GetClosest(entity.Position, World.GetAllProps().Where(x => (ObjectHash)x.Model.Hash == hash).ToArray()); }

		public static Prop GetClosestProp(this Entity entity, List<ObjectHash> hashes) { return World.GetClosest(entity.Position, World.GetAllProps().Where(x => hashes.Contains((ObjectHash)x.Model.Hash)).ToArray()); }

		public static Prop GetClosestProp(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));

			return World.GetClosest(entity.Position, World.GetAllProps().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Prop GetClosestProp(this Entity entity, List<Model> models) { return World.GetClosest(entity.Position, World.GetAllProps().Where(x => models.Contains(x.Model)).ToArray()); }

		public static Tuple<Prop, float> GetClosestPropWithDistance(this Prop entity)
		{
			Prop ped = World.GetClosest(entity.Position, World.GetAllProps());
			float dist = Vector3.Distance(entity.Position, ped.Position);

			return new Tuple<Prop, float>(ped, dist);
		}

		#endregion

		#endregion

		public static float Rad2deg(float rad) { return rad * (180.0f / (float)Math.PI); }

		public static float Deg2rad(float deg) { return deg * ((float)Math.PI / 180.0f); }

		public static Player GetPlayerFromPed(Ped ped)
		{
			return Client.Instance.GetPlayers.ToList().FirstOrDefault(pl => pl.Character.NetworkId == ped.NetworkId);
		}

		/// <summary>
		/// Controlla distanza dal Ped del giocatore a tutti i players e ritorna il piu vicino e la sua distanza
		/// </summary>
		public static Tuple<Player, float> GetClosestPlayer() { return GetClosestPlayer(CachePlayer.Cache.MyPlayer.User.posizione.ToVector3()); }

		/// <summary>
		/// Controlla la distanza tra le coordinate inserite e tutti i Players e ritorna il Player piu vicino a quelle coordinate
		/// </summary>
		/// <param name="coords"></param>
		public static Tuple<Player, float> GetClosestPlayer(Vector3 coords)
		{
			if (Client.Instance.GetPlayers.ToList().Count <= 1) return new Tuple<Player, float>(null, -1);
			Player closestPlayer = Client.Instance.GetPlayers.ToList().OrderBy(x => Vector3.Distance(x.Character.Position, coords)).FirstOrDefault(x => x != CachePlayer.Cache.MyPlayer.Player);

			return new Tuple<Player, float>(closestPlayer, Vector3.Distance(coords, closestPlayer.Character.Position));
		}

		/// <summary>
		/// GetHashKey uint
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static uint HashUint(string str) { return (uint)Game.GenerateHash(str); }

		/// <summary>
		/// GetHashKey int
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int HashInt(string str) { return Game.GenerateHash(str); }

		/// <summary>
		/// Si connette al server e ritorna tutti i personaggi online e i loro dati
		/// </summary>
		/// <returns></returns>
		public static async Task<Dictionary<string, User>> GetOnlinePlayersAndTheirData()
		{
			return await Client.Instance.Eventi.Request<Dictionary<string, User>>("lprp:callPlayers");
		}

		/// <summary>
		/// Si connette al server e al DB e ritorna tutti i personaggi salvati nel db stesso
		/// </summary>
		/// <returns></returns>
		public static async Task<Dictionary<string, User>> GetAllPlayersAndTheirData()
		{
			return await Client.Instance.Eventi.Request<Dictionary<string, User>>("lprp:callDBPlayers");
		}

		public static bool IsSpawnPointClear(this Vector3 pos, float Radius)
		{
			Vehicle[] vehs = GetVehiclesInArea(pos, Radius);

			return vehs.Length < 1 ? true : false;
		}

		public static bool IsSpawnObjPointClear(this Vector3 pos, float Radius)
		{
			Prop[] vehs = GetPropsInArea(pos, Radius);

			return vehs.Length < 1 ? true : false;
		}

		public static bool IsSpawnPedPointClear(this Vector3 pos, float Radius)
		{
			Ped[] vehs = GetPedsInArea(pos, Radius);

			return vehs.Length < 1;
		}

		public static Vehicle[] GetVehiclesInArea(this Vector3 Coords, float Radius) { return World.GetAllVehicles().Where(x => x.IsInRangeOf(Coords, Radius)).ToArray(); }

		public static Prop[] GetPropsInArea(this Vector3 Coords, float Radius) { return World.GetAllProps().Where(x => x.IsInRangeOf(Coords, Radius)).ToArray(); }

		public static Ped[] GetPedsInArea(this Vector3 Coords, float Radius) { return World.GetAllPeds().Where(x => x.IsInRangeOf(Coords, Radius)).ToArray(); }

		public static Vector2 WorldToScreen(Vector3 position)
		{
			OutputArgument screenX = new OutputArgument();
			OutputArgument screenY = new OutputArgument();

			return !Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, screenX, screenY) ? Vector2.Zero : new Vector2(screenX.GetResult<float>(), screenY.GetResult<float>());
		}

		public static void StartScenario(Ped ped, string scenario) { TaskStartScenarioInPlace(ped.Handle, scenario, 0, true); }

		public static int GetRandomInt(int end) { return new Random(GetGameTimer()).Next(end); }

		public static int GetRandomInt(int start, int end) { return new Random(GetGameTimer()).Next(start, end); }

		public static long GetRandomLong(long end)
		{
			Random rand = new Random(GetGameTimer());

			return rand.NextLong(end);
		}

		public static long GetRandomLong(long start, long end)
		{
			Random rand = new Random(GetGameTimer());

			return rand.NextLong(start, end);
		}

		public static float GetRandomFloat(float end) { return GetRandomFloat(0, end); }

		public static float GetRandomFloat(float start, float end)
		{
			Random rand = new Random(GetGameTimer());

			return (float)Math.Round(rand.NextFloat(start, end), 3);
		}

		public static Tuple<int, int> secondsToClock(int Seconds)
		{
			int seconds = Seconds;
			int hours;
			int mins;
			int secs;

			if (seconds <= 0) return new Tuple<int, int>(mins = 0, secs = 0);
			hours = (int)Math.Floor((float)(seconds / 3600));
			mins = (int)Math.Floor((float)(seconds / 60 - hours * 60));
			secs = (int)Math.Floor((float)(seconds - hours * 3600 - mins * 60));

			return new Tuple<int, int>(mins, secs);
		}

		public static string GetWeaponLabel(uint hash)
		{
			if (hash == HashUint("WEAPON_UNARMED")) return Game.GetGXTEntry("WT_UNARMED");
			if (hash == HashUint("WEAPON_COUGAR")) return Game.GetGXTEntry("WT_RAGE");
			if (hash == HashUint("WEAPON_KNIFE")) return Game.GetGXTEntry("WT_KNIFE");
			if (hash == HashUint("WEAPON_NIGHTSTICK")) return Game.GetGXTEntry("WT_NGTSTK");
			if (hash == HashUint("WEAPON_HAMMER")) return Game.GetGXTEntry("WT_HAMMER");
			if (hash == HashUint("WEAPON_BAT")) return Game.GetGXTEntry("WT_BAT");
			if (hash == HashUint("WEAPON_GOLFCLUB")) return Game.GetGXTEntry("WT_GOLFCLUB");
			if (hash == HashUint("WEAPON_CROWBAR")) return Game.GetGXTEntry("WT_CROWBAR");
			if (hash == HashUint("WEAPON_PISTOL")) return Game.GetGXTEntry("WT_PIST");
			if (hash == HashUint("WEAPON_COMBATPISTOL")) return Game.GetGXTEntry("WT_PIST_CBT");
			if (hash == HashUint("WEAPON_APPISTOL")) return Game.GetGXTEntry("WT_PIST_AP");
			if (hash == HashUint("WEAPON_PISTOL50")) return Game.GetGXTEntry("WT_PIST_50");
			if (hash == HashUint("WEAPON_MICROSMG")) return Game.GetGXTEntry("WT_SMG_MCR");
			if (hash == HashUint("WEAPON_SMG")) return Game.GetGXTEntry("WT_SMG");
			if (hash == HashUint("WEAPON_ASSAULTSMG")) return Game.GetGXTEntry("WT_SMG_ASL");
			if (hash == HashUint("WEAPON_ASSAULTRIFLE")) return Game.GetGXTEntry("WT_RIFLE_ASL");
			if (hash == HashUint("WEAPON_CARBINERIFLE")) return Game.GetGXTEntry("WT_RIFLE_CBN");
			if (hash == HashUint("WEAPON_ADVANCEDRIFLE")) return Game.GetGXTEntry("WT_RIFLE_ADV");
			if (hash == HashUint("WEAPON_MG")) return Game.GetGXTEntry("WT_MG");
			if (hash == HashUint("WEAPON_COMBATMG")) return Game.GetGXTEntry("WT_MG_CBT");
			if (hash == HashUint("WEAPON_PUMPSHOTGUN")) return Game.GetGXTEntry("WT_SG_PMP");
			if (hash == HashUint("WEAPON_SAWNOFFSHOTGUN")) return Game.GetGXTEntry("WT_SG_SOF");
			if (hash == HashUint("WEAPON_ASSAULTSHOTGUN")) return Game.GetGXTEntry("WT_SG_ASL");
			if (hash == HashUint("WEAPON_BULLPUPSHOTGUN")) return Game.GetGXTEntry("WT_SG_BLP");
			if (hash == HashUint("WEAPON_STUNGUN")) return Game.GetGXTEntry("WT_STUN");
			if (hash == HashUint("WEAPON_SNIPERRIFLE")) return Game.GetGXTEntry("WT_SNIP_RIF");
			if (hash == HashUint("WEAPON_HEAVYSNIPER")) return Game.GetGXTEntry("WT_SNIP_HVY");
			if (hash == HashUint("WEAPON_REMOTESNIPER")) return Game.GetGXTEntry("WT_SNIP_RMT");
			if (hash == HashUint("WEAPON_GRENADELAUNCHER")) return Game.GetGXTEntry("WT_GL");
			if (hash == HashUint("WEAPON_GRENADELAUNCHER_SMOKE")) return Game.GetGXTEntry("WT_GL_SMOKE");
			if (hash == HashUint("WEAPON_RPG")) return Game.GetGXTEntry("WT_RPG");
			if (hash == HashUint("WEAPON_STINGER")) return Game.GetGXTEntry("WT_RPG");
			if (hash == HashUint("WEAPON_MINIGUN")) return Game.GetGXTEntry("WT_MINIGUN");
			if (hash == HashUint("WEAPON_GRENADE")) return Game.GetGXTEntry("WT_GNADE");
			if (hash == HashUint("WEAPON_STICKYBOMB")) return Game.GetGXTEntry("WT_GNADE_STK");
			if (hash == HashUint("WEAPON_SMOKEGRENADE")) return Game.GetGXTEntry("WT_GNADE_SMK");
			if (hash == HashUint("WEAPON_BZGAS")) return Game.GetGXTEntry("WT_BZGAS");
			if (hash == HashUint("WEAPON_MOLOTOV")) return Game.GetGXTEntry("WT_MOLOTOV");
			if (hash == HashUint("WEAPON_FIREEXTINGUISHER")) return Game.GetGXTEntry("WT_FIRE");
			if (hash == HashUint("WEAPON_PETROLCAN")) return Game.GetGXTEntry("WT_PETROL");
			if (hash == HashUint("WEAPON_DIGISCANNER")) return Game.GetGXTEntry("WT_DIGI");
			if (hash == HashUint("GADGET_NIGHTVISION")) return Game.GetGXTEntry("WT_NV");
			if (hash == HashUint("OBJECT")) return Game.GetGXTEntry("WT_OBJECT");
			if (hash == HashUint("WEAPON_BALL")) return Game.GetGXTEntry("WT_BALL");
			if (hash == HashUint("WEAPON_FLARE")) return Game.GetGXTEntry("WT_FLARE");
			if (hash == HashUint("WEAPON_ELECTRIC_FENCE")) return Game.GetGXTEntry("WT_ELCFEN");
			if (hash == HashUint("VEHICLE_WEAPON_TANK")) return Game.GetGXTEntry("WT_V_TANK");
			if (hash == HashUint("VEHICLE_WEAPON_SPACE_ROCKET")) return Game.GetGXTEntry("WT_V_SPACERKT");
			if (hash == HashUint("VEHICLE_WEAPON_PLAYER_LASER")) return Game.GetGXTEntry("WT_V_PLRLSR");
			if (hash == HashUint("AMMO_RPG")) return Game.GetGXTEntry("WT_A_RPG");
			if (hash == HashUint("AMMO_TANK")) return Game.GetGXTEntry("WT_A_TANK");
			if (hash == HashUint("AMMO_SPACE_ROCKET")) return Game.GetGXTEntry("WT_A_SPACERKT");
			if (hash == HashUint("AMMO_PLAYER_LASER")) return Game.GetGXTEntry("WT_A_PLRLSR");
			if (hash == HashUint("AMMO_ENEMY_LASER")) return Game.GetGXTEntry("WT_A_ENMYLSR");
			if (hash == HashUint("WEAPON_RAMMED_BY_CAR")) return Game.GetGXTEntry("WT_PIST");
			if (hash == HashUint("WEAPON_BOTTLE")) return Game.GetGXTEntry("WT_BOTTLE");
			if (hash == HashUint("WEAPON_GUSENBERG")) return Game.GetGXTEntry("WT_GUSENBERG");
			if (hash == HashUint("WEAPON_SNSPISTOL")) return Game.GetGXTEntry("WT_SNSPISTOL");
			if (hash == HashUint("WEAPON_VINTAGEPISTOL")) return Game.GetGXTEntry("WT_VPISTOL");
			if (hash == HashUint("WEAPON_DAGGER")) return Game.GetGXTEntry("WT_DAGGER");
			if (hash == HashUint("WEAPON_FLAREGUN")) return Game.GetGXTEntry("WT_FLAREGUN");
			if (hash == HashUint("WEAPON_HEAVYPISTOL")) return Game.GetGXTEntry("WT_HEAVYPSTL");
			if (hash == HashUint("WEAPON_SPECIALCARBINE")) return Game.GetGXTEntry("WT_RIFLE_SCBN");
			if (hash == HashUint("WEAPON_MUSKET")) return Game.GetGXTEntry("WT_MUSKET");
			if (hash == HashUint("WEAPON_FIREWORK")) return Game.GetGXTEntry("WT_FWRKLNCHR");
			if (hash == HashUint("WEAPON_MARKSMANRIFLE")) return Game.GetGXTEntry("WT_MKRIFLE");
			if (hash == HashUint("WEAPON_HEAVYSHOTGUN")) return Game.GetGXTEntry("WT_HVYSHOT");
			if (hash == HashUint("WEAPON_PROXMINE")) return Game.GetGXTEntry("WT_PRXMINE");
			if (hash == HashUint("WEAPON_HOMINGLAUNCHER")) return Game.GetGXTEntry("WT_HOMLNCH");
			if (hash == HashUint("WEAPON_HATCHET")) return Game.GetGXTEntry("WT_HATCHET");
			if (hash == HashUint("WEAPON_COMBATPDW")) return Game.GetGXTEntry("WT_COMBATPDW");
			if (hash == HashUint("WEAPON_KNUCKLE")) return Game.GetGXTEntry("WT_KNUCKLE");
			if (hash == HashUint("WEAPON_MARKSMANPISTOL")) return Game.GetGXTEntry("WT_MKPISTOL");
			if (hash == HashUint("WEAPON_MACHETE")) return Game.GetGXTEntry("WT_MACHETE");
			if (hash == HashUint("WEAPON_MACHINEPISTOL")) return Game.GetGXTEntry("WT_MCHPIST");
			if (hash == HashUint("WEAPON_FLASHLIGHT")) return Game.GetGXTEntry("WT_FLASHLIGHT");
			if (hash == HashUint("WEAPON_DBSHOTGUN")) return Game.GetGXTEntry("WT_DBSHGN");
			if (hash == HashUint("WEAPON_COMPACTRIFLE")) return Game.GetGXTEntry("WT_CMPRIFLE");
			if (hash == HashUint("WEAPON_SWITCHBLADE")) return Game.GetGXTEntry("WT_SWBLADE");
			if (hash == HashUint("WEAPON_REVOLVER")) return Game.GetGXTEntry("WT_REVOLVER");
			if (hash == HashUint("WEAPON_SNSPISTOL_MK2")) return Game.GetGXTEntry("WT_SNSPISTOL2");
			if (hash == HashUint("WEAPON_REVOLVER_MK2")) return Game.GetGXTEntry("WT_REVOLVER2");
			if (hash == HashUint("WEAPON_DOUBLEACTION")) return Game.GetGXTEntry("WT_REV_DA");
			if (hash == HashUint("WEAPON_SPECIALCARBINE_MK2")) return Game.GetGXTEntry("WT_SPCARBINE2");
			if (hash == HashUint("WEAPON_BULLPUPRIFLE_MK2")) return Game.GetGXTEntry("WT_BULLRIFLE2");
			if (hash == HashUint("WEAPON_PUMPSHOTGUN_MK2")) return Game.GetGXTEntry("WT_SG_PMP2");
			if (hash == HashUint("WEAPON_MARKSMANRIFLE_MK2")) return Game.GetGXTEntry("WT_MKRIFLE2");
			if (hash == HashUint("WEAPON_POOLCUE")) return Game.GetGXTEntry("WT_POOLCUE");
			if (hash == HashUint("WEAPON_WRENCH")) return Game.GetGXTEntry("WT_WRENCH");
			if (hash == HashUint("WEAPON_BATTLEAXE")) return Game.GetGXTEntry("WT_BATTLEAXE");
			if (hash == HashUint("WEAPON_MINISMG")) return Game.GetGXTEntry("WT_MINISMG");
			if (hash == HashUint("WEAPON_BULLPUPRIFLE")) return Game.GetGXTEntry("WT_BULLRIFLE");
			if (hash == HashUint("WEAPON_AUTOSHOTGUN")) return Game.GetGXTEntry("WT_AUTOSHGN");
			if (hash == HashUint("WEAPON_RAILGUN")) return Game.GetGXTEntry("WT_RAILGUN");
			if (hash == HashUint("WEAPON_COMPACTLAUNCHER")) return Game.GetGXTEntry("WT_CMPGL");
			if (hash == HashUint("WEAPON_SNOWBALL")) return Game.GetGXTEntry("WT_SNWBALL");
			if (hash == HashUint("WEAPON_PIPEBOMB")) return Game.GetGXTEntry("WT_PIPEBOMB");
			if (hash == HashUint("GADGET_NIGHTVISION")) return Game.GetGXTEntry("WT_NV");
			if (hash == HashUint("GADGET_PARACHUTE")) return Game.GetGXTEntry("WT_PARA");
			if (hash == HashUint("WEAPON_STONE_HATCHET")) return Game.GetGXTEntry("WT_SHATCHET");
			if (hash == HashUint("COMPONENT_AT_PI_FLSH")) return Game.GetGXTEntry("WCT_FLASH");
			if (hash == HashUint("COMPONENT_PISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_PISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_PI_SUPP_02")) return Game.GetGXTEntry("WCT_SUPP");
			if (hash == HashUint("COMPONENT_PISTOL_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_COMBATPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_COMBATPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_PI_SUPP")) return Game.GetGXTEntry("WCT_SUPP");
			if (hash == HashUint("COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_APPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_APPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_APPISTOL_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_PISTOL50_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_PISTOL50_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_AR_SUPP_02")) return Game.GetGXTEntry("WCT_SUPP");
			if (hash == HashUint("COMPONENT_PISTOL50_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_SNSPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_SNSPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_SNSPISTOL_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_HEAVYPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_HEAVYPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_HEAVYPISTOL_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_VINTAGEPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_VINTAGEPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_MICROSMG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_MICROSMG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_SCOPE_MACRO")) return Game.GetGXTEntry("WCT_SCOPE_MAC");
			if (hash == HashUint("COMPONENT_MICROSMG_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_SMG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_SMG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_SMG_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_AT_SCOPE_MACRO_02")) return Game.GetGXTEntry("WCT_SCOPE_MAC");
			if (hash == HashUint("COMPONENT_SMG_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_ASSAULTSMG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_ASSAULTSMG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_MINISMG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_MINISMG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_MACHINEPISTOL_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_MACHINEPISTOL_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_MACHINEPISTOL_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_COMBATPDW_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_COMBATPDW_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_COMBATPDW_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_AT_AR_AFGRIP")) return Game.GetGXTEntry("WCT_GRIP");
			if (hash == HashUint("COMPONENT_AT_SCOPE_SMALL")) return Game.GetGXTEntry("WCT_SCOPE_SML");
			if (hash == HashUint("COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_SAWNOFfsHOTGUN_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_ASSAULTSHOTGUN_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_ASSAULTSHOTGUN_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_ASSAULTRIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_ASSAULTRIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_ASSAULTRIFLE_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_ASSAULTRIFLE_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_CARBINERIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_CARBINERIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_CARBINERIFLE_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_AT_SCOPE_MEDIUM")) return Game.GetGXTEntry("WCT_SCOPE_MED");
			if (hash == HashUint("COMPONENT_CARBINERIFLE_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_ADVANCEDRIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_ADVANCEDRIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_SPECIALCARBINE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_SPECIALCARBINE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_SPECIALCARBINE_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_BULLPUPRIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_BULLPUPRIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_BULLPUPRIFLE_VARMOD_LOW")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_COMPACTRIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_COMPACTRIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_COMPACTRIFLE_CLIP_03")) return Game.GetGXTEntry("WCT_CLIP_DRM");
			if (hash == HashUint("COMPONENT_MG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_MG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_MG_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_COMBATMG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_COMBATMG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_COMBATMG_VARMOD_LOWRIDER")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_GUSENBERG_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_GUSENBERG_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_SCOPE_LARGE")) return Game.GetGXTEntry("WCT_SCOPE_LRG");
			if (hash == HashUint("COMPONENT_AT_SCOPE_MAX")) return Game.GetGXTEntry("WCT_SCOPE_MAX");
			if (hash == HashUint("COMPONENT_SNIPERRIFLE_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("COMPONENT_MARKSMANRIFLE_CLIP_01")) return Game.GetGXTEntry("WCT_CLIP1");
			if (hash == HashUint("COMPONENT_MARKSMANRIFLE_CLIP_02")) return Game.GetGXTEntry("WCT_CLIP2");
			if (hash == HashUint("COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM")) return Game.GetGXTEntry("WCT_SCOPE_LRG");
			if (hash == HashUint("COMPONENT_MARKSMANRIFLE_VARMOD_LUXE")) return Game.GetGXTEntry("WCT_VAR_GOLD");
			if (hash == HashUint("WM_TINT0")) return Game.GetGXTEntry("WM_TINT0");
			if (hash == HashUint("WM_TINT1")) return Game.GetGXTEntry("WM_TINT1");
			if (hash == HashUint("WM_TINT2")) return Game.GetGXTEntry("WM_TINT2");
			if (hash == HashUint("WM_TINT3")) return Game.GetGXTEntry("WM_TINT3");
			if (hash == HashUint("WM_TINT4")) return Game.GetGXTEntry("WM_TINT4");
			if (hash == HashUint("WM_TINT5")) return Game.GetGXTEntry("WM_TINT5");
			if (hash == HashUint("WM_TINT6")) return Game.GetGXTEntry("WM_TINT6");
			if (hash == HashUint("WM_TINT7")) return Game.GetGXTEntry("WM_TINT7");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_BASE")) return Game.GetGXTEntry("WCT_KNUCK_01");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_PIMP")) return Game.GetGXTEntry("WCT_KNUCK_02");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_BALLAS")) return Game.GetGXTEntry("WCT_KNUCK_BG");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_DOLLAR")) return Game.GetGXTEntry("WCT_KNUCK_DLR");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_DIAMOND")) return Game.GetGXTEntry("WCT_KNUCK_DMD");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_HATE")) return Game.GetGXTEntry("WCT_KNUCK_HT");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_LOVE")) return Game.GetGXTEntry("WCD_VAR_DESC");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_PLAYER")) return Game.GetGXTEntry("WCT_KNUCK_PC");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_KING")) return Game.GetGXTEntry("WCT_KNUCK_SLG");
			if (hash == HashUint("COMPONENT_KNUCKLE_VARMOD_VAGOS")) return Game.GetGXTEntry("WCT_KNUCK_VG");
			if (hash == HashUint("COMPONENT_SWITCHBLADE_VARMOD_BASE")) return Game.GetGXTEntry("WCT_SB_BASE");
			if (hash == HashUint("COMPONENT_SWITCHBLADE_VARMOD_VAR1")) return Game.GetGXTEntry("WCT_SB_VAR1");
			if (hash == HashUint("COMPONENT_SWITCHBLADE_VARMOD_VAR2")) return Game.GetGXTEntry("WCT_SB_VAR2");

			if (hash == HashUint("WEAPON_ANIMAL") || hash == HashUint("WEAPON_PASSENGER_ROCKET") || hash == HashUint("WEAPON_AIRSTRIKE_ROCKET") || hash == HashUint("WEAPON_BRIEFCASE") || hash == HashUint("WEAPON_BRIEFCASE_02") || hash == HashUint("WEAPON_FIRE") || hash == HashUint("WEAPON_HELI_CRASH") || hash == HashUint("WEAPON_RUN_OVER_BY_CAR") || hash == HashUint("WEAPON_HIT_BY_WATER_CANNON") || hash == HashUint("WEAPON_EXHAUSTION") || hash == HashUint("WEAPON_FALL") || hash == HashUint("WEAPON_EXPLOSION") || hash == HashUint("WEAPON_BLEEDING") || hash == HashUint("WEAPON_DROWNING_IN_VEHICLE") || hash == HashUint("WEAPON_DROWNING") || hash == HashUint("WEAPON_BARBED_WIRE") || hash == HashUint("WEAPON_VEHICLE_ROCKET"))
			{
				Log.Printa(LogType.Error, "Errore nell'hash /" + hash.ToString() + "/ per arma/componente. forse non è mai stato aggiunto?");

				return Game.GetGXTEntry("WT_INVALID");
			}

			Log.Printa(LogType.Error, "Errore nell'hash /" + hash.ToString() + "/ per arma/componente. forse non è mai stato aggiunto?");

			return Game.GetGXTEntry("WT_INVALID");
		}

		public static string GetVehColorLabel(int color)
		{
			switch (color)
			{
				case 0:
					return Game.GetGXTEntry("BLACK");
				case 1:
					return Game.GetGXTEntry("GRAPHITE");
				case 2:
					return Game.GetGXTEntry("BLACK_STEEL");
				case 3:
					return Game.GetGXTEntry("DARK_SILVER");
				case 4:
					return Game.GetGXTEntry("SILVER");
				case 5:
					return Game.GetGXTEntry("BLUE_SILVER");
				case 6:
					return Game.GetGXTEntry("ROLLED_STEEL");
				case 7:
					return Game.GetGXTEntry("SHADOW_SILVER");
				case 8:
					return Game.GetGXTEntry("STONE_SILVER");
				case 9:
					return Game.GetGXTEntry("MIDNIGHT_SILVER");
				case 10:
					return Game.GetGXTEntry("CAST_IRON_SIL");
				case 11:
					return Game.GetGXTEntry("ANTHR_BLACK");
				case 12:
					return Game.GetGXTEntry("BLACK");
				case 13:
					return Game.GetGXTEntry("GREY");
				case 14:
					return Game.GetGXTEntry("LIGHT_GREY");
				case 15:
					return Game.GetGXTEntry("BLACK");
				case 16:
					return Game.GetGXTEntry("FMMC_COL1_1");
				case 27:
					return Game.GetGXTEntry("RED");
				case 28:
					return Game.GetGXTEntry("TORINO_RED");
				case 29:
					return Game.GetGXTEntry("FORMULA_RED");
				case 30:
					return Game.GetGXTEntry("BLAZE_RED");
				case 31:
					return Game.GetGXTEntry("GRACE_RED");
				case 32:
					return Game.GetGXTEntry("GARNET_RED");
				case 33:
					return Game.GetGXTEntry("SUNSET_RED");
				case 34:
					return Game.GetGXTEntry("CABERNET_RED");
				case 35:
					return Game.GetGXTEntry("CANDY_RED");
				case 36:
					return Game.GetGXTEntry("SUNRISE_ORANGE");
				case 37:
					return Game.GetGXTEntry("GOLD");
				case 38:
					return Game.GetGXTEntry("ORANGE");
				case 39:
					return Game.GetGXTEntry("RED");
				case 40:
					return Game.GetGXTEntry("DARK_RED");
				case 41:
					return Game.GetGXTEntry("ORANGE");
				case 42:
					return Game.GetGXTEntry("YELLOW");
				case 49:
					return Game.GetGXTEntry("DARK_GREEN");
				case 50:
					return Game.GetGXTEntry("RACING_GREEN");
				case 51:
					return Game.GetGXTEntry("SEA_GREEN");
				case 52:
					return Game.GetGXTEntry("OLIVE_GREEN");
				case 53:
					return Game.GetGXTEntry("BRIGHT_GREEN");
				case 54:
					return Game.GetGXTEntry("PETROL_GREEN");
				case 55:
					return Game.GetGXTEntry("LIME_GREEN");
				case 61:
					return Game.GetGXTEntry("GALAXY_BLUE");
				case 62:
					return Game.GetGXTEntry("DARK_BLUE");
				case 63:
					return Game.GetGXTEntry("SAXON_BLUE");
				case 64:
					return Game.GetGXTEntry("BLUE");
				case 65:
					return Game.GetGXTEntry("MARINER_BLUE");
				case 66:
					return Game.GetGXTEntry("HARBOR_BLUE");
				case 67:
					return Game.GetGXTEntry("DIAMOND_BLUE");
				case 68:
					return Game.GetGXTEntry("SURF_BLUE");
				case 69:
					return Game.GetGXTEntry("NAUTICAL_BLUE");
				case 70:
					return Game.GetGXTEntry("ULTRA_BLUE");
				case 71:
					return Game.GetGXTEntry("PURPLE");
				case 72:
					return Game.GetGXTEntry("SPIN_PURPLE");
				case 73:
					return Game.GetGXTEntry("RACING_BLUE");
				case 74:
					return Game.GetGXTEntry("LIGHT_BLUE");
				case 82:
					return Game.GetGXTEntry("DARK_BLUE");
				case 83:
					return Game.GetGXTEntry("BLUE");
				case 84:
					return Game.GetGXTEntry("MIDNIGHT_BLUE");
				case 88:
					return Game.GetGXTEntry("YELLOW");
				case 89:
					return Game.GetGXTEntry("RACE_YELLOW");
				case 90:
					return Game.GetGXTEntry("BRONZE");
				case 91:
					return Game.GetGXTEntry("FLUR_YELLOW");
				case 92:
					return Game.GetGXTEntry("LIME_GREEN");
				case 94:
					return Game.GetGXTEntry("UMBER_BROWN");
				case 95:
					return Game.GetGXTEntry("CREEK_BROWN");
				case 96:
					return Game.GetGXTEntry("CHOCOLATE_BROWN");
				case 97:
					return Game.GetGXTEntry("MAPLE_BROWN");
				case 98:
					return Game.GetGXTEntry("SADDLE_BROWN");
				case 99:
					return Game.GetGXTEntry("STRAW_BROWN");
				case 100:
					return Game.GetGXTEntry("MOSS_BROWN");
				case 101:
					return Game.GetGXTEntry("BISON_BROWN");
				case 102:
					return Game.GetGXTEntry("WOODBEECH_BROWN");
				case 103:
					return Game.GetGXTEntry("BEECHWOOD_BROWN");
				case 104:
					return Game.GetGXTEntry("SIENNA_BROWN");
				case 105:
					return Game.GetGXTEntry("SANDY_BROWN");
				case 106:
					return Game.GetGXTEntry("BLEECHED_BROWN");
				case 107:
					return Game.GetGXTEntry("CREAM");
				case 111:
					return Game.GetGXTEntry("WHITE");
				case 112:
					return Game.GetGXTEntry("FROST_WHITE");
				case 117:
					return Game.GetGXTEntry("BR_STEEL");
				case 118:
					return Game.GetGXTEntry("BR BLACK_STEEL");
				case 119:
					return Game.GetGXTEntry("BR_ALUMINIUM");
				case 120:
					return Game.GetGXTEntry("CHROME");
				case 128:
					return Game.GetGXTEntry("GREEN");
				case 131:
					return Game.GetGXTEntry("WHITE");
				case 135:
					return Game.GetGXTEntry("HOT PINK");
				case 136:
					return Game.GetGXTEntry("SALMON_PINK");
				case 137:
					return Game.GetGXTEntry("PINK");
				case 138:
					return Game.GetGXTEntry("BRIGHT_ORANGE");
				case 141:
					return Game.GetGXTEntry("MIDNIGHT_BLUE");
				case 143:
					return Game.GetGXTEntry("WINE_RED");
				case 145:
					return Game.GetGXTEntry("BRIGHT_PURPLE");
				case 146:
					return Game.GetGXTEntry("MIGHT_PURPLE");
				case 147:
					return Game.GetGXTEntry("BLACK_GRAPHITE");
				case 148:
					return Game.GetGXTEntry("Purple");
				case 149:
					return Game.GetGXTEntry("MIGHT_PURPLE");
				case 150:
					return Game.GetGXTEntry("LAVA_RED");
				case 151:
					return Game.GetGXTEntry("MATTE_FOR");
				case 152:
					return Game.GetGXTEntry("MATTE_OD");
				case 153:
					return Game.GetGXTEntry("MATTE_DIRT");
				case 154:
					return Game.GetGXTEntry("MATTE_DESERT");
				case 155:
					return Game.GetGXTEntry("MATTE_FOIL");
				case 158:
					return Game.GetGXTEntry("GOLD_P");
				case 159:
					return Game.GetGXTEntry("GOLD_S");
				default:
					return "Nome colore non trovato";
			}
		}

		public static string GetSourceOfDeath(uint hash) { return ConfigShared.SharedConfig.Main.Generici.DeathReasons[hash]; }
	}
}