using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfConnectPaysbuy.getmypsb_wcf
{
    public class RequestData_getmypsb_wcf
    {
        [MessageHeader]
        public string psbID;	
        public string username;	
        public string secureCode; 	
        public string command;
        public string cardNumber; 	
        public string cellumRefID;
        public string psbTypeID; 	
        public string psbPackageID; 	
        public string reference1;
        public string amount;
        public string bankTrxid;
        public string clientId;
    }
    public class ResponseData_getmypsb_wcf
    {
        [MessageBodyMember]
        public DataContract_getmypsb_wcf Obj;
    }
}