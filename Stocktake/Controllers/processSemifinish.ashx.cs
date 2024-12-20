using Stocktake.Clases;
using Stocktake.Class;
using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Summary description for processSemifinish
    /// </summary>
    public class processSemifinish : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                String semifinish = "";
                siixsem_stocktake_dbEntities m_db = new siixsem_stocktake_dbEntities();
                CCogiscanCGSDW m_db_11 = new CCogiscanCGSDW();
                DataTable lastStation = null;
                String fecha = context.Request.Form["fecha"];
                
                var serials = m_db.getSerialSemPen(fecha);
                CCogiscan m_cogiscan = new CCogiscan();
                COracle m_oracle = new COracle("192.168.0.25", "SEMPROD");
                String tempDj = "";

                foreach (getSerialSemPen_Result serial in serials)
                {
                    //Thread.Sleep(500);
                    semifinish = "";
                    lastStation = m_db_11.getLastStation(serial.se_serial_read);
                    String strSer = "";
                    if (lastStation.Rows.Count == 0)
                    {
                        CModelInfo model = m_cogiscan.query_item(serial.se_serial_read);

                        lastStation = m_db_11.getLastStationByPanel(serial.se_serial_read,model.BatchId);

                    }
                    if (lastStation != null && lastStation.Rows.Count > 0)
                    {
                        try
                        {
                            DataRow last = lastStation.Rows[0];
                            String operation = last["OPERATION_NAME"].ToString();
                            String djGroup = tempDj = last["BATCH_ID"].ToString();

                            if (operation.Contains("CHANGE ID"))
                            {
                                lastStation = m_db_11.getLastStation(last["PREV_ITEM_ID"].ToString());
                                last = lastStation.Rows[0];
                                operation = last["OPERATION_NAME"].ToString();
                            }

                            
                            string b = string.Empty;
                            String Side = "";
                            try
                            {
                                int temp = int.Parse(djGroup);
                            }
                            catch (Exception ex)
                            {
                                for (int i = 0; i < djGroup.Length; i++)
                                {
                                    if (Char.IsDigit(djGroup[i]))
                                        b += djGroup[i];
                                    else
                                        break;
                                }

                                if (b.Length > 0)
                                    djGroup = b;
                            }

                            if (operation == "SMT")
                            {
                                Side = last["ROUTE_STEP_DESC"].ToString();
                                if (!String.IsNullOrEmpty(Side))
                                    if (Side == "TOP" || Side == "BOTTOM")
                                        m_oracle.getSemifinish(djGroup, Side == "TOP" ? "SMTT" : "SMTB", ref semifinish);
                                    else
                                        m_oracle.getSemifinish(djGroup, "SMTT", ref semifinish);
                                else
                                    m_oracle.getSemifinish(djGroup, "SMTT", ref semifinish);
                            }
                            else
                            {
                                if (operation == "PACKING")
                                {
                                    if (!m_oracle.getSemifinish(djGroup, "PACK", ref semifinish))
                                        m_oracle.getSemifinish(djGroup, "OQC", ref semifinish);
                                }   
                                else
                                    if (operation == "FCT" || operation == "MIDDLE TEST")
                                {
                                    if (!m_oracle.getSemifinish(djGroup, "TEST", ref semifinish))
                                        if (!m_oracle.getSemifinish(djGroup, "PACK", ref semifinish))
                                            if (!m_oracle.getSemifinish(djGroup, "MANU", ref semifinish))
                                                m_oracle.getSemifinish(djGroup, "OQC", ref semifinish);
                                }
                                else
                                    m_oracle.getSemifinish(djGroup, "MANU", ref semifinish);
                            }

                            if (semifinish == "PENDING")
                                semifinish = "PENDING";

                            if (String.IsNullOrEmpty(semifinish))
                                semifinish = "PENDING";


                            m_db.updateSemifinish(serial.se_serial_read, djGroup, semifinish);
                            //Thread.Sleep(200);
                        }
                        catch (Exception ex) {
                            String E = ex.Message;
                            json += "\"result\":\"false\",";
                            json += "\"MessageError\":\"" + ex.Message + "\"";
                        }
                    }
                }
                json += "\"result\":\"true\",";
                json += "\"MessageSuccess\":\"" + "Se actualizaron los semifishgod." + "\"";
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