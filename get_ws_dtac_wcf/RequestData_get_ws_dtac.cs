using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfConnectPaysbuy.get_ws_dtac_wcf
{
    public class RequestData_get_ws_dtac
    {
        [MessageHeader]
        public string secureCode;	
        public string paymentChannel;
        public string paymentType; 	
        public string tid;
        public decimal amt; 	
        public string mobileNo; 	
        public string email;
    }
    public class ResponseData_get_ws_dtac
    {
        [MessageBodyMember]
        public DataContract_get_ws_dtac Obj;
    }
    //-------------------------------------------------------------
    public class RequestData_get_ws_dtac_confirm
    {
        [MessageHeader]
        public string secureCode;
        public string tid;
        public string refID; 
        public decimal amt; 
        public string confirmStatus;
        public string bankTrxid;
        public string clientId;

    }
    public class ResponseData_get_ws_dtac_confirm
    {
        [MessageBodyMember]
        public DataContract_get_ws_dtac_confirm Obj;
    }
}