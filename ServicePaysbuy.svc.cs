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
using WcfConnectPaysbuy.MBAReference;
using WcfConnectPaysbuy.get_ws_dtacReference;
using WcfConnectPaysbuy.getmypsb;
using WcfConnectPaysbuy.getmypsb_wcf;
using System.Xml;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace WcfConnectPaysbuy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {

        dbAccess dba = new dbAccess();

        getTopping cell01 = new getTopping();
        get_ws_dtac cell02 = new get_ws_dtac();
        getMyPSB cell03 = new getMyPSB();
        
        ResponseData_MBA r = new ResponseData_MBA();
        returnMeaRequest[] datalist = new returnMeaRequest[0];

        ResponseData_MBA_confirm rconfirm = new ResponseData_MBA_confirm();
        returnMeaConfirm[] datalistconfirm = new returnMeaConfirm[0];

        ResponseData_MWA rmwaselect = new ResponseData_MWA();
        returnMwaRequest[] dataMwarequest = new returnMwaRequest[0];

        ResponseData_MWA_confirm rconfirm_mwa = new ResponseData_MWA_confirm();
        returnMwaConfirm[] datalistconfirm_mwa = new returnMwaConfirm[0];

        ResponseData_get_ws_dtac rget_ws_dtac = new ResponseData_get_ws_dtac();
        get_ws_dtacReference.returnPostpaidRequest[] datalistget_ws_dtac = new get_ws_dtacReference.returnPostpaidRequest[0];

        ResponseData_get_ws_dtac_confirm rget_ws_dtac_confirm = new ResponseData_get_ws_dtac_confirm();
        get_ws_dtacReference.returnPostpaidConfirm[] datalistget_ws_data_confirm = new get_ws_dtacReference.returnPostpaidConfirm[0];

        ResponseData_getmypsb_wcf rgetmypsb = new ResponseData_getmypsb_wcf();
        ClientInsertMypsbTransaction datalistgetmypsb = new ClientInsertMypsbTransaction();


        public ResponseData_MBA mea_request_query(RequestData_MBA obj)// ดูข้อมูลใน MBA
        {
            try
            {
                r.Obj = new DataContract_MBA();
                obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                //obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";
                obj.contractAC = "013486887";

                datalist = cell01.mea_request_query(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                    obj.tid == null ? "" : obj.tid.Trim(),
                                                    obj.contractAC == null ? "" : obj.contractAC.Trim(),
                                                    obj.email == null ? "" : obj.email.Trim(),
                                                    obj.mobileNo == null ? "" : obj.mobileNo.Trim());
                r.Obj.tid = datalist[0].tid;
                r.Obj.respCode = datalist[0].respCode;
                r.Obj.respMessage = datalist[0].respMessage;
                r.Obj.name = datalist[0].name;
                r.Obj.address = datalist[0].address;
                r.Obj.dueDate = datalist[0].dueDate;
                r.Obj.amount = datalist[0].amount;
                r.Obj.invoiceDate = datalist[0].invoiceDate;
                r.Obj.discountAmount = datalist[0].discountAmount;
                r.Obj.ref1 = datalist[0].ref1;
                r.Obj.refId = datalist[0].refId;
            }
            catch
            {
                r.Obj.tid = obj.tid == null ? "" : obj.tid.Trim();
                r.Obj.respCode = "1000";
                r.Obj.respMessage = "Server Error";
                r.Obj.name = "";
                r.Obj.address = "";
                r.Obj.dueDate = "";
                r.Obj.amount = "";
                r.Obj.invoiceDate = "";
                r.Obj.discountAmount = "";
                r.Obj.ref1 = "";
                r.Obj.refId = "";
            }

            return r;
        }

        public ResponseData_MBA_confirm mea_request_confirm(RequestData_MBA_confirm obj) //confirm การชำระเงิน Cellue
        {
            try
            {
                rconfirm.Obj = new DataContract_MBA_confirm();
                //obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                ////obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";

                if ((obj.refID == null ? "" : obj.refID.Trim()) != "" 
                    || (obj.origBankTrxId == null ? "" : obj.origBankTrxId.Trim()) != ""
                    || (obj.clientId == null ? "" : obj.clientId.Trim()) != "")
                {
                    datalistconfirm = cell01.mea_request_confirm(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                            obj.tid == null ? "" : obj.tid.Trim(),
                                                            obj.refID == null ? "" : obj.refID.Trim(),
                                                            Convert.ToDouble(obj.amt) == 0 ? 0 : obj.amt,
                                                            Convert.ToDouble(obj.fee) == 0 ? 0 : obj.fee,
                                                            obj.confirmStatus == null ? "" : obj.confirmStatus.Trim(),
                                                            obj.cardsNo == null ? "" : obj.cardsNo.Trim(),
                                                            obj.origBankTrxId == null ? "" : obj.origBankTrxId.Trim());

                    using (var conn = dba.dbConnect())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("updata_psb_mea_api_cf_operation_wcf", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300; // Increase this to allow the proc longer to run
                            cmd.Parameters.AddWithValue("@refID", obj.refID);
                            cmd.Parameters.AddWithValue("@bankTrxId", obj.origBankTrxId == null ? "" : obj.origBankTrxId.Trim());
                            cmd.Parameters.AddWithValue("@clientId", obj.clientId == null ? "" : obj.clientId.Trim());

                            SqlDataReader dr = cmd.ExecuteReader();
                            dr.Close();
                        }
                        conn.Close();
                    }
                }
                else
                {
                    rconfirm.Obj.refId = "";
                    rconfirm.Obj.respCode = "9902";
                    rconfirm.Obj.respMessage = "Invalid null";
                    rconfirm.Obj.tid = "";
                    return rconfirm;
                }

                rconfirm.Obj.refId = datalistconfirm[0].refId;
                rconfirm.Obj.respCode = datalistconfirm[0].respCode;
                rconfirm.Obj.respMessage = datalistconfirm[0].respMessage;
                rconfirm.Obj.tid = datalistconfirm[0].tid;

            }
            catch
            {
                rconfirm.Obj.refId = "";
                rconfirm.Obj.respCode = "1000";
                rconfirm.Obj.respMessage = "Server Error";
                rconfirm.Obj.tid = obj.tid == null ? "" : obj.tid.Trim();
            }
            return rconfirm;
        }

        public ResponseData_MWA mwa_request_query(RequestData_MWA obj) //เก็บข้อมุล MWA
        {
            rmwaselect.Obj = new  DataContract_MWA();
            try
            {
                obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                //obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";
                dataMwarequest = cell01.mwa_request_query(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                          obj.tid == null ? "" : obj.tid.Trim(),
                                                          obj.taxID == null ? "" : obj.taxID.Trim(),
                                                          obj.suffix == null ? "" : obj.suffix.Trim(),
                                                          obj.ref1 == null ? "" : obj.ref1.Trim(),
                                                          obj.ref2 == null ? "" : obj.ref2.Trim(),
                                                          obj.amount == 0 ? 0 : obj.amount,
                                                          obj.name == null ? "" : obj.name.Trim(),
                                                          obj.address == null ? "" : obj.address,
                                                          obj.email == null ? "" : obj.email.Trim(),
                                                          obj.mobileNo == null ? "" : obj.mobileNo.Trim());
                rmwaselect.Obj.tid = dataMwarequest[0].tid;
                rmwaselect.Obj.refId = dataMwarequest[0].refId;
                rmwaselect.Obj.respCode = dataMwarequest[0].respCode;
                rmwaselect.Obj.respMessage = dataMwarequest[0].respMessage;
            }
            catch
            {
                rmwaselect.Obj.tid = obj.tid == null ? "" : obj.tid.Trim();
                rmwaselect.Obj.refId = "";
                rmwaselect.Obj.respCode = "1000";
                rmwaselect.Obj.respMessage = "Server Error";
            }

            return rmwaselect;
        }

        public ResponseData_MWA_confirm mwa_request_confirm(RequestData_MWA_confirm obj)
        {
            try
            {
                obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                //obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";
                rconfirm_mwa.Obj = new DataContract_MWA_confirm();
               

                string clientId = obj.clientId == null ? "" : obj.clientId.Trim();

                if ((obj.refID == null ? "" : obj.refID.Trim())!= "" 
                    || (obj.bankTrxid == null ? "" : obj.bankTrxid) != "" 
                    || (obj.clientId == null ? "" : obj.clientId.Trim()) != "")
                {
                    datalistconfirm_mwa = cell01.mwa_request_confirm(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                                obj.tid == null ? "" : obj.tid.Trim(),
                                                                obj.refID == null ? "" : obj.refID.Trim(),
                                                                obj.amount == 0 ? 0 : obj.amount,
                                                                obj.fee == 0 ? 0 : obj.fee,
                                                                obj.confirmStatus == null ? "" : obj.confirmStatus,
                                                                obj.cardsNo == null ? "" : obj.cardsNo,
                                                                obj.bankTrxid == null ? "" : obj.bankTrxid);

                    using (var conn = dba.dbConnect())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("updata_psb_mwa_api_cf_operation_wcf", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300; // Increase this to allow the proc longer to run
                            cmd.Parameters.AddWithValue("@refID", datalistconfirm_mwa[0].refId);
                            cmd.Parameters.AddWithValue("@bankTrxId", obj.bankTrxid == null ? "" : obj.bankTrxid);
                            cmd.Parameters.AddWithValue("@clientId", obj.clientId == null ? "" : obj.clientId.Trim());

                            SqlDataReader dr = cmd.ExecuteReader();
                            dr.Close();
                        }
                        conn.Close();
                    }
                }
                else
                {
                    rconfirm_mwa.Obj.refId = "";
                    rconfirm_mwa.Obj.respCode = "9902";
                    rconfirm_mwa.Obj.respMessage = "Invalid null";
                    rconfirm_mwa.Obj.tid = "";
                    return rconfirm_mwa;
                }

                rconfirm_mwa.Obj.tid = datalistconfirm_mwa[0].tid;
                rconfirm_mwa.Obj.refId = datalistconfirm_mwa[0].refId;
                rconfirm_mwa.Obj.respCode = datalistconfirm_mwa[0].respCode;
                rconfirm_mwa.Obj.respMessage = datalistconfirm_mwa[0].respMessage;
            }
            catch
            {
                rconfirm_mwa.Obj.tid = obj.tid == null ? "" : obj.tid.Trim();
                rconfirm_mwa.Obj.refId = "";
                rconfirm_mwa.Obj.respCode = "1000";
                rconfirm_mwa.Obj.respMessage = "Server Error";
            }
            return rconfirm_mwa;
        } // confirm การชำระเงิน 

        public ResponseData_get_ws_dtac postpaid_request_query(RequestData_get_ws_dtac obj)
        {
            try
            {
                obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                //obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";
                rget_ws_dtac.Obj = new DataContract_get_ws_dtac();
                datalistget_ws_dtac = cell02.postpaid_request_query(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                                    obj.paymentChannel == null ? "" : obj.paymentChannel,
                                                                    obj.paymentType == null ? "" : obj.paymentType,
                                                                    obj.tid == null ? "" : obj.tid,
                                                                    obj.amt == 0 ? 0 : obj.amt,
                                                                    obj.mobileNo == null ? "" : obj.mobileNo,
                                                                    obj.email == null ? "" : obj.email);



                rget_ws_dtac.Obj.tid = datalistget_ws_dtac[0].tid;
                rget_ws_dtac.Obj.respCode = datalistget_ws_dtac[0].respCode;
                rget_ws_dtac.Obj.respMessage = datalistget_ws_dtac[0].respMessage;
                rget_ws_dtac.Obj.min = datalistget_ws_dtac[0].min;
                rget_ws_dtac.Obj.max = datalistget_ws_dtac[0].max;
                rget_ws_dtac.Obj.total = datalistget_ws_dtac[0].total;
                rget_ws_dtac.Obj.balance = datalistget_ws_dtac[0].balance;
                rget_ws_dtac.Obj.reconnect = datalistget_ws_dtac[0].reconnect;
                rget_ws_dtac.Obj.refId = datalistget_ws_dtac[0].refId;
            }
            catch
            {
                rget_ws_dtac.Obj.tid = obj.tid == null ? "" : obj.tid;
                rget_ws_dtac.Obj.respCode = "1000";
                rget_ws_dtac.Obj.respMessage = "Server Error";
                rget_ws_dtac.Obj.min = "0";
                rget_ws_dtac.Obj.max = "0";
                rget_ws_dtac.Obj.total = "0";
                rget_ws_dtac.Obj.balance = "0";
                rget_ws_dtac.Obj.reconnect = "";
                rget_ws_dtac.Obj.refId = "";
            }
            return rget_ws_dtac;
        }

        public ResponseData_get_ws_dtac_confirm postpaid_request_confirm(RequestData_get_ws_dtac_confirm obj)
        {
            try
            {
                obj.secureCode = "5D0890DB01B61C57700C64BFA324D072";
                //obj.secureCode = "2BCB0C3A9ACFD36E408905275E6C2860";
                rget_ws_dtac_confirm.Obj = new DataContract_get_ws_dtac_confirm();
                

                if ((obj.refID == null ? "" : obj.secureCode.Trim()) != "" 
                    || (obj.bankTrxid == null ? "" : obj.bankTrxid.Trim()) != ""
                    || (obj.clientId == null ? "" : obj.clientId.Trim()) != "")
                {
                    datalistget_ws_data_confirm = cell02.postpaid_request_confirm(obj.secureCode == null ? "" : obj.secureCode.Trim(),
                                                                              obj.tid == null ? "" : obj.tid,
                                                                              obj.refID == null ? "" : obj.secureCode.Trim(),
                                                                              obj.amt == 0 ? 0 : obj.amt,
                                                                              obj.confirmStatus == null ? "" : obj.confirmStatus,
                                                                              obj.bankTrxid == null ? "" : obj.bankTrxid);
                    // obj.clientId)
                    using (var conn = dba.dbConnect())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("updata_psb_get_ws_dtac_api_cf_operation_wcf", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300; // Increase this to allow the proc longer to run
                            cmd.Parameters.AddWithValue("@refID", datalistconfirm[0].refId);
                            cmd.Parameters.AddWithValue("@bankTrxId", obj.bankTrxid == null ? "" : obj.bankTrxid.Trim());
                            cmd.Parameters.AddWithValue("@clientId", obj.clientId == null ? "" : obj.clientId.Trim());

                            SqlDataReader dr = cmd.ExecuteReader();
                            dr.Close();
                        }
                        conn.Close();
                    }
                }
                else
                {
                    rget_ws_dtac_confirm.Obj.refId = "";
                    rget_ws_dtac_confirm.Obj.respCode = "9902";
                    rget_ws_dtac_confirm.Obj.respMessage = "Invalid null";
                    rget_ws_dtac_confirm.Obj.tid = "";
                    return rget_ws_dtac_confirm;
                }


                rget_ws_dtac_confirm.Obj.tid = datalistget_ws_data_confirm[0].tid;
                rget_ws_dtac_confirm.Obj.respCode = datalistget_ws_data_confirm[0].respCode;
                rget_ws_dtac_confirm.Obj.respMessage = datalistget_ws_data_confirm[0].respMessage;
                rget_ws_dtac_confirm.Obj.refId = datalistget_ws_data_confirm[0].refId;
    

            }
            catch
            {
                rget_ws_dtac_confirm.Obj.tid = obj.tid == null ? "" : obj.tid;
                rget_ws_dtac_confirm.Obj.respCode = "1000";
                rget_ws_dtac_confirm.Obj.respMessage = "Server Error";
                rget_ws_dtac_confirm.Obj.refId = "";
            }
            return rget_ws_dtac_confirm;
        }

        public ResponseData_getmypsb_wcf InsertMypsbTransaction(RequestData_getmypsb_wcf obj)
        {
            rgetmypsb.Obj = new DataContract_getmypsb_wcf();

            ArrayList list = new ArrayList();
            list.Add("psbID");
            list.Add("username");
            list.Add("secureCode");
            list.Add("command");
            list.Add("cardNumber");
            list.Add("cellumRefID");
            list.Add("psbTypeID");
            list.Add("psbPackageID");
            list.Add("reference1");
            list.Add("amount");
            list.Add("bankTrxid");
            list.Add("clientId");

            ArrayList dataCel = new ArrayList
               {
                   obj.psbID == null ? "" : obj.psbID,//[0]
                   obj.username == null ? "" : obj.username ,//[1]
                   obj.secureCode == null ? "" : obj.secureCode ,//[2]
                   obj.command == null ? "" : obj.command ,//[3]
                   obj.cardNumber == null ? "" : obj.cardNumber ,//[4]
                   obj.cellumRefID == null ? "" : obj.cellumRefID ,//[5]
                   obj.psbTypeID == null ? "" : obj.psbTypeID ,//[6]
                   obj.psbPackageID == null ? "" : obj.psbPackageID ,//[7]
                   obj.reference1 == null ? "" : obj.reference1 ,//[8]
                   obj.amount == null ? "" : obj.amount ,//[9]
                   obj.bankTrxid == null ? "" : obj.bankTrxid ,//[10]
                   obj.clientId == null ? "" : obj.clientId //[11]
               };
            Int32 i = 0;
            foreach (string list_2 in list)
            {
                if (dataCel[i].ToString() == "")
                {
                    rgetmypsb.Obj.status = "FALSE";
                    rgetmypsb.Obj.description = "Invalid "+ list_2 + " Null";
                    return rgetmypsb;
                }
                i++;
            }
            //InsertMypsbTransaction
            try
            {
                datalistgetmypsb = cell03.InsertMypsbTransaction(dataCel[0].ToString(),
                                                                 dataCel[1].ToString(),
                                                                 dataCel[2].ToString(),
                                                                 dataCel[3].ToString(),
                                                                 dataCel[4].ToString(),
                                                                 dataCel[5].ToString(),
                                                                 dataCel[6].ToString(),
                                                                 dataCel[7].ToString(),
                                                                 dataCel[8].ToString(),
                                                                 dataCel[9].ToString());

                using (var conn = dba.dbConnect())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("updata_InsertMypsbTransaction_operation_wcf", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300; // Increase this to allow the proc longer to run
                        cmd.Parameters.AddWithValue("@refID", datalistgetmypsb.paysbuyRefID.Trim());
                        cmd.Parameters.AddWithValue("@bankTrxId", dataCel[10].ToString());
                        cmd.Parameters.AddWithValue("@clientId", dataCel[11].ToString());

                        SqlDataReader dr = cmd.ExecuteReader();
                        dr.Close();
                    }
                    conn.Close();
                }
            }
            catch
            {
                rgetmypsb.Obj.status = "FALSE";
                rgetmypsb.Obj.description = "Server Error";
               
            }
            return rgetmypsb;
        }


    }
}
