using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Compiler;

namespace littleRunner
{
    class Script
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
                engine.Execute("x = '" + name + "' in handler and '" + function + "' in handler['" + name + "']");
                if ((bool)engine.Globals["x"])
                    call = true;
                else
                    call = false;

                // add to cache
                if (containName)
                    hasFunction[name][function] = call;
                else
                {
                    Dictionary<string, bool> funcList = new Dictionary<string, bool>();
                    funcList.Add(function, false);
                    hasFunction.Add(name, funcList);
                }
            }

            if (call)
                engine.Execute("handler['"+name+"']['"+function + "'](*args)");
        }

        void InitializePythonEngine()
        {
            engine = new PythonEngine();

            engine.Execute("handler = {}");
            /*
            engine.Globals.Add("en", x);
            engine.Execute("import clr");
            engine.Execute("clr.AddReference(\"Test\")");
            engine.Execute("from Test import BlaNum");
            engine.Execute("print type(BlaNum.Blau)");
            engine.Execute("print type(en)");
            engine.Execute("print BlaNum.Gruen == en");
            */
        }
    }
}
