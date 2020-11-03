using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Personaggio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuovaGM.Shared;
using Logger;
using NuovaGM.Shared.Veicoli;
using NuovaGM.Client.Veicoli;

namespace NuovaGM.Client.gmPrincipale.Utility
{
	enum PedTypes
	{
		Player0,// michael  
		Player1,// franklin  
		MPPlayer,    // mp character  
		Player2,// trevor  
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
	static class Funzioni
	{
		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKVPString(string key, string value) => SetResourceKvp(key, value);
		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKVP(string key, object value) => SetResourceKvp(key, JsonConvert.SerializeObject(value));

		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKVPInt(string key, int value) => SetResourceKvpInt(key, value);

		/// <summary>
		/// Salva clientside dei dati arbitrari
		/// </summary>
		public static void SalvaKVPFloat(string key, float value) => SetResourceKvpFloat(key, value);

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static int CaricaKVPInt(string key) => GetResourceKvpInt(key);
		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static float CaricaKVPFloat(string key) => GetResourceKvpFloat(key);
		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static string CaricaKVPString(string key) => GetResourceKvpString(key);

		/// <summary>
		/// Recupera un dato arbitrario salvato clientside
		/// </summary>
		public static T CaricaKVP<T>(string key) => JsonConvert.DeserializeObject<T>(GetResourceKvpString(key));



		public static PlayerChar GetPlayerCharFromPlayerId(int id)
		{
			foreach (KeyValuePair<string, PlayerChar> p in Eventi.GiocatoriOnline)
			{
				if (GetPlayerFromServerId(Convert.ToInt32(p.Key)) == id)
					return p.Value;
			}
			return null;
		}

		public static PlayerChar GetPlayerCharFromServerId(int id)
		{
			foreach (KeyValuePair<string, PlayerChar> p in Eventi.GiocatoriOnline)
			{
				if (Convert.ToInt32(p.Key) == id)
					return p.Value;
			}
			return null;
		}

		public static PlayerChar GetPlayerData(this Player player)
		{
			return player == Game.Player ? Eventi.Player : GetPlayerCharFromServerId(player.ServerId);
		}

		public static void SendNuiMessage(object message)
		{
			API.SendNuiMessage(message.Serialize());
		}

		public static void ConcealPlayersNearby(Vector3 coord, float radius)
		{
			var players = GetPlayersInArea(coord, radius);
			foreach (var pl in players)
				if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle)
					NetworkConcealPlayer(pl.Handle, true, true);
		}

		public static void ConcealAllPlayers()
		{
			Client.Instance.GetPlayers.ToList().ForEach(pl => { if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle) { NetworkConcealPlayer(pl.Handle, true, true); } });
		}

		public static void RevealPlayersNearby(Vector3 coord, float radius)
		{
			var players = GetPlayersInArea(coord, radius);
			foreach (var pl in players)
				if (NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle)
					NetworkConcealPlayer(pl.Handle, false, false);
		}

		public static void RevealAllPlayers()
		{
			Client.Instance.GetPlayers.ToList().ForEach(pl => { if (NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle) { NetworkConcealPlayer(pl.Handle, false, false); } });
		}


		public static async Task UpdateFace(Skin skin)
		{
			SetPedHeadBlendData(Game.PlayerPed.Handle, skin.face.mom, skin.face.dad, 0, skin.face.mom, skin.face.dad, 0, skin.resemblance, skin.skinmix, 0f, false);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 0, skin.blemishes.style, skin.blemishes.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 1, skin.facialHair.beard.style, skin.facialHair.beard.opacity);
			SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, skin.facialHair.beard.color[0], skin.facialHair.beard.color[1]);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 2, skin.facialHair.eyebrow.style, skin.facialHair.eyebrow.opacity);
			SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, skin.facialHair.eyebrow.color[0], skin.facialHair.eyebrow.color[1]);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 3, skin.ageing.style, skin.ageing.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 4, skin.makeup.style, skin.makeup.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 5, skin.blusher.style, skin.blusher.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 6, skin.complexion.style, skin.complexion.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 7, skin.skinDamage.style, skin.skinDamage.opacity);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 8, skin.lipstick.style, skin.lipstick.opacity);
			SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 1, skin.lipstick.color[0], skin.lipstick.color[1]);
			SetPedHeadOverlay(Game.PlayerPed.Handle, 9, skin.freckles.style, skin.freckles.opacity);
			SetPedEyeColor(Game.PlayerPed.Handle, skin.eye.style);
			SetPedComponentVariation(Game.PlayerPed.Handle, 2, skin.hair.style, 0, 0);
			SetPedHairColor(Game.PlayerPed.Handle, skin.hair.color[0], skin.hair.color[1]);
			SetPedPropIndex(Game.PlayerPed.Handle, 2, skin.ears.style, skin.ears.color, true);
			for (int i = 0; i < skin.face.tratti.Length; i++)
			{
				SetPedFaceFeature(Game.PlayerPed.Handle, i, skin.face.tratti[i]);
			}

			await Task.FromResult(0);
		}

		public static async Task UpdateDress(Dressing dress)
		{
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Faccia, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Maschera, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Pantaloni, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Borsa_Paracadute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Scarpe, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Accessori, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Sottomaglia, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);

			if (dress.PropIndices.Cappelli_Maschere == -1) ClearPedProp(PlayerPedId(), 0);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Cappelli_Maschere, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, false);
			if (dress.PropIndices.Orecchie == -1) ClearPedProp(PlayerPedId(), 2);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Orecchie, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, false);
			if (dress.PropIndices.Occhiali_Occhi == -1) ClearPedProp(PlayerPedId(), 1);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Occhiali_Occhi, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
			if (dress.PropIndices.Unk_3 == -1) ClearPedProp(PlayerPedId(), 3);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
			if (dress.PropIndices.Unk_4 == -1) ClearPedProp(PlayerPedId(), 4);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
			if (dress.PropIndices.Unk_5 == -1) ClearPedProp(PlayerPedId(), 5);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
			if (dress.PropIndices.Orologi == -1) ClearPedProp(PlayerPedId(), 6);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Orologi, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
			if (dress.PropIndices.Bracciali == -1) ClearPedProp(PlayerPedId(), 7);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Bracciali, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
			if (dress.PropIndices.Unk_8 == -1) ClearPedProp(PlayerPedId(), 8);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_8, dress.PropIndices.Unk_8, dress.PropTextures.Unk_8, true);

			await Task.FromResult(0);
		}

		/// <summary>
		/// Prende il primo piano di un ped
		/// </summary>
		/// <param name="ped"></param>
		/// <returns></returns>
		public static async Task<Tuple<int, string>> GetPedMugshotAsync(Ped ped, bool transparent = false)
		{
			int mugshot;
			if (transparent)
				mugshot = RegisterPedheadshotTransparent(ped.Handle);
			mugshot = RegisterPedheadshot(ped.Handle);
			while (!IsPedheadshotReady(mugshot)) await BaseScript.Delay(1);
			string Txd = GetPedheadshotTxdString(mugshot);
			return new Tuple<int, string>(mugshot, Txd);
		}

		public static async Task<string> GetRandomString(int maxChars)
		{
			string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ";
			string result = "";
			for (int i = 0; i < maxChars; i++)
			{
				await BaseScript.Delay(1);
				result += chars.PickOneChar();
				await BaseScript.Delay(1);
			}
			return result;
		}

		public static RaycastResult GetEntityInFrontOfPed(Ped ped, float maxDistance = 5.0f)
		{
			Vector3 offset = GetOffsetFromEntityInWorldCoords(ped.Handle, 0.0f, 5.0f, 0.0f);
			return Function.Call<RaycastResult>(Hash._START_SHAPE_TEST_RAY, ped.Position.X, ped.Position.Y, ped.Position.Z, offset.X, offset.Y, offset.Z, 10, ped, 0);
		}

		public static Entity GetEntityInFrontOfPlayer(float distance = 5f)
		{
			try
			{
				RaycastResult raycast = World.Raycast(Game.PlayerPed.Position, Game.PlayerPed.GetOffsetPosition(new Vector3(0f, distance, 0f)), IntersectOptions.Everything);
				if (raycast.DitHitEntity)
				{
					return (Entity)raycast.HitEntity;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"{ex.Message}");
			}
			return null;
		}

		public static Vehicle GetVehicleInFrontOfPlayer(float distance = 5f)
		{
			try
			{
				Entity source = Game.PlayerPed.IsInVehicle() ? (Entity)Game.PlayerPed.CurrentVehicle : Game.PlayerPed;
				return GetVehicleInFrontOfPlayer(source, source, distance);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}
			return default(Vehicle);
		}

		public static bool IsAnyControlJustPressed()
		{
			return Enum.GetValues(typeof(Control)).Cast<Control>().Any(value => Input.IsControlJustPressed(value));
		}

		public static bool IsAnyControlPressed()
		{
			return Enum.GetValues(typeof(Control)).Cast<Control>().Any(value => Input.IsControlPressed(value));
		}


		public static async Task<VehProp> GetVehicleProperties(this Vehicle veh)
		{
			bool[] extras = new bool[13];
			for (int i = 0; i < 13; i++) extras[i] = veh.IsExtraOn(i);

			List<VehMod> mods = new List<VehMod>();
			foreach (var mod in veh.Mods.GetAllMods())
				mods.Add(new VehMod((int)mod.ModType, mod.Index, mod.LocalizedModName, mod.LocalizedModTypeName));

			VehProp vehi = new VehProp(
			veh.Model,
			veh.LocalizedName,
			veh.Mods.LicensePlate,
			(int)veh.Mods.LicensePlateStyle,
			veh.BodyHealth,
			veh.EngineHealth,
			veh.FuelLevel,
			veh.DirtLevel,

			(int)veh.Mods.PrimaryColor, (int)veh.Mods.SecondaryColor,
			veh.Mods.CustomPrimaryColor, veh.Mods.CustomSecondaryColor,
			veh.Mods.IsPrimaryColorCustom, veh.Mods.IsSecondaryColorCustom,
			(int)veh.Mods.PearlescentColor, (int)veh.Mods.RimColor,
			(int)veh.Mods.WheelType,
			(int)veh.Mods.WindowTint,

			new bool[4] { veh.Mods.HasNeonLight(VehicleNeonLight.Left), veh.Mods.HasNeonLight(VehicleNeonLight.Right), veh.Mods.HasNeonLight(VehicleNeonLight.Front), veh.Mods.HasNeonLight(VehicleNeonLight.Back) },
			extras,
			veh.Mods.NeonLightsColor,
			veh.Mods.TireSmokeColor,
			!(GetVehicleModKit(veh.Handle) == 65535),
			mods,
			veh.Mods.Livery
			);
			await Task.FromResult(0);
			return vehi;
		}

		public static async Task SetVehicleProperties(this Vehicle veh, VehProp props)
		{
			veh.Mods.LicensePlate = props.Plate;
			if (props.ModKitInstalled)
				veh.Mods.InstallModKit();
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
			for (int i = 0; i < props.NeonEnabled.Length; i++)
				veh.Mods.SetNeonLightsOn((VehicleNeonLight)i, props.NeonEnabled[i]);
			for(int i=0; i<13; i++)
				veh.ToggleExtra(i, props.Extras[i]);
			veh.Mods.NeonLightsColor = props.NeonColor;
			veh.Mods.TireSmokeColor = props.TireSmokeColor;
			var mods = veh.Mods.GetAllMods();
			foreach (var mod in props.Mods)
				SetVehicleMod(veh.Handle, mod.ModIndex, mod.Value, mods.ToList().FirstOrDefault(x=>(int)x.ModType == mod.ModIndex).Variation);
			veh.Mods.Livery = props.ModLivery;
		}

		public static Vehicle GetVehicleInFrontOfPlayer(Entity source, Entity ignore, float distance = 5f)
		{
			try
			{
				RaycastResult raycast = World.Raycast(source.Position + new Vector3(0f, 0f, -0.4f), source.GetOffsetPosition(new Vector3(0f, distance, 0f)) + new Vector3(0f, 0f, -0.4f), (IntersectOptions)71, ignore);
				if (raycast.DitHitEntity && raycast.HitEntity.Model.IsVehicle)
				{
					return (Vehicle)raycast.HitEntity;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"[WORLDPROBE] GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}
			return default(CitizenFX.Core.Vehicle);
		}

		public static Vector3 CalculateClosestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
		{
			try
			{
				float dotProduct = Vector3.Dot(start - end, point - start);
				float percent = dotProduct / (start - end).LengthSquared();
				if (percent < 0.0f)
				{
					return start;
				}
				else if (percent > 1.0f)
				{
					return end;
				}
				else
				{
					return (start + (percent * (end - start)));
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe CalculateClosestPointOnLine Error: {ex.Message}");
			}
			return default(Vector3);
		}

		public static Vector3 GameplayCamForwardVector()
		{
			try
			{
				Vector3 rotation = (float)(Math.PI / 180.0) * GetGameplayCamRot(2);
				return Vector3.Normalize(new Vector3((float)-Math.Sin(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Cos(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Sin(rotation.X)));
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}
			return default(Vector3);
		}


		public static RaycastResult _CrosshairRaycast(float distance = 1000)
		{
			try
			{
				return World.Raycast(Game.PlayerPed.Position, Game.PlayerPed.Position + distance * GameplayCamForwardVector(), IntersectOptions.Everything, Game.PlayerPed);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
			}
			return default(RaycastResult);
		}

		public struct _RaycastResult
		{
			public Entity HitEntity { get; }
			public Vector3 HitPosition { get; }
			public Vector3 SurfaceNormal { get; }
			public bool DitHit { get; }
			public bool DitHitEntity { get; }
			public int Result { get; }

			public _RaycastResult(bool DitHit, Vector3 HitPosition, Vector3 SurfaceNormal, int entityHandle, int Result)
			{
				this.DitHit = DitHit;
				this.HitPosition = HitPosition;
				this.SurfaceNormal = SurfaceNormal;
				if (entityHandle == 0)
				{
					this.HitEntity = null;
					this.DitHitEntity = false;
				}
				else
				{
					this.HitEntity = Entity.FromHandle(entityHandle);
					this.DitHitEntity = true;
				}
				this.Result = Result;
			}
		}


		public static _RaycastResult CrosshairRaycast(float distance = 1000f)
		{
			return CrosshairRaycast(Game.PlayerPed);
		}



		/// <summary>
		/// Because HitPosition and SurfaceNormal are currently broken in platform function
		/// </summary>
		/// <returns></returns>
		public static _RaycastResult CrosshairRaycast(Entity ignore, float distance = 1000f)
		{
			try
			{
				// Uncomment these to potentially save on raycasts (don't think they're ridiculously costly, but there's a # limit per tick)
				//if(CrosshairRaycastThisTick != null && distance == 1000f) return (_RaycastResult) CrosshairRaycastThisTick;

				Vector3 start = GameplayCamera.Position;
				Vector3 end = GameplayCamera.Position + distance * GameplayCamForwardVector();
				int raycastHandle = Function.Call<int>(Hash._START_SHAPE_TEST_RAY, start.X, start.Y, start.Z, end.X, end.Y, end.Z, IntersectOptions.Everything, ignore.Handle, 0);
				bool DitHit = false;
				Vector3 HitPosition = new Vector3(0);
				Vector3 SurfaceNormal = new Vector3(0);
				int HitEntity = 0;
				GetShapeTestResult(raycastHandle, ref DitHit, ref HitPosition, ref SurfaceNormal, ref HitEntity);
				//if(distance == 1000f) CrosshairRaycastThisTick = result;
				return new _RaycastResult(DitHit, HitPosition, SurfaceNormal, HitEntity, raycastHandle); ;
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe CrosshairRaycast Error: {ex.Message}");
			}
			return default(_RaycastResult);
		}

		// Apparently there's a built-in native for this...
		// TODO: Replace with that
		public static string GetEntityType(int entityHandle)
		{
			try
			{
				if (IsEntityAPed(entityHandle))
					return "PED";
				else if (IsEntityAVehicle(entityHandle))
					return "VEH";
				else if (IsEntityAnObject(entityHandle))
					return "OBJ";
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
					if (GetGroundZFor_3dCoord(position.X, position.Y, h, ref z, false))
					{
						result = z;
					}
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
			ClearPedTasksImmediately(Game.PlayerPed.Handle);
			Game.PlayerPed.IsPositionFrozen = true;
			if (Game.PlayerPed.IsVisible)
				NetworkFadeOutEntity(PlayerPedId(), true, false);
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
			SetEntityCoords(PlayerPedId(), coords.X, coords.Y, coords.Z, false, false, false, false);
			tempTimer = GetGameTimer();

			// Wait for the collision to be loaded around the entity in this new location.
			while (!HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle))
			{
				// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");
					break;
				}
				await BaseScript.Delay(0);
			}

			NetworkFadeInEntity(Game.PlayerPed.Handle, true);
			Game.PlayerPed.IsPositionFrozen = false;
			DoScreenFadeIn(500);
			SetGameplayCamRelativePitch(0.0f, 1.0f);
		}

		public static async void TeleportConVeh(Vector3 coords)
		{
			ClearPedTasksImmediately(Game.PlayerPed.Handle);
			Game.PlayerPed.IsPositionFrozen = true;
			if (Game.PlayerPed.IsVisible)
				NetworkFadeOutEntity(PlayerPedId(), true, false);
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
			SetPedCoordsKeepVehicle(PlayerPedId(), coords.X, coords.Y, coords.Z);
			tempTimer = GetGameTimer();

			// Wait for the collision to be loaded around the entity in this new location.
			while (!HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle))
			{
				// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Log.Printa(LogType.Debug, "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");
					break;
				}
				await BaseScript.Delay(0);
			}

			NetworkFadeInEntity(Game.PlayerPed.Handle, true);
			Game.PlayerPed.IsPositionFrozen = false;
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
			int ped = Game.PlayerPed.Handle;
			Vector3 coords = Game.PlayerPed.Position;
			Vector3 inDirection = GetOffsetFromEntityInWorldCoords(ped, 0.0f, 5.0f, 0.0f);
			int rayHandle = CastRayPointToPoint(coords.X, coords.Y, coords.Z, inDirection.X, inDirection.Y, inDirection.Z, 10, ped, 0);
			bool a = false;
			Vector3 b = new Vector3();
			Vector3 c = new Vector3();
			int vehicle = 0;
			GetRaycastResult(rayHandle, ref a, ref b, ref c, ref vehicle);
			return vehicle;
		}

		public static async Task<Vehicle> SpawnVehicle(dynamic modelName, Vector3 coords, float heading)
		{
			Model vehicleModel = new Model(modelName);
			if (vehicleModel.IsValid)
			{
				if (!vehicleModel.IsLoaded)
				{
					await vehicleModel.Request(3000); // for when you stream resources.
				}

				if (!IsSpawnPointClear(coords, 2f))
				{
					Vehicle[] vehs = GetVehiclesInArea(coords, 2f);
					foreach (Vehicle v in vehs)
					{
						v.Delete();
					}
				}

				Vehicle vehicle = new Vehicle(CreateVehicle((uint)vehicleModel.Hash, coords.X, coords.Y, coords.Z, heading, true, false))
				{
					NeedsToBeHotwired = false,
					RadioStation = RadioStation.RadioOff,
					IsEngineStarting = false,
					IsEngineRunning = false,
					IsDriveable = false,
					IsPersistent = true,
					PreviouslyOwnedByPlayer = true
				};
				while (!vehicle.Exists()) await BaseScript.Delay(0);
				vehicle.PlaceOnGround();
				Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
				EntityDecoration.SetDecor(vehicle, Main.decorName, Main.decorInt);
				vehicleModel.MarkAsNoLongerNeeded();
				bool ready = false;
				Client.Instance.TriggerServerCallback("cullingEntity", new Action<bool>((ok) => { ready = ok; }), vehicle.NetworkId);
				while (!ready) await BaseScript.Delay(0);
				return Game.PlayerPed.CurrentVehicle;
			}
			else
			{
				BaseScript.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO car] = ", "nome modello non corretto!" }, color = new[] { 255, 0, 0 } });
				return null;
			}
		}

		public static async Task<Vehicle> SpawnVehicleNoPlayerInside(dynamic modelName, Vector3 coords, float heading)
		{
			Model vehicleModel = new Model(modelName);
			if (vehicleModel.IsValid)
			{
				if (!vehicleModel.IsLoaded)
				{
					await vehicleModel.Request(3000); //for when you stream resources.
				}

				if (!IsSpawnPointClear(coords, 2f))
				{
					Vehicle[] vehs = GetVehiclesInArea(coords, 2f);
					foreach (Vehicle v in vehs)
					{
						v.Delete();
					}
				}
				Vehicle vehicle = new Vehicle(CreateVehicle((uint)vehicleModel.Hash, coords.X, coords.Y, coords.Z, heading, true, false))
				{
					NeedsToBeHotwired = false,
					RadioStation = RadioStation.RadioOff,
					IsEngineStarting = false,
					IsEngineRunning = false,
					IsDriveable = false,
					PreviouslyOwnedByPlayer = true
				};
				while (!vehicle.Exists()) await BaseScript.Delay(0);
				vehicle.PlaceOnGround();
				//vehicle.MarkAsNoLongerNeeded();
				bool ready = false;
				int netid = vehicle.NetworkId;
				Client.Instance.TriggerServerCallback("cullingEntity", new Action<dynamic>((ok) => { ready = ok; }), vehicle.NetworkId);
				while (!ready) await BaseScript.Delay(0);
				return vehicle;
			}
			else
			{
				BaseScript.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO car] = ", "nome modello non corretto!" }, color = new[] { 255, 0, 0 } });
				return null;
			}
		}

		public static async Task<Vehicle> SpawnLocalVehicle(dynamic modelName, Vector3 coords, float heading)
		{
			var vehicleModel = new Model(modelName);
			if (vehicleModel.IsValid)
			{
				if (!vehicleModel.IsLoaded)
					await vehicleModel.Request(3000); // for when you stream resources.

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
			else
				return null;
		}

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Ped"/> could not be spawned</remarks>
		public static async Task<Ped> CreatePedLocally(Model model, Vector3 position, float heading = 0f, PedTypes PedType = PedTypes.Mission)
		{
			if (!model.IsPed || !await model.Request(3000))
				return null;
			Ped p = new Ped(CreatePed((int)PedType, (uint)model.Hash, position.X, position.Y, position.Z, heading, false, false));
			while (!p.Exists()) await BaseScript.Delay(0);
			EntityDecoration.SetDecor(p, Main.decorName, Main.decorInt);
			return p;
		}

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Ped"/> could not be spawned</remarks>
		public static async Task<Ped> SpawnPed(dynamic model, Vector3 position, float heading = 0f, PedTypes PedType = PedTypes.Mission)
		{
			var pedModel = new Model(model);
			if (pedModel.IsValid)
			{
				if (!pedModel.IsLoaded)
					await pedModel.Request(3000); // for when you stream resources.

				if (!IsSpawnPedPointClear(position, 2f))
				{
					Ped[] vehs = GetPedsInArea(position, 2f);
					foreach (Ped v in vehs)
						if (!v.IsPlayer)
							v.Delete();
				}
			}

			Ped p = new Ped(CreatePed((int)PedType, (uint)pedModel.Hash, position.X, position.Y, position.Z, heading, true, false));
			while (!p.Exists()) await BaseScript.Delay(0);
			bool ready = false;
			EntityDecoration.SetDecor(p, Main.decorName, Main.decorInt);
			Client.Instance.TriggerServerCallback("cullingEntity", new Action<bool>((ok) => { ready = ok; }), p.NetworkId);
			while (!ready) await BaseScript.Delay(0);
			p.IsPersistent = true;
			return p;
		}
		/// <summary>
		/// Spawns a <see cref="Ped"/> of a random <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		public static async Task<Ped> SpawnRandomPed(Vector3 position)
		{
			Ped p = new Ped(CreateRandomPed(position.X, position.Y, position.Z));
			while (!p.Exists()) await BaseScript.Delay(0);
			bool ready = false;
			EntityDecoration.SetDecor(p, Main.decorName, Main.decorInt);
			Client.Instance.TriggerServerCallback("cullingEntity", new Action<bool>((ok) => { ready = ok; }), p.NetworkId);
			while (!ready) await BaseScript.Delay(0);
			return p;
		}


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
			List<Player> PlayersInArea = ignoreCallerPlayer ? Client.Instance.GetPlayers.ToList().FindAll(p => (Vector3.Distance(p.Character.Position, coords) < area) && p != Game.Player) : Client.Instance.GetPlayers.ToList().FindAll(p => (Vector3.Distance(p.Character.Position, coords) < area));
			return PlayersInArea;
		}

		/// <summary>
		/// Ritorna i Ped dei Player nell'area delle coordinate inserite
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static List<Ped> GetPlayersPedsInArea(Vector3 coords, float area)
		{
			List<Ped> PlayersPedsInArea = new List<Ped>();
			foreach (Player p in Client.Instance.GetPlayers.ToList())
			{
				Ped target = p.Character;
				if (target.IsInRangeOf(coords, area))
				{
					PlayersPedsInArea.Add(target);
				}
			}
			return PlayersPedsInArea;
		}

		#region Closest Methodi

		#region Veicoli

		public static Vehicle GetClosestVehicle(this Entity entity)
		{
			return World.GetClosest(entity.Position, World.GetAllVehicles());
		}

		public static Vehicle GetClosestVehicle(this Entity entity, string model)
		{
			return GetClosestVehicle(entity, new Model(model));
		}

		public static Vehicle GetClosestVehicle(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsVehicle)
				return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => x.Model.Hash == model.Hash).ToArray());
			else
				return null;
		}

		public static Vehicle GetClosestVehicle(this Entity entity, VehicleHash hash)
		{
			return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => x.Model.Hash == (int)hash).ToArray());
		}

		public static Vehicle GetClosestVehicle(this Entity entity, List<VehicleHash> hashes)
		{
			return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => hashes.Contains((VehicleHash)x.Model.Hash)).ToArray());
		}

		public static Vehicle GetClosestVehicle(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));
			return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Vehicle GetClosestVehicle(this Entity entity, List<Model> models)
		{
			return World.GetClosest(entity.Position, World.GetAllVehicles().Where(x => models.Contains(x.Model)).ToArray());
		}

		public static Tuple<Vehicle, float> GetClosestVehicle(this Ped entity)
		{
			var veh = World.GetClosest(entity.Position, World.GetAllVehicles());
			float dist = entity.Position.DistanceToSquared(veh.Position);
			return new Tuple<Vehicle, float>(veh, dist);
		}
		#endregion

		#region Peds
		public static Ped GetClosestPed(this Entity entity)
		{
			return World.GetClosest(entity.Position, World.GetAllPeds());
		}

		public static Ped GetClosestPed(this Entity entity, string model)
		{
			return GetClosestPed(entity, new Model(model));
		}

		public static Ped GetClosestPed(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsPed)
				return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => x.Model.Hash == model.Hash).ToArray());
			else
				return null;
		}

		public static Ped GetClosestPed(this Entity entity, PedHash hash)
		{
			return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => (PedHash)x.Model.Hash == hash).ToArray());
		}

		public static Ped GetClosestPed(this Entity entity, List<PedHash> hashes)
		{
			return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => hashes.Contains((PedHash)x.Model.Hash)).ToArray());
		}

		public static Ped GetClosestPed(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));
			return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Ped GetClosestPed(this Entity entity, List<Model> models)
		{
			return World.GetClosest(entity.Position, World.GetAllPeds().Where(x => models.Contains(x.Model)).ToArray());
		}

		public static Tuple<Ped, float> GetClosestPed(this Ped entity)
		{
			Ped ped = World.GetClosest(entity.Position, World.GetAllPeds());
			float dist = entity.Position.DistanceToSquared(ped.Position);
			return new Tuple<Ped, float>(ped, dist);
		}
		#endregion

		#region Props
		public static Prop GetClosestProp(this Entity entity)
		{
			return World.GetClosest(entity.Position, World.GetAllProps());
		}

		public static Prop GetClosestProp(this Entity entity, string model)
		{
			return GetClosestProp(entity, new Model(model));
		}

		public static Prop GetClosestProp(this Entity entity, Model model)
		{
			if (model.IsValid && model.IsProp)
				return World.GetClosest(entity.Position, World.GetAllProps().Where(x => x.Model.Hash == model.Hash).ToArray());
			else
				return null;
		}

		public static Prop GetClosestProp(this Entity entity, ObjectHash hash)
		{
			return World.GetClosest(entity.Position, World.GetAllProps().Where(x => (ObjectHash)x.Model.Hash == hash).ToArray());
		}

		public static Prop GetClosestProp(this Entity entity, List<ObjectHash> hashes)
		{
			return World.GetClosest(entity.Position, World.GetAllProps().Where(x => hashes.Contains((ObjectHash)x.Model.Hash)).ToArray());
		}

		public static Prop GetClosestProp(this Entity entity, List<string> models)
		{
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));
			return World.GetClosest(entity.Position, World.GetAllProps().Where(x => hashes.Contains(x.Model.Hash)).ToArray());
		}

		public static Prop GetClosestProp(this Entity entity, List<Model> models)
		{
			return World.GetClosest(entity.Position, World.GetAllProps().Where(x => models.Contains(x.Model)).ToArray());
		}

		public static Tuple<Prop, float> GetClosestProp(this Prop entity)
		{
			Prop ped = World.GetClosest(entity.Position, World.GetAllProps());
			float dist = entity.Position.DistanceToSquared(ped.Position);
			return new Tuple<Prop, float>(ped, dist);
		}
		#endregion

		#endregion
		public static float Rad2deg(float rad)
		{
			return rad * (180.0f / (float)Math.PI);
		}

		public static float Deg2rad(float deg)
		{
			return deg * ((float)Math.PI / 180.0f);
		}

		public static Player GetPlayerFromPed(Ped ped)
		{
			foreach (Player pl in Client.Instance.GetPlayers.ToList())
				if (pl.Character.NetworkId == ped.NetworkId)
					return pl;
			return null;
		}

		/// <summary>
		/// Controlla distanza dal Ped del giocatore a tutti i players e ritorna il piu vicino e la sua distanza
		/// </summary>
		public static Tuple<Player, float> GetClosestPlayer()
		{
			return GetClosestPlayer(Game.PlayerPed.Position);
		}

		/// <summary>
		/// Controlla la distanza tra le coordinate inserite e tutti i Players e ritorna il Player piu vicino a quelle coordinate
		/// </summary>
		/// <param name="coords"></param>
		public static Tuple<Player, float> GetClosestPlayer(Vector3 coords)
		{
			float closestDistance = -1;
			Player closestPlayer = null;

			foreach (Player p in Client.Instance.GetPlayers.ToList())
			{
				Ped target = p.Character;
				Vector3 targetCoords = target.Position;
				float distance = World.GetDistance(coords, targetCoords);
				if (closestDistance == -1 || closestDistance > distance)
				{
					closestPlayer = p;
					closestDistance = distance;
				}
			}
			return new Tuple<Player, float>(closestPlayer, closestDistance);
		}

		/// <summary>
		/// GetHashKey uint
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static uint HashUint(string str) => (uint)GetHashKey(str);

		/// <summary>
		/// GetHashKey int
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int HashInt(string str) => GetHashKey(str);

		/// <summary>
		/// Si connette al server e ritorna tutti i personaggi online e i loro dati
		/// </summary>
		/// <returns></returns>
		public static async Task<Dictionary<string, PlayerChar>> GetOnlinePlayersAndTheirData()
		{
			Dictionary<string, PlayerChar> players = new Dictionary<string, PlayerChar>();
			Client.Instance.TriggerServerCallback("ChiamaPlayersOnline", new Action<dynamic>((result) =>
			{
				players = (result as string).Deserialize<Dictionary<string, PlayerChar>>();
			}));
			while (players.Count == 0) await BaseScript.Delay(0);
			return players;
		}

		/// <summary>
		/// Si connette al server e al DB e ritorna tutti i personaggi salvati nel db stesso
		/// </summary>
		/// <returns></returns>
		public static async Task<Dictionary<string, PlayerChar>> GetAllPlayersAndTheirData()
		{
			Dictionary<string, PlayerChar> players = new Dictionary<string, PlayerChar>();
			Client.Instance.TriggerServerCallback("ChiamaPlayersDB", new Action<dynamic>((result) =>
			{
				players = (result as string).Deserialize<Dictionary<string, PlayerChar>>();
			}));
			while (players.Count == 0) await BaseScript.Delay(0);
			return players;
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
			return vehs.Length < 1 ? true : false;
		}

		public static Vehicle[] GetVehiclesInArea(this Vector3 Coords, float Radius)
		{
			Vehicle[] vehs = World.GetAllVehicles();
			List<Vehicle> vehiclesInArea = new List<Vehicle>();
			foreach (Vehicle v in vehs)
			{
				if (v.IsInRangeOf(Coords, Radius))
					vehiclesInArea.Add(v);
			}
			return vehiclesInArea.ToArray();
		}

		public static Prop[] GetPropsInArea(this Vector3 Coords, float Radius)
		{
			Prop[] props = World.GetAllProps();
			List<Prop> propsInArea = new List<Prop>();
			foreach (Prop v in props)
			{
				if (v.IsInRangeOf(Coords, Radius))
					propsInArea.Add(v);
			}
			return propsInArea.ToArray();
		}

		public static Ped[] GetPedsInArea(this Vector3 Coords, float Radius)
		{
			Ped[] peds = World.GetAllPeds();
			List<Ped> pedsInArea = new List<Ped>();
			foreach (Ped v in peds)
			{
				if (v.IsInRangeOf(Coords, Radius))
					pedsInArea.Add(v);
			}
			return pedsInArea.ToArray();
		}

		public static Vector2 WorldToScreen(Vector3 position)
		{
			var screenX = new OutputArgument();
			var screenY = new OutputArgument();
			return !Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, screenX, screenY) ?
				Vector2.Zero :
				new Vector2(screenX.GetResult<float>(), screenY.GetResult<float>());
		}

		public static void StartScenario(Ped ped, string scenario)
		{
			TaskStartScenarioInPlace(ped.Handle, scenario, 0, true);
		}

		public static int GetRandomInt(int end)
		{
			return new Random(GetGameTimer()).Next(end);
		}

		public static int GetRandomInt(int start, int end)
		{
			return new Random(GetGameTimer()).Next(start, end);
		}

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

		public static float GetRandomFloat(float end)
		{
			return GetRandomFloat(0, end);
		}

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
			if (seconds <= 0)
			{
				return new Tuple<int, int>(mins = 0, secs = 0);
			}
			else
			{
				hours = (int)(Math.Floor((float)(seconds / 3600)));
				mins = (int)(Math.Floor((float)(seconds / 60 - (hours * 60))));
				secs = (int)(Math.Floor((float)(seconds - hours * 3600 - mins * 60)));
				return new Tuple<int, int>(mins, secs);
			}
		}

		public static Dictionary<string, string> HASH_TO_LABEL = new Dictionary<string, string>()
		{
			[Convert.ToString((uint)GetHashKey("WEAPON_UNARMED"))] = "WT_UNARMED",
			[Convert.ToString((uint)GetHashKey("WEAPON_ANIMAL"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_COUGAR"))] = "WT_RAGE",
			[Convert.ToString((uint)GetHashKey("WEAPON_KNIFE"))] = "WT_KNIFE",
			[Convert.ToString((uint)GetHashKey("WEAPON_NIGHTSTICK"))] = "WT_NGTSTK",
			[Convert.ToString((uint)GetHashKey("WEAPON_HAMMER"))] = "WT_HAMMER",
			[Convert.ToString((uint)GetHashKey("WEAPON_BAT"))] = "WT_BAT",
			[Convert.ToString((uint)GetHashKey("WEAPON_GOLFCLUB"))] = "WT_GOLFCLUB",
			[Convert.ToString((uint)GetHashKey("WEAPON_CROWBAR"))] = "WT_CROWBAR",
			[Convert.ToString((uint)GetHashKey("WEAPON_PISTOL"))] = "WT_PIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATPISTOL"))] = "WT_PIST_CBT",
			[Convert.ToString((uint)GetHashKey("WEAPON_APPISTOL"))] = "WT_PIST_AP",
			[Convert.ToString((uint)GetHashKey("WEAPON_PISTOL50"))] = "WT_PIST_50",
			[Convert.ToString((uint)GetHashKey("WEAPON_MICROSMG"))] = "WT_SMG_MCR",
			[Convert.ToString((uint)GetHashKey("WEAPON_SMG"))] = "WT_SMG",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTSMG"))] = "WT_SMG_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTRIFLE"))] = "WT_RIFLE_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_CARBINERIFLE"))] = "WT_RIFLE_CBN",
			[Convert.ToString((uint)GetHashKey("WEAPON_ADVANCEDRIFLE"))] = "WT_RIFLE_ADV",
			[Convert.ToString((uint)GetHashKey("WEAPON_MG"))] = "WT_MG",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATMG"))] = "WT_MG_CBT",
			[Convert.ToString((uint)GetHashKey("WEAPON_PUMPSHOTGUN"))] = "WT_SG_PMP",
			[Convert.ToString((uint)GetHashKey("WEAPON_SAWNOFfsHOTGUN"))] = "WT_SG_SOF",
			[Convert.ToString((uint)GetHashKey("WEAPON_ASSAULTSHOTGUN"))] = "WT_SG_ASL",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPSHOTGUN"))] = "WT_SG_BLP",
			[Convert.ToString((uint)GetHashKey("WEAPON_STUNGUN"))] = "WT_STUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNIPERRIFLE"))] = "WT_SNIP_RIF",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYSNIPER"))] = "WT_SNIP_HVY",
			[Convert.ToString((uint)GetHashKey("WEAPON_REMOTESNIPER"))] = "WT_SNIP_RMT",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADELAUNCHER"))] = "WT_GL",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADELAUNCHER_SMOKE"))] = "WT_GL_SMOKE",
			[Convert.ToString((uint)GetHashKey("WEAPON_RPG"))] = "WT_RPG",
			[Convert.ToString((uint)GetHashKey("WEAPON_PASSENGER_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_AIRSTRIKE_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_STINGER"))] = "WT_RPG",
			[Convert.ToString((uint)GetHashKey("WEAPON_MINIGUN"))] = "WT_MINIGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_GRENADE"))] = "WT_GNADE",
			[Convert.ToString((uint)GetHashKey("WEAPON_STICKYBOMB"))] = "WT_GNADE_STK",
			[Convert.ToString((uint)GetHashKey("WEAPON_SMOKEGRENADE"))] = "WT_GNADE_SMK",
			[Convert.ToString((uint)GetHashKey("WEAPON_BZGAS"))] = "WT_BZGAS",
			[Convert.ToString((uint)GetHashKey("WEAPON_MOLOTOV"))] = "WT_MOLOTOV",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIREEXTINGUISHER"))] = "WT_FIRE",
			[Convert.ToString((uint)GetHashKey("WEAPON_PETROLCAN"))] = "WT_PETROL",
			[Convert.ToString((uint)GetHashKey("WEAPON_DIGISCANNER"))] = "WT_DIGI",
			[Convert.ToString((uint)GetHashKey("GADGET_NIGHTVISION"))] = "WT_NV",
			[Convert.ToString((uint)GetHashKey("OBJECT"))] = "WT_OBJECT",
			[Convert.ToString((uint)GetHashKey("WEAPON_BRIEFCASE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BRIEFCASE_02"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BALL"))] = "WT_BALL",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLARE"))] = "WT_FLARE",
			[Convert.ToString((uint)GetHashKey("WEAPON_ELECTRIC_FENCE"))] = "WT_ELCFEN",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_TANK"))] = "WT_V_TANK",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_SPACE_ROCKET"))] = "WT_V_SPACERKT",
			[Convert.ToString((uint)GetHashKey("VEHICLE_WEAPON_PLAYER_LASER"))] = "WT_V_PLRLSR",
			[Convert.ToString((uint)GetHashKey("AMMO_RPG"))] = "WT_A_RPG",
			[Convert.ToString((uint)GetHashKey("AMMO_TANK"))] = "WT_A_TANK",
			[Convert.ToString((uint)GetHashKey("AMMO_SPACE_ROCKET"))] = "WT_A_SPACERKT",
			[Convert.ToString((uint)GetHashKey("AMMO_PLAYER_LASER"))] = "WT_A_PLRLSR",
			[Convert.ToString((uint)GetHashKey("AMMO_ENEMY_LASER"))] = "WT_A_ENMYLSR",
			[Convert.ToString((uint)GetHashKey("WEAPON_RAMMED_BY_CAR"))] = "WT_PIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_BOTTLE"))] = "WT_BOTTLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_GUSENBERG"))] = "WT_GUSENBERG",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNSPISTOL"))] = "WT_SNSPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_VINTAGEPISTOL"))] = "WT_VPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_DAGGER"))] = "WT_DAGGER",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLAREGUN"))] = "WT_FLAREGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYPISTOL"))] = "WT_HEAVYPSTL",
			[Convert.ToString((uint)GetHashKey("WEAPON_SPECIALCARBINE"))] = "WT_RIFLE_SCBN",
			[Convert.ToString((uint)GetHashKey("WEAPON_MUSKET"))] = "WT_MUSKET",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIREWORK"))] = "WT_FWRKLNCHR",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANRIFLE"))] = "WT_MKRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_HEAVYSHOTGUN"))] = "WT_HVYSHOT",
			[Convert.ToString((uint)GetHashKey("WEAPON_PROXMINE"))] = "WT_PRXMINE",
			[Convert.ToString((uint)GetHashKey("WEAPON_HOMINGLAUNCHER"))] = "WT_HOMLNCH",
			[Convert.ToString((uint)GetHashKey("WEAPON_HATCHET"))] = "WT_HATCHET",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMBATPDW"))] = "WT_COMBATPDW",
			[Convert.ToString((uint)GetHashKey("WEAPON_KNUCKLE"))] = "WT_KNUCKLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANPISTOL"))] = "WT_MKPISTOL",
			[Convert.ToString((uint)GetHashKey("WEAPON_MACHETE"))] = "WT_MACHETE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MACHINEPISTOL"))] = "WT_MCHPIST",
			[Convert.ToString((uint)GetHashKey("WEAPON_FLASHLIGHT"))] = "WT_FLASHLIGHT",
			[Convert.ToString((uint)GetHashKey("WEAPON_DBSHOTGUN"))] = "WT_DBSHGN",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMPACTRIFLE"))] = "WT_CMPRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_SWITCHBLADE"))] = "WT_SWBLADE",
			[Convert.ToString((uint)GetHashKey("WEAPON_REVOLVER"))] = "WT_REVOLVER",
			[Convert.ToString((uint)GetHashKey("WEAPON_FIRE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_HELI_CRASH"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_RUN_OVER_BY_CAR"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_HIT_BY_WATER_CANNON"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_EXHAUSTION"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_FALL"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_EXPLOSION"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BLEEDING"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_DROWNING_IN_VEHICLE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_DROWNING"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_BARBED_WIRE"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_VEHICLE_ROCKET"))] = "WT_INVALID",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNSPISTOL_MK2"))] = "WT_SNSPISTOL2",
			[Convert.ToString((uint)GetHashKey("WEAPON_REVOLVER_MK2"))] = "WT_REVOLVER2",
			[Convert.ToString((uint)GetHashKey("WEAPON_DOUBLEACTION"))] = "WT_REV_DA",
			[Convert.ToString((uint)GetHashKey("WEAPON_SPECIALCARBINE_MK2"))] = "WT_SPCARBINE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPRIFLE_MK2"))] = "WT_BULLRIFLE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_PUMPSHOTGUN_MK2"))] = "WT_SG_PMP2",
			[Convert.ToString((uint)GetHashKey("WEAPON_MARKSMANRIFLE_MK2"))] = "WT_MKRIFLE2",
			[Convert.ToString((uint)GetHashKey("WEAPON_POOLCUE"))] = "WT_POOLCUE",
			[Convert.ToString((uint)GetHashKey("WEAPON_WRENCH"))] = "WT_WRENCH",
			[Convert.ToString((uint)GetHashKey("WEAPON_BATTLEAXE"))] = "WT_BATTLEAXE",
			[Convert.ToString((uint)GetHashKey("WEAPON_MINISMG"))] = "WT_MINISMG",
			[Convert.ToString((uint)GetHashKey("WEAPON_BULLPUPRIFLE"))] = "WT_BULLRIFLE",
			[Convert.ToString((uint)GetHashKey("WEAPON_AUTOSHOTGUN"))] = "WT_AUTOSHGN",
			[Convert.ToString((uint)GetHashKey("WEAPON_RAILGUN"))] = "WT_RAILGUN",
			[Convert.ToString((uint)GetHashKey("WEAPON_COMPACTLAUNCHER"))] = "WT_CMPGL",
			[Convert.ToString((uint)GetHashKey("WEAPON_SNOWBALL"))] = "WT_SNWBALL",
			[Convert.ToString((uint)GetHashKey("WEAPON_PIPEBOMB"))] = "WT_PIPEBOMB",
			[Convert.ToString((uint)GetHashKey("GADGET_NIGHTVISION"))] = "WT_NV",
			[Convert.ToString((uint)GetHashKey("GADGET_PARACHUTE"))] = "WT_PARA",
			[Convert.ToString((uint)GetHashKey("WEAPON_STONE_HATCHET"))] = "WT_SHATCHET",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_FLSH"))] = "WCT_FLASH",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_SUPP_02"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_PI_SUPP"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_APPISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_AR_SUPP_02"))] = "WCT_SUPP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PISTOL50_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNSPISTOL_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_HEAVYPISTOL_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_VINTAGEPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_VINTAGEPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MACRO"))] = "WCT_SCOPE_MAC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MICROSMG_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MACRO_02"))] = "WCT_SCOPE_MAC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SMG_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MINISMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MINISMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MACHINEPISTOL_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATPDW_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_AR_AFGRIP"))] = "WCT_GRIP",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_SMALL"))] = "WCT_SCOPE_SML",
			[Convert.ToString((uint)GetHashKey("COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SAWNOFfsHOTGUN_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSHOTGUN_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTSHOTGUN_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ASSAULTRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MEDIUM"))] = "WCT_SCOPE_MED",
			[Convert.ToString((uint)GetHashKey("COMPONENT_CARBINERIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_BULLPUPRIFLE_VARMOD_LOW"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMPACTRIFLE_CLIP_03"))] = "WCT_CLIP_DRM",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_COMBATMG_VARMOD_LOWRIDER"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_GUSENBERG_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_GUSENBERG_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_LARGE"))] = "WCT_SCOPE_LRG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_MAX"))] = "WCT_SCOPE_MAX",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SNIPERRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_CLIP_01"))] = "WCT_CLIP1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_CLIP_02"))] = "WCT_CLIP2",
			[Convert.ToString((uint)GetHashKey("COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM"))] = "WCT_SCOPE_LRG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_MARKSMANRIFLE_VARMOD_LUXE"))] = "WCT_VAR_GOLD",
			[Convert.ToString((uint)GetHashKey("WM_TINT0"))] = "WM_TINT0",
			[Convert.ToString((uint)GetHashKey("WM_TINT1"))] = "WM_TINT1",
			[Convert.ToString((uint)GetHashKey("WM_TINT2"))] = "WM_TINT2",
			[Convert.ToString((uint)GetHashKey("WM_TINT3"))] = "WM_TINT3",
			[Convert.ToString((uint)GetHashKey("WM_TINT4"))] = "WM_TINT4",
			[Convert.ToString((uint)GetHashKey("WM_TINT5"))] = "WM_TINT5",
			[Convert.ToString((uint)GetHashKey("WM_TINT6"))] = "WM_TINT6",
			[Convert.ToString((uint)GetHashKey("WM_TINT7"))] = "WM_TINT7",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_BASE"))] = "WCT_KNUCK_01",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_PIMP"))] = "WCT_KNUCK_02",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_BALLAS"))] = "WCT_KNUCK_BG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_DOLLAR"))] = "WCT_KNUCK_DLR",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_DIAMOND"))] = "WCT_KNUCK_DMD",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_HATE"))] = "WCT_KNUCK_HT",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_LOVE"))] = "WCD_VAR_DESC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_PLAYER"))] = "WCT_KNUCK_PC",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_KING"))] = "WCT_KNUCK_SLG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_KNUCKLE_VARMOD_VAGOS"))] = "WCT_KNUCK_VG",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_BASE"))] = "WCT_SB_BASE",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_VAR1"))] = "WCT_SB_VAR1",
			[Convert.ToString((uint)GetHashKey("COMPONENT_SWITCHBLADE_VARMOD_VAR2"))] = "WCT_SB_VAR2"
		};

		public static string GetWeaponLabel(uint hash)
		{
			if (HASH_TO_LABEL.ContainsKey(hash.ToString()))
			{
				string label = HASH_TO_LABEL[hash.ToString()];
				if (label != null)
				{
					return label;
				}
			}
			else
			{
				Log.Printa(LogType.Error, "Errore nell'hash /" + hash.ToString() + "/ per arma/componente. forse non è mai stato aggiunto?");
			}

			return "WT_INVALID";
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
	}

	public class VehicleList : IEnumerable<int>
	{
		public IEnumerator<int> GetEnumerator()
		{
			OutputArgument OutArgEntity = new OutputArgument();
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_FIRST_VEHICLE")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_NEXT_VEHICLE")), handle, OutArgEntity)) 
				yield return OutArgEntity.GetResult<int>();
			Function.Call((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("END_FIND_VEHICLE")), handle);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class PedList : IEnumerable<int>
	{
		public IEnumerator<int> GetEnumerator()
		{
			OutputArgument OutArgEntity = new OutputArgument();
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_FIRST_PED")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_NEXT_PED")), handle, OutArgEntity))
				yield return OutArgEntity.GetResult<int>();
			Function.Call((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("END_FIND_PED")), handle);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class ObjectList : IEnumerable<int>
	{
		public IEnumerator<int> GetEnumerator()
		{
			OutputArgument OutArgEntity = new OutputArgument();
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_FIRST_OBJECT")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("FIND_NEXT_OBJECT")), handle, OutArgEntity))
				yield return OutArgEntity.GetResult<int>();
			Function.Call((CitizenFX.Core.Native.Hash)((uint)Game.GenerateHash("END_FIND_OBJECT")), handle);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}


	static class WorldProbe
	{
		// Intended for:
		// Getting closest ped, closest vehicle, vehicle in front (raycast), raycast normal, raycast collision coordinates
		static _RaycastResult? CrosshairRaycastThisTick = null;

		static WorldProbe()
		{
			Client.Instance.AddTick(new Func<Task>(() => { CrosshairRaycastThisTick = null; return Task.FromResult(0); }));
		}

		public static CitizenFX.Core.Vehicle GetVehicleInFrontOfPlayer(float distance = 5f)
		{
			try
			{
				Entity source = Game.PlayerPed.IsInVehicle() ? (Entity)Game.PlayerPed.CurrentVehicle : Game.PlayerPed;
				return GetVehicleInFrontOfPlayer(source, source, distance);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}
			return default(CitizenFX.Core.Vehicle);
		}

		public static CitizenFX.Core.Vehicle GetVehicleInFrontOfPlayer(Entity source, Entity ignore, float distance = 5f)
		{
			try
			{
				RaycastResult raycast = World.Raycast(source.Position + new Vector3(0f, 0f, -0.4f), source.GetOffsetPosition(new Vector3(0f, distance, 0f)) + new Vector3(0f, 0f, -0.4f), (IntersectOptions)71, ignore);
				if (raycast.DitHitEntity && raycast.HitEntity.Model.IsVehicle)
				{
					return (CitizenFX.Core.Vehicle)raycast.HitEntity;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"[WORLDPROBE] GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}
			return default(CitizenFX.Core.Vehicle);
		}

		public static Vector3 CalculateClosestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
		{
			try
			{
				float dotProduct = Vector3.Dot(start - end, point - start);
				float percent = dotProduct / (start - end).LengthSquared();
				if (percent < 0.0f)
				{
					return start;
				}
				else if (percent > 1.0f)
				{
					return end;
				}
				else
				{
					return (start + (percent * (end - start)));
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe CalculateClosestPointOnLine Error: {ex.Message}");
			}
			return default(Vector3);
		}

		public static Vector3 GameplayCamForwardVector()
		{
			try
			{
				Vector3 rotation = (float)(Math.PI / 180.0) * GetGameplayCamRot(2);
				return Vector3.Normalize(new Vector3((float)-Math.Sin(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Cos(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Sin(rotation.X)));
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}
			return default(Vector3);
		}


		public static RaycastResult _CrosshairRaycast()
		{
			try
			{
				return World.Raycast(Game.PlayerPed.Position, Game.PlayerPed.Position + 1000 * GameplayCamForwardVector(), IntersectOptions.Everything, Game.PlayerPed);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
			}
			return default(RaycastResult);
		}

		public struct _RaycastResult
		{
			public Entity HitEntity { get; }
			public Vector3 HitPosition { get; }
			public Vector3 SurfaceNormal { get; }
			public bool DitHit { get; }
			public bool DitHitEntity { get; }
			public int Result { get; }

			public _RaycastResult(bool DitHit, Vector3 HitPosition, Vector3 SurfaceNormal, int entityHandle, int Result)
			{
				this.DitHit = DitHit;
				this.HitPosition = HitPosition;
				this.SurfaceNormal = SurfaceNormal;
				if (entityHandle == 0)
				{
					this.HitEntity = null;
					this.DitHitEntity = false;
				}
				else
				{
					this.HitEntity = Entity.FromHandle(entityHandle);
					this.DitHitEntity = true;
				}
				this.Result = Result;
			}
		}


		public static _RaycastResult CrosshairRaycast(float distance = 1000f)
		{
			return CrosshairRaycast(Game.PlayerPed);
		}

		/// <summary>
		/// Because HitPosition and SurfaceNormal are currently broken in platform function
		/// </summary>
		/// <returns></returns>
		public static _RaycastResult CrosshairRaycast(Entity ignore, float distance = 1000f)
		{
			try
			{
				// Uncomment these to potentially save on raycasts (don't think they're ridiculously costly, but there's a # limit per tick)
				//if(CrosshairRaycastThisTick != null && distance == 1000f) return (_RaycastResult) CrosshairRaycastThisTick;

				Vector3 start = GameplayCamera.Position;
				Vector3 end = GameplayCamera.Position + distance * WorldProbe.GameplayCamForwardVector();
				int raycastHandle = Function.Call<int>(Hash._START_SHAPE_TEST_RAY, start.X, start.Y, start.Z, end.X, end.Y, end.Z, IntersectOptions.Everything, ignore.Handle, 0);
				OutputArgument DitHit = new OutputArgument();
				OutputArgument HitPosition = new OutputArgument();
				OutputArgument SurfaceNormal = new OutputArgument();
				OutputArgument HitEntity = new OutputArgument();
				Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, raycastHandle, DitHit, HitPosition, SurfaceNormal, HitEntity);

				var result = new _RaycastResult(DitHit.GetResult<bool>(), HitPosition.GetResult<Vector3>(), SurfaceNormal.GetResult<Vector3>(), HitEntity.GetResult<int>(), raycastHandle);

				//if(distance == 1000f) CrosshairRaycastThisTick = result;
				return result;
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe CrosshairRaycast Error: {ex.Message}");
			}
			return default(_RaycastResult);
		}

		// Apparently there's a built-in native for this...
		// TODO: Replace with that
		public static string GetEntityType(int entityHandle)
		{
			try
			{
				if (Function.Call<bool>(Hash.IS_ENTITY_A_PED, entityHandle))
				{
					return "PED";
				}
				else if (Function.Call<bool>(Hash.IS_ENTITY_A_VEHICLE, entityHandle))
				{
					return "VEH";
				}
				else if (Function.Call<bool>(Hash.IS_ENTITY_AN_OBJECT, entityHandle))
				{
					return "OBJ";
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GetEntityType Error: {ex.Message}");
			}
			return "UNK";
		}


		public static async Task<float?> FindGroundZ(Vector2 position)
		{
			float? result = null;
			try
			{
				float[] groundCheckHeight = new float[] { 100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f, 450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f };

				foreach (float h in groundCheckHeight)
				{
					//await BaseScript.Delay(0);
					OutputArgument z = new OutputArgument();
					if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, (float)h, z, false))
					{
						result = z.GetResult<float>();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe FindGroundZ Error: {ex.Message}");
			}
			return result;
		}
	}
}