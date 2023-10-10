using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles
{
    internal static class insurance
    {
        public static void Init() { }

        private static async Task InsuranceCheck()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && playerPed.CurrentVehicle.Driver != playerPed)
            {
                if (playerPed.CurrentVehicle.IsOnFire || playerPed.CurrentVehicle.IsDead)
                    HUD.ShowAdvancedNotification("Insurance", "Payment of Compensation", "Since you were in the vehicle when it was destroyed but were not the driver, you will be reimbursed a portion of the cost of the vehicle.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);
                if (playerPed.CurrentVehicle.Driver == playerPed && playerPed.CurrentVehicle.Speed < 10)
                {
                    if (playerPed.CurrentVehicle.IsDead || playerPed.CurrentVehicle.IsOnFire)
                        HUD.ShowAdvancedNotification("Insurance", "Payment of Compensation", "Since the vehicle was not exceeding the speed limit at the time of the accident and was not the main cause of the accident, you will be reimbursed part of the cost of the vehicle.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);
                }
                else if (playerPed.CurrentVehicle.Driver == playerPed && playerPed.CurrentVehicle.Speed > 10)
                {
                    if (playerPed.CurrentVehicle.IsDead || playerPed.CurrentVehicle.IsOnFire)
                        HUD.ShowAdvancedNotification("Insurance", "Payment of Compensation", "Since the vehicle was exceeding the speed limit at the time of the accident, it was not possible to reimburse compensation for the aforementioned vehicle.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);
                }
            }
        }
    }
}