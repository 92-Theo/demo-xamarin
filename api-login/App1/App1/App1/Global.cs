using System.Runtime.Serialization;

namespace App1
{
    [DataContract]
    public class ReturnCode
    {
        [DataMember]
        public string value { get; set; }
    }
}
