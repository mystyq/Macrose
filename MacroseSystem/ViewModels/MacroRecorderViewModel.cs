using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Macrose.Enums;
using Macrose.Models;

namespace Macrose.ViewModels
{
    public class MacroRecorderViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private bool _isRecording = false;
        private IKeyboardMouseEvents _hook;
        private List<MacroCommand> _macroCommands;
        private Macro _macro;

        public MacroRecorderViewModel()
            : base(string.Empty)
        {
            _hook = Hook.GlobalEvents();
            _hook.OnCombination(new Dictionary<Combination, Action>
            {
                { Combination.FromString("F2"), RecordMacro }
            });
            _macro = new Macro(new List<MacroCommand>());
        }

        private void RecordMacro()
        {
            _isRecording ^= true;
            if (_isRecording)
            {
                _macroCommands = new List<MacroCommand>();
                _hook.MouseClick += MouseClicked;
                _hook.KeyDown += KeyDown;
            }
            else
            {
                _hook.MouseClick -= MouseClicked;
                _hook.KeyDown -= KeyDown;
                if (_macroCommands.Count > 0)
                {
                    Macro macro = new Macro(_macroCommands);
                    _macro = macro;
                    OnPropertyChanged("MacroCommands");
                }
            }
        }

        private void MouseClicked(object sender, MouseEventArgs args)
        {
            WindowsAPI.GetWindowRect(WindowsAPI.GetForegroundWindow(), out WindowsAPI.RECT rect);
            Point point = new Point(args.X - rect.Left, args.Y - rect.Top);
            MacroCommand macroCommand = new MacroCommand()
            {
                Data = point,
                Delay = 30
            };
            if (args.Button.Equals(MouseButtons.Left))
            {
                macroCommand.Event = MacroEvent.MouseLeft;
            }
            else if (args.Button.Equals(MouseButtons.Right))
            {
                macroCommand.Event = MacroEvent.MouseRight;
            }
            _macroCommands.Add(macroCommand);
        }

        private void KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode.ToString().Equals("F2")) return;

            MacroCommand macroCommand = new MacroCommand
            {
                Delay = 30,
                Event = MacroEvent.KeyPress,
                Data = args.KeyData
            };
            _macroCommands.Add(macroCommand);
        }

        public List<MacroCommand> MacroCommands
        {
            get
            {
                return this._macro.MacroCommands;
            }
            set
            {
                this._macro.MacroCommands = value;
                OnPropertyChanged("MacroCommands");
            }
        }

        private void SaveMacro()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                sfd.DefaultExt = "macro";
                sfd.Filter = "Macro|*.macro";
                sfd.FilterIndex = 1;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(memoryStream, _macro);
                        File.WriteAllBytes(sfd.FileName, memoryStream.ToArray());
                    }
                }
            }
        }

        private void DeleteMacro()
        {
            _macro = new Macro(new List<MacroCommand>());
            OnPropertyChanged("MacroCommands");
        }

        public void Dispose()
        {
            if (_hook != null) _hook.Dispose();
        }

        protected override void CreateCommands()
        {
            Commands.Add(new CommandViewModel("Save", null, new DelegateCommand(x => SaveMacro(), x => _macro.MacroCommands.Count > 0)));
            Commands.Add(new CommandViewModel("Delete", null, new DelegateCommand(x => DeleteMacro(), x => _macro.MacroCommands.Count > 0)));
        }
    }
}
