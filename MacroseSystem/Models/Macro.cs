using System;
using System.Collections.Generic;

namespace Macrose.Models
{
    [Serializable]
    public class Macro
    {
        public Macro(List<MacroCommand> macroCommands)
        {
            MacroCommands = macroCommands;
        }

        public List<MacroCommand> MacroCommands { get; set; }
    }
}
