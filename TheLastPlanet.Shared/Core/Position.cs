using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;

namespace TheLastPlanet.Shared
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }

        public static readonly Position Zero = new Position();

        public Position()
        {
        }

        public Position(float x, float y, float z, float heading)
        {
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(Vector3 value, float heading)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            Heading = heading;
        }

        public Position(Vector3 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        public Position(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Position Subtract(Position position)
        {
            X -= position.X;
            Y -= position.Y;
            Z -= position.Z;
            Heading -= position.Heading;

            return this;
        }

        public Position Add(Position position)
        {
            X += position.X;
            Y += position.Y;
            Z += position.Z;
            Heading += position.Heading;

            return this;
        }

        public Position Clone()
        {
            return new Position(X, Y, Z, Heading);
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Z = {Z} [Heading = {Heading}]";
        }

        [JsonIgnore]
        public bool IsZero => X == 0 && Y == 0 && Z == 0;

        public float[] ToArray()
        {
            return new[] { X, Y, Z };
        }

        [JsonIgnore]
        public Vector3 ToVector3 => new(X, Y, Z);

        public float Distance(Vector3 value)
		{
            float x = X - value.X;
            float y = Y - value.Y;
            float z = Z - value.Z;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public bool IsInRangeOf(Vector3 value, float radius)
		{
            return Distance(value) <= radius;
		}

        public static Position operator +(Position left, Position right)
        {
            return new Position(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Position operator *(Position left, Position right)
        {
            return new Position(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Position operator +(Position value)
        {
            return value;
        }

        public static Position operator -(Position left, Position right)
        {
            return new Position(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Position operator -(Position value)
        {
            return new Position(-value.X, -value.Y, -value.Z);
        }

        public static Position operator *(float scale, Position value)
        {
            return new Position(value.X * scale, value.Y * scale, value.Z * scale);
        }

        public static Position operator *(Position value, float scale)
        {
            return new Position(value.X * scale, value.Y * scale, value.Z * scale);
        }
  
        public static Position operator /(Position value, float scale)
        {
            return new Position(value.X / scale, value.Y / scale, value.Z / scale);
        }

        public static Position operator /(float scale, Position value)
        {
            return new Position(scale / value.X, scale / value.Y, scale / value.Z);
        }

        public static Position operator /(Position value, Position scale)
        {
            return new Position(value.X / scale.X, value.Y / scale.Y, value.Z / scale.Z);
        }

        public static Position operator +(Position value, float scalar)
        {
            return new Position(value.X + scalar, value.Y + scalar, value.Z + scalar);
        }

        public static Position operator +(float scalar, Position value)
        {
            return new Position(scalar + value.X, scalar + value.Y, scalar + value.Z);
        }

        public static Position operator -(Position value, float scalar)
        {
            return new Position(value.X - scalar, value.Y - scalar, value.Z - scalar);
        }

        public static Position operator -(float scalar, Position value)
        {
            return new Position(scalar - value.X, scalar - value.Y, scalar - value.Z);
        }
#if CLIENT
        public async Task<Position> FindGroundZ()
        {
            float z = 0;

            try
            {
                int time = Game.GameTime;

                while (z == 0)
                {
                    if (Game.GameTime - time >= 5000)
                    {
                        Log.Printa(LogType.Warning, $"Position FindGroundZ: Troppo tempo a caricare la coordinata Z, interrompo e inserisco default..");
                        return new Position(X, Y, -199.9f, Heading);
                    }

                    await BaseScript.Delay(50);
                    bool pippo = API.GetGroundZFor_3dCoord(X, Y, Z, ref z, false);
                    Z += 10;
                }

                return new Position(X, Y, z, Heading);
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, $"Position FindGroundZ Error: {ex.Message}");
                return new Position(X, Y, -199.9f, Heading);
            }
        }
#endif
    }


    public class RotatablePosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public RotatablePosition(float x, float y, float z, float yaw, float pitch, float roll)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
    }
}