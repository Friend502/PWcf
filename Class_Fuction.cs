using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

using System.Text.RegularExpressions;
using System.IO;
namespace PWCF
{
    public class Class_Fuction
    {
        public string ChckIIF(bool Expression, string TruePart, string FalsePart)
        {
            string ReturnValue = Expression == true ? TruePart : FalsePart;

            return ReturnValue;
        }

        public string returncheck_Message2(string P_CAL_TAX_MBL_message)
        {
            if (P_CAL_TAX_MBL_message != "")
            {

                List<string> list = new List<string>();
                list.Add("500 Important data cannot not be null.");//Important data cannot not be null.
                list.Add("592 Cannot process . Over a 5 or 7 year old car.");//Cannot process . Over a 5 or 7 year old car.
                list.Add("100 Tax is already paid.");//Tax is already paid.
                list.Add("103 data cannot not.");//Server error.
                list.Add("999 Server error.");//Server error.
                list.Add("000 Success.");//Server error.
                foreach (string wordcheck in list)
                {
                    if (P_CAL_TAX_MBL_message.Substring(0, 3) == wordcheck.Substring(0, 3))
                    {
                        P_CAL_TAX_MBL_message = wordcheck;
                    }
                }

            }
            else
            {
                P_CAL_TAX_MBL_message = ("999 Server error.");//Server error.
            }
            return P_CAL_TAX_MBL_message;

        }

        public string returncheck_Message(string P_CAL_TAX_MBL_message)
        {
            string returemess = "";
            List<string> list = new List<string>();
            list.Add("500");//Important data cannot not be null.
            list.Add("592");//Cannot process . Over a 5 or 7 year old car.
            list.Add("100");//Tax is already paid.
            list.Add("103");//Server error.
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
        public string Logfile(string data, string inquiryRefId,string Newfolder)
        {
            try
            {
                string filename = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "_");
                string sPathName = System.Web.Hosting.HostingEnvironment.MapPath("~") + "/" + Newfolder +"/" + inquiryRefId + ".csv";
                StreamWriter sw = new StreamWriter(sPathName, true);
                sw.WriteLine(DateTime.Now.ToString() + "," + data);
                sw.Flush();
                sw.Close();
            }
            catch
            {

                string sPathName = System.Web.Hosting.HostingEnvironment.MapPath("~") + @"\LogError\" + "FileError" + ".csv";
                StreamWriter sw = new StreamWriter(sPathName, true);
                sw.WriteLine(DateTime.Now.ToString() + "," + data);
                sw.Flush();
                sw.Close();
            }
            return "Log";
        }

        

        public string returncheck_Message_103(string P_CAL_TAX_MBL_message)
        {
            string returemess = "";
            List<string> list = new List<string>();
            list.Add("500");
            list.Add("103");//Server error.
            foreach (string i in list)
            {
                if (P_CAL_TAX_MBL_message.IndexOf(i) >= 0)
                {
                    returemess = "500 Important data cannot not be null";
                    return returemess;
                }
            }
            return returemess;
        }

        public string T_vehicleType(string a)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/T_vehicleType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
            string[] textData = System.IO.File.ReadAllLines(directory);
            foreach (string wordcheck in textData)
              {
                 string[] s = wordcheck.Split(',');
                 if (a == s[0].ToString())
                    {
                        a = s[5].ToString();
                    }

               }
            return a;
        }
        public string T_provinceType(string a)
        {
            string directory2 = System.Web.Hosting.HostingEnvironment.MapPath("~/T_provinceType.txt"); // ดึกข้อมูลจาก .txt file มาแสดง
            string[] textData2 = System.IO.File.ReadAllLines(directory2);
            foreach (string wordcheck2 in textData2)
                {
                    string[] s2 = wordcheck2.Split(',');
                    if (a == s2[0].ToString())
                    {
                        a = s2[2].ToString();
                    }

                }
            return a;
        }

       
    }
}