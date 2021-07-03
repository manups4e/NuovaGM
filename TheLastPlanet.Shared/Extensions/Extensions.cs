using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

#if CLIENT
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using static CitizenFX.Core.Native.API;
#endif
namespace TheLastPlanet.Shared
{
	public static class RandomExtensionSuperMethod
	{

		private static Logger.Log Logger = new Log();
		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to minValue, and less than maxValue.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A double-precision floating point number that is greater than or equal to minValue, and less than maxValue.</returns>
		public static double NextDouble(this Random rand, double minValue, double maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (maxValue - minValue) * rand.NextDouble() + minValue;
		}

		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to 0.0f, and less than 1.0f.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A single-precision floating point number that is greater than or equal to 0.0f, and less than 1.0f.</returns>
		public static float NextFloat(this Random rand)
		{
			if (rand is null)
				rand = new Random();
			return (float)rand.NextDouble();
		}

		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to minValue, and less than maxValue.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A single-precision floating point number that is greater than or equal to minValue, and less than maxValue.</returns>
		public static float NextFloat(this Random rand, float minValue, float maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (float)rand.NextDouble(minValue, maxValue);
		}

		/// <summary>
		/// Picks a random char of the string passed
		/// </summary>
		/// <param name="chars">the string containing all the chars</param>
		/// <param name="rnd"> the Random() instance</param>
		/// <returns></returns>
		public static char PickOneChar(this string chars, Random rnd)
		{
			rnd ??= new Random();

			return chars[rnd.Next(chars.Length)];
		}

		/// <summary>
		/// Picks a random char of the string passed
		/// </summary>
		/// <param name="chars"></param>
		/// <returns></returns>
		public static char PickOneChar(this string chars)
		{
			return chars.PickOneChar(new Random(DateTime.Now.Millisecond));
	    }


		/// <summary>
		/// Returns a random decimal number that is greater than or equal to 0.0m, and less than 1.0m.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A decimal number that is greater than or equal to 0.0m, and less than 1.0m.</returns>
		public static decimal NextDecimal(this Random rand)
		{
			rand ??= new Random();
			IEnumerable<string> d = Enumerable.Range(0, 29).Select(x => rand.Next(10).ToString());
			decimal result = decimal.Parse($"0.{string.Join(string.Empty, d)}");
			return result / 1.000000000000000000000000000000000m;
		}

		/// <summary>
		/// Returns a random decimal number that is greater than or equal to minValue, and less than maxValue.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A decimal number that is greater than or equal to minValue, and less than maxValue.</returns>
		public static decimal NextDecimal(this Random rand, decimal minValue, decimal maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (maxValue - minValue) * rand.NextDecimal() + minValue;
		}

		/// <summary>
		/// Returns a random byte.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 8-bit unsigned integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static byte NextByte(this Random rand)
		{
			rand ??= new Random();

			return (byte)rand.Next(byte.MaxValue);
		}

		/// <summary>
		/// Returns a random byte that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 8-bit unsigned integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static byte NextByte(this Random rand, byte maxValue)
		{
			rand ??= new Random();

			return (byte)rand.Next(maxValue);
		}

		/// <summary>
		/// Returns a random byte that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 8-bit unsigned integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static byte NextByte(this Random rand, byte minValue, byte maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (byte)rand.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a non-negative random sbyte.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 8-bit signed integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static sbyte NextSByte(this Random rand)
		{
			rand ??= new Random();

			return (sbyte)rand.Next(sbyte.MaxValue);
		}

		/// <summary>
		/// Returns a non-negative random sbyte that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 8-bit signed integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static sbyte NextSByte(this Random rand, sbyte maxValue)
		{
			rand ??= new Random();

			return maxValue < 0 ? throw new MinMaggioreDiMax() : (sbyte)rand.Next(maxValue);
		}

		/// <summary>
		/// Returns a random sbyte that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 8-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static sbyte NextSByte(this Random rand, sbyte minValue, sbyte maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (sbyte)rand.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a non-negative random short.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 16-bit signed integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static short NextShort(this Random rand)
		{
			rand ??= new Random();

			return (short)rand.Next(short.MaxValue);
		}

		/// <summary>
		/// Returns a non-negative random short that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 16-bit signed integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static short NextShort(this Random rand, short maxValue)
		{
			rand ??= new Random();

			return maxValue < 0 ? throw new MinMaggioreDiMax() : (short)rand.Next(maxValue);
		}

		/// <summary>
		/// Returns a random short that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 16-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static short NextShort(this Random rand, short minValue, short maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (short)rand.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a random ushort.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 16-bit unsigned integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static ushort NextUShort(this Random rand)
		{
			rand ??= new Random();

			return (ushort)rand.Next(ushort.MaxValue);
		}

		/// <summary>
		/// Returns a random ushort that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 16-bit unsigned integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static ushort NextUShort(this Random rand, ushort maxValue)
		{
			rand ??= new Random();

			return (ushort)rand.Next(maxValue);
		}

		/// <summary>
		/// Returns a random ushort that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 16-bit unsigned integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static ushort NextUShort(this Random rand, ushort minValue, ushort maxValue)
		{
			rand ??= new Random();

			return minValue > maxValue ? throw new MinMaggioreDiMax() : (ushort)rand.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a random uint.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 32-bit unsigned integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static uint NextUInt(this Random rand)
		{
			rand ??= new Random();
			byte[] buffer = new byte[4];
			rand.NextBytes(buffer);
			return BitConverter.ToUInt32(buffer, 0);
		}

		/// <summary>
		/// Returns a random uint that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 32-bit unsigned integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static uint NextUInt(this Random rand, uint maxValue)
		{
			return rand.NextUInt(uint.MinValue, maxValue);
		}

		/// <summary>
		/// Returns a random uint that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 32-bit unsigned integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static uint NextUInt(this Random rand, uint minValue, uint maxValue)
		{
			rand ??= new Random();
			if (minValue > maxValue)
				throw new MinMaggioreDiMax();
			else if (minValue == maxValue)
				return minValue;
			uint range = maxValue - minValue;
			uint bias = uint.MaxValue - uint.MaxValue % range;
			byte[] buffer = new byte[4];
			uint result;
			do
			{
				result = rand.NextUInt();
			} while (result >= bias);
			return result % range + minValue;
		}

		/// <summary>
		/// Returns a non-negative random long.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static long NextLong(this Random rand)
		{
			return (long)rand.NextULong((ulong)long.MaxValue);
		}

		/// <summary>
		/// Returns a non-negative random long that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static long NextLong(this Random rand, long maxValue)
		{
			return maxValue < 0 ? throw new MinMaggioreDiMax() : (long)rand.NextULong((ulong)maxValue);
		}

		/// <summary>
		/// Returns a random long that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 64-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static long NextLong(this Random rand, long minValue, long maxValue)
		{
			rand ??= new Random();
			if (minValue > maxValue)
				throw new MinMaggioreDiMax();
			else if (minValue == maxValue)
				return minValue;
			ulong umin = minValue < 0 ?
				(ulong)(minValue - long.MinValue) :
				(ulong)minValue + (ulong)long.MaxValue + 1;
			ulong umax = maxValue < 0 ?
				(ulong)(maxValue - long.MinValue) :
				(ulong)maxValue + (ulong)long.MaxValue + 1;
			ulong result = rand.NextULong(umin, umax);
			return result >= (ulong)long.MaxValue + 1 ?
				(long)(result - (ulong)long.MaxValue) - 1 :
				long.MaxValue + (long)result;
		}

		/// <summary>
		/// Returns a random ulong.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <returns>A 64-bit unsigned integer that is greater than or equal to 0 and less than MaxValue.</returns>
		public static ulong NextULong(this Random rand)
		{
			rand ??= new Random();
			byte[] buffer = new byte[8];
			rand.NextBytes(buffer);
			return BitConverter.ToUInt64(buffer, 0);
		}

		/// <summary>
		/// Returns a random ulong that is less than the specified maximum.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
		/// <returns>A 64-bit unsigned integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
		public static ulong NextULong(this Random rand, ulong maxValue)
		{
			return rand.NextULong(ulong.MinValue, maxValue);
		}

		/// <summary>
		/// Returns a random ulong that is within a specified range.
		/// </summary>
		/// <param name="rand">A random number generator.</param>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 64-bit unsigned integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
		public static ulong NextULong(this Random rand, ulong minValue, ulong maxValue)
		{
			rand ??= new Random();
			if (minValue > maxValue)
				throw new MinMaggioreDiMax();
			/*else*/ if (minValue == maxValue)
				return minValue;
			ulong range = maxValue - minValue;
			ulong bias = ulong.MaxValue - ulong.MaxValue % range;
			ulong result;
			do
			{
				result = rand.NextULong();
			} while (result >= bias);
			return result % range + minValue;
		}


		public static T Clamp<T>(T value, T min, T max) where T : System.IComparable<T>
		{
			T result = value;
			if (value.CompareTo(max) > 0)
				result = max;

			if (value.CompareTo(min) < 0)
				result = min;

			return result;
		}

		public static bool NextBool(this Random r, int truePercentage = 50)
		{
			return r.NextDouble() < truePercentage / 100.0;
		}

		public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> Funzioni)
		{
			foreach (T value in list)
			{
				await Funzioni(value);
			}
		}

		public static T GetVal<T>(this IDictionary<string, object> dict, string key, T defaultVal)
		{
			if (dict.ContainsKey(key))
				if (dict[key] is T)
					return (T)dict[key];
			return defaultVal;
		}

#if CLIENT

		public static UIMenu AddSubMenu(this UIMenu menu, string text)
		{
			return HUD.MenuPool.AddSubMenu(menu, text);
		}

		public static UIMenu AddSubMenu(this UIMenu menu, string text, string description)
		{
			return HUD.MenuPool.AddSubMenu(menu, text, description);
		}
		public static UIMenu AddSubMenu(this UIMenu menu, string text, PointF offset)
		{
			return HUD.MenuPool.AddSubMenu(menu, text, offset);
		}
		public static UIMenu AddSubMenu(this UIMenu menu, string text, string description, PointF offset)
		{
			return HUD.MenuPool.AddSubMenu(menu, text, description, offset);
		}

		public static async Task<float> FindGroundZ(this Vector3 position)
		{
			float z = 0;
			try
			{
				float h = position.Z;
				int time = GetGameTimer();
				while (z == 0)
				{
					if(GetGameTimer() - time > 5000)
					{
						Logger.Debug($"Vector3 FindGroundZ: Troppo tempo a caricare la coordinata Z, esco dall'attesa..");
						return -199.99f;
					}
					await BaseScript.Delay(50);
					bool pippo = GetGroundZFor_3dCoord(position.X, position.Y, h, ref z, false);
					h+=10;
				}
				return z;
			}
			catch (Exception ex)
			{
				Logger.Error( $"Vector3 FindGroundZ Error: {ex.Message}");
				return -199f;
			}
		}

		public static async Task<float> FindGroundZ(this Vector4 position)
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
				Logger.Error( $"Vector4 FindGroundZ Error: {ex.Message}");
			}
			await Task.FromResult(0);
			return result;
		}

		public static async Task<float> FindGroundZ(this Position position)
		{
			float z = 0;
			try
			{
				float h = position.Z;
				int time = GetGameTimer();
				while (z == 0)
				{
					if (GetGameTimer() - time > 5000)
					{
						Logger.Debug($"Position FindGroundZ: Troppo tempo a caricare la coordinata Z, esco dall'attesa..");
						return -199.99f;
					}
					await BaseScript.Delay(50);
					bool pippo = GetGroundZFor_3dCoord(position.X, position.Y, h, ref z, false);
					h += 10;
				}
				return z;
			}
			catch (Exception ex)
			{
				Logger.Error($"Vector3 FindGroundZ Error: {ex.Message}");
				return -199f;
			}
		}

		public static async Task<Vector3> GetVector3WithGroundZ(this Vector3 position)
		{
			try
			{
				float Z = await position.FindGroundZ();
				return new Vector3(position.X, position.Y, Z);
			}
			catch (Exception ex)
			{
				Logger.Error( $"Vector3 GetVector3WithGroundZ Error: {ex.Message}");
				return new Vector3(position.X, position.Y, -199.99f);
			}
		}
		public static async Task<Vector4> GetVector4WithGroundZ(this Vector4 position)
		{
			try
			{
				float Z = await position.FindGroundZ();
				return new Vector4(position.X, position.Y, Z, position.W);
			}
			catch (Exception ex)
			{
				Logger.Error( $"Vector4 GetVector4WithGroundZ Error: {ex.Message}");
				return new Vector4(position.X, position.Y, -199.99f, position.W);
			}
		}

		public static async Task<Position> GetPositionWithGroundZ(this Position position)
		{
			try
			{
				float Z = await position.FindGroundZ();
				return new Position(position.X, position.Y, Z, position.Heading);
			}
			catch (Exception ex)
			{
				Logger.Error($"Position GetVector3WithGroundZ Error: {ex.Message}");
				return new Position(position.X, position.Y, -199.99f, position.Heading);
			}
		}

		/// <summary>
		/// Carica la zona dove la telecamera è stata creata (anche se il ped è lontano). Si resetta con ClearFocus().
		/// </summary>
		/// <param name="pos"></param>
		public static void SetFocus(this Vector3 pos)
		{
			SetFocusPosAndVel(pos.X, pos.Y, pos.Z, 0, 0, 0);
		}

		/// <summary>
		/// Carica la zona dove la telecamera è stata creata (anche se il ped è lontano). Si resetta con ClearFocus().
		/// </summary>
		/// <param name="pos"></param>
		public static void SetFocus(this Vector4 pos)
		{
			SetFocusPosAndVel(pos.X, pos.Y, pos.Z, 0, 0, 0);
		}

		/// <summary>
		/// Carica la zona dove la telecamera è stata creata (anche se il ped è lontano). Si resetta con ClearFocus().
		/// </summary>
		/// <param name="pos"></param>
		public static void SetFocus(this Position pos)
		{
			SetFocusPosAndVel(pos.X, pos.Y, pos.Z, 0, 0, 0);
		}
#endif

		public static PointF Add(this PointF c1, PointF c2)
		{
			return new PointF(c1.X + c2.X, c1.Y + c2.Y);
		}

		public static PointF Subtract(this PointF c1, PointF c2)
		{
			return new PointF(c1.X - c2.X, c1.Y - c2.Y);
		}

		public static List<T> Slice<T>(this List<T> list, int start, int end)
		{
			return list.Skip(start).Take(end - start + 1).ToList();
		}

		public static bool IsBetween<T>(this T value, T start, T end) where T : IComparable
		{
			return value.CompareTo(start) >= 0 && value.CompareTo(end) <= 0;
		}

		// WIP
		public static Color ToColor(this string color)
		{
			try
			{
				return Color.FromArgb(int.Parse(color.Replace("#", ""),
							 System.Globalization.NumberStyles.AllowHexSpecifier));
			}
			catch (Exception ex)
			{
				Logger.Error( $"ToColor exception: {ex.Data}");
			}
			return Color.FromArgb(255, 255, 255, 255);
		}

		public static Vector2 ToVector2(this float[] xyArray)
		{
			try
			{
				return new Vector2(xyArray[0], xyArray[1]);
			}
			catch (Exception ex)
			{
				Logger.Debug($"ToVector2 exception: {ex.Data}");
			}
			return Vector2.Zero;
		}

		public static Vector3 ToVector3(this float[] xyzArray)
		{
			try
			{
				return new Vector3(xyzArray[0], xyzArray[1], xyzArray[2]);
			}
			catch (Exception ex)
			{
				Logger.Debug($"ToVector3 exception: {ex.Data}");
			}
			return Vector3.Zero;
		}

		public static Vector3 ToVector3(this Vector4 vector)
		{
			try
			{
				return new Vector3(vector.X, vector.Y, vector.Z);
			}
			catch (Exception ex)
			{
				Logger.Debug($"ToVector3 exception: {ex.Data}");
			}
			return Vector3.Zero;
		}

		public static Vector4 ToVector4(this float[] xyzwArray)
		{
			try
			{
				return new Vector4(xyzwArray[0], xyzwArray[1], xyzwArray[2], xyzwArray[3]);
			}
			catch (Exception ex)
			{
				Logger.Debug($"ToVector4 exception: {ex.Data}");
			}
			return Vector4.Zero;
		}


		public static Position ToPosition(this Vector3 vec)
		{
			return new Position(vec);
		}
		public static Position ToPosition(this Vector4 vec)
		{
			return new Position(vec.ToVector3(), vec.W);
		}

		public static bool Intersects(float[] A, float[] B, float[] P)
		{
			if (A[1] > B[1])
				return Intersects(B, A, P);

			if (P[1] == A[1] || P[1] == B[1])
				P[1] += 0.0001f;

			if (P[1] > B[1] || P[1] < A[1] || P[0] > Math.Max(A[0], B[00]))
				return false;

			if (P[0] < Math.Min(A[0], B[0]))
				return true;

			double red = (P[1] - A[1]) / (double)(P[0] - A[0]);
			double blue = (B[1] - A[1]) / (double)(B[0] - A[0]);
			return red >= blue;
		}

		public static bool Contains(float[][] shape, float[] pnt)
		{
			bool inside = false;
			int len = shape.Length;
			for (int i = 0; i < len; i++)
			{
				if (Intersects(shape[i], shape[(1 + i) % len], pnt))
					inside = !inside;
			}
			return inside;
		}
	}

	internal class IgnoreJsonAttributesResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
			foreach (JsonProperty prop in props)
			{
				prop.Ignored = false;   // Ignore [JsonIgnore]
				//prop.Converter = null;  // Ignore [JsonConverter]
				//prop.PropertyName = prop.UnderlyingName;  // Use original property name instead of [JsonProperty] name
			}
			return props;
		}
	}

	public class MinMaggioreDiMax : Exception { }
}