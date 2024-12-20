using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktake.Class
{
    class COracle
    {
        String m_server;
        String m_SID;
        private String m_user;
        private String m_pass;
        OracleConnection m_OracleDB;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string Server { get => m_server; set => m_server = value; }
        public string SID { get => m_SID; set => m_SID = value; }

        public COracle (String serv, String Sid)
        {
            m_server = serv;
            m_SID = Sid;
            m_user = "APPS";
            m_pass = "apps";
            m_OracleDB = GetDBConnection();
            m_OracleDB.Open();
        }

        private OracleConnection GetDBConnection()
        {


            Console.WriteLine("Getting Connection ...");

            // 'Connection string' to connect directly to Oracle.
            string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + Server + ")(PORT = " + "1521" + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + SID + ")));Password=" + m_pass + ";User ID=" + m_user + ";Enlist=false;Pooling=true";

            OracleConnection conn = new OracleConnection();
            try
            {
                conn.ConnectionString = connString;
            }
            catch(Exception ex)
            {
                conn = null;
                logger.Error(ex, "Error al conectarse a base de datos de Oracle");
            }

            return conn;
        }
        public  bool QuerySerial(String serial, ref int resultTest)
        {
            bool result = false;
            string sql = "SELECT * FROM insp_result_summary_info where board_barcode in ('" + serial.ToUpper() + "')"; ;

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;

                        while (reader.Read())
                        {
                            int IRCODEIndex = reader.GetOrdinal("INSP_RESULT_CODE");
                            int VLRCODEIndex = reader.GetOrdinal("VC_LAST_RESULT_CODE");

                            long? INSP_RESULT_CODE = null;
                            long? VC_LAST_RESULT_CODE = null;

                            if (!reader.IsDBNull(IRCODEIndex))
                                INSP_RESULT_CODE = Convert.ToInt64(reader.GetValue(IRCODEIndex));
                            if (!reader.IsDBNull(VLRCODEIndex))
                                VC_LAST_RESULT_CODE = Convert.ToInt64(reader.GetValue(VLRCODEIndex));

                            if (INSP_RESULT_CODE == 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 1;   //// OK
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE != 0)
                                resultTest = 2;   //// NG
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == 0)
                                resultTest = 3;   //// FALSE CALL (OK)
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 4;   //// NO JUZGADA

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error en serial: " + serial);
                result = false;
            }
            return result;

        }
        public bool getSemifinish(String djGroup, String operation,ref String semifinish)
        {
            bool result = false;
            string sql = "SELECT assembly_name FROM SIIXSEM.DJ_GROUP_DTL WHERE GROUP_NO = '" + djGroup + "' AND COMPLETION_SUBINV = '"+ operation + "'";
            string sqlWIP = "SELECT assembly_name FROM SIIXSEM.DJ_GROUP_DTL WHERE GROUP_NO = '" + djGroup + "' AND assembly_name LIKE '%" + operation + "%'";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();
                OracleCommand cmdWIP = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;

                        while (reader.Read())
                        {
                            semifinish = reader.GetString(0);
                        }
                    }
                    else
                    {
                        cmdWIP.Connection = m_OracleDB;
                        cmdWIP.CommandText = sqlWIP;
                        using (DbDataReader readerWIP = cmdWIP.ExecuteReader())
                        {
                            if (readerWIP.HasRows)
                            {
                                result = true;

                                while (readerWIP.Read())
                                {
                                    semifinish = readerWIP.GetString(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error en serial: " + djGroup);
                result = false;
            }
            return result;

        }
        public bool QuerySerials(List<String> serials, ref int resultTest)
        {
            bool result = false;
            String qSerials = "";

            foreach(String serial in serials)
            {
                qSerials += "'" + serial.ToUpper() + "',";
            }

            string sql = "SELECT * FROM insp_result_summary_info where board_barcode in (" + qSerials.Substring(0,qSerials.Length-1) + ")"; ;

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;

                        while (reader.Read())
                        {
                            int IRCODEIndex = reader.GetOrdinal("INSP_RESULT_CODE");
                            int VLRCODEIndex = reader.GetOrdinal("VC_LAST_RESULT_CODE");

                            long? INSP_RESULT_CODE = null;
                            long? VC_LAST_RESULT_CODE = null;

                            if (!reader.IsDBNull(IRCODEIndex))
                                INSP_RESULT_CODE = Convert.ToInt64(reader.GetValue(IRCODEIndex));
                            if (!reader.IsDBNull(VLRCODEIndex))
                                VC_LAST_RESULT_CODE = Convert.ToInt64(reader.GetValue(VLRCODEIndex));

                            if (INSP_RESULT_CODE == 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 1;   //// OK
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE != 0)
                                resultTest = 2;   //// NG
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == 0)
                                resultTest = 3;   //// FALSE CALL (OK)
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 4;   //// NO JUZGADA
                            //break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
                result = false;
            }
            return result;

        }

        public DataTable getSubAssy(String djGroup)
        {
            DataTable m_subAssy = new DataTable();
            float COST_ACUM = 0.0f;
            //siixsem_scrap_dbEntities m_dbScrap = new siixsem_scrap_dbEntities();

            m_subAssy.Columns.Add("ASSEMBLY_DESC");
            m_subAssy.Columns.Add("WIP_ENTITY_NAME");
            m_subAssy.Columns.Add("ASSEMBLY_NAME");
            m_subAssy.Columns.Add("LINKAGE_SEQ");
            m_subAssy.Columns.Add("COST");
            m_subAssy.Columns.Add("COST_ACUM");

            string sql = "SELECT   GRP.ASSEMBLY_DESC, WE.WIP_ENTITY_NAME, GRP.ASSEMBLY_NAME, GRP.LINKAGE_SEQ " +
				        "FROM SIIXSEM.DJ_GROUP_DTL GRP " +
				        "INNER JOIN WIP.WIP_ENTITIES WE ON GRP.DJ_NO = WE.WIP_ENTITY_NAME " +
				        "INNER JOIN WIP.WIP_DISCRETE_JOBS DJ  ON DJ.WIP_ENTITY_ID = WE.WIP_ENTITY_ID " +
				        "INNER JOIN APPS.FND_LOOKUP_VALUES LV ON DJ.STATUS_TYPE = LV.LOOKUP_CODE " +
				        "INNER JOIN SIIXSEM.DJ_PICK_LIST PKL  ON GRP.DJ_NO = PKL.DJ_NO " +
				        "WHERE  GRP.STATUS = 'S' " +
                        "AND GRP.GROUP_NO = '"+ djGroup +"' " +
				        "AND LV.LOOKUP_TYPE = 'WIP_JOB_STATUS' " +
				        "AND LV.ENABLED_FLAG = 'Y' " +
				        "AND LV.LANGUAGE= 'US' " +
                        "ORDER BY GRP.LINKAGE_SEQ DESC"; 

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;
                //m_dbScrap.reset_temp_or();

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                float COST = getCostByAssemName(reader.GetString(2));
                                COST_ACUM += COST;
                                m_subAssy.Rows.Add(reader.GetString(0).ToUpper(), reader.GetString(1).ToUpper(), reader.GetString(2).ToUpper(), reader.GetInt32(3).ToString().ToUpper(), COST, COST_ACUM);
                                //m_dbScrap.insertItemTemp(reader.GetInt32(3), reader.GetString(2).ToUpper(), reader.GetString(0).ToUpper(), reader.GetString(1).ToUpper(),COST,COST_ACUM);
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
            }
            return m_subAssy;
        }
        public float getCostByDJ(String djNo)
        {
            float COST = 0.0F;

             
            string sql = "SELECT SUM(total) SA_COST " +
                        "FROM (SELECT DISTINCT mpl.item_cd, " +
                        "                         (mpl.quantity_per_assembly * dl.unit_price) total " +
                        "                       FROM siixsem.dj_master_pick_list mpl " +
                        "                       INNER JOIN siixsem.item_price_info_hdr hd ON mpl.item_id = hd.item_id " + 
                        "                       INNER JOIN  siixsem.item_price_info_dtl dl ON hd.item_price_info_hdr_id = dl.item_price_info_hdr_id " +
                        "                       WHERE mpl.dj_no = " + djNo + " " +
                        "                        AND mpl.picked_flag = 'Y' " +
                        "                        AND dl.active_flag = 'Y') sb " +
                        "WHERE 1=1";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                COST = reader.GetFloat(0);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
            }
            return COST;
        }

        public float getCostByAssemName(String Assembly)
        {
            float COST = 0.0F;

            string sql = "SELECT SUM((bic.component_quantity * dl.unit_price)) total " +
                        "FROM inv.mtl_system_items_b msi " +
                        "INNER JOIN apps.bom_bill_of_materials bom  ON bom.organization_id = msi.organization_id " +
                        "INNER JOIN apps.bom_inventory_components bic ON  bom.bill_sequence_id = bic.bill_sequence_id " +
                        "INNER JOIN inv.mtl_system_items_b msil ON  bom.organization_id = msil.organization_id " +
                        "INNER JOIN siixsem.item_price_info_hdr hd ON msil.inventory_item_id = hd.item_id " +
                        "INNER JOIN  siixsem.item_price_info_dtl dl ON hd.item_price_info_hdr_id = dl.item_price_info_hdr_id " +
                        "WHERE  bom.assembly_item_id = msi.inventory_item_id " +
                        "AND bic.component_item_id = msil.inventory_item_id " +
                        "AND NVL(bic.disable_date, SYSDATE) >= SYSDATE " +
                        "AND msi.segment1 =  '" + Assembly + "' " +
                        "AND dl.active_flag = 'Y'";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                COST = reader.GetFloat(0);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
            }
            return COST;
        }
        public DataTable getComponentsByAssemName(String Assembly)
        {
            DataTable m_subAssy = new DataTable();
            float COST_ACUM = 0.0f;

            m_subAssy.Columns.Add("cod_component");
            m_subAssy.Columns.Add("COST");
            m_subAssy.Columns.Add("COST_ACUM");
            m_subAssy.Columns.Add("DESCRIPTION");
            m_subAssy.Columns.Add("component_quantity");
            m_subAssy.Columns.Add("unit_price");

            string sql = "SELECT  msil.segment1 cod_component, " +
                        "ROUND((bic.component_quantity * dl.unit_price),4) total,msil.description, bic.component_quantity, dl.unit_price " +
                        "FROM inv.mtl_system_items_b msi " +
                        "INNER JOIN apps.bom_bill_of_materials bom  ON bom.organization_id = msi.organization_id " +
                        "INNER JOIN apps.bom_inventory_components bic ON  bom.bill_sequence_id = bic.bill_sequence_id " +
                        "INNER JOIN inv.mtl_system_items_b msil ON  bom.organization_id = msil.organization_id " +
                        "INNER JOIN siixsem.item_price_info_hdr hd ON msil.inventory_item_id = hd.item_id " +
                        "INNER JOIN  siixsem.item_price_info_dtl dl ON hd.item_price_info_hdr_id = dl.item_price_info_hdr_id " +
                        "WHERE  bom.assembly_item_id = msi.inventory_item_id " +
                        "AND bic.component_item_id = msil.inventory_item_id " +
                        "AND NVL(bic.disable_date, SYSDATE) >= SYSDATE " +
                        "AND msi.segment1 =  '" + Assembly + "' " +
                        "AND dl.active_flag = 'Y' " +
                        "ORDER BY total desc";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                float COST = reader.GetFloat(1);
                                COST_ACUM += COST;
                                m_subAssy.Rows.Add(reader.GetString(0).ToUpper(), COST, Math.Round(COST_ACUM,4), reader.GetString(2).ToUpper(),reader.GetInt32(3), Math.Round(reader.GetFloat(4),4));

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
            }
            return m_subAssy;
        }
        public void Close()
        {
            m_OracleDB.Dispose();
            m_OracleDB.Close();
            OracleConnection.ClearPool(m_OracleDB);
        }
    }
}
