using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheExample.Cache.Common
{
    public class CacheContextBuilder
    {
        private CacheContextInitializer _initializer;
        public CacheContextBuilder ConfigureInitializer(Func<CacheContextConfiguration, CacheContextInitializer> configureAction)
        {
            _initializer = configureAction.Invoke(new CacheContextConfiguration());
            return this;
        }

        public CacheContext Build()
        {
            return _initializer.Initialize();
        }
    }
}
