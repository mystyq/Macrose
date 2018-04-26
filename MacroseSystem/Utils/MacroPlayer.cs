using Macrose.Enums;
using Macrose.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Threading;

namespace Macrose.Utils
{
    public static class MacroPlayer
    {
        private static DispatcherTimer _timer = new DispatcherTimer();
        private static Macro _macro;
        private static Queue<MacroCommand> _macroCommandQueue;

        static MacroPlayer()
        {
            _timer.Tick += new EventHandler(TimerTick);
        }

        public static void SetMacro(Macro macro)
        {
            _macro = macro;
        }

        public static void PlayMacro()
        {
            if (_macro == null) return;

            _timer.Stop();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            _macroCommandQueue = new Queue<MacroCommand>(_macro.MacroCommands);
            _timer.Start();
        }

        private static void TimerTick(object sender, EventArgs e)
        {
            if (_macroCommandQueue.Count > 0)
            {
                var macroCommand = _macroCommandQueue.Dequeue();
                if (!macroCommand.Event.Equals(MacroEvent.KeyPress))
                {
                    WindowsAPI.GetWindowRect(WindowsAPI.GetForegroundWindow(), out WindowsAPI.RECT rect);
                    Point point = (Point)macroCommand.Data;
                    int x = point.X + rect.Left;
                    int y = point.Y + rect.Top;
                    WindowsAPI.SetCursorPos(x, y);
                    int mouseEvent = macroCommand.Event.Equals(MacroEvent.MouseLeft)
                        ? (WindowsAPI.MOUSEEVENTF_LEFTDOWN | WindowsAPI.MOUSEEVENTF_LEFTUP) 
                        : (WindowsAPI.MOUSEEVENTF_RIGHTDOWN | WindowsAPI.MOUSEEVENTF_RIGHTUP);
                    WindowsAPI.mouse_event(mouseEvent, 0, 0, 0, 0);
                }
                else
                {
                    byte keyCode = Convert.ToByte(macroCommand.Data);
                    WindowsAPI.keybd_event(keyCode, 0x45, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
                    WindowsAPI.keybd_event(keyCode, 0x45, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
                }
                _timer.Interval = new TimeSpan(0, 0, 0, 0, macroCommand.Delay);
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
