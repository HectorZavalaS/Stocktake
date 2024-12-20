using Stocktake.Clases;
using Stocktake.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de queryItems
    /// </summary>
    public class queryItems : IHttpHandler
    {

        CCogiscan m_cogiscan = new CCogiscan();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            String html = "";
            String semifinish = "";
            try
            {
                //String serial = context.Request.Form["serial"];
                m_cogiscan.getInfoserials();



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