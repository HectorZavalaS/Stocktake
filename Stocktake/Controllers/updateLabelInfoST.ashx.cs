using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de updateLabelInfoST
    /// </summary>
    public class updateLabelInfoST : IHttpHandler
    {

        siixsem_stocktake_dbEntities m_db = new siixsem_stocktake_dbEntities();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                String lblQr = context.Request.Form["qr"];
                int qty = Convert.ToInt32(context.Request.Form["qty"]);

                if (!String.IsNullOrEmpty(lblQr) && qty != 0)
                {

                    int result = m_db.updateQtyMagST(lblQr,qty).First().RESULT;

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