using System;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Lavoro
{
    public enum Employment
    {
        Unemployed,
        Police,
        Medic,
        CarDealer,
        RealEstate,
        Taxi,
        Fisherman,
        Hunter,
        Towing
    }

    public static class JobHandler
    {
        public static void Init()
        {
            EventDispatcher.Mount("lprp:job:employee:hired", new Action<int, string, string>((seed, job, emp) =>
            {
                HUD.ShowNotification($"You've been hired as {emp}!");

            }));
            EventDispatcher.Mount("lprp:job:employee:fired", new Action<int, string, string>((seed, job, emp) =>
            {
                HUD.ShowNotification($"You've been fired from the {emp} job!");
            }));
        }
    }
}
