using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfConnectPaysbuy.getmypsb_wcf
{
    [DataContract]
    public class DataContract_getmypsb_wcf
    {

        [DataMember]
        public string status ;
        [DataMember]
        public string description;
        [DataMember]
        public string cellumRefID;
        [DataMember]
        public string paysbuyRefID;
        [DataMember]
        public string payTypeID;
        [DataMember]
        public string payProvider;
        [DataMember]
        public string payReference;
        [DataMember]
        public string payAmount;


    }
}