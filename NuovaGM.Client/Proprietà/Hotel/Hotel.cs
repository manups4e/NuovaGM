using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.Proprietà.Hotel
{
	public class Hotel
	{
		public string Name;
		public Vector3 Coords;
		public PrezzoHotel Prezzi = new PrezzoHotel();
	}

	public class PrezzoHotel
	{
		public int StanzaPiccola;
		public int StanzaMedia;
		public int Appartamento;
	}
}
