using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_import_export
{
	public class ImportCEOGarage3 : ImportCEOGarage
	{
		public ImportCEOGarage3() : base()
		{
			Part = new GaragePart()
			{
				Garage1 = new Garage(255489, "imp_sm_13_cargarage_a"),
				Garage2 = new Garage(255745, "imp_sm_13_cargarage_b"),
				Garage3 = new Garage(256001, "imp_sm_13_cargarage_c"),
				ModShop = new GarageModShop(256257, "imp_sm_13_modgarage")
			};
			Style = new GarageStyle();
			Numbering = new GarageNumbering();
			Lighting = new GarageLighting();
		}
	}
}
