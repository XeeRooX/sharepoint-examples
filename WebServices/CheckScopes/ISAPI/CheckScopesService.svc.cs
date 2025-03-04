using CheckScopes.Common;
using CheckScopes.Dtos;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace CheckScopes.ISAPI
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CheckScopesService : ICheckScopesService
    {
        public ScopeInfoOut GetScopeInfoSingleton()
        {
            var obj =  SingletonClass.Instance();
            var hashCode = obj.GetHashCode().ToString();

            return new ScopeInfoOut { HashOfObject = hashCode };
        }

        public ScopeInfoOut GetScopeInfoStatic()
        {
            var hashCode =  StaticClass.staticObject.GetHashCode().ToString();
            return new ScopeInfoOut { HashOfObject = hashCode };
        }

        public ScopeInfoOut GetScopeInfoTransient()
        {
            var obj = new object();
            var hashCode = obj.GetHashCode().ToString();

            return new ScopeInfoOut { HashOfObject = hashCode };
        }
    }
}