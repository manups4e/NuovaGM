fx_version 'adamant'--'bodacious' -- crash con bodacious serverside SIGSEGV
game 'gta5'

loadscreen_manual_shutdown 'yes'

files {
    'Client/Newtonsoft.Json.dll',
    'Client/AnySerializer.dll',
	"html/**/*",
}

client_scripts {
    "Client/TheLastPlanet.Client.net.dll",
}

server_scripts {
	--"CONFIGURAZIONE/**/*.lua",
	"Server/TheLastPlanet.Server.net.dll",
}

ui_page "html/thelastplanet.html"
loadscreen 'html/loadingscreen/loading.html'
