using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Logger;

namespace TheLastPlanet.Client.Core.Utility
{
	internal static class WorldProbe
	{
		static WorldProbe()
		{
			Log.Printa(LogType.Debug, "WorldProbe attivo");
			//Client.Instance.AddTick(new Func<Task>(() => { CrosshairRaycastThisTick = null; return Task.FromResult(0); }));
		}

		public static Vehicle GetVehicleInFrontOfPlayer(float distance = 5f)
		{
			try
			{
				Entity source = CachePlayer.Cache.MyPlayer.Character.StatiPlayer.InVeicolo ? (Entity)CachePlayer.Cache.MyPlayer.Ped.CurrentVehicle : CachePlayer.Cache.MyPlayer.Ped;

				return GetVehicleInFrontOfPlayer(source, source, distance);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"[WorldProbe] GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}

			return default;
		}

		public static Vehicle GetVehicleInFrontOfPlayer(Entity source, Entity ignore, float distance = 5f)
		{
			try
			{
				RaycastResult raycast = World.Raycast(source.Position + new Vector3(0f, 0f, -0.4f), source.GetOffsetPosition(new Vector3(0f, distance, 0f)) + new Vector3(0f, 0f, -0.4f), (IntersectOptions)71, ignore);

				if (raycast.DitHitEntity && raycast.HitEntity.Model.IsVehicle) return (Vehicle)raycast.HitEntity;
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"[WorldProbe] GetVehicleInFrontOfPlayer Error: {ex.Message}");
			}

			return default;
		}

		public static Vector3 CalculateClosestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
		{
			try
			{
				float dotProduct = Vector3.Dot(start - end, point - start);
				float percent = dotProduct / (start - end).LengthSquared();

				if (percent < 0.0f)
					return start;
				if (percent > 1.0f) return end;

				return start + percent * (end - start);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe CalculateClosestPointOnLine Error: {ex.Message}");
			}

			return default;
		}

		public static Vector3 GameplayCamForwardVector()
		{
			try
			{
				Vector3 rotation = (float)(Math.PI / 180.0) * GameplayCamera.Rotation;

				return Vector3.Normalize(new Vector3((float)-Math.Sin(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Cos(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Sin(rotation.X)));
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}

			return default;
		}

		public static Vector3 CamForwardVector(this Camera cam)
		{
			try
			{
				Vector3 rotation = (float)(Math.PI / 180.0) * cam.Rotation;

				return Vector3.Normalize(new Vector3((float)-Math.Sin(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Cos(rotation.Z) * (float)Math.Abs(Math.Cos(rotation.X)), (float)Math.Sin(rotation.X)));
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe GameplayCamForwardVector Error: {ex.Message}");
			}

			return default;
		}

		public static async Task<AsyncRaycastResult> GamePlayCamCrosshairRaycast(float distance = 1000, Entity ignoredEntity = null)
		{
			try
			{
				Vector3 position = GameplayCamera.Position;
				Vector3 direction = position + distance * GameplayCamForwardVector();

				return await Raycast(position, direction, distance, IntersectOptions.Everything, ignoredEntity);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
			}

			return default;
		}

		public static async Task<AsyncRaycastResult> CrosshairRaycast(this Camera cam, float distance = 1000, Entity ignoredEntity = null)
		{
			try
			{
				Vector3 position = cam.CamForwardVector();
				Vector3 direction = position + distance * GameplayCamForwardVector();

				return await Raycast(position, direction, distance, IntersectOptions.Everything, ignoredEntity);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
			}

			return default;
		}

		public static async Task<AsyncRaycastResult> CrosshairRaycast(this Camera cam, IntersectOptions options = IntersectOptions.Everything, float distance = 1000, Entity ignoredEntity = null)
		{
			try
			{
				Vector3 position = cam.CamForwardVector();
				Vector3 direction = position + distance * GameplayCamForwardVector();

				return await Raycast(position, direction, distance, options, ignoredEntity);
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"WorldProbe _CrosshairRaycast Error: {ex.Message}");
			}

			return default;
		}

		public static async Task<Entity> GetEntityInFrontOfPed(this Ped ped, float maxDistance = 5.0f)
		{
			try
			{
				AsyncRaycastResult raycast = await Raycast(ped.Position, ped.ForwardVector * maxDistance, IntersectOptions.Everything);

				if (raycast.DitHitEntity) return (Entity)raycast.HitEntity;
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"{ex.Message}");
			}

			return null;
		}

		public static async Task<Entity> GetEntityInFrontOfPlayer(this Player player, float distance = 5f)
		{
			try
			{
				AsyncRaycastResult raycast = await Raycast(player.Character.Position, player.Character.ForwardVector * distance, IntersectOptions.Everything);

				if (raycast.DitHitEntity) return (Entity)raycast.HitEntity;
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Error, $"{ex.Message}");
			}

			return null;
		}

		public static async Task<AsyncRaycastResult> Raycast(Vector3 source, Vector3 target, IntersectOptions options, Entity ignoredEntity = null)
		{
			int RayShape = StartShapeTestLosProbe(source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoredEntity == null ? 0 : ignoredEntity.Handle, 7);
			Vector3 hitPositionArg = new Vector3();
			bool hitSomethingArg = false;
			int entityHandleArg = 0;
			Vector3 surfaceNormalArg = new Vector3();
			int result = 0;

			while (result != 2)
			{
				await BaseScript.Delay(0);
				result = GetShapeTestResult(RayShape, ref hitSomethingArg, ref hitPositionArg, ref surfaceNormalArg, ref entityHandleArg);
			}

			return new AsyncRaycastResult(hitSomethingArg, hitPositionArg, surfaceNormalArg, entityHandleArg, result);
		}

		public static async Task<AsyncRaycastResult> Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options, Entity ignoredEntity = null)
		{
			Vector3 target = source + direction * maxDistance;
			int RayShape = StartShapeTestLosProbe(source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoredEntity == null ? 0 : ignoredEntity.Handle, 7);
			Vector3 hitPositionArg = new Vector3();
			bool hitSomethingArg = false;
			int entityHandleArg = 0;
			Vector3 surfaceNormalArg = new Vector3();
			int result = 0;

			while (result != 2)
			{
				await BaseScript.Delay(0);
				result = GetShapeTestResult(RayShape, ref hitSomethingArg, ref hitPositionArg, ref surfaceNormalArg, ref entityHandleArg);
			}

			return new AsyncRaycastResult(hitSomethingArg, hitPositionArg, surfaceNormalArg, entityHandleArg, result);
		}
	}

	public struct AsyncRaycastResult
	{
		public Entity HitEntity { get; }
		public Vector3 HitPosition { get; }
		public Vector3 SurfaceNormal { get; }
		public bool DitHit { get; }
		public bool DitHitEntity { get; }
		public int Result { get; }

		public AsyncRaycastResult(bool ditHit, Vector3 hitPosition, Vector3 surfaceNormal, int entityHandle, int result) : this()
		{
			DitHit = ditHit;
			HitPosition = hitPosition;
			SurfaceNormal = surfaceNormal;

			if (entityHandle == 0)
			{
				HitEntity = null;
				DitHitEntity = false;
			}
			else
			{
				HitEntity = Entity.FromHandle(entityHandle);
				DitHitEntity = true;
			}

			Result = result;
		}
	}
}