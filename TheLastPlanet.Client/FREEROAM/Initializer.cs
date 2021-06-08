using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.AdminAC;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.FreeRoam.Managers;
using TheLastPlanet.Client.FreeRoam.Scripts;
using TheLastPlanet.Client.ListaPlayers;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.FreeRoam
{
	class Initializer
	{
		public static async Task Init()
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
