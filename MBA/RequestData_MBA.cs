using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WcfConnectPaysbuy.MBA
{
    public class RequestData_MBA
    {
        [MessageHeader]
        public string secureCode;
        public string tid;
        public string contractAC;
        public string email;
        public string mobileNo;
    }

    public class ResponseData_MBA
    {
        [MessageBodyMember]
        public DataContract_MBA Obj;
    }
    //-------------------------------------------------------------
    public class RequestData_MBA_confirm
    {
        [MessageHeader]
        public string secureCode;
        public string tid;
        public string refID;
        public decimal amt;
        public decimal fee;
        public string confirmStatus;
        public string cardsNo;
        public string clientId;
        public string origBankTrxId;
    }
    public class ResponseData_MBA_confirm
    {
        [MessageBodyMember]
        public DataContract_MBA_confirm Obj;
    }
}