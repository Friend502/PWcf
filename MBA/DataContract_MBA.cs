using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfConnectPaysbuy.MBA
{
        [DataContract]
        public class DataContract_MBA
        {
            [DataMember]
            public string tid { get; set; }
            [DataMember]
            public string respCode { get; set; }
            [DataMember]
            public string respMessage { get; set; }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public string address { get; set; }
            [DataMember]
            public string ref1 { get; set; }
            [DataMember]
            public string dueDate { get; set; }
            [DataMember]
            public string amount { get; set; }
            [DataMember]
            public string invoiceDate { get; set; }
            [DataMember]
            public string discountAmount { get; set; }
            [DataMember]
            public string refId { get; set; }
        }
        [DataContract]
        public class DataContract_MBA_confirm
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