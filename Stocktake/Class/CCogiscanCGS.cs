using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2;

namespace Stocktake.Class
{
    class CCogiscanCGS
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
        public CCogiscanCGS()
        {
            m_ip = "192.168.3.2:50000";
            m_user = "CGSAPP";
            m_password = "T0mcat4Fun";
            m_alias = "CGS";
            m_DB2Command = null;
            m_Db2DataReader = null;
            //m_CgsDB = new DB2Connection();

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
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
            }

            return result;
        }
        public void close()
        {
            if (m_CgsDB != null) m_CgsDB.Close();
        }
        public List<String> getAllSerialsWaitingAOI()
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
                                        + "WHERE ITC.SHORT_NAME <> 'Product Carrier'  AND "
                                        + "      OPERATION_NAME = 'AOI Inspection' AND STATUS = 'W' order by I.INIT_TMST DESC";

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
                logger.Error(ex, "[getAllSerialsWaitingAOI] Error ");
            }

            return serials;
        }
        public List<String> getAllDJsWaitingAOI()
        {
            List<String> serials = new List<string>();

            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "SELECT DISTINCT BATCH.BATCH_ID FROM CGS.ITEM I " +
                                            "JOIN CGSPCM.PRODUCT PROD ON(PROD.PRODUCT_KEY = I.ITEM_KEY) " +
                                            "JOIN CGS.PART_NUMBER PN ON(PN.PART_NUMBER_KEY = I.PART_NUMBER_KEY) " +
                                            "JOIN CGSPCM.ROUTE_STEP RS ON(RS.ROUTE_STEP_KEY = PROD.ROUTE_STEP_KEY) " +
                                            "JOIN CGSPCM.OPERATION OPER ON(OPER.OPERATION_KEY = RS.OPERATION_KEY) " +
                                            "JOIN CGSPCM.PRODUCT_BATCH BATCH ON(BATCH.BATCH_KEY = PROD.BATCH_KEY) " +
                                            "JOIN CGS.ITEM_TYPE IT ON(IT.ITEM_TYPE_KEY = I.ITEM_TYPE_KEY) " +
                                            "JOIN CGS.ITEM_TYPE_CLASS ITC ON(ITC.ITEM_CLASS_KEY = IT.ITEM_CLASS_KEY) " +
                                            "LEFT OUTER JOIN CGS.ITEM TOOL ON(TOOL.ITEM_KEY = PROD.TOOL_KEY) " +
                                            " WHERE ITC.SHORT_NAME <> 'Product Carrier'  AND " +
                                            " OPERATION_NAME = 'AOI Inspection' AND STATUS = 'W' and I.INIT_TMST > '2020-07-01'" +
                                            " order by BATCH.BATCH_ID DESC";

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
                logger.Error(ex, "[getAllDJSWaitingAOI] Error ");
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
        public List<String> getAllSerialsActiveAOI()
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
                                        + "WHERE ITC.SHORT_NAME <> 'Product Carrier' AND "
                                        + "      OPERATION_NAME = 'AOI Inspection' AND STATUS = 'A' and order by I.INIT_TMST DESC";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        serials.Add(m_Db2DataReader.GetString(0).ToUpper()) ;
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getAllSerialsActiveAOI] Error ");
            }

            return serials;

        }
        public CModelInfo getInfoModel(String NP)
        {
            CModelInfo model = new CModelInfo();
            try
            {
                close();
                connect();
                m_DB2Command = m_CgsDB.CreateCommand();

                ////////////////////////////// SE OBTIENE LA FECHA DEL SERIAL Y SU BATCH

                m_DB2Command.CommandText = "select  CGS.PART_NUMBER.PART_NUMBER, " +
                                                    "CGS.PART_NUMBER.REVISION, " +
                                                    "CGSPCM.ROUTE.ROUTE_NAME, " +
                                                    "CGS.PART_NUMBER.NB_PANEL_ROWS, " +
                                                    "CGS.PART_NUMBER.NB_PANEL_COLUMNS, " +
                                                    "CGS.PART_NUMBER.NB_CIRCUIT_PER_PANEL, " +
                                                    "CGS.PART_NUMBER.PN_DESC, " +
                                                    "CGS.PART_NUMBER.PRINT_LABEL_FOR_PANEL, " +
                                                    "CGS.PART_NUMBER.SN_VALIDATION_REGEX " +
                                            "from    CGS.PART_NUMBER, " +
                                                    "CGSRTE.ROUTE_FOR_PN, " +
                                                    "CGSPCM.ROUTE " +
                                            "where CGS.PART_NUMBER.PART_NUMBER_KEY = CGSRTE.ROUTE_FOR_PN.PART_NUMBER_KEY " +
                                                "and CGSPCM.ROUTE.ROUTE_KEY = CGSRTE.ROUTE_FOR_PN.ROUTE_KEY " +
                                                "and CGS.PART_NUMBER.REVISION NOT LIKE '%OBSOLETO%' " +
                                                "and CGS.PART_NUMBER.PN_DESC NOT LIKE '%NO USAR%' " +
                                                "and CGS.PART_NUMBER.REVISION NOT LIKE '%NO USAR%' " +
                                                "and CGS.PART_NUMBER.PN_DESC NOT LIKE '%OBSOLETO%' " +
                                                "and CGS.PART_NUMBER.PN_DESC NOT LIKE '%Obsoleto%' " +
                                                "and CGS.PART_NUMBER.PART_NUMBER = '" + NP + "' " +
                                                "ORDER BY CGS.PART_NUMBER.PART_NUMBER";

                m_Db2DataReader = m_DB2Command.ExecuteReader();
                if (m_Db2DataReader.HasRows)
                {
                    while (m_Db2DataReader.Read())
                    {
                        model.FG_NAME = m_Db2DataReader.GetString(0);
                        model.REV = m_Db2DataReader.GetString(1);
                        model.ROUTE = m_Db2DataReader.GetString(2);
                        model.NUMBER_BOARDS = Convert.ToInt32(m_Db2DataReader.GetString(5));
                        model.DESCRIPTION = m_Db2DataReader.GetString(6);
                        model.POKAYOKE = m_Db2DataReader.GetString(8);
                    }
                }
                m_Db2DataReader.Close();
                close();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "[getInfoModel] Error ");
            }
            return model;

        }
    }

}
