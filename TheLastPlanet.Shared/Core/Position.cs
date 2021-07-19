using System;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }

        public static readonly Position Zero = new();

        public Position()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            Heading = 0f;
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

        public Position(Position value, float heading)
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

        public Position(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            Heading = reader.ReadSingle();
        }

        public void PackSerializedBytes(BinaryWriter writer)
        {
			writer.Write(X);
			writer.Write(Y);
			writer.Write(Z);
			writer.Write(Heading);
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

		public Position Clone() => new Position(X, Y, Z, Heading);

		public override string ToString() => $"X = {X}, Y = {Y}, Z = {Z} [Heading = {Heading}]";

		[Ignore][JsonIgnore]
        public bool IsZero => X == 0 && Y == 0 && Z == 0;

		public float[] ToArray() => new[] { X, Y, Z };

		[Ignore][JsonIgnore]
        public Vector3 ToVector3 => new(X, Y, Z);
        [Ignore][JsonIgnore]
        public Vector4 ToVector4 => new(X, Y, Z, Heading);

        public float Distance(Vector3 value)
        {
            float x = X - value.X;
            float y = Y - value.Y;
            float z = Z - value.Z;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }
        public float Distance(Position value)
        {
            float x = X - value.X;
            float y = Y - value.Y;
            float z = Z - value.Z;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

		public bool IsInRangeOf(Vector3 value, float radius) => Distance(value) <= radius;

		public static Position operator +(Position left, Position right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public static Position operator +(Position value) => value;
        public static Position operator +(Position value, float scalar) => new(value.X + scalar, value.Y + scalar, value.Z + scalar);
        public static Position operator +(float scalar, Position value) => new(scalar + value.X, scalar + value.Y, scalar + value.Z);
        public static Position operator -(Position left, Position right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		public static Position operator -(Position value) => new(-value.X, -value.Y, -value.Z);
        public static Position operator -(Position value, float scalar) => new(value.X - scalar, value.Y - scalar, value.Z - scalar);
        public static Position operator -(float scalar, Position value) => new(scalar - value.X, scalar - value.Y, scalar - value.Z);
        public static Position operator *(float scale, Position value) => new(value.X * scale, value.Y * scale, value.Z * scale);
		public static Position operator *(Position value, float scale) => new(value.X * scale, value.Y * scale, value.Z * scale);
        public static Position operator *(Position left, Position right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        public static Position operator /(Position value, float scale) => new(value.X / scale, value.Y / scale, value.Z / scale);
		public static Position operator /(float scale, Position value) => new(scale / value.X, scale / value.Y, scale / value.Z);
		public static Position operator /(Position value, Position scale) => new(value.X / scale.X, value.Y / scale.Y, value.Z / scale.Z);

        public static bool operator ==(Position left, Position right) => left?.Equals(right) ?? false;
        public static bool operator !=(Position left, Position right) => !(left == right);

        public static bool operator >(Position left, Position right) => left.X > right.X || left.Y > right.Y || left.Z > right.Z;
		public static bool operator <(Position left, Position right) => left.X < right.X || left.Y < right.Y || left.Z < right.Z;

		public static bool operator >=(Position left, Position right) => left.X >= right.X || left.Y >= right.Y || left.Z >= right.Z;
		public static bool operator <=(Position left, Position right) => left.X <= right.X || left.Y <= right.Y || left.Z <= right.Z;

		public static bool operator >=(float left, Position right) => left >= right.X || left >= right.Y || left >= right.Z;
		public static bool operator <=(float left, Position right) => left <= right.X || left <= right.Y || left <= right.Z;

		public bool Equals(ref Position other) => other is not null && MathUtil.NearEqual(other.X, X) && MathUtil.NearEqual(other.Y, Y) && MathUtil.NearEqual(other.Z, Z);
		public bool Equals(Position other) => Equals(ref other);

		public override int GetHashCode() => this.GetHashCode();
	}


    [Serialization]
    public partial class RotatablePosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public RotatablePosition() { }
        public RotatablePosition(float x, float y, float z, float yaw, float pitch, float roll)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
        public RotatablePosition(Vector3 pos, Vector3 rot)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
            Yaw = rot.X;
            Pitch = rot.Y;
            Roll = rot.Z;
        }

        [Ignore][JsonIgnore]
        public Vector3 ToVector3 => new(X, Y, Z);
        [Ignore][JsonIgnore]
        public Vector3 ToRot => new(Yaw, Pitch, Roll);

    }
}