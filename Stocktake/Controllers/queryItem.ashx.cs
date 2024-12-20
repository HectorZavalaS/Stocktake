using Stocktake.Clases;
using Stocktake.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de queryItem
    /// </summary>
    public class queryItem : IHttpHandler
    {
        CCogiscan m_cogiscan = new CCogiscan();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            String html = "";
            String semifinish = "";
            //CCogiscanCGSDW m_db_11 = new CCogiscanCGSDW();
            //DataTable lastStation = null;
            //COracle m_oracle = new COracle("172.25.0.15", "MXPRD");
            try
            {
                String serial = context.Request.Form["serial"];
                CModelInfo infoModel = m_cogiscan.query_item(serial);
                semifinish = "PENDING";
                //lastStation = m_db_11.getLastStation(serial);
                //if (lastStation != null)
                //{

                //    DataRow last = lastStation.Rows[0];
                //    String operation = last["OPERATION_NAME"].ToString(); 
                //    if (operation == "SMT")
                //    {
                //        String Side = last["ROUTE_STEP_DESC"].ToString();
                //        if (!String.IsNullOrEmpty(Side))
                //            if(Side == "TOP" || Side == "BOTTOM")
                //                m_oracle.getSemifinish(last["BATCH_ID"].ToString(), Side == "TOP" ? "SMTT" : "SMTB", ref semifinish);
                //            else
                //                m_oracle.getSemifinish(last["BATCH_ID"].ToString(), "SMTT", ref semifinish);
                //        else
                //            m_oracle.getSemifinish(last["BATCH_ID"].ToString(), "SMTT", ref semifinish);
                //    }
                //    else
                //    {
                //        if(operation=="PACKING")
                //            m_oracle.getSemifinish(last["BATCH_ID"].ToString(),"PACK", ref semifinish);
                //        else
                //            if (operation == "FCT")
                //                m_oracle.getSemifinish(last["BATCH_ID"].ToString(), "TEST", ref semifinish);
                //            else
                //                m_oracle.getSemifinish(last["BATCH_ID"].ToString(), "MANU", ref semifinish);
                //    }

                    

                    json += "\"result\":\"true\",";
                    json += "\"serial\":\"" + serial + "\",";
                    json += "\"batchid\":\"" + infoModel.BatchId + "\",";
                    json += "\"operation\":\"" + infoModel.Operation + "\",";
                    json += "\"status\":\"" + infoModel.Status + "\",";
                    json += "\"route\":\"" + infoModel.Route + "\",";
                    json += "\"partNumber\":\"" + infoModel.PartNumber + "\",";
                    json += "\"semifinish\":\"" + semifinish + "\"";
                //}

            }
            catch (Exception ex)
            {
                json += "\"result\":\"false\",";
                json += "\"MessageError\":\"" + ex.Message + "\"";
            }
            json += "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}