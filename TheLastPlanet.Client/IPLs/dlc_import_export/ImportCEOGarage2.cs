using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_import_export
{
	public class ImportCEOGarage2 : ImportCEOGarage
	{
		public ImportCEOGarage2() : base()
		{
			Part = new GaragePart()
			{
				Garage1 = new Garage(254465, "imp_dt1_11_cargarage_a"),
				Garage2 = new Garage(254721, "imp_dt1_11_cargarage_b"),
				Garage3 = new Garage(254977, "imp_dt1_11_cargarage_c"),
				ModShop = new GarageModShop(255233, "imp_dt1_11_modgarage")
			};
			Style = new GarageStyle();
			Numbering = new GarageNumbering();
			Lighting = new GarageLighting();
		}
	}
}
