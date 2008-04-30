using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Compiler;

namespace littleRunner
{
    public class Script
    {
        private PythonEngine engine;
        private World world;


        public void GlobalsAdd(string name, object obj)
        {
            try
            {
                engine.Globals.Add(name, obj);
            }
            catch(Exception e)
            {
                DebugInfo.WriteException(e);
                throw new littleRunnerScriptVariablesException("That (Script-)Name exists already.");
            }
        }
        public void Execute(string command)
        {
            engine.Execute(command);
        }

        public Script(World world)
        {
            InitializePythonEngine();

            this.world = world;
        }


        public void callFunction(string name, string function, params object[] args)
        {
            Dictionary<object, object> handlers = (Dictionary<object, object>)engine.Globals["handler"];
            if (handlers.ContainsKey(name) &&
                ((Dictionary<object, object>)handlers[name]).ContainsKey(function))
            {
                engine.Globals["args"] = args;
                engine.Execute("handler." + name + "." + function + "(*args)");
            }
        }


        void InitializePythonEngine()
        {
            engine = new PythonEngine();

            string script = Encoding.UTF8.GetString(Properties.Resources.Script);
            engine.Execute(script);
        }
    }
}
