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
using NuovaGM.Shared;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Status;

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
		private static Dictionary<int, Ped> animaliTrovati = new Dictionary<int, Ped>();
		private static Dictionary<int, Ped> animaliUccisi = new Dictionary<int, Ped>();
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
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static void Spawnato()
		{
			Cacciatore = ConfigClient.Conf.Lavori.Generici.Cacciatore;
			Blip caccia = World.CreateBlip(Cacciatore.inizioCaccia.ToVector3());
			caccia.Sprite = BlipSprite.Hunting;
			caccia.Color = BlipColor.TrevorOrange;
			caccia.Scale = 1.0f;
			caccia.Name = "Zona di Caccia";
			caccia.IsShortRange = true;
			SetBlipDisplay(caccia.Handle, 4);
			Client.GetInstance.RegisterTickHandler(ControlloCaccia);
		}

		public static async Task ControlloCaccia()
		{
			if (World.GetDistance(Game.PlayerPed.Position, Cacciatore.inizioCaccia.ToVector3()) < 1.375f)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu di caccia");
				if (Game.IsControlJustPressed(0, Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
					ApriMenuAffittoArmi();
			}
			await Task.FromResult(0);
		}

		public static async Task CacciaInCorso()
		{
			Ped animale = World.GetAllPeds().FirstOrDefault(x => animali.Contains((PedHash)x.Model.Hash));
			if (animale != null)
			{
				if (World.GetDistance(animale.Position, Cacciatore.zonaDiCaccia.ToVector3()) < Cacciatore.limiteArea)
				{
					KeyValuePair<int, Ped> KVAnimale = new KeyValuePair<int, Ped>(animale.Handle, animale);
					Debug.WriteLine("Hash animale = " + animale.Handle);
					if (!animaliTrovati.ContainsKey(KVAnimale.Key))
						animaliTrovati.Add(KVAnimale.Key, KVAnimale.Value);
				}
			}
			await BaseScript.Delay(500);
			if(World.GetDistance(Game.PlayerPed.Position, Cacciatore.zonaDiCaccia.ToVector3()) > Cacciatore.limiteArea)
			{
				if (affittatoBianca)
					BaseScript.TriggerServerEvent("lprp:removeWeapon", Bianca);
				if (affittatoFuoco)
					BaseScript.TriggerServerEvent("lprp:removeWeapon", DaFuoco);
				HUD.ShowNotification("Ti sei allontanato dalla zona di caccia senza aver restituito le armi! Pagherai una multa!", NotificationColor.Red, true);
				StaCacciando = false;
				BaseScript.TriggerServerEvent("lprp:removeBank", 1000);
				if (AreadiCaccia.Exists())
					AreadiCaccia.Delete();
				foreach (var p in animaliTrovati)
					foreach (var b in p.Value.AttachedBlips)
						b.Delete();
				foreach (string s in animalGroups)
					Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
				animaliTrovati.Clear();
				animaliUccisi.Clear();
				Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
				Client.GetInstance.DeregisterTickHandler(CacciaInCorso);
				Client.GetInstance.DeregisterTickHandler(CacciaInCorso1);
			}
		}

		public static async Task CacciaInCorso1()
		{
			foreach (var anim in animaliTrovati)
			{
				if (World.GetDistance(anim.Value.Position, Cacciatore.zonaDiCaccia.ToVector3()) < Cacciatore.limiteArea)
				{
					if (anim.Value.IsDead)
					{
						if (anim.Value.GetKiller() != null)
						{
							if (anim.Value.GetKiller() == Game.PlayerPed)
							{
								var hash = anim.Value.Model.Hash;
								float aggValore = 0;
								switch ((uint)hash)
								{
									case 3630914197: // deer
										if (IsPedMale(anim.Value.Handle))
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
										aggValore = 0.004f;
										break;
									case 2864127842: // aquila
										aggValore = 0.004f;
										break;
									case 307287994: // leone di montagna
										aggValore = 0.005f;
										break;
								}
								StatsNeeds.RegistraStats(Skills.HUNTING, aggValore);
								if (!animaliUccisi.ContainsKey(anim.Key))
									animaliUccisi.Add(anim.Key, anim.Value);
							}
						}
						animaliTrovati.Remove(anim.Key);
					}
				}
			}
			
			foreach (var anim in animaliUccisi)
			{
				if (World.GetDistance(Game.PlayerPed.Position, anim.Value.Position) < 2 && anim.Value.Model.Hash != (int)PedHash.MountainLion)
				{
					if (Game.PlayerPed.Weapons.HasWeapon(WeaponHash.Knife))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per saccheggiare il cadavere");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							Game.PlayerPed.Weapons.Select(WeaponHash.Knife);

							TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
							Screen.Fading.FadeOut(500);
							await BaseScript.Delay(501);
							var hash = anim.Value.Model.Hash;
							Screen.Fading.FadeIn(500);
							await BaseScript.Delay(501);
							string animale = "";
							float aggValore = 0;
							switch ((uint)hash)
							{
								case 3630914197: // deer
									if (IsPedMale(anim.Value.Handle))
									{
										animale = " Cervo";
										aggValore = 0.002f;
									}
									else
									{
										animale = " Cerva";
										aggValore = 0.001f;
									}
									break;
								case 3462393972: // boar
									animale = " Cinghiale";
									aggValore = 0.0012f;
									break;
								case 1682622302: // coyote
									animale = " Coyote";
									aggValore = 0.002f;
									break;
								case 3753204865: // coniglio
									animale = " Coniglio";
									aggValore = 0.003f;
									break;
								case 2864127842: // aquila
									animale = "'Aquila";
									aggValore = 0.003f;
									break;
								case 307287994: // leone di montagna
									aggValore = 0.003f;
									break;
							}
							StatsNeeds.RegistraStats(Skills.HUNTING, aggValore);
							HUD.ShowNotification($"Hai ucciso e squoiato un~y~{animale}~w~ hai ottenuto (inserire carne)", NotificationColor.GreenDark, true);
							anim.Value.Delete();
							animaliUccisi.Remove(anim.Key);
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
					if (item == armi)
					{
						if ((Eventi.Player.hasWeapon(DaFuoco) || affittatoFuoco) && (Eventi.Player.hasWeapon(Bianca) || affittatoBianca))
						{
							HUD.ShowNotification("Hai già le armi che noi affittiamo.", NotificationColor.Red, true);
							return;
						}
						int prezzo = 0;
						if (!Eventi.Player.hasWeapon(DaFuoco))
						{
							if (Eventi.Player.Money >= 250 || Eventi.Player.Bank >= 250)
							{
								Game.PlayerPed.Weapons.Give(WeaponHash.SniperRifle, 100, false, true);
								prezzo += 250;
								affittatoFuoco = true;
							}
						}
						if (!Eventi.Player.hasWeapon(Bianca))
						{
							if (Eventi.Player.Money >= 50 || Eventi.Player.Bank >= 50)
							{
								Game.PlayerPed.Weapons.Give(WeaponHash.Knife, 1, false, true);
								prezzo += 50;
								affittatoBianca = true;
							}
						}
						if (Eventi.Player.Money >= prezzo)
						{
							BaseScript.TriggerServerEvent("lprp:removeMoney", prezzo);
							HUD.ShowNotification("Le armi che non avevi già ti sono state date in affitto");
						}
						else
						{
							if (Eventi.Player.Bank >= prezzo)
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
						if (Eventi.Player.hasLicense("Caccia"))
						{
							StaCacciando = true;
							AreadiCaccia = World.CreateBlip(Cacciatore.zonaDiCaccia.ToVector3(), Cacciatore.limiteArea);
							AreadiCaccia.Name = "Zona di caccia";
							AreadiCaccia.Alpha = 25;
							AreadiCaccia.Color = BlipColor.TrevorOrange;
							foreach(string s in animalGroups)
								Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Dislike, true);
							Client.GetInstance.RegisterTickHandler(CacciaInCorso);
							Client.GetInstance.RegisterTickHandler(CacciaInCorso1);
							//AreadiCaccia.Sprite = BlipSprite.ShootingRange;
							SetBlipDisplay(AreadiCaccia.Handle, 4);
							HUD.MenuPool.CloseAllMenus();
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Hai iniziato a cacciare, non allontanarti dalla zona segnata in mappa o perderai le armi affittate e pagherai una multa!!", 5000);
							await BaseScript.Delay(10000);
							HUD.ShowHelp("Puoi cacciare gli animali che trovi nell'area, se hai il coltello e ti avvicini all'animale ucciso, puoi prenderne la carne!", 5000);
						}
						else
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
					
					if (affittatoBianca)
						BaseScript.TriggerServerEvent("lprp:removeWeapon", Bianca);
					if (affittatoFuoco)
						BaseScript.TriggerServerEvent("lprp:removeWeapon", DaFuoco);
					HUD.ShowNotification("Grazie di aver scelto il nostro servizio di gestione caccia!\nTorna presto!", NotificationColor.GreenDark);
					StaCacciando = false;
					Client.GetInstance.DeregisterTickHandler(CacciaInCorso);
					if (AreadiCaccia.Exists())
						AreadiCaccia.Delete();
					foreach (var p in animaliTrovati)
						foreach (var b in p.Value.AttachedBlips)
							b.Delete();
					foreach (string s in animalGroups)
						Game.PlayerPed.RelationshipGroup.SetRelationshipBetweenGroups(new RelationshipGroup(Funzioni.HashInt(s)), Relationship.Neutral, true);
					animaliTrovati.Clear();
					animaliUccisi.Clear();
					Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
					HUD.MenuPool.CloseAllMenus();
				};
			}
			affittoArmi.Visible = true;
		}
	}
}
