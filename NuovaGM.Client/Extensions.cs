using CitizenFX.Core;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	public static class Extensions
	{
		public static T Clamp<T>(T value, T min, T max)
			where T : System.IComparable<T>
		{
			T result = value;
			if (value.CompareTo(max) > 0)
			{
				result = max;
			}

			if (value.CompareTo(min) < 0)
			{
				result = min;
			}

			return result;
		}

		public static bool NextBool(this Random r, int truePercentage = 50)
		{
			return r.NextDouble() < truePercentage / 100.0;
		}

		public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> Funzioni)
		{
			foreach (var value in list)
			{
				await Funzioni(value);
			}
		}

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
				Client.Printa(LogType.Error, $"ToColor exception: {ex.Data}");
			}
			return Color.FromArgb(255, 255, 255, 255);
		}

		public static float[] ToArray(this Vector2 vector)
		{
			try
			{
				return new float[] { vector.X, vector.Y };
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Debug, $"ToArray exception: {ex.Data}");
			}
			return null;
		}

		public static float[] ToArray(this Vector3 vector)
		{
			try
			{
				return new float[] { vector.X, vector.Y, vector.Z };
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Debug, $"ToArray exception: {ex.Data}");
			}
			return null;
		}

		public static float[] ToArray(this Vector4 vector)
		{
			try
			{
				return new float[] { vector.X, vector.Y, vector.Z, vector.W };
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Debug, $"ToArray exception: {ex.Data}");
			}
			return null;
		}

		public static Vector2 ToVector2(this float[] xyArray)
		{
			try
			{
				return new Vector2(xyArray[0], xyArray[1]);
			}
			catch (Exception ex)
			{
				Client.Printa(LogType.Debug, $"ToVector2 exception: {ex.Data}");
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
				Client.Printa(LogType.Debug, $"ToVector3 exception: {ex.Data}");
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
				Client.Printa(LogType.Debug, $"ToVector4 exception: {ex.Data}");
			}
			return Vector4.Zero;
		}

		public static bool Intersects(float[] A, float[] B, float[] P)
		{
			if (A[1] > B[1])
			{
				return Intersects(B, A, P);
			}

			if (P[1] == A[1] || P[1] == B[1])
			{
				P[1] += 0.0001f;
			}

			if (P[1] > B[1] || P[1] < A[1] || P[0] > Math.Max(A[0], B[00]))
			{
				return false;
			}

			if (P[0] < Math.Min(A[0], B[0]))
			{
				return true;
			}

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
				{
					inside = !inside;
				}
			}
			return inside;
		}
	}
}
