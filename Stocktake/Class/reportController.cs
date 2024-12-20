using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

///// 0 - inicio de sesion
///// 1 - nueva remision
///// 2 - modicar remision
///// 3 - imprimir remision


/// 4 DIGITOS 
/// 1    MODULO          : 7
/// 2    TIPO DE MODULO  : 3
/// 3-4  NUMERO DE ERROR : 00-99

/*************************************************************************************************************************************/
/*     CODIGOS DE ERROR                                                                                                              */
/*                                                                                                                                   */
/*                                                                                                                                   */
/*                                                                                                                                   */
/*                                                                                                                                   */
/*                                                                                                                                   */
namespace Stocktake.Class
{
    public class reportsController
    {
        static private ReportDocument crystalReport = new ReportDocument();
        public reportsController()
        {
            crystalReport = new ReportDocument();
        }
        public bool loadReport(string nameReport)
        {
            bool result = false;
            if (crystalReport != null)
            {
                crystalReport.Close();
                crystalReport.Load(nameReport);
                result = true;
            }
            return result;
        }
        public string connect(string user, string password, string server, string bd)
        {
            try
            {
                crystalReport.SetDatabaseLogon(user, password,server,bd);//nombre de la db
            }
            catch (Exception ex)
            {
                return "Error: 7301. " + ex.Message; ///// no se conecto
            }
            return "Success";
        }
        public string connect()
        {
            try
            {
                crystalReport.SetDatabaseLogon("sa", "Passw0rd", @"SEPRONALDB01\SEPRONAL", "Almacen");//nombre de la db
                //crystalReport.SetDatabaseLogon("sa", "Passw0rd", @"148.232.32.238\SEPRONAL", "Almacen");//nombre de la db
                //crystalReport.SetDatabaseLogon("sa", "Passw0rd");
            }
            catch (Exception ex)
            {
                return "Error: 7301. " + ex.Message; ///// no se conecto
            }
            return "Success";
        }
        public string addFieldValue(string nameField, string valueField)
        {
            try
            {
                crystalReport.DataDefinition.FormulaFields[nameField].Text = valueField;
                //crystalReport.Refresh();
            }
            catch (Exception ex)
            {
                return "Error: 7302. " + ex.Message; ///// no se conecto
            }
            return "Succes";
        }
        public string setParameter(string nameField, string valueField)
        {
            try
            {
                crystalReport.SetParameterValue(nameField, valueField);
            }
            catch (Exception ex)
            {
                return "Error: 7302. " + ex.Message; ///// no se conecto
            }
            return "Succes";
        }
        public void refresh()
        {
            crystalReport.Refresh();
        }
        public bool generateReport(string path, string fileName, ref string pdfEmbebed,string rutaR, ref string info)
        {
            bool res = false;
            string realPath = "";
            try
            {
                DiskFileDestinationOptions crDiskFileDestinationOptions;
                ExportOptions crExportOptions;

                realPath = path + fileName;

                crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                crDiskFileDestinationOptions.DiskFileName = realPath;
                crExportOptions = crystalReport.ExportOptions;
                {
                    crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                    crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                }

                crystalReport.Export();
                pdfEmbebed = getEmbeddedPDF(rutaR,  fileName);
                res = true;

            }
            catch (Exception ex) {
                res = false;
                info = "Error: 7303. " + ex.Message + "<br>" + ex.Source + "<br>" + ex.StackTrace + "<br>" + ex.TargetSite + "<br>" + crystalReport.Name + "<br>" + crystalReport.RecordSelectionFormula + "<br>" + crystalReport.ReportAppServer + "<br>" + crystalReport.DataSourceConnections.ToString() + "<br>************************************************************<br>" + path + fileName  +"<br>************************************************************<br>" + realPath;
            }
            return res;
        }
        public string getEmbeddedPDF(string rPath, string fileName)
        {
            string embed = "<object data='" + rPath + "' type='application/pdf' width='100%' style='height: 500px; margin - top: -15px;'>";
            embed += "If you are unable to view file, you can download from <a href = '" + rPath + "'>here</a>";
            embed += " or download <a target = '_blank' href = 'http://get.adobe.com/reader/'>Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            return embed;
        }
        public bool generateReportExcel(string path, string fileName, ref string excelPath, string rutaR, ref string info)
        {
            bool res = false;
            string realPath = "";
            try
            {
                DiskFileDestinationOptions crDiskFileDestinationOptions;
                ExportOptions crExportOptions;

                realPath = path + fileName;

                crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                crDiskFileDestinationOptions.DiskFileName = realPath;
                crExportOptions = crystalReport.ExportOptions;
                {
                    crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                    crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    crExportOptions.ExportFormatType = ExportFormatType.Excel;
                }

                crystalReport.Export();
                excelPath = getEmbeddedExcel(rutaR, fileName);
                res = true;

            }
            catch (Exception ex)
            {
                res = false;
                info = "Error: 7303. " + ex.Message + "<br>" + ex.Source + "<br>" + ex.StackTrace + "<br>" + ex.TargetSite + "<br>" + crystalReport.Name + "<br>" + crystalReport.RecordSelectionFormula + "<br>" + crystalReport.ReportAppServer + "<br>" + crystalReport.DataSourceConnections.ToString() + "<br>************************************************************<br>" + path + fileName + "<br>************************************************************<br>" + realPath;
            }
            return res;
        }
        public string getEmbeddedExcel(string rPath, string fileName)
        {
            string embed = "<a class='button small blue rounded' href = '" + rPath + fileName + "'>Exportar a Excel</a>";
            return embed;
        }
    }
}