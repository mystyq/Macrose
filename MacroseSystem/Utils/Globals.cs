using System;
using System.Windows;
using System.Windows.Threading;

namespace Macrose.Utils
{
    public static class Globals
    {
        public static volatile bool processing = false;

        public static void DoEvents()
        {
            if (Application.Current == null) return;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => { }));
        }
    }
}
