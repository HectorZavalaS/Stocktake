using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;
using System.Text;

namespace Stocktake.Class
{
    public class CWMI
    {
        private ManagementObjectSearcher m_searcher;
        public CWMI()
        {
            //m_searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer");
        }

        public bool existPrinterTCS(ref String htmlResult)
        {
            bool result = false;
            htmlResult = "";
            m_searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer WHERE DriverName = 'TSC Alpha-3R'");

            if (m_searcher != null)
            {
                result = true;
                foreach (ManagementObject queryObj in m_searcher.Get())
                {
                    htmlResult += "<option>" + queryObj["Name"].ToString() + "</option>";
                }
            }
            return result;
        }
        String getMacAddressFromIP(String ip)
        {

            ManagementScope theScope = new ManagementScope("\\\\"+ ip +"\\root\\cimv2");
            string macAdd= "";

            StringBuilder theQueryBuilder = new StringBuilder();
            theQueryBuilder.Append("SELECT MACAddress FROM Win32_NetworkAdapter");
            ObjectQuery theQuery = new ObjectQuery(theQueryBuilder.ToString());
            ManagementObjectSearcher theSearcher = new ManagementObjectSearcher(theScope, theQuery);
            ManagementObjectCollection theCollectionOfResults = theSearcher.Get();

            foreach (ManagementObject theCurrentObject in theCollectionOfResults)
            {
                macAdd = theCurrentObject["MACAddress"].ToString();
            }
            return macAdd;
        }
    }
}