using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    public class GameSession
    {
        private Dictionary<string, Dictionary<string, object>> data;

        public object Get(string level, string key)
        {
            if (!data.ContainsKey(level) || !data[level].ContainsKey(key))
                return null;

            return data[level][key];
        }
        public void Set(string level, string key, object value)
        {
            if (!data.ContainsKey(level))
                data[level] = new Dictionary<string, object>();

            data[level][key] = value;
        }


        public GameSession()
        {
            data = new Dictionary<string, Dictionary<string, object>>();
        }
    }
}
