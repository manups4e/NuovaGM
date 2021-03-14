
RegisterServerEvent("lprp:chiamaConfigServer")
AddEventHandler("lprp:chiamaConfigServer", function()
	TriggerEvent("lprp:RiceviConfig", Config)
end)

Citizen.CreateThread(function()
	print("^2Configurazione caricata^7")
	TriggerEvent("lprp:RiceviConfig", Config)
end)