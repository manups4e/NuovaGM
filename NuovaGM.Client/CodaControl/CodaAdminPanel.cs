using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Dynamic;
using static CitizenFX.Core.Native.API;
namespace NuovaGM.Client.CodaControl
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
            Client.Instance.AddEventHandler("lprp:coda: sessionResponse", new Action<ExpandoObject>(SessionResponse));
            Client.Instance.RegisterNuiEventHandler("ClosePanel", new Action<ExpandoObject>(ClosePanel));
            Client.Instance.RegisterNuiEventHandler("RefreshPanel", new Action<ExpandoObject>(RefreshPanel));
            Client.Instance.RegisterNuiEventHandler("KickUser", new Action<ExpandoObject>(KickUser));
            Client.Instance.RegisterNuiEventHandler("ChangePriority", new Action<ExpandoObject>(ChangePriority));

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

        private static void SessionResponse(dynamic session)
        {
            try
            {
                if (!pannelloCodaAperto)
                {
                    List<dynamic> sessionAccounts = JsonConvert.DeserializeObject<List<dynamic>>(session);
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

        private static void ClosePanel(dynamic data)
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

        private static void RefreshPanel(dynamic data)
        {
            try
            {
                ClosePanel("");
                ExecuteCommand("sessione");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - RefreshPanel()");
            }
        }

        private static void KickUser(dynamic data)
        {
            try
            {
                ExecuteCommand($"ckick {data.License}");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - KickUser()");
            }
        }

        private static void ChangePriority(dynamic data)
        {
            try
            {
                if (data.Value == "False")
                {
                    ExecuteCommand($"rimuovipriorita {data.License}");
                    return;
                }
                else
                    ExecuteCommand($"daipriorita {data.License} {int.Parse(data.Value.ToString())}");

            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName} - Admin_Panel] - ChangePriority()");
            }
        }

    }
}
