using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public class vRPGEngineException : Exception
    {
        #region Fields
        private readonly string message;
        private readonly string caller;
        
        private readonly int line;
        #endregion

        public vRPGEngineException(string message, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null)
                : base(message + string.Format(" - at function/method: {0}, at line {1}", caller, line))
        {
            this.message    = message;
            this.caller     = caller;
            this.line       = line;
        }
        public vRPGEngineException([CallerLineNumber] int line = 0, [CallerMemberName] string caller = null)
                : this(string.Empty, line, caller)
        {
        }
    }
}
