using Stocktake.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocktake.Controllers
{
    /// <summary>
    /// Summary description for getReport
    /// </summary>
    public class getReport : IHttpHandler
    {

        reportsController m_report = new reportsController();
        CUtils m_utils = new CUtils();
        public void ProcessRequest(HttpContext context)
        {
            string json = "{";
            string pdf = "";
            string info = "";
            string excel = "";
            try
            {
                string date = context.Request.Form["date"];
                string path = context.Request.Form["dir"];

                //m_report.connect("sa", "S11x4dm1n2018!", @"192.168.3.28\SIIXSEMSQL2016", "siixsem_stocktake_db");
                m_report.loadReport(context.Server.MapPath("~/Reports/stocktake.rpt"));
                //m_report.connect("sa", "S11x4dm1n2018!", @"192.168.3.28\SIIXSEMSQL2016", "siixsem_stocktake_db");
                //m_report.connect("sa", "S11x4dm1n2018!", @"192.168.3.28\SIIXSEMSQL2016", "siixsem_main_db");
                //m_report.addFieldValue("idCat", idP);
               
                //m_report.setParameter("@date", date);

                m_report.connect("sa", "S11x4dm1n2018!", @"192.168.3.28\SIIXSEMSQL2016", "siixsem_stocktake_db");
                //m_report.connect("sa", "S11x4dm1n2018!", @"192.168.3.28\SIIXSEMSQL2016", "siixsem_main_db");
                m_report.addFieldValue("d", "'" + date + "'");

                m_report.refresh();

                string FileName = "stkRpt_" + DateTime.Now.ToShortDateString().Replace("/", "") + ".pdf";
                string FileNameE = "stkRpt_" + DateTime.Now.ToShortDateString().Replace("/", "") +  ".xls";//nombre archivo
                //string ruta = "PDF/incidencias/" + FileName;
                //string rutaE = "EXCEL/incidencias/";
                string ruta = path + "/PDF/stocktake/" + FileName;
                string rutaE = path + "/EXCEL/stocktake/";


                m_utils.createDirectory(context.Server.MapPath("~/PDF/stocktake/"));
                m_utils.createDirectory(context.Server.MapPath("~/EXCEL/stocktake/"));//directorio archivo
                if (m_report.generateReport(context.Server.MapPath("~/PDF/stocktake/"), FileName, ref pdf, ruta, ref info) && m_report.generateReportExcel(context.Server.MapPath("~/EXCEL/stocktake/"), FileNameE, ref excel, rutaE, ref info))
                {
                    json += "\"result\" : \"true\",";
                    json += "\"messageSuccess\":\"Se realizó la operación con éxito.\",";
                    json += "\"excel\":\"" + excel + "\",";
                    json += "\"pdf\":\"" + pdf + "\"";
                }
                else
                {
                    json += "\"result\" : \"false\",";
                    json += "\"messageError\":\"" + info.Replace("/n","<br>") + "\"";
                }

            }
            catch (Exception ex)
            {
                json += "\"result\" : \"false\",\n";
                json += "\"messageError\":\"" + ex.Message + "\"";
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