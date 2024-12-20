using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de getLabelInfoST
    /// </summary>
    public class getLabelInfoST : IHttpHandler
    {
        siixsem_stocktake_dbEntities m_db = new siixsem_stocktake_dbEntities();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try {
                String label = context.Request.Form["qr"];
                if (!String.IsNullOrEmpty(label))
                {
                    getLabelInfoST_Result result = m_db.getLabelInfoST(label).First();

                    if (result != null)
                    {
                        json += "\"result\":\"true\",";
                        json += "\"label\":\""+result.se_barcode+"\",";
                        json += "\"serial\":\"" + result.se_serial_read + "\",";
                        json += "\"batchid\":\"" + result.se_dj_group + "\",";
                        json += "\"route\":\"" + result.se_cgs_route + "\",";
                        json += "\"partNumber\":\"" + result.se_model + "\",";
                        json += "\"semifinish\":\"" + result.se_semifinish + "\",";
                        json += "\"mag\":\"" + result.se_magazine + "\",";
                        json += "\"qty\":\"" + result.se_qty.ToString() + "\"";
                    }
                    else
                    {
                        json += "\"result\":\"false\",";
                        json += "\"MessageError\":\"" + "The scanned label doesn't exist!" + "\"";
                    }
                }
                else
                {
                    json += "\"result\":\"false\",";
                    json += "\"MessageError\":\"" + "The field can be empty!" + "\"";
                }


            }
            catch(Exception ex) {
                json += "\"result\":\"false\",";
                json += "\"MessageError\":\"" + ex.Message + "\"";
            }
            json += "}";
            context.Response.ContentType = "texto/normal";
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