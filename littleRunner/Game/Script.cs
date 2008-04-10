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
        private Dictionary<string, Dictionary<string, bool>> hasFunction;


        public void GlobalsAdd(string name, object obj)
        {
            engine.Globals.Add(name, obj);
        }
        public void Execute(string command)
        {
            engine.Execute(command);
        }

        public Script(World world)
        {
            InitializePythonEngine();
            hasFunction = new Dictionary<string, Dictionary<string, bool>>();
            this.world = world;
        }

        public void callFunction(string name, string function, params object[] args)
        {
            bool call = false;
            bool containName = hasFunction.ContainsKey(name);

            if (containName && hasFunction[name].ContainsKey(function))
            {
                // cached
                call = hasFunction[name][function];
            }
            else
            {
                engine.Execute("x = '" + name + "' in handler and '" + function + "' in handler." + name);
                if ((bool)engine.Globals["x"])
                    call = true;
                else
                    call = false;

                // name in cache, add function
                if (containName)
                    hasFunction[name][function] = call;
                else // nothing in cache, add everything
                {
                    Dictionary<string, bool> funcList = new Dictionary<string, bool>();
                    funcList.Add(function, call);
                    hasFunction.Add(name, funcList);
                }
            }

            if (call)
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
