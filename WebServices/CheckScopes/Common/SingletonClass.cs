using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckScopes.Common
{
    public class SingletonClass
    {
        private static SingletonClass _instance;
        private static readonly object _lock = new object();

        private SingletonClass()
        {
        }

        public static SingletonClass Instance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonClass();
                    }
                }
            }
           
            return _instance;
        }

    }
}
