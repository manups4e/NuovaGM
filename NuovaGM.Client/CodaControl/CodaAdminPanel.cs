using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using static CitizenFX.Core.Native.API;
namespace TheLastPlanet.Client.CodaControl
{
    enum SessionState
    {
        Coda,
        Grazia,
        Caricamento,
        Attivo,
    }

    enum Reserved
    {
        Public
    }

    static class CodaAdminPanel
	{
		internal static string resourceName = GetCurrentResourceName();
        public static bool pannelloCodaAperto = false;

		public static void Init()
		{
			Client.Instance.AddEventHandler("onResourceStop", new Action<string>(OnResourceStop));
            Client.Instance.AddEventHandler("lprp:coda: sessionResponse", new Action<string>(SessionResponse));
            Client.Instance.RegisterNuiEventHandler("ClosePanel", new Action<IDictionary<string, object>>(ClosePanel));
            Client.Instance.RegisterNuiEventHandler("RefreshPanel", new Action<IDictionary<string, object>>(RefreshPanel));
            Client.Instance.RegisterNuiEventHandler("KickUser", new Action<IDictionary<string, object>>(KickUser));
            Client.Instance.RegisterNuiEventHandler("ChangePriority", new Action<IDictionary<string, object>>(ChangePriority));

        }

        private static void OnResourceStop(string name)
        {
            try
            {
                if (name == resourceName)
                { SetNuiFocus(false, false); pannelloCodaAperto = false;  return; }
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - OnResourceStop()");
            }
        }

        private static void SessionResponse(string session)
        {
            try
            {
                if (!pannelloCodaAperto)
                {
                    List<dynamic> sessionAccounts = session.Deserialize<List<dynamic>>();
                    string text = "";
                    sessionAccounts.ForEach(k =>
                    {
                        text = $"{text}<tr>" +
                        $"<td>{k["Handle"]}</td>" +
                        $"<td>{k["License"]}</td>" +
                        $"<td>{k["Discord"]}</td>" +
                        $"<td>{k["Steam"]}</td>" +
                        $"<td>{k["Name"]}</td>" +
                        $"<td>{k["Priority"]}</td>" +
                        $"<td>{Enum.GetName(typeof(SessionState), (int)k["State"])}</td>" +
                        $"<td><button class=button onclick=Change('{k["License"]}')>Modifica</button></td>" +
                        $"</tr>";
                    });
                    Funzioni.SendNuiMessage(new {sessionlist = text});
                    Debug.WriteLine(text);
                    SetNuiFocus(true, true);
                    pannelloCodaAperto = true;
                }
                else
                    HUD.ShowNotification("Pannello coda già aperto!!", NotificationColor.Red, true);
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - SessionResponse()");
            }
        }

        private static void ClosePanel(IDictionary<string, object> data)
        {
            try
            {
                SetNuiFocus(false, false);
                Funzioni.SendNuiMessage(new { panel = "close" });
                pannelloCodaAperto = false;
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - ClosePanel()");
            }
        }

        private static void RefreshPanel(IDictionary<string, object> data)
        {
            try
            {
                SetNuiFocus(false, false);
                Funzioni.SendNuiMessage(new { panel = "close" });
                pannelloCodaAperto = false;
                ExecuteCommand("sessione");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - RefreshPanel()");
            }
        }

        private static void KickUser(IDictionary<string, object> data)
        {
            try
            {
                ExecuteCommand($"ckick {data["License"]}");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - KickUser()");
            }
        }

        private static void ChangePriority(IDictionary<string, object> data)
        {
            try
            {
                if (data["Value"] as string == "False")
                {
                    ExecuteCommand($"rimuovipriorita {data["License"]}");
                    return;
                }
                else
					ExecuteCommand($"daipriorita {data["License"]} {int.Parse(data["Value"].ToString())}");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - ChangePriority()");
            }
        }

    }
}
