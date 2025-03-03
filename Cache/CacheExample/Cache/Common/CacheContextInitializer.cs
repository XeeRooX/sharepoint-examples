using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheExample.Cache.Common
{
    public abstract class CacheContextInitializer
    {
        public abstract CacheContext Initialize();
    }
}
