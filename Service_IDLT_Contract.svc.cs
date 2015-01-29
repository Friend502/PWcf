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
namespace PWCF
{ 
    public class Service_IDLT_Contract : IDLT_Service
    {
        //--- เรียนก WebService DLT
        Webservice_IDLT.ServiceSoapClient x2 = new Webservice_IDLT.ServiceSoapClient();
        Class_Fuction Class_Fuction = new Class_Fuction();

        //-----------------------
        public ResponseP_CAL_TAX_MBL GetInquiryVehicleInformation(RequestP_CAL_TAX_MBL obj)
        {
            //--------------------Data fig Paysbuy -----------------------//
            string SOFF_NAME = "สำนักงานขนส่งจังหวัดกรุงเทพ";//สำนักงานขนส่งจังหวัด
            string SBR_NAME="";//สำนักงานขนส่งสาขา
            string SADV_FLAG="0"; //ต้องการชำระภาษี ล่วงหน้า
            string SPHONE_NETW="995"; //รหัสหน่วยรับชำระ
            string SIDTAX_NETW="1111111111"; //เลขที่ผู้เสียภาษี 
            string SOPR_PAY="1"; //ช่องทางการชำระ
            //------------------------------------------------------------//
            string secureCode = obj.secureCode; //Cellum secureCode assign by Paysbuy
            string inquiryRefId = obj.inquiryRefId; //inquiry reference uniq id
            string vehicleTypeId = obj.vehicleTypeId; //GetVehicleType.vehicleTypeId 
            string plateLetter = obj.plateLetter; //หมวดอักษร
            string plateNumber = obj.plateNumber; //เลขลำดับ
            string provinceId = obj.provinceId; //GetProvince.provinceId
            string customerIdCardNumber = obj.customerIdCardNumber; //Id บัตรประชาชน
            string customerName = obj.customerName; //ชื่อ
            string customerPhone = obj.customerPhone; //เบอร์โทร
            string customerEmail = obj.customerEmail; //email ลูกค้า
            string insuranceFlag = obj.insuranceFlag; //การมี พรบ 
            string insuranceNumber = obj.insuranceNumber; //หมายเลข พรบ
            string insuranceEndDate = obj.insuranceEndDate; //วันที่กรรมธรรม์คุ้มครอง สิ้นสุด
            string clientId = obj.clientId; //registered mobile number

            string TOKEN = "";
            TOKEN = x2.Login("DLTDTAC", "DLTDTAC!@#$");

            // Request Data to DLT
            //---------------------------------------------------------------------------------------
            DLT listData = x2.P_CAL_TAX_MBL(TOKEN, //TOKEN (loging and Password)
                                                  SOFF_NAME, //สำนักงานขนส่งจังหวัด
                                                  SBR_NAME, //สำนักงานขนส่งสาขา
                                                  vehicleTypeId, //ประเภทรถ
                                                  plateLetter, //หมวดอักษร
                                                  plateNumber, //เลขลำดับ
                                                  customerIdCardNumber, //ID บัตรประชาชน
                                                  customerName, //ชื่อ
                                                  customerPhone, //เบอร์โทร
                                                  insuranceFlag, //การมี พรบ (1,2,3)
                                                  insuranceNumber,//หมาเลข พรบ
                                                  insuranceEndDate, //สิ้นสุด
                                                  SADV_FLAG,//ต้องการชำระภาษี ล่วงหน้า
                                                  SPHONE_NETW, //รหัสหน่วยรับชำระ
                                                  SIDTAX_NETW, //เลขที่ผู้เสียภาษี
                                                  SOPR_PAY); //ช่องทางการชำระ
            //---------------------------------------------------------------------------------------


            // Response Data to Cellor
            ResponseP_CAL_TAX_MBL r = new ResponseP_CAL_TAX_MBL();
            r.Obj = new DataP_CAL_TAX_MBL();
            r.Obj.responseCode =""; // responseCode
            r.Obj.responseDescription = ""; // responseDescription
            r.Obj.inquiryRefId = ""; //inquiryRefid
            r.Obj.Message = listData.Message; //(500,592,100 คือ ผิดพลาด แต่ถ้า "" ไม่มี Error ข้อมูลด้านล้างจะขึ้น)
            r.Obj.paysbuyRefId = listData.Result01;//หมายเลขอ้างอิงของกรม
            r.Obj.displayTaxTotal = listData.Result02 == "" ? "0.00" : listData.Result02; //จำนวนเงิน ค่าภาษีรถ
            r.Obj.displayFinesTotal = listData.Result03 == "" ? "0.00" : listData.Result03; //เงินเพิ่ม
            r.Obj.displayInsuranceTotal = listData.Result04 == "" ? "0.00" : listData.Result01; //ค่าเบี้ยประกันภัย
            r.Obj.displayShippingDocumentTotal = listData.Result05 == "" ? "0.00" : listData.Result01; //ค่าส่งเอกสารกลับ
            r.Obj.currentTaxExpireDate = listData.Result06; //วันสิ้นอายุภาษีปัจจุบัน
            r.Obj.nextTaxExpireDate = listData.Result07; //วันสิ้นอายุภาษีใหม่
            r.Obj.paymentDueDate = listData.Result08; //วันครบกำหนดชำระเงิน
            r.Obj.totalAmount = (Convert.ToDouble(r.Obj.displayTaxTotal) + Convert.ToDouble(r.Obj.displayFinesTotal)
                                 + Convert.ToDouble(r.Obj.displayInsuranceTotal) + Convert.ToDouble(r.Obj.displayShippingDocumentTotal)).ToString(); 
                                 //จำนวนชำระทั้งหมดสิ้น(จำนวนเงิน ค่าภาษีรถ+เงินเพิ่ม+ค่าเบี้ยประกันภัย+ค่าส่งเอกสารกลับ)
             

            return r;
        }

        /// <summary>
        ///----------------------------------------------ชำระเงิน
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseP_RECV_TAX_MBL GetConfirmTaxPayment(RequestP_RECV_TAX_MBL obj)
        {
            string SPHONE_NETW = "995"; //รหัสหน่วยรับชำระ
            string SRECV_DATE = "Now"; //วันที่รับเงิน
          


            string secureCode = obj.secureCode ; //Cellum secureCode assign by Paysbuy
            string paysbuyRefId = obj.paysbuyRefId; //InquiryVehicleInformation.paysbuyRefId (หมายเลขอ้างอิงของกรม)
            string confirmPaymentAmoun = obj.confirmPaymentAmoun;//InquiryVehicleInformation.totalAmount
            string transactionId = obj.transactionId;//Cellum uniq transaction id
            string bankTransactionId = obj.bankTransactionId;//Cellum original bank transaction id
            string recipientsFullName = obj.recipientsFullName;//ชื่อ นามสกุล ผู้รับเอกสาร
            string recipientsAddress = obj.recipientsAddress;//ที่อยู่ ผู้รับเอกสาร
            string recipientsDistrict = obj.recipientsDistrict;//ตำบล
            string recipientsAmphor = obj.recipientsAmphor;//อำเภอ
            string recipientsProvince = obj.recipientsProvince;//จังหวัด
            string recipientsZipcode = obj.recipientsZipcode;//รหัสไปรษณีย์
            string insuranceFullname = obj.insuranceFullname;//ชื่อ นามสกุล ผู้เอาประกันภัย
            string insuranceVillage = obj.insuranceVillage;//หมู่บ้าน ผู้เอาประกันภัย
            string insuranceAddressNumber = obj.insuranceAddressNumber;//บ้านเลขที่ ผู้เอาประกันภัย
            string insuranceMoo = obj.insuranceMoo;//หมู่ที่ ผู้เอาประกันภัย
            string insuranceSoi = obj.insuranceSoi;//ซอย ผู้เอาประกันภัย
            string insuranceRoad = obj.insuranceRoad;//ถนน ผู้เอาประกันภัย
            string insuranceDistrict = obj.insuranceDistrict;//แขวง ตำบล ผู้เอาประกันภัย
            string insuranceAmphor = obj.insuranceAmphor;//เขต อำเภอ ผู้เอาประกันภัย
            string insuranceProvince = obj.insuranceProvince;//จังหวัด ผู้เอาประกันภัย
            string insuranceZipcode = obj.insuranceZipcode;//รหัสไปรษณีย์ ผู้เอาประกันภัย

            string TOKEN = "";
            TOKEN = x2.Login("DLTDTAC", "DLTDTAC!@#$");
            DLT listData = x2.P_RECV_TAX_MBL(TOKEN,
                                             SPHONE_NETW, //รหัสหน่วยรับชำระ
                                             paysbuyRefId, //หมาเลขอ้างอิงของกรม
                                             SRECV_DATE, //วันที่รับเงิน
                                             confirmPaymentAmoun, //จำนวนเงินรวม
                                             recipientsFullName, //ชื่อ นามสกุล ผู้รับเอกสาร
                                             recipientsAddress, //ที่อยู่ ผู้รับเอกสาร
                                             recipientsDistrict, //ผู้รับเอกสาร
                                             recipientsAmphor, //ผู้รับเอกสาร
                                             recipientsProvince, //ผู้รับเอกสาร
                                             recipientsZipcode, //รหัสไปรษณีย์ ผู้รับเอกสาร
                                             insuranceFullname, //ชื่อ นามสกุล ผู้เอาประกันภัย
                                             insuranceVillage, //หมู่บ้าน ผู้เอาประกันภัย
                                             insuranceAddressNumber, //บ้านเลขที่ ผู้เอาประกันภัย
                                             insuranceMoo, //หมู่ที่ ผู้เอาประกันภัย
                                             insuranceSoi, //ซอย ผู้เอาประกันภัย
                                             insuranceRoad, //ถนน ผู้เอาประกันภัย
                                             insuranceDistrict, //แขวง ตำบล ผู้เอาประกันภัย
                                             insuranceAmphor, //เขต อำเภอ ผู้เอาประกันภัย
                                             insuranceProvince, //จังหวัด ผู้เอาประกันภัย
                                             insuranceZipcode); //รหัสไปรษณีย์ ผู้เอาประกันภัย
            // Response Data to Cellor 
            ResponseP_RECV_TAX_MBL r = new ResponseP_RECV_TAX_MBL();
            r.Obj = new DataP_RECV_TAX_MBL();
            if (listData.Message == "000 – ไม่พบข้อผิดพลาด")
            {

                r.Obj.responseCode = listData.Message;
                r.Obj.responseDescription = listData.Message;
                r.Obj.paysbuyTransactionID = listData.Message;
            }
            else
            {
                r.Obj.responseCode = listData.Message;
                r.Obj.responseDescription = listData.Message;
                r.Obj.paysbuyTransactionID = listData.Message;
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
                        province.Add(new province(objReader["prov_id"].ToString() //ใส่ข้อมูลลง List พิมพ์ XML 
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
            return province;
        }

        public List<VehicleType> GetVehicleType(RequestvehicleType obj)
        {
            string host = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri.ToString();
            string[] url = host.Split('/'); // 4


            List<VehicleType> vehicleType = new List<VehicleType>();
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/vehicleType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
            string[] textData = System.IO.File.ReadAllLines(directory);
            foreach (string wordcheck in textData)
            {
                string[] s = wordcheck.Split(',');
                vehicleType.Add(new VehicleType(s[0].ToString(), url[0]+"//" +url[2] + "/img/" + s[1].ToString(), s[2].ToString(),
                                                s[3].ToString(), s[4].ToString(), s[5].ToString()));
            }

            return vehicleType;
        }

    }

}

        
