using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfConnectPaysbuy.MWA
{
    [DataContract]
    public class DataContract_MWA
    {
        [DataMember]
        public string tid { get; set; }
        [DataMember]
        public string respCode { get; set; }
        [DataMember]
        public string respMessage { get; set; }
        [DataMember]
        public string refId { get; set; }
       
    }
    [DataContract]
    public class DataContract_MWA_confirm
    {
        [DataMember]
        public string tid { get; set; }
        [DataMember]
        public string respCode { get; set; }
        [DataMember]
        public string respMessage { get; set; }
        [DataMember]
        public string refId { get; set; }
    }
}