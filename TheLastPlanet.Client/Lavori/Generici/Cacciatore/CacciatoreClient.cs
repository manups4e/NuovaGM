using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Impostazioni.Client.Configurazione.Lavori.Generici;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.Veicoli;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Status;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Lavori.Generici.Cacciatore
{
	internal static class CacciatoreClient
	{
		public static bool StaCacciando = false;
		public static Cacciatori Cacciatore;
		private static List<string> animalGroups = new List<string>
		{
			"WILD_ANIMAL",
			"SHARK",
			"COUGAR",
			"GUARD_DOG",
			"DOMESTIC_ANIMAL",
			"DEER"
		};
		private static string DaFuoco = "WEAPON_SNIPERRIFLE";
		private static string Bianca = "weapon_knife";
		private static bool affittatoFuoco = false;
		private static bool affittatoBianca = false;
		private static Dictionary<int, AnimaleDaCacciare> animaliUccisi = new Dictionary<int, AnimaleDaCacciare>();
		private static List<PedHash> animali = new List<PedHash>()
		{
			PedHash.Deer,
			PedHash.Boar,
			PedHash.Coyote,
			PedHash.Rabbit,
			PedHash.ChickenHawk,
			PedHash.MountainLion
		};
		private static Blip AreadiCaccia;

		public static void Init()
		{
			ClientSession.Instance.AddEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(ControlloAnimale));
			ClientSession.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static void Spawnato()
		{
			Cacciatore = ClientSession.Impostazioni.Lavori.Generici.Cacciatore;
			Blip caccia = World.CreateBlip(Cacciatore.inizioCaccia);
			caccia.Sprite = BlipSprite.Hunting;
			caccia.Color = BlipColor.TrevorOrange;
			caccia.Scale = 1.0f;
			caccia.Name = "Zona di Caccia";
			caccia.IsShortRange = true;
			SetBlipDisplay(caccia.Handle, 4);
		}

		public static async Task ControlloCaccia()
		{
			if (CachePlayer.Cache.MyPlayer.Ped.IsInRangeOf(Cacciatore.inizioCaccia, 1.375f))
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu di caccia");
				if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen) ApriMenuAffittoArmi();
			}

			await Task.FromResult(0);
		}

		public static async Task ControlloBordi()
		{
			Ped p = CachePlayer.Cache.MyPlayer.Ped;

			if (!p.IsInRangeOf(Cacciatore.zonaDiCaccia, Cacciatore.limiteArea))
			{
				if (affittatoBianca) p.Weapons.Remove(WeaponHash.Knife);
				if (affittatoFuoco) p.Weapons.Remove(WeaponHash.SniperRifle);
				HUD.ShowNotification("Ti sei allontanato dalla zona di caccia senza aver restituito le armi! Pagherai una multa!", NotificationColor.Red, true);
				StaCacciando = false;
				BaseScript.TriggerServerEvent("lprp:removeBank", 1000);
				if (AreadiCaccia.Exists()) AreadiCaccia.Delete();
				foreach (string s in animalGroups) p.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
				animaliUccisi.Clear();
				p.Weapons.Select(WeaponHash.Unarmed);
				ClientSession.Instance.RemoveTick(ControlloBordi);
				ClientSession.Instance.RemoveTick(ControlloUccisi);
			}

			await BaseScript.Delay(1000);
		}

		private static async void ControlloAnimale(int victim, int player, uint weaponHash, bool wasMeleeDamage)
		{
			if (!StaCacciando) return;
			Ped animale = new Ped(victim);

			if (!animali.Contains((PedHash)animale.Model.Hash)) return;
			Player assassino = new Player(player);
			AnimaleDaCacciare an = new AnimaleDaCacciare() { Entity = animale, Premiato = false };

			if (assassino.Character != CachePlayer.Cache.MyPlayer.Ped) return;
			if (!animaliUccisi.ContainsKey(animale.Handle)) animaliUccisi.Add(animale.Handle, an);
			int hash = animale.Model.Hash;
			float aggValore = 0;

			switch ((uint)hash)
			{
				case 3630914197: // deer
					aggValore = IsPedMale(animale.Handle) ? 0.003f : 0.002f;

					break;
				case 3462393972: // boar
					aggValore = 0.0025f;

					break;
				case 1682622302: // coyote
					aggValore = 0.003f;

					break;
				case 3753204865: // coniglio
					aggValore = 0.006f;

					break;
				case 2864127842: // aquila
					aggValore = 0.004f;

					break;
				case 307287994: // leone di montagna
					aggValore = 0.005f;

					break;
			}

			StatsNeeds.RegistraStats(Skills.HUNTING, aggValore);
		}

		public static async Task ControlloUccisi()
		{
			Ped p = CachePlayer.Cache.MyPlayer.Ped;

			foreach (KeyValuePair<int, AnimaleDaCacciare> anim in animaliUccisi.Where(anim => p.IsNearEntity(anim.Value.Entity, new Vector3(2, 2, 2)) && anim.Value.Entity.Model.Hash != (int)PedHash.MountainLion).Where(anim => p.Weapons.HasWeapon(WeaponHash.Knife)))
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per saccheggiare il cadavere");

				if (Input.IsControlJustPressed(Control.Context))
				{
					p.Weapons.Select(WeaponHash.Knife);
					TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TEND_TO_DEAD", 0, true); // oppure CODE_HUMAN_MEDIC_KNEEL 
					Screen.Fading.FadeOut(2000);
					await BaseScript.Delay(2001);
					int hash = anim.Value.Entity.Model.Hash;
					string msg = "";
					float aggValore = 0;
					string carne = "";

					switch ((uint)hash)
					{
						case 3630914197: // deer
							if (IsPedMale(anim.Key))
							{
								msg = " Cervo";
								aggValore = 0.002f;
							}
							else
							{
								msg = " Cerva";
								aggValore = 0.001f;
							}

							carne = "carnecervo";

							break;
						case 3462393972: // boar
							msg = " Cinghiale";
							aggValore = 0.002f;
							carne = "carnecinghiale";

							break;
						case 1682622302: // coyote
							msg = " Coyote";
							aggValore = 0.002f;
							carne = "carnecoyote";

							break;
						case 3753204865: // coniglio
							msg = " Coniglio";
							aggValore = 0.004f;
							carne = "carneconiglio";

							break;
						case 2864127842: // aquila
							msg = "'Aquila";
							aggValore = 0.003f;
							carne = "carneaquila";

							break;
						case 307287994: // leone di montagna
							aggValore = 0.003f;

							break;
					}

					StatsNeeds.RegistraStats(Skills.HUNTING, aggValore);
					animaliUccisi.ToList().Remove(anim);
					anim.Value.Entity.Delete();
					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(500);
					await BaseScript.Delay(501);
					p.Task.ClearAll();
					HUD.ShowNotification($"Hai ucciso e squoiato un~y~{msg}~w~ hai ottenuto 2 pezzi di ~b~{ConfigShared.SharedConfig.Main.Generici.ItemList[carne].label}~w~.", NotificationColor.GreenDark, true);
					BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", carne, 2, 0.5f);
				}
			}

			await Task.FromResult(0);
		}

		private static async void ApriMenuAffittoArmi()
		{
			UIMenu affittoArmi = new UIMenu("Menu dei cacciatori", "Per i veri amanti di un nobile sport");
			HUD.MenuPool.Add(affittoArmi);

			if (!StaCacciando)
			{
				UIMenuItem armi = new UIMenuItem("Affitta armi da caccia", "Se non li hai, ti verranno dati un fucile da cecchino e un coltello\nPrezzo: 250 fucile, 50 coltello");
				UIMenuItem inizia = new UIMenuItem("Inizia a cacciare", "Si comincia!");
				affittoArmi.AddItem(armi);
				affittoArmi.AddItem(inizia);
				affittoArmi.OnItemSelect += async (menu, item, index) =>
				{
					if (CachePlayer.Cache.MyPlayer.User.HasLicense("Caccia"))
					{
						if (item == armi)
						{
							if ((CachePlayer.Cache.MyPlayer.User.HasWeapon(DaFuoco) || affittatoFuoco) && (CachePlayer.Cache.MyPlayer.User.HasWeapon(Bianca) || affittatoBianca))
							{
								HUD.ShowNotification("Hai già le armi che noi affittiamo.", NotificationColor.Red, true);

								return;
							}

							int prezzo = 0;

							if (!CachePlayer.Cache.MyPlayer.User.HasWeapon(DaFuoco))
								if (CachePlayer.Cache.MyPlayer.User.Money >= 250 || CachePlayer.Cache.MyPlayer.User.Bank >= 250)
								{
									CachePlayer.Cache.MyPlayer.Ped.Weapons.Give(WeaponHash.SniperRifle, 100, false, true);
									prezzo += 250;
									affittatoFuoco = true;
								}

							if (!CachePlayer.Cache.MyPlayer.User.HasWeapon(Bianca))
								if (CachePlayer.Cache.MyPlayer.User.Money >= 50 || CachePlayer.Cache.MyPlayer.User.Bank >= 50)
								{
									CachePlayer.Cache.MyPlayer.Ped.Weapons.Give(WeaponHash.Knife, 1, false, true);
									prezzo += 50;
									affittatoBianca = true;
								}

							if (CachePlayer.Cache.MyPlayer.User.Money >= prezzo)
							{
								BaseScript.TriggerServerEvent("lprp:removeMoney", prezzo);
								HUD.ShowNotification("Le armi che non avevi già ti sono state date in affitto");
							}
							else
							{
								if (CachePlayer.Cache.MyPlayer.User.Bank >= prezzo)
								{
									BaseScript.TriggerServerEvent("lprp:removeBank", prezzo);
									HUD.ShowNotification("Le armi che non avevi già ti sono state date in affitto");
								}
								else
								{
									HUD.ShowNotification("Non hai i soldi necessari ad affittare le armi!");
								}
							}
						}
						else if (item == inizia)
						{
							StaCacciando = true;
							AreadiCaccia = World.CreateBlip(Cacciatore.zonaDiCaccia, Cacciatore.limiteArea);
							AreadiCaccia.Name = "Zona di caccia";
							AreadiCaccia.Alpha = 25;
							AreadiCaccia.Color = BlipColor.TrevorOrange;
							foreach (string s in animalGroups) CachePlayer.Cache.MyPlayer.Ped.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Dislike, true);
							ClientSession.Instance.AddTick(ControlloBordi);
							ClientSession.Instance.AddTick(ControlloUccisi);
							SetBlipDisplay(AreadiCaccia.Handle, 4);
							HUD.MenuPool.CloseAllMenus();
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Hai iniziato a cacciare, non allontanarti dalla zona segnata in mappa o perderai le armi affittate e pagherai una multa!!", 5000);
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Puoi cacciare gli animali che trovi nell'area, se hai il coltello e ti avvicini all'animale ucciso, puoi prenderne la carne!", 5000);
						}
					}
					else
					{
						HUD.ShowNotification("Non hai una licenza di caccia!", NotificationColor.Red, true);
					}
				};
			}
			else
			{
				UIMenuItem fine = new UIMenuItem("Smetti di cacciare", "Restituirai le tue armi");
				affittoArmi.AddItem(fine);
				fine.Activated += (menu, item) =>
				{
					if (affittatoBianca) BaseScript.TriggerServerEvent("lprp:removeWeapon", Bianca);
					if (affittatoFuoco) BaseScript.TriggerServerEvent("lprp:removeWeapon", DaFuoco);
					HUD.ShowNotification("Grazie di aver scelto il nostro servizio di gestione caccia!\nTorna presto!", NotificationColor.GreenDark);
					StaCacciando = false;
					ClientSession.Instance.RemoveTick(ControlloBordi);
					if (AreadiCaccia.Exists()) AreadiCaccia.Delete();
					foreach (string s in animalGroups) CachePlayer.Cache.MyPlayer.Ped.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
					animaliUccisi.Clear();
					CachePlayer.Cache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
					HUD.MenuPool.CloseAllMenus();
				};
			}

			affittoArmi.Visible = true;
		}
	}

	internal class AnimaleDaCacciare
	{
		public Ped Entity;
		public bool Premiato = false;
	}
}