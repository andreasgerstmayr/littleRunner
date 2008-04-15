using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    class littleRunnerException : ApplicationException
    {
        public littleRunnerException(string message)
            : base(message)
        {
        }
    }

    class littleRunnerScriptException : littleRunnerException
    {
        public littleRunnerScriptException(string message)
            : base(message)
        {
        }
    }

    class littleRunnerScriptVariablesException : littleRunnerScriptException
    {
        public littleRunnerScriptVariablesException(string message)
            : base(message)
        {
        }
    }

    class littleRunnerScriptFunctionException : littleRunnerScriptException
    {
        public littleRunnerScriptFunctionException(string message)
            : base(message)
        {
        }
    }
}
