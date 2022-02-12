using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Profili
{
    public class JobBusinessProfile : JobProfile
    {
        public override JobProfile[] Dependencies { get; set; }
        public string Seed { get; set; }
        public Business Business { get; set; }

        public override async void Begin(LavoroBase job)
        {
            await Cache.PlayerCache.Loaded();
            Seed = $"lprp:businesses::{job.Lavoro.ToString().ToLower()}";

            Business = await Client.Instance.Events.Get<Business>("lprp:business:fetch", Seed) ?? new Business()
            {
                Seed = Seed,
                Balance = 0,
                Registered = DateTime.Now.Ticks
            };

            Client.Instance.Events.Mount("lprp:business:update", new Action<Business>((a) => Business.Balance = a.Balance));
        }

        public void Commit() => Client.Instance.Events.Send("lprp:business:update", Business);
    }
}
