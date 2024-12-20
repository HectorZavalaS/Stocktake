using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2;

namespace Stocktake.Class
{
    class CCogiscanCGSDW
    {
        String m_ip;
        String m_user;
        String m_password;
        String m_alias;
        String m_strConection;
        DB2Connection m_CgsDB;
        DB2Command m_DB2Command;
        DB2DataReader m_Db2DataReader;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public CCogiscanCGSDW()
        {
            m_ip = "192.168.3.11:50000";
            m_user = "CGSAPP";
            m_password = "T0mcat4Fun";
            m_alias = "CGSDW";
            m_DB2Command = null;
            m_Db2DataReader = null;

            m_strConection = "server=" + m_ip + ";database=" + m_alias + ";uid=" + m_user + ";pwd=" + m_password + ";";
        }
        public bool connect()
        {
            bool result = false;
            try
            {
                m_CgsDB = new DB2Connection(m_strConection);
                
                m_CgsDB.Open();
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
            }

            return result;
        }
        public void close()
        {
            if(m_CgsDB!=null) m_CgsDB.Close();
        }
        public List<String> getPanelSerials(String serial)
        {
            //DataTable traceability = new DataTable();
            String date = "";
            String batch_id = "";
            List<String> serials = new List<string>();

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT BATCH_ID,EVENT_TMST FROM CGSPCM.PRODUCT_TRACEABILITY_ALL WHERE PRODUCT_ID = '" + serial.ToUpper() + "' AND EVENT_TYPE = 'RELEASE PRODUCT'"; 

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        date = m_Db2DataReader.GetString(0);
                        batch_id = m_Db2DataReader.GetString(1);
                    }
                    //m_Db2DataReader.Close();

                    ////////////////////////////////////////////////////////////////////////////////////////////
                    ///       SE OBTIENEN LOS SERIALES DEL PANEL
                    /////////////////////////////////////////

                    m_DB2Command.CommandText = "SELECT PRODUCT_ID  FROM CGSPCM.PRODUCT_TRACEABILITY_ALL where BATCH_ID = '" + batch_id + "' and EVENT_TMST = '" + date + "'";

                    m_Db2DataReader = m_DB2Command.ExecuteReader();

                    if (m_Db2DataReader.HasRows)
                    {
                        while (m_Db2DataReader.Read())
                        {
                            serials.Add(m_Db2DataReader.GetString(0).ToUpper());
                        }
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch(Exception ex)
            {
                logger.Error(ex, "[getPanelSerials] Error " + serial);
            }

            return serials;
        }
        public List<String> getSerialsWaitingAOI(String Batch_id)
        {
            List<String> serials = new List<string>();

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT I.ITEM_ID FROM CGS.ITEM I "
                                        + "JOIN CGSPCM.PRODUCT PROD ON(PROD.PRODUCT_KEY = I.ITEM_KEY) "
                                        + "JOIN CGS.PART_NUMBER PN ON(PN.PART_NUMBER_KEY = I.PART_NUMBER_KEY) "
                                        + "JOIN CGSPCM.ROUTE_STEP RS ON(RS.ROUTE_STEP_KEY = PROD.ROUTE_STEP_KEY) "
                                        + "JOIN CGSPCM.OPERATION OPER ON(OPER.OPERATION_KEY = RS.OPERATION_KEY) "
                                        + "JOIN CGSPCM.PRODUCT_BATCH BATCH ON(BATCH.BATCH_KEY = PROD.BATCH_KEY) "
                                        + "JOIN CGS.ITEM_TYPE IT ON(IT.ITEM_TYPE_KEY = I.ITEM_TYPE_KEY) "
                                        + "JOIN CGS.ITEM_TYPE_CLASS ITC ON(ITC.ITEM_CLASS_KEY = IT.ITEM_CLASS_KEY) "
                                        + "LEFT OUTER JOIN CGS.ITEM TOOL ON(TOOL.ITEM_KEY = PROD.TOOL_KEY) "
                                        + "WHERE ITC.SHORT_NAME <> 'Product Carrier' and BATCH.BATCH_ID = '" + Batch_id + "' AND "
                                        + "      OPERATION_NAME = 'AOI Inspection' AND STATUS = 'W'";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        serials.Add(m_Db2DataReader.GetString(0).ToUpper());
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getSerialsWaitingAOI] Error " + Batch_id);
            }

            return serials;
        }

        public String getLineByBatch(String Batch_id)
        {
            String Line = "";

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT DISTINCT TOOL_ID ,BATCH_ID  FROM CGSPCM.PRODUCT_TRACEABILITY_ALL where BATCH_ID = '" + Batch_id + "' and OPERATION_NAME = 'AOI Inspection'";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        Line = m_Db2DataReader.GetString(0).ToUpper();
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getSerialsWaitingAOI] Error " + Batch_id);
            }

            return Line;
        }
        public List<String> getSerialsActiveAOI(String Batch_id)
        {
            List<String> serials = new List<string>();

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT I.ITEM_ID FROM CGS.ITEM I "
                                        + "JOIN CGSPCM.PRODUCT PROD ON(PROD.PRODUCT_KEY = I.ITEM_KEY) "
                                        + "JOIN CGS.PART_NUMBER PN ON(PN.PART_NUMBER_KEY = I.PART_NUMBER_KEY) "
                                        + "JOIN CGSPCM.ROUTE_STEP RS ON(RS.ROUTE_STEP_KEY = PROD.ROUTE_STEP_KEY) "
                                        + "JOIN CGSPCM.OPERATION OPER ON(OPER.OPERATION_KEY = RS.OPERATION_KEY) "
                                        + "JOIN CGSPCM.PRODUCT_BATCH BATCH ON(BATCH.BATCH_KEY = PROD.BATCH_KEY) "
                                        + "JOIN CGS.ITEM_TYPE IT ON(IT.ITEM_TYPE_KEY = I.ITEM_TYPE_KEY) "
                                        + "JOIN CGS.ITEM_TYPE_CLASS ITC ON(ITC.ITEM_CLASS_KEY = IT.ITEM_CLASS_KEY) "
                                        + "LEFT OUTER JOIN CGS.ITEM TOOL ON(TOOL.ITEM_KEY = PROD.TOOL_KEY) "
                                        + "WHERE ITC.SHORT_NAME <> 'Product Carrier' and BATCH.BATCH_ID = '" + Batch_id + "' AND "
                                        + "      OPERATION_NAME = 'AOI Inspection' AND STATUS = 'A'";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        serials.Add(m_Db2DataReader.GetString(0).ToUpper());
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getSerialsActiveAOI] Error " + Batch_id);
            }

            return serials;

        }

        public DataTable getTracHistory(String serial)
        {
            DataTable history = new DataTable();
            DataTable historyPart1 = null;
            bool hasChangeID = false;
            String strChangeId = "";
            history.Columns.Add("BATCH_ID");
            history.Columns.Add("PRODUCT_PN");
            history.Columns.Add("PRODUCT_ID");
            history.Columns.Add("OPERATION_NAME");
            history.Columns.Add("ROUTE_STEP_DESC");
            history.Columns.Add("TOOL_ID");
            history.Columns.Add("ROUTE_NAME");
            history.Columns.Add("PREV_ITEM_ID");

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT BATCH_ID, PRODUCT_PN, PRODUCT_ID,CASE WHEN EVENT_TYPE = 'CHANGE ID' THEN 'CHANGE ID' ELSE OPERATION_NAME END OPERATION_NAME, ROUTE_STEP_DESC, " +
                                            "CASE WHEN TOOL_ID IS NULL THEN ' ' ELSE TOOL_ID END TOOL_ID, ROUTE_NAME, CASE WHEN PREV_ITEM_ID IS NULL THEN ' ' ELSE PREV_ITEM_ID END PREV_ITEM_ID " +
                                            "FROM CGSPCM.PRODUCT_TRACEABILITY_ALL " +
                                            "where PRODUCT_ID = '" + serial.Trim() + "' AND (EVENT_TYPE = 'END OPER' OR EVENT_TYPE = 'CHANGE ID') " +
                                            "	 AND ((TOOL_ID NOT LIKE '%SPG%' AND TOOL_ID NOT LIKE '%COMPLEX%' ) OR TOOL_ID IS NULL) " +
                                            "    AND OPERATION_NAME NOT LIKE '%Reflow%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%AOI%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%ICT%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Conformal%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Router%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%PQC%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%MIDDLE%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%X-RAY%' " +
                                            "ORDER BY EVENT_TMST";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        String nSerial = m_Db2DataReader.GetString(7).ToUpper();
                        if (!String.IsNullOrEmpty(nSerial) && nSerial != " ")
                        {
                            hasChangeID = true;
                            strChangeId = nSerial;
                        }

                        //DataRow []res = history.Select("OPERATION_NAME = '" + m_Db2DataReader.GetString(3).ToUpper() +  "'");

                        //if(res==null)
                            history.Rows.Add(m_Db2DataReader.GetString(0).ToUpper(), m_Db2DataReader.GetString(1).ToUpper(), m_Db2DataReader.GetString(2).ToUpper(), m_Db2DataReader.GetString(3).ToUpper(),
                                                m_Db2DataReader.GetString(4).ToUpper(), m_Db2DataReader.GetValue(5) == null ? " " : m_Db2DataReader.GetString(5).ToUpper(), m_Db2DataReader.GetString(6).ToUpper(),
                                                m_Db2DataReader.GetString(7).ToUpper());
                        
                    }
                }
                if(hasChangeID)
                {
                    historyPart1 = getTracHistory(strChangeId);
                    historyPart1.Merge(history);
                    history = historyPart1;
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getTracHistory] Error " + serial);
            }

            return history;

        }
        public DataTable getLastStation(String serial)
        {
            DataTable history = new DataTable();
            history.Columns.Add("BATCH_ID");
            history.Columns.Add("PRODUCT_PN");
            history.Columns.Add("PRODUCT_ID");
            history.Columns.Add("OPERATION_NAME");
            history.Columns.Add("ROUTE_STEP_DESC");
            history.Columns.Add("TOOL_ID");
            history.Columns.Add("ROUTE_NAME");
            history.Columns.Add("PREV_ITEM_ID");

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT BATCH_ID, PRODUCT_PN, PRODUCT_ID,CASE WHEN EVENT_TYPE = 'CHANGE ID' THEN 'CHANGE ID' ELSE OPERATION_NAME END OPERATION_NAME, ROUTE_STEP_DESC, " +
                                            "CASE WHEN TOOL_ID IS NULL THEN ' ' ELSE TOOL_ID END TOOL_ID, ROUTE_NAME, CASE WHEN PREV_ITEM_ID IS NULL THEN ' ' ELSE PREV_ITEM_ID END PREV_ITEM_ID " +
                                            "FROM CGSPCM.PRODUCT_TRACEABILITY_ALL " +
                                            "where PRODUCT_ID = '" + serial.ToUpper().Trim() + "' AND (EVENT_TYPE = 'END OPER' OR EVENT_TYPE = 'CHANGE ID' OR EVENT_TYPE = 'RELEASE PRODUCT') " +
                                            "	 AND ((TOOL_ID NOT LIKE '%SPG%' AND TOOL_ID NOT LIKE '%COMPLEX%' ) OR TOOL_ID IS NULL) " +
                                            "    AND OPERATION_NAME NOT LIKE '%Reflow%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%AOI%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%ICT%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Conformal%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Router%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%PQC%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%MIDDLE%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%X-RAY%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Rework%' " +
                                            "ORDER BY EVENT_TMST DESC " +
                                            "FETCH FIRST 1 ROWS ONLY";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        history.Rows.Add(m_Db2DataReader.GetString(0).ToUpper(), m_Db2DataReader.GetString(1).ToUpper(), m_Db2DataReader.GetString(2).ToUpper(), m_Db2DataReader.GetString(3).ToUpper(),
                                            m_Db2DataReader.GetString(4).ToUpper(), m_Db2DataReader.GetValue(5) == null ? " " : m_Db2DataReader.GetString(5).ToUpper(), m_Db2DataReader.GetString(6).ToUpper(),
                                            m_Db2DataReader.GetString(7).ToUpper());

                    }
                }

                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getTracHistory] Error " + serial);
            }

            return history;
        }
        public DataTable getLastStationByPanel(String serial, String djGroup)
        {
            DataTable history = new DataTable();
            history.Columns.Add("BATCH_ID");
            history.Columns.Add("PRODUCT_PN");
            history.Columns.Add("PRODUCT_ID");
            history.Columns.Add("OPERATION_NAME");
            history.Columns.Add("ROUTE_STEP_DESC");
            history.Columns.Add("TOOL_ID");
            history.Columns.Add("ROUTE_NAME");
            history.Columns.Add("PREV_ITEM_ID");

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH


                m_DB2Command.CommandText = "SELECT BATCH_ID, PRODUCT_PN, PRODUCT_ID,CASE WHEN EVENT_TYPE = 'CHANGE ID' THEN 'CHANGE ID' ELSE OPERATION_NAME END OPERATION_NAME, ROUTE_STEP_DESC, " +
                                            "CASE WHEN TOOL_ID IS NULL THEN ' ' ELSE TOOL_ID END TOOL_ID, ROUTE_NAME, CASE WHEN PREV_ITEM_ID IS NULL THEN ' ' ELSE PREV_ITEM_ID END PREV_ITEM_ID " +
                                            "FROM CGSPCM.PRODUCT_TRACEABILITY_ALL " +
                                            "where PANEL_ID = '" + serial.ToUpper().Trim() + "' AND BATCH_ID ='" + djGroup + "' AND (EVENT_TYPE = 'END OPER' OR EVENT_TYPE = 'START OPER' OR EVENT_TYPE = 'CHANGE ID' OR EVENT_TYPE = 'RELEASE PRODUCT') " +
                                            "	 AND ((TOOL_ID NOT LIKE '%SPG%' AND TOOL_ID NOT LIKE '%COMPLEX%' ) OR TOOL_ID IS NULL) " +
                                            "    AND OPERATION_NAME NOT LIKE '%Reflow%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%AOI%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%ICT%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Conformal%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Router%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%PQC%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%MIDDLE%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%X-RAY%' " +
                                            "    AND OPERATION_NAME NOT LIKE '%Rework%' " +
                                            "ORDER BY EVENT_TMST DESC " +
                                            "FETCH FIRST 1 ROWS ONLY";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        history.Rows.Add(m_Db2DataReader.GetString(0).ToUpper(), m_Db2DataReader.GetString(1).ToUpper(), m_Db2DataReader.GetString(2).ToUpper(), m_Db2DataReader.GetString(3).ToUpper(),
                                            m_Db2DataReader.GetString(4).ToUpper(), m_Db2DataReader.GetValue(5) == null ? " " : m_Db2DataReader.GetString(5).ToUpper(), m_Db2DataReader.GetString(6).ToUpper(),
                                            m_Db2DataReader.GetString(7).ToUpper());

                    }
                }

                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getTracHistory] Error " + serial);
            }

            return history;
        }
    }
}
