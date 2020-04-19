﻿using CitizenFX.Core;

namespace NuovaGM.Shared
{
	public class VeicoloLavorativoEAffitto
	{
		public Vehicle veicolo;
		public string Proprietario;

		public VeicoloLavorativoEAffitto(Vehicle veh, string proprietario)
		{
			veicolo = veh;
			Proprietario = proprietario;
		}
	}

	public class VeicoloPersonale
	{
		public bool IsVehicleStored;
		public string CharOwner;
		public string IdentifierProprietario;
		public string VehName;
		public string Targa;
		public string NomeIdProprietario;
		public int NAssicurazione;
		public Veicoli.Modifiche.VehProp Modifiche;

		public VeicoloPersonale(bool stored, string charOwner, string identifier, string nameveh, string plate, string nomeOwner, int assicurazione, Veicoli.Modifiche.VehProp modifiche)
		{
			IsVehicleStored = stored;
			CharOwner = charOwner;
			IdentifierProprietario = identifier;
			VehName = nameveh;
			Targa = plate;
			NomeIdProprietario = nomeOwner;
			NAssicurazione = assicurazione;
			Modifiche = modifiche;
		}
	}

	public class VeicoloPol
	{
		public string Plate;
		public int Model;
		public int Handle;

		public VeicoloPol(string plate, int model, int handle)
		{
			Plate = plate;
			Model = model;
			Handle = handle;
		}
	}
}
