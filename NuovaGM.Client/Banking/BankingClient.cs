using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System;
using static CitizenFX.Core.Native.API;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace NuovaGM.Client.Banking
{
	public class BankingClient
	{
		static List<dynamic> lista = new List<dynamic>() { "100", "200", "500", "1000", "2000", "5000", "10000", "20000", "50000", "Altro Importo" };

		static public List<Vector3> atmpos = new List<Vector3>() // DA DIVIDERE TRA FLEECA, MAZEBANK Ed eventuali 
        {
			new Vector3(-717.651f, -915.619f,  19.215f),
			new Vector3(147.657f, -1035.346f, 29.343f),
			new Vector3(146.091f, -1035.148f, 29.343f),
			new Vector3(-1315.867f, -834.832f,  16.961f),
			new Vector3(288.923f, -1256.765f,  29.441f),
			new Vector3(-56.838f, -1752.119f,  29.421f),
			new Vector3(-845.966f, -341.163f,  38.681f),
			new Vector3(1153.797f, -326.707f,  69.205f),
			new Vector3(1769.342f, 3337.526f,  41.433f),
			new Vector3(1769.801f, 3336.802f, 41.433f),
			new Vector3(174.312f, 6637.667f,  31.573f),
			new Vector3(-2538.903f, 2317.082f,  33.215f),
			new Vector3(-2538.834f, 2315.985f,  33.215f),
			new Vector3(2559.105f, 350.899f,  108.621f),
			new Vector3(-386.733f, 6045.953f, 31.501f),
			new Vector3(-283.0f, 6225.99f, 31.49f),
			new Vector3(-135.165f, 6365.738f, 31.101f),
			new Vector3(-110.753f, 6467.703f, 31.784f),
			new Vector3(-94.9690f, 6455.301f, 31.784f),
			new Vector3(155.4300f, 6641.991f, 31.784f),
			new Vector3(174.6720f, 6637.218f, 31.784f),
			new Vector3(1703.138f, 6426.783f, 32.730f),
			new Vector3(1735.114f, 6411.035f, 35.164f),
			new Vector3(1702.842f, 4933.593f, 42.051f),
			new Vector3(1967.333f, 3744.293f, 32.272f),
			new Vector3(1821.917f, 3683.483f, 34.244f),
			new Vector3(1174.532f, 2705.278f, 38.027f),
			new Vector3(540.0420f, 2671.007f, 42.177f),
			new Vector3(2564.399f, 2585.100f, 38.016f),
			new Vector3(2558.683f, 349.6010f, 108.050f),
			new Vector3(2558.051f, 389.4817f, 108.660f),
			new Vector3(1077.692f, -775.796f, 58.218f),
			new Vector3(1139.018f, -469.886f, 66.789f),
			new Vector3(1168.975f, -457.241f, 66.641f),
			new Vector3(1153.884f, -326.540f, 69.245f),
			new Vector3(381.2827f, 323.2518f, 103.270f),
			new Vector3(236.4638f, 217.4718f, 106.840f),
			new Vector3(265.0043f, 212.1717f, 106.780f),
			new Vector3(285.2029f, 143.5690f, 104.970f),
			new Vector3(157.7698f, 233.5450f, 106.450f),
			new Vector3(-164.568f, 233.5066f, 94.919f),
			new Vector3(-1827.04f, 785.5159f, 138.020f),
			new Vector3(-1409.39f, -99.2603f, 52.473f),
			new Vector3(-1205.35f, -325.579f, 37.870f),
			new Vector3(-1215.64f, -332.231f, 37.881f),
			new Vector3(-2072.41f, -316.959f, 13.345f),
			new Vector3(-2975.72f, 379.7737f, 14.992f),
			new Vector3(-2962.60f, 482.1914f, 15.762f),
			new Vector3(-2955.70f, 488.7218f, 15.486f),
			new Vector3(-3044.22f, 595.2429f, 7.595f),
			new Vector3(-3144.13f, 1127.415f, 20.868f),
			new Vector3(-3241.10f, 996.6881f, 12.500f),
			new Vector3(-3241.11f, 1009.152f, 12.877f),
			new Vector3(-1305.40f, -706.240f, 25.352f),
			new Vector3(-538.225f, -854.423f, 29.234f),
			new Vector3(-711.156f, -818.958f, 23.768f),
			new Vector3(-526.566f, -1222.90f, 18.434f),
			new Vector3(-256.831f, -719.646f, 33.444f),
			new Vector3(-203.548f, -861.588f, 30.205f),
			new Vector3(114.205f, -776.737f, 31.418f),
			new Vector3(111.021f, -775.579f, 31.439f),
			new Vector3(112.9290f, -818.710f, 31.386f),
			new Vector3(119.9000f, -883.826f, 31.191f),
			new Vector3(149.4551f, -1038.95f, 29.366f),
			new Vector3(-846.304f, -340.402f, 38.687f),
			new Vector3(-1204.35f, -324.391f, 37.877f),
			new Vector3(-1216.27f, -331.461f, 37.773f),
			new Vector3(-261.692f, -2012.64f, 30.121f),
			new Vector3(-273.001f, -2025.60f, 30.197f),
			new Vector3(314.187f, -278.621f, 54.170f),
			new Vector3(-351.534f, -49.529f, 49.042f),
			new Vector3(24.589f, -946.056f, 29.357f),
			new Vector3(-254.112f, -692.483f, 33.616f),
			new Vector3(-1570.197f, -546.651f, 34.955f),
			new Vector3(-1415.909f, -211.825f, 46.500f),
			new Vector3(-1430.112f, -211.014f, 46.500f),
			new Vector3(33.232f, -1347.849f, 29.497f),
			new Vector3(129.216f, -1292.347f, 29.269f),
			new Vector3(288.58f, -1282.28f, 29.659f),
			new Vector3(295.839f, -895.640f, 29.217f),
			new Vector3(1686.753f, 4815.809f, 42.008f),
			new Vector3(-302.408f, -829.945f, 32.417f),
			new Vector3(5.134f, -919.949f, 29.557f),
			new Vector3(89.69f, 2.38f, 68.31f)
		};

		static public List<Vector3> bankCoordsVaults = new List<Vector3>()
		{
			new Vector3(-105.929f, 6477.292f, 31.626f),
			new Vector3(254.509f, 225.887f, 101.875f),
			new Vector3(-2957.678f, 480.944f, 15.706f),
			new Vector3(146.997f, -1045.069f, 29.368f)
		};

		static public List<Vector3> cleanspotcoords = new List<Vector3>()
		{
			new Vector3(1274.053f, -1711.756f, 54.771f),
			new Vector3(-1096.847f, 4947.532f, 218.354f)
		};

		public static bool InterfacciaAperta = false;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(onPlayerSpawn));
			Client.GetInstance.RegisterEventHandler("lprp:banking:transactionstatus", new Action<bool, string>(Status));
		}

		static public async void onPlayerSpawn()
		{
			/*
				foreach (Vector3 b in atmpos)
				{
					Blip atmblip = new Blip(AddBlipForCoord(b.X, b.Y, b.Z));
					atmblip.Sprite = (BlipSprite)(277);
					atmblip.Color = BlipColor.Green;
					SetBlipDisplay(atmblip.Handle, 4);
					atmblip.Scale = 0.85f;
					atmblip.IsShortRange = true;
					atmblip.Name = "A.T.M";
					await Task.FromResult(0);
				}
				foreach (Vector3 b in bankCoords)
				{
					Blip bankblip = new Blip(AddBlipForCoord(b.X, b.Y, b.Z));
					bankblip.Sprite = BlipSprite.DollarSign;
					bankblip.Color = BlipColor.Green;
					SetBlipDisplay(bankblip.Handle, 4);
					bankblip.Scale = 0.8f;
					bankblip.IsShortRange = true;
					bankblip.Name = "Banca";
				}
				foreach (Vector3 b in cleanspotcoords)
				{
					Blip cleanerblips = new Blip(AddBlipForCoord(b.X, b.Y, b.Z));
					cleanerblips.Sprite = BlipSprite.DollarSign;
					cleanerblips.Color = BlipColor.Red;
					SetBlipDisplay(cleanerblips.Handle, 4);
					cleanerblips.Scale = 0.8f;
					cleanerblips.IsShortRange = true;
					cleanerblips.Name = "Riciclaggio Denaro Gangs";
				}
			*/
		}

		static public async Task Markers()
		{
			for (int i = 0; i < atmpos.Count; i++)
			{
				if (World.GetDistance(Game.PlayerPed.Position, atmpos[i]) < 100f)
				{
					World.DrawMarker(MarkerType.DollarSign, atmpos[i], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1.1f, 0.8f, 1.0f), Color.FromArgb(160, 0, 255, 190), false, false, true);
				}

				if (World.GetDistance(Game.PlayerPed.Position, atmpos[i]) < 1.375f && !HUD.MenuPool.IsAnyMenuOpen() && !InterfacciaAperta)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire il conto");
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						AttivaBanca();
//						BankMenu();
					}
				}
			}

			/*          foreach (Vector3 b in bankCoords)
						{
							if (World.GetDistance(Game.PlayerPed.Position, b) < 30f)
								World.DrawMarker(MarkerType.DollarSign, b, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1.1f, 1.1f, 1.6f), Color.FromArgb(160, 0, 255, 190), false, false, true);
							if (World.GetDistance(Game.PlayerPed.Position, b) < 1.375f)
							{
								Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire il tuo conto bancario");
								if (Game.IsControlJustPressed(0, Control.Context))
								{
									Funzioni.ShowNotification("PlaceHolder funzionante");
								}
							}
						}
			*/
			for (int i = 0; i < cleanspotcoords.Count; i++)
			{
				if (World.GetDistance(Game.PlayerPed.Position, cleanspotcoords[i]) < 60f)
				{
					World.DrawMarker(MarkerType.DollarSign, cleanspotcoords[i], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1.5f, 1.5f, 1.6f), System.Drawing.Color.FromArgb(120, 255, 0, 0), false, false, true);
				}

				if (World.GetDistance(Game.PlayerPed.Position, cleanspotcoords[i]) < 1.375f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per pulire i soldi");
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						HUD.ShowNotification("PlaceHolder sporchi funzionante");
					}
				}
			}
			await Task.FromResult(0);
		}

		static public async void BankMenu()
		{
			UIMenu Banca = new UIMenu(" ", "~y~Desanta Banking, Benvenuto!", new Point(0, 0), Main.Textures["Michael"].Key, Main.Textures["Michael"].Value);
			HUD.MenuPool.Add(Banca);
			int saldoBanca = Eventi.Player.Bank;

			UIMenuItem Saldo = new UIMenuItem("Saldo Bancario", "I tuoi soldi in banca");
			Saldo.SetRightLabel("~g~" + saldoBanca.ToString());
			Banca.AddItem(Saldo);
			UIMenuListItem Preleva = new UIMenuListItem("Preleva: ", lista, 0, "Quanto vuoi Prelevare?");
			UIMenuListItem Deposita = new UIMenuListItem("Deposita: ", lista, 0, "Quanto vuoi Depositare?");
			Banca.AddItem(Preleva);
			Banca.AddItem(Deposita);

			int valore;
			Banca.OnListSelect += async (_menu, _listItem, _itemIndex) =>
			{
				if (_listItem == Preleva)
				{
					if (Eventi.Player.Bank > 0)
					{
						string Item = (_listItem as UIMenuListItem).Items[_itemIndex].ToString();
						Debug.WriteLine(Item);
						if (Item != "Altro Importo")
						{
							valore = Convert.ToInt32(Item);
							Debug.WriteLine("valore = " + valore);
							BaseScript.TriggerServerEvent("lprp:banking:atmwithdraw", valore);
							if (valore <= Eventi.Player.Bank)
							{
								saldoBanca -= valore;
								Saldo.SetRightLabel("~g~" + saldoBanca);
							}
						}
						else
						{
							valore = Convert.ToInt32(await HUD.GetUserInput("Importo", "0", 10));
							BaseScript.TriggerServerEvent("lprp:banking:atmwithdraw", valore);
							if (valore <= Eventi.Player.Bank)
							{
								saldoBanca -= valore;
								Saldo.SetRightLabel("~g~" + saldoBanca);
							}
						}
					}
					else
					{
						HUD.ShowAdvancedNotification("De Santa Banking", "Conto in rosso", "Siamo spiacenti di informarla che non può prelevare con conto in rosso o in negativo.", "CHAR_BANK_MAZE", (IconType)2);
					}
				}
				if (_listItem == Deposita)
				{
					if (Eventi.Player.Money > 0)
					{
						string Item = (_listItem as UIMenuListItem).Items[_itemIndex].ToString();
						if (Item != "Altro Importo")
						{
							valore = Convert.ToInt32(Item);
							BaseScript.TriggerServerEvent("lprp:banking:atmdeposit", valore);
							if (valore <= Eventi.Player.Money)
							{
								saldoBanca += valore;
								Saldo.SetRightLabel("~g~" + saldoBanca);
							}
						}
						else
						{
							valore = Convert.ToInt32(await HUD.GetUserInput("Importo", "0", 10));
							BaseScript.TriggerServerEvent("lprp:banking:atmdeposit", valore);
							if (valore <= Eventi.Player.Money)
							{
								saldoBanca += valore;
								Saldo.SetRightLabel("~g~" + saldoBanca);
							}
						}
					}
					else
					{
						HUD.ShowAdvancedNotification("De Santa Banking", "Portafoglio Vuoto", "Siamo spiacenti ma lei NON dispone di denaro nel portafoglio.", "CHAR_BANK_MAZE", (IconType)2);
					}
				}
			};

			UIMenu Bonifico = HUD.MenuPool.AddSubMenu(Banca, "Bonifico", "Bonifico istantaneo verso chiunque");
			UIMenuItem destinatario = new UIMenuItem("Nome Intestatario", "A chi vuoi inviare i soldi?");
			UIMenuItem importo = new UIMenuItem("Importo da inviare", "Quanto??");
			UIMenuItem conferma = new UIMenuItem("~g~Invia~w~", "Confermi?");
			Bonifico.AddItem(destinatario);
			Bonifico.AddItem(importo);
			Bonifico.AddItem(conferma);

			int valoreBonifico = 0;
			string nome = "";
			Bonifico.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (_item == destinatario)
				{
					int result;
					nome = await HUD.GetUserInput("A chi vuoi inviare?", "Nome Cognome", 50);
					if (int.TryParse(nome, out result))
						HUD.ShowNotification("Devi inserire il nome valido di una persona!", NotificationColor.Red, true);
					else
					{
						if (nome.Length < 3)
							HUD.ShowNotification("Nome inserito troppo corto!", NotificationColor.Red, true);
						else if (!nome.Contains(" "))
							HUD.ShowNotification("Errore! Devi inserire Nome e Cognome del destinatario!", NotificationColor.Red, true);
						else
							destinatario.SetRightLabel(nome);
					}
				}
				if (_item == importo)
				{
					string lettere = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,!,\\,\", £,$,%,&,/,(,),=,?,',ì,^,è,*,é,+,à";
					string[] lettera = lettere.Split(',');
					string val = await HUD.GetUserInput("Quantità", "", 20);
					foreach (string a in lettera)
					{
						if (!val.All(o => char.IsDigit(o)))
							HUD.ShowNotification("Errore! Devi inserire solo numeri!!");
					}
					if (int.TryParse(val, out int i))
					{
						valoreBonifico = Convert.ToInt32(val);
						importo.SetRightLabel(val + "$");
					}
				}
				if (_item == conferma)
				{
					if (Eventi.Player.Bank >= valoreBonifico)
					{
						BaseScript.TriggerServerEvent("lprp:banking:sendMoney", nome, valoreBonifico);
						Saldo.SetRightLabel("~g~" + (saldoBanca - valoreBonifico));
					}
					else
						HUD.ShowAdvancedNotification("De Santa Banking", "Conto in rosso", "Siamo spiacenti di informarla che non può prelevare con conto in rosso o in negativo.", "CHAR_BANK_MAZE", (IconType)2);
				}
			};
			Banca.Visible = true;
			await Task.FromResult(0);
		}

		static public void Status(bool success, string msg)
		{
			if (success)
			{
				HUD.ShowNotification("Transazione Completata!\nIl tuo nuovo Saldo bancario è di ~b~" + msg + "$", NotificationColor.GreenLight);
			}
			else
			{
				HUD.ShowNotification(msg);
			}
		}

		private static Scaleform atm = new Scaleform("ATM");
		private static int currentSelection = 0;
		private static int MenuAttuale = 0;
		private static int iLocal_674 = 0;
		private static int iLocal_675 = 0;
		private static float fLocal_591 = -1f;
		private static float fLocal_592 = 0;
		private static int SoldiTransazione;
		private static string Destinatario;



		private static async void AttivaBanca()
		{
			StartAudioScene("ATM_PLAYER_SCENE");
			atm = new Scaleform("ATM");
			MenuAttuale = 0;
			currentSelection = 0;
			TryBankingNew(true, 0);
			Client.GetInstance.RegisterTickHandler(ControlliBank);
			InterfacciaAperta = true;
		}

		private async static Task ControlliBank()
		{
			int iVar0 = 0;
			int iVar1 = 0;
			float fVar2;
			DisableAllControlActions(0);
			DisableAllControlActions(1);
			DisableAllControlActions(2);
			Game.EnableControlThisFrame(2, Control.FrontendPauseAlternate);
			Game.EnableControlThisFrame(2, Control.FrontendUp);
			Game.EnableControlThisFrame(2, Control.FrontendLeft);
			Game.EnableControlThisFrame(2, Control.FrontendDown);
			Game.EnableControlThisFrame(2, Control.FrontendRight);
			Game.EnableControlThisFrame(2, Control.CursorAccept);
			Game.EnableControlThisFrame(2, Control.CursorCancel);


			if (Game.IsControlJustPressed(2, Control.FrontendUp))
			{
				atm.CallFunction("SET_INPUT_EVENT", 8);
				Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
			}
			if (Game.IsControlJustPressed(2, Control.FrontendDown))
			{
				atm.CallFunction("SET_INPUT_EVENT", 9); 
				Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
			}
			if (Game.IsControlJustPressed(2, Control.FrontendLeft))
			{
				atm.CallFunction("SET_INPUT_EVENT", 10);
				Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
			}
			if (Game.IsControlJustPressed(2, Control.FrontendRight))
			{
				atm.CallFunction("SET_INPUT_EVENT", 11);
				Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
			}

			if (IsInputDisabled(2))
			{
				Game.EnableControlThisFrame(2, Control.CursorX);
				Game.EnableControlThisFrame(2, Control.CursorY);
				Game.EnableControlThisFrame(2, Control.CursorScrollUp);
				Game.EnableControlThisFrame(2, Control.CursorScrollDown);
				float fVar0;
				float fVar1;

				if (fLocal_591 == -1f)
				{
					fLocal_591 = Game.GetControlNormal(2, Control.CursorX);
					fLocal_592 = Game.GetControlNormal(2, Control.CursorY);
				}
				else if (fLocal_591 != Game.GetControlNormal(2, Control.CursorX) || fLocal_592 != Game.GetControlNormal(2, Control.CursorY))
				{
					atm.CallFunction("SHOW_CURSOR", true);
				}
				ShowCursorThisFrame();

				fVar0 = Game.GetControlNormal(2, Control.CursorX);
				fVar1 = Game.GetControlNormal(2, Control.CursorY);

				if (((fVar0 >= 0f && fVar0 <= 1f) && fVar1 >= 0f) && fVar1 <= 1f)
					atm.CallFunction("SET_MOUSE_INPUT", fVar0, fVar1);

				fVar2 = 1f + (10f * Timestep());
				if (Game.IsControlPressed(2, Control.CursorScrollDown) || Game.IsControlPressed(2, Control.FrontendDown))
					iVar1 = -200;
				if (Game.IsControlPressed(2, Control.CursorScrollUp) || Game.IsControlPressed(2, Control.FrontendUp))
					iVar1 = 200;
				atm.CallFunction("SET_ANALOG_STICK_INPUT", 0f, 0f, iVar1 * fVar2);
			}
			else
			{
				atm.CallFunction("SHOW_CURSOR", false);
				fLocal_591 = -1f;
				Game.EnableControlThisFrame(2, Control.FrontendRightAxisX);
				Game.EnableControlThisFrame(2, Control.FrontendRightAxisY);
				iVar0 = Game.GetControlValue(0, Control.FrontendRightAxisX) - 128;
				iVar1 = Game.GetControlValue(0, Control.FrontendRightAxisY) - 128;
				if (iVar0 < 10 && iVar0 > -10)
					iVar0 = 0;
				if (iVar1 < 10 && iVar1 > -10)
					iVar1 = 0;
				fVar2 = 1f + (10f * Timestep());
				if (iLocal_674 != iVar0 || iLocal_675 != iVar1)
				{
					atm.CallFunction("SET_ANALOG_STICK_INPUT", 0f, -iVar0 * fVar2, -iVar1 * fVar2);
					iLocal_674 = iVar0;
					iLocal_675 = iVar1;
				}
			}

			if (Game.IsControlJustPressed(2, Control.FrontendAccept) || Game.IsControlJustPressed(2, Control.CursorAccept))
			{
				atm.CallFunction("SET_INPUT_SELECT");
				BeginScaleformMovieMethod(atm.Handle, "GET_CURRENT_SELECTION");
				int ind = EndScaleformMovieMethodReturn();
				while (!IsScaleformMovieMethodReturnValueReady(ind))
				{
					atm.Render2D();
					await BaseScript.Delay(0);
				}
				currentSelection = GetScaleformMovieFunctionReturnInt(ind);
				Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
				switch (MenuAttuale)
				{
					case 0: // menu principale
						switch (currentSelection)
						{
							case 0:
								TryBankingNew(false, 0); // torna al menu principale
								MenuAttuale = 0;
								break;
							case 1:
								TryBankingNew(false, 1); // ritira
								MenuAttuale = 1;
								break;
							case 2: 
								TryBankingNew(false, 2); // deposita
								MenuAttuale = 2;
								break;
							case 3:
								TryBankingNew(false, 3); // giroconto
								MenuAttuale = 3;
								break;
							case 4:
								TryBankingNew(false, 4); // lista transazioni
								MenuAttuale = 4;
								break;
						}
						break;
					case 1: // ritira
						switch (currentSelection)
						{
							case 1: // 50
								if (Eventi.Player.Money >= 50)
								{
									TryBankingNew(false, 5, 50);
									MenuAttuale = 5;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 2: // 100
								if (Eventi.Player.Money >= 100)
								{
									TryBankingNew(false, 5, 100);
									MenuAttuale = 5;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 3: // 200
								if (Eventi.Player.Money >= 200)
								{
									TryBankingNew(false, 5, 200);
									MenuAttuale = 5;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 5: // 500
								if (Eventi.Player.Money >= 500)
								{
									TryBankingNew(false, 5, 500);
									MenuAttuale = 5;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 6: // 1000
								if (Eventi.Player.Money >= 1000)
								{
									TryBankingNew(false, 5, 1000);
									MenuAttuale = 5;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 7: // personalizzato
								string valore = await HUD.GetUserInput("Inserisci il valore che desideri ritirare", "", Eventi.Player.Bank.ToString().Length);
								if (valore != "")
								{
									if (valore.All(o => char.IsDigit(o)))
									{
										if (Eventi.Player.Bank >= Convert.ToInt32(valore))
										{
											TryBankingNew(false, 5, Convert.ToInt32(valore));
											MenuAttuale = 5;
										}
										else
										{
											TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
											MenuAttuale = 1;
										}
									}
									else
									{
										TryBankingNew(false, 13, 0, "Devi inserire solo numeri!");
										MenuAttuale = 1;
									}
								}
								else
								{
									TryBankingNew(false, 13, 0, "Devi inserire almeno una cifra!");
									MenuAttuale = 1;
								}
								break;
						}
						break;
					case 2: // deposita
						switch (currentSelection)
						{
							case 1: // 50
								if (Eventi.Player.Money >= 50)
								{
									TryBankingNew(false, 6, 50);
									MenuAttuale = 6;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
									MenuAttuale = 2;
								}
								break;
							case 2: // 100
								if (Eventi.Player.Money >= 100)
								{
									TryBankingNew(false, 6, 100);
									MenuAttuale = 6;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
									MenuAttuale = 2;
								}
								break;
							case 3: // 200
								if (Eventi.Player.Money >= 200)
								{
									TryBankingNew(false, 6, 200);
									MenuAttuale = 6;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
									MenuAttuale = 2;
								}
								break;
							case 5: // 500
								if (Eventi.Player.Money >= 500)
								{
									TryBankingNew(false, 6, 500);
									MenuAttuale = 6;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
									MenuAttuale = 2;
								}
								break;
							case 6: // 1000
								if (Eventi.Player.Money >= 1000)
								{
									TryBankingNew(false, 6, 1000);
									MenuAttuale = 6;
								}
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
									MenuAttuale = 2;
								}
								break;
							case 7: // personalizzato
								string valore = await HUD.GetUserInput("Inserisci il valore che desideri depositare", "", Eventi.Player.Bank.ToString().Length);
								if (valore != "")
								{
									if (valore.All(o => char.IsDigit(o)))
									{
										if (Eventi.Player.Money >= Convert.ToInt32(valore))
										{
											TryBankingNew(false, 6, Convert.ToInt32(valore));
											MenuAttuale = 6;
										}
										else
										{
											TryBankingNew(false, 13, 0, "Non hai abbastanza soldi addosso!");
											MenuAttuale = 2;
										}
									}
									else
									{
										TryBankingNew(false, 13, 0, "Devi inserire solo numeri!");
										MenuAttuale = 2;
									}
								}
								else
								{
									TryBankingNew(false, 13, 0, "Devi inserire almeno una cifra!");
									MenuAttuale = 2;
								}
								break;

						}
						break;

					case 3: // GIROCONTO
						int soldi = 0;
						switch (currentSelection)
						{
							case 1: // 50
								if (Eventi.Player.Money >= 50)
									soldi = 50;
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 2: // 100
								if (Eventi.Player.Money >= 100)
									soldi = 100;
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 3: // 200
								if (Eventi.Player.Money >= 200)
									soldi = 200;
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 5: // 500
								if (Eventi.Player.Money >= 500)
									soldi = 500;
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 6: // 1000
								if (Eventi.Player.Money >= 1000)
									soldi = 1000;
								else
								{
									TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
									MenuAttuale = 1;
								}
								break;
							case 7: // personalizzato
								string valore = await HUD.GetUserInput("Inserisci il valore che desideri ritirare", "", Eventi.Player.Bank.ToString().Length);
								if (valore != "")
								{
									if (valore.All(o => char.IsDigit(o)))
									{
										if (Eventi.Player.Bank >= Convert.ToInt32(valore))
											soldi = Convert.ToInt32(valore);
										else
										{
											TryBankingNew(false, 13, 0, "Non hai abbastanza soldi sul conto!");
											MenuAttuale = 1;
										}
									}
									else
									{
										TryBankingNew(false, 13, 0, "Devi inserire solo numeri!");
										MenuAttuale = 1;
									}
								}
								else
								{
									TryBankingNew(false, 13, 0, "Devi inserire almeno una cifra!");
									MenuAttuale = 1;
								}
								break;
						}
						if (soldi != 0)
						{
							string destinatario = await HUD.GetUserInput("A chi vuoi trasferire i soldi?", "", 100);
							if (destinatario.Length < 3)
							{
								TryBankingNew(false, 13, 0, "Nome inserito troppo corto!");
								break;
							}
							else if (!destinatario.Contains(" "))
							{
								TryBankingNew(false, 13, 0, "Errore! Devi inserire Nome e Cognome del destinatario separati da uno spazio!");
								break;
							}
							else if (destinatario.Any(o => char.IsDigit(o)))
							{
								TryBankingNew(false, 13, 0, "Errore! I nomi non possono contenere numeri!");
								break;
							}
							TryBankingNew(false, 9, soldi, "", "", destinatario);
							MenuAttuale = 9;
						}
						break;
					case 4: // log
						break;
					case 5: // ritiro
						switch(currentSelection)
						{
							case 1:
								TryBankingNew(false, 7, 0, "", "atmwithdraw"); // ritira
								break;
							case 2:
								TryBankingNew(false, 1); // ritira
								break;
						}
						MenuAttuale = 0;
						break;
					case 6:
						switch (currentSelection)
						{
							case 1:
								TryBankingNew(false, 7, 0, "", "atmdeposit"); // deposita
								break;
							case 2:
								TryBankingNew(false, 2); // deposita
								break;
						}
						MenuAttuale = 0;
						break;
					case 9:
						switch (currentSelection)
						{
							case 1:
								TryBankingNew(false, 10, 0, "", "sendMoney"); // invia
								break;
							case 2:
								TryBankingNew(false, 3);
								break;
						}
						MenuAttuale = 0;
						break;
				}
			}
			if (Game.IsControlJustPressed(2, Control.FrontendCancel) || Game.IsControlJustPressed(2, Control.CursorCancel))
			{
				if (MenuAttuale == 0)
				{
					Game.PlaySound("PIN_BUTTON", "ATM_SOUNDS");
					atm.Dispose();
					Client.GetInstance.DeregisterTickHandler(ControlliBank);
					StopAudioScene("ATM_PLAYER_SCENE");
					InterfacciaAperta = false;
				}
				else
				{
					switch (MenuAttuale)
					{
						case 1:
						case 2:
						case 3:
						case 4:
							MenuAttuale = 0;
							break;
						case 5:
							MenuAttuale = 1;
							break;
						case 6:
							MenuAttuale = 2;
							break;
						case 7:
							MenuAttuale = 3;
							break;
					}
					TryBankingNew(false, MenuAttuale);
				}
			}
			atm.Render2D(); // qui si mostra nel suo splendore!
		}

		private static async void TryBankingNew(bool firstload, int menu, int soldi = 0, string messaggio = "", string evento = "", string destinatario = "")
		{
			while (!atm.IsLoaded) await BaseScript.Delay(0);

			if (firstload)
			{
				atm.CallFunction("enterPINanim");
				atm.CallFunction("pinBeep");
			}

			atm.CallFunction("SET_DATA_SLOT_EMPTY");

			switch (menu)
			{
				case 0:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_SER");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					AddText("MPATM_DIDM");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					AddText("MPATM_WITM");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(3);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Giroconto");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(4);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Registro");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MENU");
					break;
				case 1:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_WITMT");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(50, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(100, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(3);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(200, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(5);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(500, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(6);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(1000, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(7);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Personalizzato");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_CASH_OPTIONS");
					break;
				case 2:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_DITMT");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(50, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(100, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(3);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(200, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(5);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(500, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(6);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(1000, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(7);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Personalizzato");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_CASH_OPTIONS");
					break;
				case 3:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Seleziona l'ammontare da trasferire");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(50, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(100, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(3);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(200, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(5);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(500, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(6);
					BeginTextCommandScaleformString("ESDOLLA");
					AddTextComponentFormattedInteger(1000, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(7);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform("Personalizzato");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_CASH_OPTIONS");
					break;
				case 4:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_LOG");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					AddText("MPATM_BACK");
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_TRANSACTIONS");
					break;

				case 5: // Conferma ritiro
					SoldiTransazione = soldi;
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					BeginTextCommandScaleformString("MPATC_CONFW");
					AddTextComponentFormattedInteger(soldi, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					AddText("MO_YES");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					AddText("MO_NO");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "DISPLAY_MESSAGE");
					EndScaleformMovieMethod();
					break;
				case 6: // Conferma deposito
					SoldiTransazione = soldi;
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					BeginTextCommandScaleformString("MPATM_CONF");
					AddTextComponentFormattedInteger(soldi, true);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					AddText("MO_YES");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					AddText("MO_NO");
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MESSAGE");
					break;
				case 7: // ATTESA
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_PEND");
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MESSAGE");
					
					await BaseScript.Delay(Funzioni.GetRandomInt(2500, 4500));

					BaseScript.TriggerServerEvent("lprp:banking:"+evento, SoldiTransazione);
					SoldiTransazione = 0;
					TryBankingNew(false, 13, 0, GetLabelText("MPATM_TRANCOM"));
					MenuAttuale = 0;
					currentSelection = 0;
					break;
				case 9:
					SoldiTransazione = soldi;
					Destinatario = destinatario;
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform($"Desideri trasferire ${soldi} a {destinatario}?");
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(1);
					AddText("MO_YES");
					EndScaleformMovieMethod();

					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(2);
					AddText("MO_NO");
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MESSAGE");
					break;
				case 10:
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					AddText("MPATM_PEND");
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MESSAGE");

					await BaseScript.Delay(Funzioni.GetRandomInt(2500, 4500));

					BaseScript.TriggerServerEvent("lprp:banking:" + evento, Destinatario, SoldiTransazione);
					SoldiTransazione = 0;
					Destinatario = "";
					TryBankingNew(false, 13, 0, GetLabelText("MPATM_TRANCOM"));
					MenuAttuale = 0;
					currentSelection = 0;
					break;
				case 13: // messaggio di errore personalizzato
					BeginScaleformMovieMethod(atm.Handle, "SET_DATA_SLOT");
					ScaleformMovieMethodAddParamInt(0);
					BeginTextCommandScaleformString("STRING");
					AddTextComponentScaleform(messaggio);
					EndTextCommandScaleformString();
					EndScaleformMovieMethod();

					atm.CallFunction("DISPLAY_MESSAGE");

					await BaseScript.Delay(2000);
					TryBankingNew(false, 0);
					break;
			}

			BeginScaleformMovieMethod(atm.Handle, "DISPLAY_BALANCE");
			PushScaleformMovieMethodParameterButtonName(Eventi.Player.FullName);
			AddText("MPATM_ACBA");
			PushScaleformMovieMethodParameterButtonName(Eventi.Player.Bank.ToString());
			EndScaleformMovieMethod();
		}

		static void AddText(string text)
		{
			BeginTextCommandScaleformString(text);
			EndTextCommandScaleformString();
		}
	}
}
