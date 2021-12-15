using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.AdminAC;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.ListaPlayers;
using TheLastPlanet.Client.MODALITA.FREEROAM.Managers;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MODALITA.FREEROAM
{
	class Initializer
	{
		public static async Task Init()
		{
			LoaderSpawner.Init();
			/*
			ExperienceManager.Init();
			WorldEventsManager.Init();
			ExperienceManager.Init();
			PlayerBlipsHandler.Init();
			//AGGIUNGERE GESTIONE METEO
			//AGGIUNGERE GESTIONE ORARIO
			//AGGIUNGERE GESTIONE STATISTICHE
			//AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
			BaseEventsFreeRoam.Init();
			//Death.Init();
			PlayerTags.Init();
			*/
			await Task.FromResult(0);
		}

		public static async Task Stop()
		{
			ExperienceManager.Init();
			WorldEventsManager.Init();
			ExperienceManager.Init();
			//AGGIUNGERE GESTIONE METEO
			//AGGIUNGERE GESTIONE ORARIO
			//AGGIUNGERE GESTIONE STATISTICHE
			//AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
			//Death.Init();
			PlayerTags.Init();
			await Task.FromResult(0);
		}
	}
}
