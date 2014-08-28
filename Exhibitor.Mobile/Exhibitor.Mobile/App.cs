using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Exhibitor.Mobile
{
    public class App
    {
        public static Page GetMainPage()
        {
            ServiceLocator.Current.SetService<MainForm>(new MainForm());
            return ServiceLocator.Current.GetService<MainForm>();


        }
    }
}
