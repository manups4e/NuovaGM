using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.RolePlay.Lavori.Generici.Pescatore;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.RolePlay.Lavori
{
	internal static class JobManager
	{
		public static List<LavoroBase> Registered { get; set; } = new List<LavoroBase>();

		private static void Init() 
		{
            RegisterJob(new PescatoreJob());
		}

        public static async void RegisterJob(LavoroBase job)
        {
            if (Registered.Contains(job)) return;

            Registered.Add(job);

            foreach (var profile in job.Profiles)
            {
                profile.Job = job;
                profile.Begin(job);

                if (profile.Dependencies == null) continue;

                foreach (var dependency in profile.Dependencies)
                {
                    if (job.Profiles.FirstOrDefault(self => self.GetType() == dependency.GetType()) == null)
                    {
                        throw new JobException(job,
                            $"Profile `{profile.GetType().Name}` must have the dependency `{dependency.GetType().Name}` in order to function correctly.");
                    }
                }
            }
            /*
            foreach (var blip in job.Blips)
            {
                blip.Commit();
            }
            */
            await Cache.Loaded();

            job.Init();
        }

        public static T GetJob<T>() where T : LavoroBase
        {
            return (T)Registered.FirstOrDefault(self => self.GetType() == typeof(T));
        }

    }
}