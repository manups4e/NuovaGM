using CitizenFX.Core;
using Logger;

namespace TheLastPlanet.Shared
{
    public class SharedStateBag<T>
    {
        public string Name { get; set; }
        public SharedStateBag(string name)
        {
            Name = name;
        }

        private void SetEnt(Entity entity, T value, bool replicate) => entity.State.Set(Name, value.ToBytes(), replicate);

        public void Set(Ped ped, T value, bool replicate) => SetEnt(ped, value, replicate);

        public void Set(Vehicle veh, T value, bool replicate) => SetEnt(veh, value, replicate);

        public void Set(Prop prop, T value, bool replicate) => SetEnt(prop, value, replicate);

        public void Set(Player player, T value, bool replicate) => player.State.Set(Name, value.ToBytes(), replicate);

        public T Get(Ped ped) => TypeCache<T>.IsSimpleType ? (T)ped.State.Get(Name) : (ped.State.Get(Name) as byte[]).FromBytes<T>();
        public T Get(Vehicle veh) => TypeCache<T>.IsSimpleType ? (T)veh.State.Get(Name) : (veh.State.Get(Name) as byte[]).FromBytes<T>();
        public T Get(Prop prop) => TypeCache<T>.IsSimpleType ? (T)prop.State.Get(Name) : (prop.State.Get(Name) as byte[]).FromBytes<T>();
        public T Get(Player player) =>  TypeCache<T>.IsSimpleType ? (T)player.State.Get(Name) : (player.State.Get(Name) as byte[]).FromBytes<T>();
    }

    public static class SharedStateBagsExtensions
    {
        public static void SetState<T>(this Ped ped, string name, T value, bool replicate) => ped.State.Set(name, value.ToBytes(), replicate);

        public static void SetState<T>(this Vehicle veh, string name, T value, bool replicate) => veh.State.Set(name, value.ToBytes(), replicate);

        public static void SetState<T>(this Prop prop, string name, T value, bool replicate) => prop.State.Set(name, value.ToBytes(), replicate);

        public static void SetState<T>(this Player player, string name, T value, bool replicate) => player.State.Set(name, value.ToBytes(), replicate);

        public static T GetState<T>(this Ped ped, string name) => (ped.State.Get(name) as byte[]).FromBytes<T>();
        public static T GetState<T>(this Vehicle veh, string name) => (veh.State.Get(name) as byte[]).FromBytes<T>();
        public static T GetState<T>(this Prop prop, string name) => (prop.State.Get(name) as byte[]).FromBytes<T>();
        public static T GetState<T>(this Player player, string name) =>  (player.State.Get(name) as byte[]).FromBytes<T>();

    }
}