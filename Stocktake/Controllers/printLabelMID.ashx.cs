using Stocktake.Class;
using Stocktake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Descripción breve de printLabelMID
    /// </summary>
    public class printLabelMID : IHttpHandler
    {

        siixsem_stocktake_dbEntities m_db_st = new siixsem_stocktake_dbEntities();
        siixsem_sys_lblPrint_dbEntities m_db = new siixsem_sys_lblPrint_dbEntities();

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
                int idU = Convert.ToInt32(context.Request.Form["idU"]);

                //String zone = m_db_st.getUserZone(idU).First().RESULT;

                if (m_db.existDjGroup(djGroup).First().RESULT == 1)
                {

                    int count = Convert.ToInt32(m_db_st.getLastQtyByDJ(djGroup).First().COUNTLBL);
                    String mag = djGroup + "|" + count.ToString();
                    String qr = mag + "|" + model + "|" + semifinish + "|" + qty;
                    String template = m_db_st.getLabelMI(model, djGroup, semifinish, cgsRoute, qty, qr, mag.Replace("|", "-")).First().label;

                    if (template != "DOESNT_EXIST")
                    {
                        if (m_db_st.insertLabelMI(idU, serial, model, djGroup, semifinish, cgsRoute, qty, qr, mag.Replace("|", "-")).First().RESULT != -1)
                        {
                            m_db_st.updateLastQtyByDJ(djGroup, count);
                            m_print.sendToPrinter(namePrinter, template, ref error, mag);
                            json += "\"result\":\"true\",";
                            json += "\"MessageError\":\"" + "Se imprimio la etiqueta." + "\"";
                        }
                        else
                        {
                            json += "\"result\":\"false\",";
                            json += "\"MessageError\":\"" + "La etiqueta ya fue escaneada.<br>No se imprimira la etiqueta." + "\"";
                        }
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