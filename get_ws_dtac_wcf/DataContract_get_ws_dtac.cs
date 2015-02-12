using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfConnectPaysbuy.get_ws_dtac_wcf
{
    [DataContract]
    public class DataContract_get_ws_dtac
    {
        [DataMember]
        public string tid;
        [DataMember]
        public string respCode;
        [DataMember]
        public string respMessage;
        [DataMember]
        public string min;
        [DataMember]
        public string max;
        [DataMember]
        public string total;
        [DataMember]
        public string balance;
        [DataMember]
        public string reconnect;
        [DataMember]
        public string refId;
    }
    [DataContract]
    public class DataContract_get_ws_dtac_confirm
    {
        [DataMember]
        public string tid;
        [DataMember]
        public string respCode;
        [DataMember]
        public string respMessage;
        [DataMember]
        public string refId;
    }
}