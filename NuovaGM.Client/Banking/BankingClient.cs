using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

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

				if (World.GetDistance(Game.PlayerPed.Position, atmpos[i]) < 1.375f && !HUD.MenuPool.IsAnyMenuOpen())
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per prelevare / depositare soldi");
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						BankMenu();
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
	}
}
