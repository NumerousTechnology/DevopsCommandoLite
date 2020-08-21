using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DevopsCommandoLite.Terminal.Commands
{
    public class CommandResult
    {
        public bool IsSuccess { get; set;  }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
