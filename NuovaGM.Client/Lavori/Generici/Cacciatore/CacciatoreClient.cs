using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Status;
using NuovaGM.Shared;

namespace NuovaGM.Client.Lavori.Generici.Cacciatore
{
	static class CacciatoreClient
	{
		public static bool StaCacciando = false;
		public static Cacciatori Cacciatore;
		private static List<string> animalGroups = new List<string> { "WILD_ANIMAL", "SHARK", "COUGAR", "GUARD_DOG", "DOMESTIC_ANIMAL", "DEER" };
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
			PedHash.MountainLion,
		};
		private static Blip AreadiCaccia;

		public static void Init()
		{
			Client.Instance.AddEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(ControlloAnimale));
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static void Spawnato()
		{
			Cacciatore = Client.Impostazioni.Lavori.Generici.Cacciatore;
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
			if (Game.PlayerPed.IsInRangeOf(Cacciatore.inizioCaccia, 1.375f))
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu di caccia");
				if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
					ApriMenuAffittoArmi();
			}
			await Task.FromResult(0);
		}

		public static async Task ControlloBordi()
		{
			if(Game.PlayerPed.IsInRangeOf(Cacciatore.zonaDiCaccia, Cacciatore.limiteArea))
			{
				if (affittatoBianca)
					Game.PlayerPed.Weapons.Remove(WeaponHash.Knife);
				if (affittatoFuoco)
					Game.PlayerPed.Weapons.Remove(WeaponHash.SniperRifle);
				HUD.ShowNotification("Ti sei allontanato dalla zona di caccia senza aver restituito le armi! Pagherai una multa!", NotificationColor.Red, true);
				StaCacciando = false;
				BaseScript.TriggerServerEvent("lprp:removeBank", 1000);
				if (AreadiCaccia.Exists())
					AreadiCaccia.Delete();
				foreach (string s in animalGroups)
					Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
				animaliUccisi.Clear();
				Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
				Client.Instance.RemoveTick(ControlloBordi);
				Client.Instance.RemoveTick(ControlloUccisi);
			}
			await BaseScript.Delay(1000);
		}

		private static async void ControlloAnimale(int victim, int player, uint weaponHash, bool wasMeleeDamage)
		{
			if (StaCacciando)
			{
				Ped animale = new Ped(victim);
				if (animali.Contains((PedHash)animale.Model.Hash))
				{
					Player assassino = new Player(player);
					AnimaleDaCacciare an = new AnimaleDaCacciare() { Entity = animale, Premiato = false };
					if (assassino.Character == Game.PlayerPed) // rimuovere questo controllo
					{
						if (!animaliUccisi.ContainsKey(animale.Handle)) animaliUccisi.Add(animale.Handle, an);
						var hash = animale.Model.Hash;
						float aggValore = 0;
						switch ((uint)hash)
						{
							case 3630914197: // deer
								if (IsPedMale(animale.Handle))
									aggValore = 0.003f;
								else
									aggValore = 0.002f;
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
				}
			}
		}

		public static async Task ControlloUccisi()
		{
			foreach (var anim in animaliUccisi)
			{
				if (Game.PlayerPed.IsNearEntity(anim.Value.Entity, new Vector3(2, 2, 2)) && anim.Value.Entity.Model.Hash != (int)PedHash.MountainLion)
				{
					if (Game.PlayerPed.Weapons.HasWeapon(WeaponHash.Knife))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per saccheggiare il cadavere");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Weapons.Select(WeaponHash.Knife);

							TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TEND_TO_DEAD", 0, true); // oppure CODE_HUMAN_MEDIC_KNEEL 
							Screen.Fading.FadeOut(2000);
							await BaseScript.Delay(2001);
							var hash = anim.Value.Entity.Model.Hash;
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
							Game.PlayerPed.Task.ClearAll();
							HUD.ShowNotification($"Hai ucciso e squoiato un~y~{msg}~w~ hai ottenuto 2 pezzi di ~b~{ConfigShared.SharedConfig.Main.Generici.ItemList[carne].label}~w~.", NotificationColor.GreenDark, true);
							BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", carne, 2, 0.5f);
						}
					}
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
					if (Game.Player.GetPlayerData().hasLicense("Caccia"))
					{
						if (item == armi)
						{
							if ((Game.Player.GetPlayerData().hasWeapon(DaFuoco) || affittatoFuoco) && (Game.Player.GetPlayerData().hasWeapon(Bianca) || affittatoBianca))
							{
								HUD.ShowNotification("Hai già le armi che noi affittiamo.", NotificationColor.Red, true);
								return;
							}
							int prezzo = 0;
							if (!Game.Player.GetPlayerData().hasWeapon(DaFuoco))
							{
								if (Game.Player.GetPlayerData().Money >= 250 || Game.Player.GetPlayerData().Bank >= 250)
								{
									Game.PlayerPed.Weapons.Give(WeaponHash.SniperRifle, 100, false, true);
									prezzo += 250;
									affittatoFuoco = true;
								}
							}
							if (!Game.Player.GetPlayerData().hasWeapon(Bianca))
							{
								if (Game.Player.GetPlayerData().Money >= 50 || Game.Player.GetPlayerData().Bank >= 50)
								{
									Game.PlayerPed.Weapons.Give(WeaponHash.Knife, 1, false, true);
									prezzo += 50;
									affittatoBianca = true;
								}
							}
							if (Game.Player.GetPlayerData().Money >= prezzo)
							{
								BaseScript.TriggerServerEvent("lprp:removeMoney", prezzo);
								HUD.ShowNotification("Le armi che non avevi già ti sono state date in affitto");
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= prezzo)
								{
									BaseScript.TriggerServerEvent("lprp:removeBank", prezzo);
									HUD.ShowNotification("Le armi che non avevi già ti sono state date in affitto");
								}
								else
									HUD.ShowNotification("Non hai i soldi necessari ad affittare le armi!");
							}
						}
						else if (item == inizia)
						{
							StaCacciando = true;
							AreadiCaccia = World.CreateBlip(Cacciatore.zonaDiCaccia, Cacciatore.limiteArea);
							AreadiCaccia.Name = "Zona di caccia";
							AreadiCaccia.Alpha = 25;
							AreadiCaccia.Color = BlipColor.TrevorOrange;
							foreach (string s in animalGroups)
								Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Dislike, true);
							Client.Instance.AddTick(ControlloBordi);
							Client.Instance.AddTick(ControlloUccisi);
							SetBlipDisplay(AreadiCaccia.Handle, 4);
							HUD.MenuPool.CloseAllMenus();
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Hai iniziato a cacciare, non allontanarti dalla zona segnata in mappa o perderai le armi affittate e pagherai una multa!!", 5000);
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Puoi cacciare gli animali che trovi nell'area, se hai il coltello e ti avvicini all'animale ucciso, puoi prenderne la carne!", 5000);
						}
					}
					else
						HUD.ShowNotification("Non hai una licenza di caccia!", NotificationColor.Red, true);
				};
			}
			else
			{
				UIMenuItem fine = new UIMenuItem("Smetti di cacciare", "Restituirai le tue armi");
				affittoArmi.AddItem(fine);

				fine.Activated += (menu, item) =>
				{
					
					if (affittatoBianca)
						BaseScript.TriggerServerEvent("lprp:removeWeapon", Bianca);
					if (affittatoFuoco)
						BaseScript.TriggerServerEvent("lprp:removeWeapon", DaFuoco);
					HUD.ShowNotification("Grazie di aver scelto il nostro servizio di gestione caccia!\nTorna presto!", NotificationColor.GreenDark);
					StaCacciando = false;
					Client.Instance.RemoveTick(ControlloBordi);
					if (AreadiCaccia.Exists())
						AreadiCaccia.Delete();
					foreach (string s in animalGroups)
						Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
					animaliUccisi.Clear();
					Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
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
