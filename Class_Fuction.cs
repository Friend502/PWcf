using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

using System.Text.RegularExpressions;
namespace PWCF
{
    public class Class_Fuction
    {
        public string ChckIIF(bool Expression, string TruePart, string FalsePart)
        {
            string ReturnValue = Expression == true ? TruePart : FalsePart;

            return ReturnValue;
        }

 
        public string returncheck_Message(string P_CAL_TAX_MBL_message)
        {
            string returemess = "";
            List<string> list = new List<string>();
            list.Add("500 - ข้อมูลสำคัญมีค่าเป็น Null หรือไม่ถูกต้อง");
            list.Add("592 - อายุรถเกิน 5 ปี หรือ 7 ปี แล้วแต่กรณี");
            list.Add("100 - ไม่พบข้อมูล");
            list.Add("103 - จัดเก็บข้อมูลไม่สำเร็จ");
            foreach (string i in list)
            {
                if (P_CAL_TAX_MBL_message.IndexOf(i) >= 0)
                {
                    returemess = "1";
                    return returemess;
                }
            }
            return returemess;
        }
    }
}