using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WcfConnectPaysbuy.MWA
{
    public class RequestData_MWA
    {
        [MessageHeader]
        public string secureCode;
        public string tid;
        public string taxID;
        public string suffix; 
        public string ref1;
        public string ref2; 
        public decimal amount;
        public string name;
        public string address; 
        public string email;
        public string mobileNo;

    }
    public class ResponseData_MWA
    {
        [MessageBodyMember]
        public DataContract_MWA Obj;
    }

    public class RequestData_MWA_confirm
    {
        [MessageHeader]
        public string secureCode;
        public string tid;
        public string refID;
        public decimal amount;
        public decimal fee;
        public string confirmStatus;
        public string cardsNo;
        public string bankTrxid;
        public string clientId;

    }
    public class ResponseData_MWA_confirm
    {
        [MessageBodyMember]
        public DataContract_MWA_confirm Obj;
    }

}