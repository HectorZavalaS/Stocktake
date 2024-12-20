using Stocktake.Class;
using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de rePrintLabel
    /// </summary>
    public class rePrintLabel : IHttpHandler
    {

        siixsem_stocktake_dbEntities m_db_st = new siixsem_stocktake_dbEntities();
        CPrint m_print = new CPrint();
        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                String error = "";
                String djGroup = context.Request.Form["dj_group"];
                String model = context.Request.Form["model"];
                String semifinish = context.Request.Form["semi"];
                String qty = context.Request.Form["qty"];
                String cgsRoute = context.Request.Form["cgs_route"];
                String namePrinter = context.Request.Form["name_printer"];
                String serial = context.Request.Form["serial"];
                String mag = context.Request.Form["mag"];
                int idU = Convert.ToInt32(context.Request.Form["idU"]);

                String zone = m_db_st.getUserZone(idU).First().RESULT;

                if (zone != "NOT_FOUND")
                {

                    String qr = mag + "|" + model + "|" + djGroup + "|" + semifinish + "|" + qty;
                    String template = m_db_st.getLabelST(model, djGroup, semifinish, cgsRoute, qty, qr, mag).First().label;

                    if (template != "DOESNT_EXIST")
                    {
                        m_print.sendToPrinter(namePrinter, template, ref error, mag);
                        json += "\"result\":\"true\",";
                        json += "\"MessageError\":\"" + "Se imprimio la etiqueta." + "\"";
                    }
                    else
                    {
                        json += "\"result\":\"false\",";
                        json += "\"MessageError\":\"" + "No se encontro el código ZPL de la etiqueta Stocktake." + "\"";
                    }

                }
                else
                {
                    json += "\"result\":\"false\",";
                    json += "\"MessageError\":\"" + "No se encontro la zona del usuario." + "\"";
                }
            }
            catch (Exception ex)
            {
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