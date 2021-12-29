using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_import_export
{
	public class ImportCEOGarage4 : ImportCEOGarage
	{
		public ImportCEOGarage4() : base()
		{
			Part = new GaragePart()
			{
				Garage1 = new Garage(256513, "imp_sm_15_cargarage_a"),
				Garage2 = new Garage(256769, "imp_sm_15_cargarage_b"),
				Garage3 = new Garage(257025, "imp_sm_15_cargarage_c"),
				ModShop = new GarageModShop(257281, "imp_sm_15_modgarage")
			};
			Style = new GarageStyle();
			Numbering = new GarageNumbering();
			Lighting = new GarageLighting();
		}
	}
}
