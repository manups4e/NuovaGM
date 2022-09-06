using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Lavoro
{
    public enum Employment
    {
        Unemployed,
        Polizia,
        Medico,
        VenditoreAuto,
        VenditoreCase,
        Taxi,
        Pescatore,
        Cacciatore,
        RimozioneForzata
    }

    public static class JobHandler
    {
        public static void Init()
        {
            EventDispatcher.Mount("lprp:job:employee:hired", new Action<int, string, string>((seed, job, emp) =>
            {
                HUD.ShowNotification($"Sei stato assunto come {emp}!");

            }));
            EventDispatcher.Mount("lprp:job:employee:fired", new Action<int, string, string>((seed, job, emp) =>
            {
                HUD.ShowNotification($"Sei stato licenziato! Non sei più {emp}!");
            }));
        }
    }
}
