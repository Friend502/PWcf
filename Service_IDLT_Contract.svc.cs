using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Services;
using PWCF.Webservice_IDLT;
using System.ServiceModel.Description;
using System.Xml;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using Paysbuy.Model.Logs;
namespace PWCF
{ 
    public class Service_IDLT_Contract : IDLT_Service
    {
        //--- เรียนก WebService DLT
        Webservice_IDLT.ServiceSoapClient x2 = new Webservice_IDLT.ServiceSoapClient();
        ResponseP_CAL_TAX_MBL r = new ResponseP_CAL_TAX_MBL();
        Class_Fuction cFuction = new Class_Fuction();
        string a = "";

        //-----------------------
        public ResponseP_CAL_TAX_MBL GetInquiryVehicleInformation(RequestP_CAL_TAX_MBL obj)
        {
            //--------------------Data fig Paysbuy -----------------------//
            //Log

            r.Obj = new DataP_CAL_TAX_MBL();
            LogEvents logsw = new LogEvents(obj.inquiryRefId + " GetInquiryVehicleInformation");

            logsw.TimeStamp(obj.secureCode + "," + obj.inquiryRefId + "," + obj.vehicleTypeId + ","
                             + obj.plateLetter + "," + obj.plateNumber + "," + obj.provinceId + ","
                             + obj.customerIdCardNumber + "," + obj.customerName + "," + obj.customerPhone + ","
                             + obj.customerEmail + "," + obj.insuranceFlag + "," + obj.insuranceNumber + ","
                             + obj.insuranceEndDate + "," + obj.clientId + "," + "Paysbuy receive data cellum GetInquiryVehicleInformation");

            string buyInsurance = "N";// เปิดสถานะ
            obj.secureCode = "HV";
            if (obj.secureCode == WebConfigurationManager.AppSettings["secureCode"])
            {
                string SBR_NAME = "NULL";//สำนักงานขนส่งสาขา
                string SADV_FLAG = "0"; //ต้องการชำระภาษี ล่วงหน้า
                string SPHONE_NETW = "995"; //รหัสหน่วยรับชำระ
                string SIDTAX_NETW = "0125547001802"; //เลขที่ผู้เสียภาษี 
                string SOPR_PAY = "1"; //ช่องทางการชำระ

                //------------------------------------------------------------//
                string secureCode = obj.secureCode == null ? "" : obj.secureCode; //Cellum secureCode assign by Paysbuy
                string inquiryRefId = obj.inquiryRefId == null ? "" : obj.inquiryRefId; //inquiry reference uniq id

                ////GetVehicleType.vehicleTypeId (ประเภทรถยนต์)
                string vehicleTypeId = obj.vehicleTypeId == null ? "0" : obj.vehicleTypeId;
                string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/T_vehicleType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
                string[] textData = System.IO.File.ReadAllLines(directory);
                foreach (string wordcheck in textData)
                {
                    string[] s = wordcheck.Split(',');
                    if (vehicleTypeId == s[0].ToString())
                    {
                        vehicleTypeId = s[5].ToString();
                    }

                }
                string plateLetter = obj.plateLetter == null ? "" : obj.plateLetter; //หมวดอักษร
                string plateNumber = obj.plateNumber == null ? "" : obj.plateNumber; //เลขลำดับ

                //GetProvince.provinceId (สำนักงานต่อประกัน)
                string provinceId = obj.provinceId == null ? "" : obj.provinceId;
                string directory2 = System.Web.Hosting.HostingEnvironment.MapPath("~/T_provinceType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
                string[] textData2 = System.IO.File.ReadAllLines(directory2);
                foreach (string wordcheck2 in textData2)
                {
                    string[] s2 = wordcheck2.Split(',');
                    if (provinceId == s2[0].ToString())
                    {
                        provinceId = s2[2].ToString();
                    }

                }

                string customerIdCardNumber = obj.customerIdCardNumber == null ? "" : obj.customerIdCardNumber; //Id บัตรประชาชน
                string customerName = obj.customerName == null ? "" : obj.customerName; //ชื่อ
                string customerPhone = obj.customerPhone == null ? "" : obj.customerPhone; //เบอร์โทร
                string customerEmail = obj.customerEmail == null ? "" : obj.customerEmail; //email ลูกค้า
                //---------------ตรวจสอบ พรบ
                string insuranceFlag = obj.insuranceFlag == null ? "" : obj.insuranceFlag; //การมี พรบ (1)
                string insuranceNumber = obj.insuranceNumber == null ? "" : obj.insuranceNumber; //หมายเลข พรบ
                string insuranceEndDate = obj.insuranceEndDate == null ? "" : obj.insuranceEndDate; //วันที่กรรมธรรม์คุ้มครอง สิ้นสุด
                
                if (insuranceFlag == "1" )
                {
                    
                    if (insuranceNumber == "" || insuranceEndDate == "")
                    {
                        
                        r.Obj.Message = "100 Invalid field"; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                        //return r;
                    }
                    else
                    {
                        buyInsurance = "N";
                    }
                }


                else if (insuranceFlag == "")
                {
                        r.Obj.Message = "100 Invalid field"; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                        buyInsurance = "N";
                        //return r;

                }else if ( Convert.ToInt32(insuranceFlag) == 2 || Convert.ToInt32(insuranceFlag) == 3)
                {
                        r.Obj.Message = "100 Invalid field"; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                        buyInsurance = "Y";
                        //return r;
                }
                else if (Convert.ToInt32(insuranceFlag) > 3)
                {
                    r.Obj = new DataP_CAL_TAX_MBL();
                    r.Obj.Message = "100 Invalid field"; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                    buyInsurance = "N";
                    //Log
                }
                else
                {
                    buyInsurance = "N";
                }

                

                string clientId = obj.clientId == null ? "" : obj.clientId; //registered mobile number
                string TOKEN = "";
                DLT listData = new DLT();
                try
                {
                    TOKEN = x2.Login("DLTDTAC", "DLTDTAC!@#$");
                    a = TOKEN;
                // Request Data to DLT
                //---------------------------------------------------------------------------------------
                    //listData = x2.P_CAL_TAX_MBL(TOKEN, //TOKEN (loging and Password)
                    //                                       provinceId, //สำนักงานขนส่งจังหวัด "1"
                    //                                       SBR_NAME, //สำนักงานขนส่งสาขา ""
                    //                                       vehicleTypeId, //ประเภทรถ "1"
                    //                                       plateLetter, //หมวดอักษร
                    //                                       plateNumber, //เลขลำดับ
                    //                                       customerIdCardNumber, //ID บัตรประชาชน
                    //                                       customerName, //ชื่อ
                    //                                       customerPhone, //เบอร์โทร
                    //                                       insuranceFlag, //การมี พรบ (1,2,3)
                    //                                       insuranceNumber,//หมาเลข พรบ
                    //                                       insuranceEndDate, //สิ้นสุด
                    //                                       SADV_FLAG,//ต้องการชำระภาษี ล่วงหน้า
                    //                                       SPHONE_NETW, //รหัสหน่วยรับชำระ
                    //                                       SIDTAX_NETW, //เลขที่ผู้เสียภาษี
                    //                                       SOPR_PAY); //ช่องทางการชำระ

                    listData = x2.P_CAL_TAX_MBL(TOKEN, //TOKEN (loging and Password)
                                                          "กรุงเทพมหานคร", //สำนักงานขนส่งจังหวัด "1"
                                                          SBR_NAME, //สำนักงานขนส่งสาขา ""
                                                          "รถยนต์บรรทุกส่วนบุคคล", //ประเภทรถ "1"
                                                          "ตม", //หมวดอักษร
                                                          "8345", //เลขลำดับ
                                                          "1430900025039", //ID บัตรประชาชน
                                                          "ประเวทย์ วันสิงสู่", //ชื่อ
                                                          "0868419215", //เบอร์โทร
                                                          "2", //การมี พรบ (1,2,3)
                                                          "",//หมาเลข พรบ
                                                          "25580101", //สิ้นสุด
                                                          SADV_FLAG,//ต้องการชำระภาษี ล่วงหน้า
                                                          SPHONE_NETW, //รหัสหน่วยรับชำระ
                                                          SIDTAX_NETW, //เลขที่ผู้เสียภาษี
                                                          SOPR_PAY); //ช่องทางการชำระ

                    //Log
                   logsw.TimeStamp(obj.inquiryRefId == null ? "" : obj.inquiryRefId + "," + listData.Message + "," + listData.Result01 + "," + listData.Result02 + ","
                          + listData.Result03 + "," + listData.Result04 + "," + listData.Result04 + ","
                          + listData.Result05 + "," + listData.Result06 + "," + listData.Result07 + ","
                          + listData.Result08 + "DLT return data ");
                }
                catch 
                {
                    r.Obj.Message = "999 Server error."; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                    return r;
                }

                if (listData.Message != "")
                {
                    if (cFuction.returncheck_Message(listData.Message.Substring(0, 3)) == "1")
                    {
                        r.Obj.Message = cFuction.returncheck_Message_103(listData.Message.Substring(0, 3)); //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                    }
                }
                // Response Data to Cellor
                
                r.Obj.inquiryRefId = obj.inquiryRefId == null ? "" : obj.inquiryRefId; //inquiryRefid

                if (listData.Message != "")
                {
                    if (cFuction.returncheck_Message(listData.Message.Substring(0, 3)) != "1")
                    {
                        r.Obj.Message = "000 Success."; 
                    }
                    else
                    {
                        r.Obj.Message = cFuction.returncheck_Message_103(listData.Message.Substring(0, 3));
                    }
                }
                else
                {
                    r.Obj.Message = "500 - Important data cannot not be null.";
                }

                r.Obj.responseCode = cFuction.returncheck_Message2(r.Obj.Message).Substring(0, 3); // responseCode
                r.Obj.responseDescription = cFuction.returncheck_Message2(r.Obj.Message).Substring(4, (cFuction.returncheck_Message2(r.Obj.Message).Length) - 4); // responseDescription

                r.Obj.paysbuyRefId = listData.Result01 == null ? "" : listData.Result01;//หมายเลขอ้างอิงของกรม
                r.Obj.buyInsurance = buyInsurance; //เปิดสถานะให้ ใส่ข้อมูล พรบ
                r.Obj.displayTaxTotal = listData.Result02 == null ? "0.00" : listData.Result02; //จำนวนเงิน ค่าภาษีรถ
                r.Obj.displayFinesTotal = listData.Result03 == null ? "0.00" : listData.Result03; //เงินเพิ่ม
                r.Obj.displayInsuranceTotal = listData.Result04 == null ? "0.00" : listData.Result04; //ค่าเบี้ยประกันภัย
                r.Obj.displayShippingDocumentTotal = listData.Result05 == null ? "0.00" : listData.Result05; //ค่าส่งเอกสารกลับ
                r.Obj.currentTaxExpireDate = listData.Result06 == null ? "" : listData.Result06; //วันสิ้นอายุภาษีปัจจุบัน
                r.Obj.nextTaxExpireDate = listData.Result07 == null ? "" : listData.Result07; //วันสิ้นอายุภาษีใหม่
                r.Obj.paymentDueDate = listData.Result08 == null ? "" : listData.Result08; //วันครบกำหนดชำระเงิน
                r.Obj.Paysbuyfee = "10.00"; //fee paysbuy
                r.Obj.totalAmount = (Convert.ToDouble(r.Obj.Paysbuyfee) + Convert.ToDouble(r.Obj.displayTaxTotal) + Convert.ToDouble(r.Obj.displayFinesTotal)
                                     + Convert.ToDouble(r.Obj.displayInsuranceTotal) + Convert.ToDouble(r.Obj.displayShippingDocumentTotal)).ToString();
                //จำนวนชำระทั้งหมดสิ้น(จำนวนเงิน ค่าภาษีรถ+เงินเพิ่ม+ค่าเบี้ยประกันภัย+ค่าส่งเอกสารกลับ)
                //Log



                logsw.TimeStamp(r.Obj.responseCode + "," + r.Obj.responseDescription + "," + r.Obj.inquiryRefId + ","
                                + r.Obj.Message + "," + r.Obj.paysbuyRefId + "," + r.Obj.buyInsurance + "," + r.Obj.displayTaxTotal + ","
                                + r.Obj.displayFinesTotal + "," + r.Obj.displayFinesTotal + "," + r.Obj.displayInsuranceTotal + ","
                                + r.Obj.displayShippingDocumentTotal + "," + r.Obj.currentTaxExpireDate + "," + r.Obj.nextTaxExpireDate + ","
                                + r.Obj.paymentDueDate + "," + r.Obj.totalAmount + "," + " Paysbuy return data to Cellum");
            }
            else
            {
                r.Obj.Message = "500 Important data cannot not be null."; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
                r.Obj.responseCode = cFuction.returncheck_Message2(r.Obj.Message).Substring(0, 3); // responseCode
                r.Obj.responseDescription = cFuction.returncheck_Message2(r.Obj.Message).Substring(4, (cFuction.returncheck_Message2(r.Obj.Message).Length) - 4); // responseDescription
                //Log
                logsw.TimeStamp("500 Important data cannot not be null.");
            }

            return r;
        }

        /// <summary>
        ///----------------------------------------------ชำระเงิน
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseP_RECV_TAX_MBL GetConfirmTaxPayment(RequestP_RECV_TAX_MBL obj)
        {
            ResponseP_RECV_TAX_MBL r = new ResponseP_RECV_TAX_MBL();
            r.Obj = new DataP_RECV_TAX_MBL();
            int arrdata = 25;
            string[] Data = new string[arrdata];
            int foi = 0;
            string Logdata = "";


            string SPHONE_NETW = "995"; //รหัสหน่วยรับชำระ
            string SRECV_DATE = DateTime.Now.ToString("yyyyMMdd"); //วันที่รับเงิน
            obj.secureCode = "HV";
            if (obj.secureCode == WebConfigurationManager.AppSettings["secureCode"])
            {
                List<ResponseP_RECV_TAX_MBL> DataListCell = new List<ResponseP_RECV_TAX_MBL>();
                ArrayList dataCel = new ArrayList { obj.secureCode == null ? "" : obj.secureCode, //Cellum secureCode assign by Paysbuy [0]
                                                    obj.paysbuyRefId == null ? "99999" : obj.paysbuyRefId,//InquiryVehicleInformation.paysbuyRefId (หมายเลขอ้างอิงของกรม)[1]
                                                    obj.confirmPaymentAmoun == null ? "" : obj.confirmPaymentAmoun,//InquiryVehicleInformation.totalAmount[2]
                                                    obj.transactionId == null ? "" : obj.transactionId,//Cellum uniq transaction id[3]
                                                    obj.bankTransactionId == null ? "" : obj.bankTransactionId,//Cellum original bank transaction id[4]
                                                    //บังคับใส่ข้อมูล
                                                    obj.recipientsFullName == null ? "" : obj.recipientsFullName,//ชื่อ นามสกุล ผู้รับเอกสาร[5]
                                                    obj.recipientsAddress == null ? "" : obj.recipientsAddress,//ที่อยู่ ผู้รับเอกสาร[6]
                                                    obj.recipientsDistrict == null ? "" : obj.recipientsDistrict,//ตำบล[7]
                                                    obj.recipientsAmphor == null ? "" : obj.recipientsAmphor,//อำเภอ[8]
                                                    obj.recipientsProvince == null ? "" : obj.recipientsProvince,//จังหวัด[9]
                                                    obj.recipientsZipcode == null ? "" : obj.recipientsZipcode,//รหัสไปรษณีย์[10]
                                                    //บังคับใส่ข้อมูล
                                                    //ไม่บังคับใส่ข้อมูล
                                                    obj.insuranceFullname == null ? "" : obj.insuranceFullname,//ชื่อ นามสกุล ผู้เอาประกันภัย[11]
                                                    obj.insuranceVillage == null ? "" : obj.insuranceVillage,//หมู่บ้าน ผู้เอาประกันภัย[12]
                                                    obj.insuranceAddressNumber == null ? "" : obj.insuranceAddressNumber,//บ้านเลขที่ ผู้เอาประกันภัย[13]
                                                    obj.insuranceMoo == null ? "" : obj.insuranceMoo,//หมู่ที่ ผู้เอาประกันภัย[14]
                                                    obj.insuranceSoi == null ? "" : obj.insuranceSoi,//ซอย ผู้เอาประกันภัย[15]
                                                    obj.insuranceRoad == null ? "" : obj.insuranceRoad,//ถนน ผู้เอาประกันภัย[16]
                                                    obj.insuranceDistrict == null ? "" : obj.insuranceDistrict,//แขวง ตำบล ผู้เอาประกันภัย[17]
                                                    obj.insuranceAmphor == null ? "" : obj.insuranceAmphor,//เขต อำเภอ ผู้เอาประกันภัย[18]
                                                    obj.insuranceProvince == null ? "" : obj.insuranceProvince,//จังหวัด ผู้เอาประกันภัย[19]
                                                    obj.insuranceZipcode == null ? "" : obj.insuranceZipcode,//รหัสไปรษณีย์ ผู้เอาประกันภัย[20]
                                                    obj.inquiryRefId == null ? "" : obj.inquiryRefId
                };

                LogEvents logsw = new LogEvents(dataCel[21].ToString()+" GetConfirmTaxPayment");


                foreach (string wordcheck in dataCel)
                {
                    Data[foi] = Convert.ToString(wordcheck);
                    Logdata = Logdata + Data[foi] + ",";
                    foi++;
                }

                logsw.TimeStamp(Logdata);

                for(int i = 0; i <= 10; i++)
                {
                    if (Data[i] == "")
                    {
                        r.Obj.responseCode = "500";
                        r.Obj.responseDescription = "Important data cannot not be null.";
                        r.Obj.paysbuyTransactionID = obj.transactionId == null ? "" : obj.transactionId;
                        logsw.TimeStamp(r.Obj.responseCode + "," + r.Obj.responseDescription + "," + r.Obj.paysbuyTransactionID);
                       // logsw.ExportFile(@"C:\Users\Boom\Documents\ProjectByDtac\Paysbuy_Security\WCF\PhoneWCF\PWCF\PWCF\bin\");
                        return r;
                    }
                }



                string TOKEN = "";
                TOKEN = x2.Login("DLTDTAC", "DLTDTAC!@#$");
                DLT listData = x2.P_RECV_TAX_MBL(TOKEN,
                                                 SPHONE_NETW, //รหัสหน่วยรับชำระ
                                                 Data[1], //หมาเลขอ้างอิงของกรม
                                                 SRECV_DATE, //วันที่รับเงิน
                                                 Data[2], //จำนวนเงินรวม
                                                 Data[5], //ชื่อ นามสกุล ผู้รับเอกสาร
                                                 Data[6], //ที่อยู่ ผู้รับเอกสาร
                                                 Data[7], //ผู้รับเอกสาร
                                                 Data[8], //ผู้รับเอกสาร
                                                 Data[9], //ผู้รับเอกสาร
                                                 Data[10], //รหัสไปรษณีย์ ผู้รับเอกสาร
                                                 Data[11], //ชื่อ นามสกุล ผู้เอาประกันภัย
                                                 Data[12], //หมู่บ้าน ผู้เอาประกันภัย
                                                 Data[13], //บ้านเลขที่ ผู้เอาประกันภัย
                                                 Data[14], //หมู่ที่ ผู้เอาประกันภัย
                                                 Data[15], //ซอย ผู้เอาประกันภัย
                                                 Data[16], //ถนน ผู้เอาประกันภัย
                                                 Data[17], //แขวง ตำบล ผู้เอาประกันภัย
                                                 Data[18], //เขต อำเภอ ผู้เอาประกันภัย
                                                 Data[19], //จังหวัด ผู้เอาประกันภัย
                                                 Data[20]); //รหัสไปรษณีย์ ผู้เอาประกันภัย
                // Response Data to Cellor 

                if (listData.Message != "")
                {
                    if (listData.Message.Substring(0, 3) == "000")
                    {
                        r.Obj.responseCode = cFuction.returncheck_Message2(listData.Message).Substring(0, 3);
                        r.Obj.responseDescription = cFuction.returncheck_Message2(listData.Message).Substring(4, (cFuction.returncheck_Message2(listData.Message).Length) - 4);
                        r.Obj.paysbuyTransactionID = dataCel[3].ToString();
                        logsw.TimeStamp(r.Obj.responseCode + "," + r.Obj.responseDescription + "," + r.Obj.paysbuyTransactionID);
                    }
                    else
                    {
                        r.Obj.responseCode = cFuction.returncheck_Message2(listData.Message).Substring(0, 3);
                        r.Obj.responseDescription = cFuction.returncheck_Message2(listData.Message).Substring(4, (cFuction.returncheck_Message2(listData.Message).Length) - 4);
                        r.Obj.paysbuyTransactionID = dataCel[3].ToString();
                        logsw.TimeStamp(r.Obj.responseCode + "," + r.Obj.responseDescription + "," + r.Obj.paysbuyTransactionID);
                    }
                }
            }
            else
            {
                LogEvents logsw = new LogEvents("999999" + " GetConfirmTaxPayment");
                    r.Obj.responseCode = "500";
                    r.Obj.responseDescription = "Important data cannot not be null.";
                    r.Obj.paysbuyTransactionID = obj.transactionId == null ? "" : obj.transactionId;
                    logsw.TimeStamp(r.Obj.responseCode + "," + r.Obj.responseDescription + "," + r.Obj.paysbuyTransactionID);
            }
            return r;
        }

        /// <summary>
        /// ----------------------------จังหวัด
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<province> Getprovince(Requestprovince obj)
        {

                DataTable dt = new DataTable();
                List<province> province = new List<province>();
             if (obj.secureCode != null)
                {
                String strConnString = WebConfigurationManager.AppSettings["dbConnection"];
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //----------------- ตรวจสอบค่า obj.secureCode ตรงกับของ paysbuy

                //----------------- ตรวจสอบค่า obj.secureCode ตรงกับของ paysbuy     
                cmd.CommandText = "selectProvince";
                cmd.Connection = con;
                try
                {
                    con.Open();
                    using (SqlDataReader objReader = cmd.ExecuteReader())
                    {
                        while (objReader.Read())
                        {
                            province.Add(new province(Convert.ToInt32(objReader["prov_id"].ToString()) //ใส่ข้อมูลลง List พิมพ์ XML 
                            , objReader["prov_name_T"].ToString()
                            , objReader["prov_name_E"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                province.Add(new province(Convert.ToInt32("0") //ใส่ข้อมูลลง List พิมพ์ XML 
                           , ""
                           , ""));
            }

            return province;
        }

        public List<VehicleType> GetVehicleType(RequestvehicleType obj)
        {
            List<VehicleType> vehicleType = new List<VehicleType>();
            if (obj.secureCode != null)
            {
                string host = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri.ToString();
                string[] url = host.Split('/'); // 4

                string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/T_vehicleType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
                string[] textData = System.IO.File.ReadAllLines(directory);
                foreach (string wordcheck in textData)
                {
                    string[] s = wordcheck.Split(',');
                    vehicleType.Add(new VehicleType(Convert.ToInt32(s[0].ToString()), url[0] + "//" + url[2] + "/img/" + s[1].ToString(), s[2].ToString(),
                                                    s[3].ToString(), s[4].ToString(), s[5].ToString()));
                }
            }
            else
            {
                vehicleType.Add(new VehicleType(Convert.ToInt32("0"), " " + "//" + " " + "/img/" + " ", " ",
                                                    " ", " ", " "));
            }
            return vehicleType;
        }

      //0-----------------------------------
     
    }

}

        
