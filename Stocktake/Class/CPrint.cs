using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace Stocktake.Class
{
    public class CPrint
    {
        public bool sendToPrinter(String printer, String ZPL, ref String errorMessage, String serials)
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = printer;
            return RawPrinterHelper.SendStringToPrinter(printer, ZPL, ref errorMessage, serials);
        }
        public bool sendToPrinterIP(String ip, String ZPL, ref String errorMessage)
        {
            // Printer IP Address and communication port
            string ipAddress = ip;
            int port = 9100;
            bool result = false;

            try
            {
                // Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(ipAddress, port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());
                writer.Write(ZPL);
                writer.Flush();

                // Close Connection
                writer.Close();
                client.Close();
                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }
            return result;

        }
    }
}