using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfConnectPaysbuy.MBA;
using WcfConnectPaysbuy.MWA;
using WcfConnectPaysbuy.get_ws_dtac_wcf;
using WcfConnectPaysbuy.getmypsb_wcf;

namespace WcfConnectPaysbuy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        ResponseData_MBA mea_request_query(RequestData_MBA obj);

        [OperationContract]
        ResponseData_MBA_confirm mea_request_confirm(RequestData_MBA_confirm obj);  
       
        [OperationContract]
        ResponseData_MWA mwa_request_query(RequestData_MWA obj);

        [OperationContract]
        ResponseData_MWA_confirm mwa_request_confirm(RequestData_MWA_confirm obj);

        [OperationContract]
        ResponseData_get_ws_dtac postpaid_request_query(RequestData_get_ws_dtac obj);

        [OperationContract]
        ResponseData_get_ws_dtac_confirm postpaid_request_confirm(RequestData_get_ws_dtac_confirm obj);

        [OperationContract]
        ResponseData_getmypsb_wcf InsertMypsbTransaction(RequestData_getmypsb_wcf obj);
        // TODO: Add your service operations here
    }

}
