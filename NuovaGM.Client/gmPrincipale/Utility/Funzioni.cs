using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static NuovaGM.Shared.Veicoli.Modifiche;

namespace NuovaGM.Client.gmPrincipale.Utility
{
	static class Funzioni
	{
		public static PlayerChar GetPlayerCharFromPlayerId(int id)
		{
			foreach (KeyValuePair<string, PlayerChar> p in Eventi.GiocatoriOnline)
			{
				if (GetPlayerFromServerId(Convert.ToInt32(p.Key)) == id)
					return p.Value;
			}
			return null;
		}

		public static void SendNuiMessage(object message)
		{
			API.SendNuiMessage(JsonConvert.SerializeObject(message));
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

		public static void ConcealPlayersNearby(Vector3 coord, float radius)
		{
			var players = GetPlayersInArea(coord, radius);
			foreach (var pl in players)
				if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle)
					NetworkConcealPlayer(pl.Handle, true, true);
		}

		public static void ConcealAllPlayers()
		{
			Client.GetInstance.GetPlayers.ToList().ForEach(pl => { if (!NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle) { NetworkConcealPlayer(pl.Handle, true, true); } });
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
			Client.GetInstance.GetPlayers.ToList().ForEach(pl => { if (NetworkIsPlayerConcealed(pl.Handle) && pl.Handle != Game.Player.Handle) { NetworkConcealPlayer(pl.Handle, false, false); } });
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
			while (!IsPedheadshotReady(mugshot))
				await BaseScript.Delay(1);
			string Txd = GetPedheadshotTxdString(mugshot);
			return new Tuple<int, string>(mugshot, Txd);
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
				Client.Printa(LogType.Error, $"{ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe GetVehicleInFrontOfPlayer Error: {ex.Message}");
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


		public static VehProp GetVehicleProperties(Vehicle veh)
		{
			int vehicle = veh.Handle;
			int Color1 = 0;
			int Color2 = 0;
			int perl = 0;
			int wheel = 0;
			int neonColorR = 0;
			int neonColorG = 0;
			int neonColorB = 0;
			int tyreSmokeColorR = 0;
			int tyreSmokeColorG = 0;
			int tyreSmokeColorB = 0;
			GetVehicleColours(vehicle, ref Color1, ref Color2);
			GetVehicleExtraColours(vehicle, ref perl, ref wheel);
			GetVehicleNeonLightsColour(vehicle, ref neonColorR, ref neonColorG, ref neonColorB);
			GetVehicleTyreSmokeColor(vehicle, ref tyreSmokeColorR, ref tyreSmokeColorG, ref tyreSmokeColorB);
			List<bool> neon = new List<bool>{
				IsVehicleNeonLightEnabled(vehicle, 0),
				IsVehicleNeonLightEnabled(vehicle, 1),
				IsVehicleNeonLightEnabled(vehicle, 2),
				IsVehicleNeonLightEnabled(vehicle, 3)
			};
			List<bool> extras = new List<bool>();
			for (int id = 0; id < 13; id++)
			{
				extras.Add(IsVehicleExtraTurnedOn(vehicle, id));
			}

			VehProp vehi = new VehProp(
			veh.Model,
			GetVehicleNumberPlateText(vehicle),
			GetVehicleNumberPlateTextIndex(vehicle),
			GetVehicleBodyHealth(vehicle),
			GetVehicleEngineHealth(vehicle),
			GetVehicleDirtLevel(vehicle),

			Color1, Color2,
			perl, wheel,

			GetVehicleWheelType(vehicle),
			GetVehicleWindowTint(vehicle),

			neon,
			extras,
			neonColorR, neonColorG, neonColorB,
			tyreSmokeColorR, tyreSmokeColorG, tyreSmokeColorB,

			GetVehicleMod(vehicle, 0),
			GetVehicleMod(vehicle, 1),
			GetVehicleMod(vehicle, 2),
			GetVehicleMod(vehicle, 3),
			GetVehicleMod(vehicle, 4),
			GetVehicleMod(vehicle, 5),
			GetVehicleMod(vehicle, 6),
			GetVehicleMod(vehicle, 7),
			GetVehicleMod(vehicle, 8),
			GetVehicleMod(vehicle, 9),
			GetVehicleMod(vehicle, 10),
			GetVehicleMod(vehicle, 11),
			GetVehicleMod(vehicle, 12),
			GetVehicleMod(vehicle, 13),
			GetVehicleMod(vehicle, 14),
			GetVehicleMod(vehicle, 15),
			GetVehicleMod(vehicle, 16),
			IsToggleModOn(vehicle, 18),
			IsToggleModOn(vehicle, 20),
			IsToggleModOn(vehicle, 22),
			GetVehicleMod(vehicle, 23),
			GetVehicleMod(vehicle, 24),
			GetVehicleMod(vehicle, 25),
			GetVehicleMod(vehicle, 26),
			GetVehicleMod(vehicle, 27),
			GetVehicleMod(vehicle, 28),
			GetVehicleMod(vehicle, 29),
			GetVehicleMod(vehicle, 30),
			GetVehicleMod(vehicle, 31),
			GetVehicleMod(vehicle, 32),
			GetVehicleMod(vehicle, 33),
			GetVehicleMod(vehicle, 34),
			GetVehicleMod(vehicle, 35),
			GetVehicleMod(vehicle, 36),
			GetVehicleMod(vehicle, 37),
			GetVehicleMod(vehicle, 38),
			GetVehicleMod(vehicle, 39),
			GetVehicleMod(vehicle, 40),
			GetVehicleMod(vehicle, 41),
			GetVehicleMod(vehicle, 42),
			GetVehicleMod(vehicle, 43),
			GetVehicleMod(vehicle, 44),
			GetVehicleMod(vehicle, 45),
			GetVehicleMod(vehicle, 46),
			GetVehicleLivery(vehicle)
			);
			return vehi;
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
				Client.Printa(LogType.Error, $"[WORLDPROBE] GetVehicleInFrontOfPlayer Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe CalculateClosestPointOnLine Error: {ex.Message}");
			}
			return default(Vector3);
		}

		public static Vector3 GameplayCamForwardVector()
		{
			try
			{
				Vector3 rotation = (float)(Math.PI / 180.0) * Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 2);
				return Vector3.Normalize(new Vector3((float)-Math.Sin(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Cos(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Sin(rotation.X)));
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}
			return default(Vector3);
		}


		public static RaycastResult _CrosshairRaycast()
		{
			try
			{
				return World.Raycast(CitizenFX.Core.Game.PlayerPed.Position, CitizenFX.Core.Game.PlayerPed.Position + 1000 * GameplayCamForwardVector(), IntersectOptions.Everything, CitizenFX.Core.Game.PlayerPed);
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
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
			return CrosshairRaycast(CitizenFX.Core.Game.PlayerPed);
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

				Vector3 start = CitizenFX.Core.GameplayCamera.Position;
				Vector3 end = CitizenFX.Core.GameplayCamera.Position + distance * GameplayCamForwardVector();
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
				Client.Printa(LogType.Error, $"WorldProbe CrosshairRaycast Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe GetEntityType Error: {ex.Message}");
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
					//await BaseScript.BaseScript.Delay(0);
					OutputArgument z = new OutputArgument();
					if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, (float)h, z, false))
					{
						result = z.GetResult<float>();
					}
				}
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Error, $"WorldProbe FindGroundZ Error: {ex.Message}");
			}
			await Task.FromResult(0);
			return result;
		}


		public static async void Teleport(int entity, Vector3 coords)
		{
			RequestCollisionAtCoord(coords.X, coords.Y, coords.Z);
			SetEntityCoords(entity, coords.X, coords.Y, coords.Z, false, false, false, false);
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
					PreviouslyOwnedByPlayer = true
				};
				vehicle.PlaceOnGround();
				Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
				EntityDecoration.SetDecor(vehicle, Main.decorName, Main.decorInt);
				vehicle.MarkAsNoLongerNeeded();
				return Game.PlayerPed.CurrentVehicle;
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
				vehicle.PlaceOnGround();
				EntityDecoration.SetDecor(vehicle, Main.decorName, Main.decorInt);
				//SetVehicleEngineOn(vehicle.Handle, false, true, true);
				return vehicle;
			}
			else
				return null;
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
				vehicle.PlaceOnGround();
				EntityDecoration.SetDecor(vehicle, Main.decorName, Main.decorInt);
				vehicle.MarkAsNoLongerNeeded();
				return vehicle;
			}
			else
			{
				BaseScript.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO car] = ", "nome modello non corretto!" }, color = new[] { 255, 0, 0 } });
				return null;
			}
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
		public static List<Player> GetPlayersInArea(Vector3 coords, float area)
		{
			List<Player> PlayersInArea = Client.GetInstance.GetPlayers.ToList().FindAll(p => p.Character.Position.DistanceToSquared(coords) <= area && p != Game.Player);
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
			foreach (Player p in Client.GetInstance.GetPlayers.ToList())
			{
				Ped target = p.Character;
				if (World.GetDistance(target.Position, coords) <= area)
				{
					PlayersPedsInArea.Add(target);
				}
			}
			return PlayersPedsInArea;
		}

		public static Tuple<Vehicle, float> GetClosestVehicle(Vector3 Coords)
		{
			Vehicle[] vehs = World.GetAllVehicles();
			float closestDistance = -1f;
			Vehicle closestVehicle = null;
			foreach (Vehicle v in vehs)
			{
				Vector3 vehcoords = v.Position;
				float distance = World.GetDistance(vehcoords, Coords);
				if (closestDistance == -1 || closestDistance > distance)
				{
					closestVehicle = v;
					closestDistance = distance;
				}
			}
			return new Tuple<Vehicle, float>(closestVehicle, closestDistance);
		}

		public static float Rad2deg(float rad)
		{
			return rad * (180.0f / (float)Math.PI);
		}

		public static float Deg2rad(float deg)
		{
			return deg * ((float)Math.PI / 180.0f);
		}

		public static Tuple<Ped, float> GetClosestPed()
		{
			Ped[] Peds = World.GetAllPeds();
			float closestDistance = -1f;
			Ped closestPed = null;
			foreach (Ped v in Peds)
			{
				Vector3 pedcoords = v.Position;
				float distance = World.GetDistance(pedcoords, Game.PlayerPed.Position);
				if (closestDistance == -1 || closestDistance > distance)
				{
					closestPed = v;
					closestDistance = distance;
				}
			}
			return new Tuple<Ped, float>(closestPed, closestDistance);
		}

		public static Tuple<Ped, float> GetClosestPed(Vector3 Coords)
		{
			Ped[] Peds = World.GetAllPeds();
			float closestDistance = -1f;
			Ped closestPed = null;
			foreach (Ped v in Peds)
			{
				Vector3 pedcoords = v.Position;
				float distance = World.GetDistance(pedcoords, Coords);
				if (closestDistance == -1 || closestDistance > distance)
				{
					closestPed = v;
					closestDistance = distance;
				}
			}
			return new Tuple<Ped, float>(closestPed, closestDistance);
		}

		public static Player GetPlayerFromPed(Ped ped)
		{
			foreach (Player pl in Client.GetInstance.GetPlayers.ToList())
				if (pl.Character.Handle == ped.Handle)
					return pl;
			return null;
		}

		/// <summary>
		/// Controlla distanza dal Ped del giocatore a tutti i players e ritorna il piu vicino e la sua distanza
		/// </summary>
		public static Tuple<Player, float> GetClosestPlayer()
		{
			float closestDistance = -1;
			Player closestPlayer = null;
			Vector3 Coords = Game.PlayerPed.Position;

			foreach (Player p in Client.GetInstance.GetPlayers.ToList())
			{
				Ped target = p.Character;
				if (p != Game.Player)
				{
					Vector3 targetCoords = target.Position;
					float distance = World.GetDistance(Coords, targetCoords);

					if (closestDistance == -1 || closestDistance > distance)
					{
						closestPlayer = p;
						closestDistance = distance;
					}
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
		/// Controlla la distanza tra le coordinate inserite e tutti i Players e ritorna il Player piu vicino a quelle coordinate
		/// </summary>
		/// <param name="coords"></param>
		public static Tuple<Player, float> GetClosestPlayer(Vector3 coords)
		{
			float closestDistance = -1;
			Player closestPlayer = null;

			foreach (Player p in Client.GetInstance.GetPlayers.ToList())
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
		/// Si connette al server e ritorna tutti i personaggi online e i loro dati
		/// </summary>
		/// <returns></returns>
		public static Task<Dictionary<string, PlayerChar>> GetOnlinePlayersAndTheirData()
		{
			var personaggi = new TaskCompletionSource<Dictionary<string, PlayerChar>>();
			BaseScript.TriggerServerEvent("lprp:getPlayers", new Action<object>((arg) =>
			{
				personaggi.SetResult(JsonConvert.DeserializeObject<Dictionary<string, PlayerChar>>(arg as string));
			}));
			return personaggi.Task;
		}

		/// <summary>
		/// Si connette al server e al DB e ritorna tutti i personaggi salvati nel db stesso
		/// </summary>
		/// <returns></returns>
		public static async Task<Dictionary<string, PlayerChar>> GetAllPlayersAndTheirData()
		{
			Dictionary<string, PlayerChar> personaggi = new Dictionary<string, PlayerChar>();

			BaseScript.TriggerServerEvent("lprp:getDBPlayers", new Action<object>((arg) =>
			{
				dynamic parsedPlayers = JsonConvert.DeserializeObject(arg as string);
				for (int i = 0; i < parsedPlayers.Count; i++)
				{
					if (parsedPlayers[i]["char_data"].Value != "{}")
					{
						personaggi.Add((string)parsedPlayers[i]["Name"].Value, new PlayerChar(parsedPlayers[i] as JObject));
					}
				}
			}));

			while (JsonConvert.SerializeObject(personaggi) == "{}") await BaseScript.Delay(0);
			return personaggi;
		}

		public static Vehicle[] GetVehiclesInArea(Vector3 Coords, float Radius)
		{
			Vehicle[] vehs = World.GetAllVehicles();
			List<Vehicle> vehiclesInArea = new List<Vehicle>();
			foreach (Vehicle v in vehs)
			{
				Vector3 vehcoords = v.Position;
				float distance = World.GetDistance(vehcoords, Coords);
				if (distance <= Radius)
					vehiclesInArea.Add(v);
			}
			return vehiclesInArea.ToArray();
		}

		public static Vector2 WorldToScreen(Vector3 position)
		{
			var screenX = new OutputArgument();
			var screenY = new OutputArgument();
			return !Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, screenX, screenY) ?
				Vector2.Zero :
				new Vector2(screenX.GetResult<float>(), screenY.GetResult<float>());
		}

		public static bool IsSpawnPointClear(Vector3 Coords, float Radius)
		{
			Vehicle[] vehs = GetVehiclesInArea(Coords, Radius);
			return vehs.Length < 1 ? true : false;
		}

		public static void StartScenario(Ped ped, string scenario)
		{
			TaskStartScenarioInPlace(ped.Handle, scenario, 0, true);
		}

		public static int GetRandomInt(int end)
		{
			return GetRandomIntInRange(0, end);
		}

		public static int GetRandomInt(int start, int end)
		{
			return GetRandomIntInRange(start, end);
		}

		public static long GetRandomLong(long start, long end)
		{
			return new Random(GetGameTimer()).NextLong(start, end);
		}

		public static long GetRandomLong(long end)
		{
			return new Random(GetGameTimer()).NextLong(0, end);
		}

		public static float GetRandomFloat(float end)
		{
			return (float)Math.Round(GetRandomFloatInRange(0, end), 2);
		}

		public static float GetRandomFloat(float start, float end)
		{
			return (float)Math.Round(GetRandomFloatInRange(start, end), 2);
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
				Client.Printa(LogType.Error, "Errore nell'hash /" + hash.ToString() + "/ per arma/componente. forse non è mai stato aggiunto?");
			}

			return "WT_INVALID";
		}
	}

	public class VehicleList : IEnumerable<int>
	{
		public IEnumerator<int> GetEnumerator()
		{
			OutputArgument OutArgEntity = new OutputArgument();
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_FIRST_VEHICLE")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_NEXT_VEHICLE")), handle, OutArgEntity))
			{
				yield return OutArgEntity.GetResult<int>();
			}
			Function.Call((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("END_FIND_VEHICLE")), handle);
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
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_FIRST_PED")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_NEXT_PED")), handle, OutArgEntity))
			{
				yield return OutArgEntity.GetResult<int>();
			}
			Function.Call((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("END_FIND_PED")), handle);
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
			int handle = Function.Call<int>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_FIRST_OBJECT")), OutArgEntity);
			yield return OutArgEntity.GetResult<int>();
			while (Function.Call<bool>((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("FIND_NEXT_OBJECT")), handle, OutArgEntity))
			{
				yield return OutArgEntity.GetResult<int>();
			}
			Function.Call((CitizenFX.Core.Native.Hash)((uint)CitizenFX.Core.Game.GenerateHash("END_FIND_OBJECT")), handle);
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
			Client.GetInstance.RegisterTickHandler(new Func<Task>(() => { CrosshairRaycastThisTick = null; return Task.FromResult(0); }));
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
				Client.Printa(LogType.Error, $"WorldProbe GetVehicleInFrontOfPlayer Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"[WORLDPROBE] GetVehicleInFrontOfPlayer Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe CalculateClosestPointOnLine Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}
			return default(Vector3);
		}


		public static RaycastResult _CrosshairRaycast()
		{
			try
			{
				return World.Raycast(CitizenFX.Core.Game.PlayerPed.Position, CitizenFX.Core.Game.PlayerPed.Position + 1000 * GameplayCamForwardVector(), IntersectOptions.Everything, CitizenFX.Core.Game.PlayerPed);
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
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
			return CrosshairRaycast(CitizenFX.Core.Game.PlayerPed);
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

				Vector3 start = CitizenFX.Core.GameplayCamera.Position;
				Vector3 end = CitizenFX.Core.GameplayCamera.Position + distance * WorldProbe.GameplayCamForwardVector();
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
				Client.Printa(LogType.Error, $"WorldProbe CrosshairRaycast Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe GetEntityType Error: {ex.Message}");
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
				Client.Printa(LogType.Error, $"WorldProbe FindGroundZ Error: {ex.Message}");
			}
			return result;
		}
	}

	public static class RandomExtensionMethods
	{
		/// <summary>
		/// Returns a random long from min (inclusive) to max (exclusive)
		/// </summary>
		/// <param name="random">The given random instance</param>
		/// <param name="min">The inclusive minimum bound</param>
		/// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
		public static long NextLong(this Random random, long min, long max)
		{
			if (max <= min)
			{
				throw new ArgumentOutOfRangeException("max", "max must be > min!");
			}

			//Working with ulong so that modulo works correctly with values > long.MaxValue
			ulong uRange = (ulong)(max - min);

			//Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
			//for more information.
			//In the worst case, the expected number of calls is 2 (though usually it's
			//much closer to 1) so this loop doesn't really hurt performance at all.
			ulong ulongRand;
			do
			{
				byte[] buf = new byte[8];
				random.NextBytes(buf);
				ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
			} while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return (long)(ulongRand % uRange) + min;
		}

		/// <summary>
		/// Returns a random long from 0 (inclusive) to max (exclusive)
		/// </summary>
		/// <param name="random">The given random instance</param>
		/// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
		public static long NextLong(this Random random, long max)
		{
			return random.NextLong(0, max);
		}

		/// <summary>
		/// Returns a random long over all possible values of long (except long.MaxValue, similar to
		/// random.Next())
		/// </summary>
		/// <param name="random">The given random instance</param>
		public static long NextLong(this Random random)
		{
			return random.NextLong(long.MinValue, long.MaxValue);
		}
	}
}