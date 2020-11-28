using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.Telefono.Models
{
    public abstract class App
    {
        public string Name { get; set; }
        public int Icon { get; set; }
		int SelectedItem;
		public bool OverrideBack { get; set; }
        public Phone Phone { get; set; }

        public App(string name, int icon, Phone phone, bool overrideBack = true)
        {
            Name = name;
            Icon = icon;
            Phone = phone;
            OverrideBack = overrideBack;
        }

        public abstract Task Tick();

        public abstract void Initialize(Phone phone);

        public abstract void Kill();

	}
}
