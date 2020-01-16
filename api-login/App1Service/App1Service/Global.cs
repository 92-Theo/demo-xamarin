using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace App1Service
{
    [DataContract]
    public class ReturnCode
    {
        [DataMember]
        public string value { get; set; }
    }
}