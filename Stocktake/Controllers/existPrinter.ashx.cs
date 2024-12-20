using Stocktake.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de existPrinter
    /// </summary>
    public class existPrinter : IHttpHandler
    {

        CWMI m_queryWin = new CWMI();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                //String djG = context.Request.Form["djG"];
                String html = "";
                bool result = m_queryWin.existPrinterTCS(ref html);

                if (result == true)
                {
                    json += "\"result\":\"true\",";
                    json += "\"html\":\""+ html +"\"";
                }
                else
                    json += "\"result\":\"false\"";


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