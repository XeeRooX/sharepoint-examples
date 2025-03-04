using CheckScopes.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CheckScopes.ISAPI
{
    [ServiceContract]
    interface ICheckScopesService
    {
        [OperationContract(Name = "ScopeInfoTransient")]
        [WebGet(UriTemplate = "scope/transient", ResponseFormat = WebMessageFormat.Json)]
        ScopeInfoOut GetScopeInfoTransient();

        [OperationContract(Name = "ScopeInfoStatic")]
        [WebGet(UriTemplate = "scope/static", ResponseFormat = WebMessageFormat.Json)]
        ScopeInfoOut GetScopeInfoStatic();

        [OperationContract(Name = "ScopeInfoSingleton")]
        [WebGet(UriTemplate = "scope/singleton", ResponseFormat = WebMessageFormat.Json)]
        ScopeInfoOut GetScopeInfoSingleton();
    }
}
