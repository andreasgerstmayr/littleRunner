using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Compiler;

using littleRunner.Gamedata.Worlddata;

namespace littleRunner
{
    public class Script
    {
        private static PythonEngine mainEngine;
        private static List<string> savedGlobals;
        private PythonEngine engine;
        private World world;
        public bool Init;


        public void GlobalsAdd(string name, object obj)
        {
            try
            {
                engine.Globals.Add(name, obj);
                Script.savedGlobals.Add(name);
            }
            catch (Exception e)
            {
                DebugInfo.WriteException(e);
                throw new littleRunnerScriptVariablesException("That (Script-)Name exists already.");
            }
        }
        public void GlobalsAdd(string name)
        {
            try
            {
                Script.savedGlobals.Add(name);
            }
            catch (Exception e)
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
            this.Init = true;
            InitializePythonEngine();

            this.world = world;
        }


        public void callFunction(string name, string function, params object[] args)
        {
            if (Init)
                return;

            lock (this)
            {
                if (!engine.Globals.ContainsKey("handler"))
                    return;


                Dictionary<object, object> handlers = (Dictionary<object, object>)engine.Globals["handler"];
                if (handlers.ContainsKey(name) &&
                    ((Dictionary<object, object>)handlers[name]).ContainsKey(function))
                {
                    engine.Globals["args"] = args;
                    try
                    {
                        engine.Execute("handler." + name + "." + function + "(*args)");
                    }
                    catch (Exception e)
                    {
                        DebugInfo.WriteException(e);
                        throw new littleRunnerScriptFunctionException("Error calling '" + name + "." + function + "' handler");
                    }
                }

            }
        }


        void InitializePythonEngine()
        {
            if (Script.mainEngine == null || Script.savedGlobals == null)
            {
                Script.mainEngine = new PythonEngine();
                Script.savedGlobals = new List<string>();
            }
            else
            {
                foreach (string g in Script.savedGlobals)
                    Script.mainEngine.Globals.Remove(g);
                Script.savedGlobals.Clear();
            }


            engine = Script.mainEngine;

            string script = Encoding.UTF8.GetString(Properties.Resources.Script);
            engine.Execute(script);
        }
    }
}
