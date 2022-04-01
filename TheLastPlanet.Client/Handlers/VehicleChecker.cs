using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Handlers
{
    public delegate void PedEnteredVehicle(Ped ped, Vehicle vehicle, VehicleSeat seatIndex);
    public delegate void PedLeftVehicle(Ped ped, Vehicle vehicle, VehicleSeat seatIndex);
    public delegate void PedEnteringVehicle(Ped ped, Vehicle vehicle, VehicleSeat seatIndex);
    public static class VehicleChecker
    {
        private static bool isInVehicle = false;
        private static bool isEnteringVehicle = false;
        private static Vehicle currentVehicle;
        private static VehicleSeat currentSeat;
        public static event PedEnteredVehicle OnPedEnteredVehicle;
        public static event PedLeftVehicle OnPedLeftVehicle;
        public static event PedEnteringVehicle OnPedEnteringVehicle;

        public static async void Init()
        {
            await PlayerCache.Loaded();
            Client.Instance.AddTick(VehicleCheck);
        }

        private static async Task VehicleCheck()
        {
            Ped me = PlayerCache.MyPlayer.Ped;
            int meHandle = me.Handle;
            if (!(isInVehicle || me.IsDead))
            {
                if (DoesEntityExist(GetVehiclePedIsTryingToEnter(meHandle)) && !isEnteringVehicle)
                {
                    var veh = GetVehiclePedIsTryingToEnter(meHandle);
                    if (NetworkGetEntityIsNetworked(veh))
                    {
                        var seat = (VehicleSeat)GetSeatPedIsTryingToEnter(meHandle);
                        var netId = VehToNet(veh);
                        isEnteringVehicle = true;
                        OnPedEnteringVehicle?.Invoke(me, currentVehicle, seat);
                        Client.Instance.Events.Send("baseevents:enteringVehicle", veh, seat, netId);
                    }
                }
                else if (!DoesEntityExist(GetVehiclePedIsTryingToEnter(meHandle)) && !me.IsInVehicle() && isEnteringVehicle)
                {
                    if (NetworkGetEntityIsNetworked(GetVehiclePedIsTryingToEnter(meHandle)))
                    {

                        Client.Instance.Events.Send("baseevents:enteringAborted");
                        isEnteringVehicle = false;
                    }
                }
                else if (me.IsInVehicle())
                {
                    if (NetworkGetEntityIsNetworked(me.CurrentVehicle.Handle))
                    {
                        isEnteringVehicle = false;
                        isInVehicle = true;
                        currentVehicle = me.CurrentVehicle;
                        currentSeat = me.SeatIndex;

                        OnPedEnteredVehicle?.Invoke(me, currentVehicle, currentSeat);
                        Client.Instance.Events.Send("baseevents:enteredVehicle", currentVehicle.Handle, currentSeat, currentVehicle.NetworkId);
                    }
                }
            }
            else if (isInVehicle)
            {
                if (!me.IsInVehicle() || me.IsDead)
                {
                    OnPedLeftVehicle?.Invoke(me, currentVehicle, currentSeat);
                    Client.Instance.Events.Send("baseevents:leftVehicle", currentVehicle.Handle, currentSeat);
                    isInVehicle = false;
                    currentVehicle = null;
                    currentSeat = 0;
                }
            }
            await BaseScript.Delay(50);
        }
    }
}
