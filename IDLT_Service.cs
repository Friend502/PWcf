using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace PWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
   
    [ServiceContract]
    public interface IDLT_Service
    {
        [OperationContract]
        List<province> Getprovince(Requestprovince obj);  
 
        [OperationContract]
        List<VehicleType> GetVehicleType(RequestvehicleType obj);  

        [OperationContract]
        [FaultContract(typeof(string))]
        ResponseP_CAL_TAX_MBL GetInquiryVehicleInformation(RequestP_CAL_TAX_MBL obj); 

        [OperationContract]
        [FaultContract(typeof(string))]
        ResponseP_RECV_TAX_MBL GetConfirmTaxPayment(RequestP_RECV_TAX_MBL obj);



       
    }

    // XML------------------------------------------------------------------- P_CAL_TAX_MBL
    public class RequestP_CAL_TAX_MBL
    {
        [MessageHeader]

        public string secureCode; //Cellum secureCode assign by Paysbuy
        public string inquiryRefId; //inquiry reference uniq id
        public string vehicleTypeId; //GetVehicleType.vehicleTypeId 
        public string plateLetter; //หมวดอักษร
        public string plateNumber; //เลขลำดับ
        public string provinceId; //GetProvince.provinceId
        public string customerIdCardNumber; //Id บัตรประชาชน
        public string customerName; //ชื่อ
        public string customerPhone; //เบอร์โทร
        public string customerEmail; //email ลูกค้า
        public string insuranceFlag; //การมี พรบ 
        public string insuranceNumber; //หมายเลข พรบ
        public string insuranceEndDate; //วันที่กรรมธรรม์คุ้มครอง สิ้นสุด
        public string clientId; //registered mobile number
    }
    public class ResponseP_CAL_TAX_MBL
    {
        [MessageBodyMember]
        public DataP_CAL_TAX_MBL Obj;
    }

    [DataContract]
    public class DataP_CAL_TAX_MBL
    {
        [DataMember]
        public string responseCode { get; set; } // responseCode
        [DataMember]
        public string responseDescription { get; set; } //responseDescription
        [DataMember]
        public string inquiryRefId { get; set; } //inquiryRefId
        [DataMember]
        public string Message { get; set; } // ข้อความ
        [DataMember]
        public string paysbuyRefId { get; set; } //หมายเลขอ้างอิงของกรม
        [DataMember]
        public string displayTaxTotal { get; set; } //จำนวนเงิน ค่าภาษีรถ
        [DataMember]
        public string displayFinesTotal { get; set; } //เงินเพิ่ม
        [DataMember]
        public string displayInsuranceTotal { get; set; } //ค่าเบี้ยประกันภัย
        [DataMember]
        public string displayShippingDocumentTotal { get; set; } //ค่าส่งเอกสารกลับ
        [DataMember]
        public string currentTaxExpireDate { get; set; } //วันสิ้นอายุภาษีปัจจุบัน
        [DataMember]
        public string nextTaxExpireDate { get; set; } //วันสิ้นอายุภาษีใหม่
        [DataMember]
        public string paymentDueDate { get; set; } //วันครบกำหนดชำระเงิน
        [DataMember]
        public string totalAmount { get; set; } //จำนวนชำระทั้งหมดสิ้น(จำนวนเงิน ค่าภาษีรถ+เงินเพิ่ม+ค่าเบี้ยประกันภัย+ค่าส่งเอกสารกลับ)
    }



    //----------------------------------------------------------------------- P_CAL_TAX_MBL

    //----------------------------------------------------------------------- P_RECV_TAX_MBL
   
    public class RequestP_RECV_TAX_MBL
    {
        [MessageHeader]
        public string secureCode; //Cellum secureCode assign by Paysbuy
        public string paysbuyRefId; //InquiryVehicleInformation.paysbuyRefId
        public string confirmPaymentAmoun;//InquiryVehicleInformation.totalAmount
        public string transactionId;//Cellum uniq transaction id
        public string bankTransactionId;//Cellum original bank transaction id
        public string recipientsFullName;//ชื่อ นามสกุล ผู้รับเอกสาร
        public string recipientsAddress;//ที่อยู่ ผู้รับเอกสาร
        public string recipientsDistrict;//ตำบล
        public string recipientsAmphor;//อำเภอ
        public string recipientsProvince;//จังหวัด
        public string recipientsZipcode;//รหัสไปรษณีย์
        public string insuranceFullname;//ชื่อ นามสกุล ผู้เอาประกันภัย
        public string insuranceVillage;//หมู่บ้าน ผู้เอาประกันภัย
        public string insuranceAddressNumber;//บ้านเลขที่ ผู้เอาประกันภัย
        public string insuranceMoo;//หมู่ที่ ผู้เอาประกันภัย
        public string insuranceSoi;//ซอย ผู้เอาประกันภัย
        public string insuranceRoad;//ถนน ผู้เอาประกันภัย
        public string insuranceDistrict;//แขวง ตำบล ผู้เอาประกันภัย
        public string insuranceAmphor;//เขต อำเภอ ผู้เอาประกันภัย
        public string insuranceProvince;//จังหวัด ผู้เอาประกันภัย
        public string insuranceZipcode;//รหัสไปรษณีย์ ผู้เอาประกันภัย

    }
    public class ResponseP_RECV_TAX_MBL
    {
        [MessageBodyMember]
        public DataP_RECV_TAX_MBL Obj;
    }

    [DataContract]
    public class DataP_RECV_TAX_MBL
    {

        [DataMember]
        public string responseCode { get; set; } // response code
        [DataMember]
        public string responseDescription { get; set; } // response description
        [DataMember]
        public string paysbuyTransactionID { get; set; } // paysbuy system transactionId
      

    }
    //---------------------------------------------------------------------- P_RECV_TAX_MBL

    //---------------------------------------------------------------------- Province
    public class Requestprovince
    {
        [MessageHeader]
        public string secureCode;

    }

    [DataContract]
    public class province
    {
        public province(string provinceid, string provincenameTH, string provincenameENG)
        {
            Provinceid = provinceid;
            ProvincenameTH = provincenameTH;
            ProvincenameENG = provincenameENG;

        }
        [DataMember]
        public string Provinceid { get; set; }
        [DataMember]
        public string ProvincenameTH { get; set; }
        [DataMember]
        public string ProvincenameENG { get; set; }

    }
    //-------------------------------------------------------------------- Province
    //-------------------------------------------------------------------- Name Title

    //----------------------------------------------------------------------VehicleType
     public class RequestvehicleType
    {
        [MessageHeader]
        public string secureCode;

    }
    [DataContract]
    public class VehicleType
    {
        public VehicleType(string vehicleTypeId, string vehicleTypeImage, string vehicleTypeNameTH,
                            string vehicleTypeNameTHDetail, string vehicleTypeNameEN , string vehicleTypeNameENDetail)
        {
            VehicleTypeId = vehicleTypeId;
            VehicleTypeImage = vehicleTypeImage;
            VehicleTypeNameTH = vehicleTypeNameTH;
            VehicleTypeNameTHDetail = vehicleTypeNameTHDetail;
            VehicleTypeNameEN = vehicleTypeNameEN;
            VehicleTypeNameENDetail = vehicleTypeNameENDetail;

        }
        [DataMember]
        public string VehicleTypeId { get; set; }
        [DataMember]
        public string VehicleTypeImage { get; set; }
        [DataMember]
        public string VehicleTypeNameTH { get; set; }
        [DataMember]
        public string VehicleTypeNameTHDetail { get; set; }
        [DataMember]
        public string VehicleTypeNameEN { get; set; }
        [DataMember]
        public string VehicleTypeNameENDetail { get; set; }
    }
    //----------------------------------------------------------------------VehicleType
    
}
