Config.Client.Veicoli.DanniVeicoli = {
    -- [[  PARTE SUL CARBURANTE ]] --
    FuelCapacity = 65.0, -- in pratica.. il carburante va da 0 a 100% ma il calcolo è in percentuale a 65.0.. che storia eh?
    FuelRpmImpact = 0.0045, -- beh.. rpm sai cosa sono.. quanto impatta nel calcolo del consumo del motore?
    FuelAccelImpact = 0.0002, -- accelerazione come sopra..
    FuelTractionImpact = 0.0001, -- trazione del veicolo come sopra..
    trucks = { "HAULER", "PACKER", "PHANTOM" }, -- veicoli per il lavoro del fuel ( rifornimento stazioni )
    tanker = "TANKER", -- questa è la tanica.. se ne trovi una moddata e vuoi metterla... cambiala qui ;)
    maxtankerfuel = 2100, -- massimo carburante trasportabile dalla cisterna (litri)
    refuelCost = 0.85, -- ok se vuoi calcolare il costo del carburante per caricare la cisterna da lavoro.. il calcolo è maxtankerfuel * refuelCost quindi... in questo caso 2100 * 0.85 (circa 620)

    -- [[  PARTE SUI DANNI AL VEICOLO ]] --

    deformationMultiplier = -1,					-- Quanto dovrebbe il veicolo visivamente deformarsi da una collisione. Intervallo 0.0 a 10.0 Dove 0.0 non è una deformazione e 10.0 è una deformazione 10x. -1 = Non toccare. Il danno visivo non si sincronizza bene con altri giocatori.
    deformationExponent = 0.4,					-- Quanto dovrebbe essere compresso l'handling di deformazione verso 1,0. (Rendi le auto più simili). Un valore di 1 = nessuna modifica. Valori più bassi comprimono di più, i valori sopra 1 si espandono. Non impostare a zero o negativo.
    collisionDamageExponent = 0.6,				-- Quanto dovrebbe essere compresso l'handling di deformazione verso 1,0. (Rendi le auto più simili). Un valore di 1 = nessuna modifica. Valori più bassi comprimono di più, i valori sopra 1 si espandono. Non impostare a zero o negativo.
    damageFactorEngine = 10.0,					-- I valori sono da 1 a 100. Valori più alti significano più danni al veicolo. Un buon punto di partenza è 10
    damageFactorBody = 10.0,					-- I valori sono da 1 a 100. Valori più alti significano più danni al veicolo. Un buon punto di partenza è 10
    damageFactorPetrolTank = 64.0,				-- I valori sono da 1 a 200. Valori più alti significano più danni al veicolo. Un buon punto di partenza è 64
    engineDamageExponent = 0.6,					-- Quanto dovrebbe essere compresso l'handling di deformazione verso 1,0. (Rendi le auto più simili). Un valore di 1 = nessuna modifica. Valori più bassi comprimono di più, i valori sopra 1 si espandono. Non impostare a zero o negativo.
    weaponsDamageMultiplier = 0.01,				-- Quanti danni deve subire il veicolo dal fuoco delle armi. Intervallo 0.0 a 10.0, dove 0.0 non è danno e 10.0 è 10x danno. -1 = non toccare, lo script lascia fare al gioco
    degradingHealthSpeedFactor = 8,			    -- Velocità di salute degradazione della salute del veicolo, ma non spegnimento. Il valore di 10 significa che ci vorranno circa 0,25 secondi per punto di salute, quindi il degrado da 800 a 305 richiederà circa 1 minuto di guida pulita. Valori più alti significano una degradazione più rapida
    cascadingFailureSpeedFactor = 5.0,			-- Sane values are 1 to 100. When vehicle health drops below a certain point, cascading failure sets in, and the health drops rapidly until the vehicle dies. Higher values means faster failure. A good starting point is 5
    degradingFailureThreshold = 600.0,			-- Sotto questo valore, la degradazione della salute inizia.
    cascadingFailureThreshold = 500.0,			-- Al di sotto di questo valore, verrà impostato l'errore a cascata della salute (il motore iniziera a funzionare sempre peggio)
    engineSafeGuard = 80.0,		    			-- Valore di salute finale. Se impostato troppo alto il motore non fumerà da rotto. Settato troppo basso e la macchina prenderà fuoco con un solo proiettile. con valore 100 una macchina media impiega 3-4 proiettili al motore prima di prendere fuoco.
    torqueMultiplierEnabled = true,				-- Riduce la coppia (torque) del motore mentre il motore stesso degrada
    limpMode = false,							-- Se true, il motore non si spegne mai anche se il veicolo è danneggiato, in questo modo puoi ancora viaggiare a bassa velocità (a meno che il veicolo non sia capovolto e preventVehicleFlip è su true)
    limpModeMultiplier = 0.15,					-- Moltiplicatore di coppia da utilizzare quando il veicolo va piano piano. I valori sani sono da 0,05 a 0,25
    preventVehicleFlip = true,					-- Se è true, non puoi girare un veicolo capovolto
    sundayDriver = false,						-- Se true, la risposta dell'acceleratore viene ridimensionata per consentire una guida lenta e facile. Non impedirà il pieno gas. Non funziona con acceleratori binari come una tasto della tastiera.
    displayMessage = true,						-- Mostra un messaggino quando il motore si guasta
    compatibilityMode = false,					-- impedisce ad altri script di modificare la salute del serbatoio del carburante per evitare guasti al motore casuali.
    randomTireBurstInterval = 60,				-- Numero di minuti (statisticamente, non precisamente) da guidare sopra le 22 miglia orarie prima di ottenere una foratura. 0 = la funzione è disabilitata

    -- Moltiplicatore di danno in base alla Classe
    -- Il fattore di danno per motore, carrozzeria e serbatoio verrnno moltiplicati per questo valore, in base alla classe del veicolo
    -- Usalo per aumentare o diminuire il danno per ogni classe

    classDamageMultiplier = 
    {
        1.0,		--	0: Compacts
        1.0,		--	1: Sedans
        1.0,		--	2: SUVs
        1.0,		--	3: Coupes
        1.0,		--	4: Muscle
        1.0,		--	5: Sports Classics
        1.0,		--	6: Sports
        1.0,		--	7: Super
        0.25,		--	8: Motorcycles
        0.7,		--	9: Off-road
        0.25,		--	10: Industrial
        1.0,		--	11: Utility
        1.0,		--	12: Vans
        1.0,		--	13: Cycles
        0.5,		--	14: Boats
        1.0,		--	15: Helicopters
        1.0,		--	16: Planes
        1.0,		--	17: Service
        0.75,		--	18: Emergency
        0.75,		--	19: Military
        1.0,		--	20: Commercial
        1.0			--	21: Trains
    },
}