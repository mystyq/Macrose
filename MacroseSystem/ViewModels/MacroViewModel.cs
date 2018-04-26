using Gma.System.MouseKeyHook;
using Macrose.Models;
using Macrose.Utils;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Macrose.ViewModels
{
    public class MacroViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private IKeyboardMouseEvents _hook;
        private DelegateCommand _openMacroCommand;
        private DelegateCommand _setMacroHotkeyCommand;
        private DelegateCommand _deleteMacroCommand;
        private BinaryFormatter _binaryFormatter;
        private Macro _macro;

        public MacroViewModel() 
            : base(string.Empty)
        {
            this.FileName = "Browse for macro and set hotkey -> [...]";
            _binaryFormatter = new BinaryFormatter();
        }

        public DelegateCommand OpenMacroCommand
        {
            get
            {
                if (_openMacroCommand == null)
                {
                    _openMacroCommand = new DelegateCommand(x => OpenMacro());
                }
                return _openMacroCommand;
            }
        }

        public DelegateCommand SetMacroHotkeyCommand
        {
            get
            {
                if (_setMacroHotkeyCommand == null)
                {
                    _setMacroHotkeyCommand = new DelegateCommand(x => SetMacroHotkey());
                }
                return _setMacroHotkeyCommand;
            }
        }

        public DelegateCommand DeleteMacroCommand
        {
            get
            {
                if (_deleteMacroCommand == null)
                {
                    _deleteMacroCommand = new DelegateCommand(x => DeleteMacro());
                }
                return _deleteMacroCommand;
            }
        }

        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private string _hotkey = "Set Hotkey";
        public string Hotkey
        {
            get
            {
                return _hotkey;
            }
            set
            {
                _hotkey = value;
                OnPropertyChanged("Hotkey");
            }
        }

        private void OpenMacro()
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName) == ".macro")
                    {
                        try
                        {
                            using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                            {
                                _macro = (Macro)_binaryFormatter.Deserialize(fs);
                                this.FileName = ofd.SafeFileName;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Could not load macro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DeleteMacro()
        {
            if (_hook != null) _hook.Dispose();
            OnPropertyChanged("Delete");
        }

        private void SetMacroHotkey()
        {
            this.Hotkey = "[Press any key]";
            if (_hook != null) _hook.Dispose();
            _hook = Hook.GlobalEvents();
            void keyDownEventHandler(object sender, KeyEventArgs e)
            {
                _hook.KeyDown -= keyDownEventHandler;
                this.Hotkey = e.KeyCode.ToString();
                e.Handled = true;
                _hook.KeyDown += OnKeyDown;
            }
            _hook.KeyDown += keyDownEventHandler;
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode.ToString().Equals(this.Hotkey))
            {
                args.SuppressKeyPress = true;
                args.Handled = true;
                this.PlayMacro();
            }
        }

        private void PlayMacro()
        {
            if (_macro == null) return;

            MacroPlayer.SetMacro(_macro);
            MacroPlayer.PlayMacro();
        }

        protected override void CreateCommands()
        {
        }

        public void Dispose()
        {
            if (_hook != null) _hook.Dispose();
        }
    }
}
