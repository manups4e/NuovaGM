using CitizenFX.Core;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

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
            Z = value.Y;
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
            return $"{{ X = {X}, Y = {Y}, Z = {Z}, Heading = {Heading} }}";
        }

        public bool IsZero
        {
            get { return X == 0 && Y == 0 && Z == 0; }
        }

        public float[] ToArray()
        {
            return new float[] { X, Y, Z };
        }

        public Vector3 ToVector3 { get => new(X, Y, Z); }


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