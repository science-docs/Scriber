using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public delegate object CommandExecution(CompilerState state, object[] arguments);

    public class Command
    {
        public string Name { get; set; }
        public CommandExecution Execution { get; set; }
    }
}
