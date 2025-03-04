using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CheckScopes.Dtos
{
    [DataContract]
    public class ScopeInfoOut
    {
        [DataMember]
        public string HashOfObject { get; set; }
    }
}
