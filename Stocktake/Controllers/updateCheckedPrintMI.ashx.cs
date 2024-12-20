using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de updateCheckedPrintMI
    /// </summary>
    public class updateCheckedPrintMI : IHttpHandler
    {
        siixsem_stocktake_dbEntities m_db = new siixsem_stocktake_dbEntities();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                String lblQr = context.Request.Form["lblQr"];

                if (!String.IsNullOrEmpty(lblQr))
                {

                    int result = m_db.updateCheckedPrintMI(lblQr).First().RESULT;

                    if (result == 1)
                        json += "\"result\":\"true\"";
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