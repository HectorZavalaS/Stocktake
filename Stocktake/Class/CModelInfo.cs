using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktake.Class
{
    public class CModelInfo
    {
        String m_BatchId;
        String m_DjNumber;
        String m_Operation;
        String m_Status;
        String m_Route;
        String m_partNumber;
        private String PART_NUMBER;
        String REVISION;
        String ROUTE_NAME;
        int NB_CIRCUIT_PER_PANEL;
        String PN_DESC;
        String SN_VALIDATION_REGEX;

        public string BatchId { get => m_BatchId; set => m_BatchId = value; }
        public string DjNumber { get => m_DjNumber; set => m_DjNumber = value; }
        public string Operation { get => m_Operation; set => m_Operation = value; }
        public string Status { get => m_Status; set => m_Status = value; }
        public string Route { get => m_Route; set => m_Route = value; }
        public string PartNumber { get => m_partNumber; set => m_partNumber = value; }
        public string FG_NAME { get => PART_NUMBER; set => PART_NUMBER = value; }
        public string REV { get => REVISION; set => REVISION = value; }
        public string ROUTE { get => ROUTE_NAME; set => ROUTE_NAME = value; }
        public int NUMBER_BOARDS { get => NB_CIRCUIT_PER_PANEL; set => NB_CIRCUIT_PER_PANEL = value; }
        public string DESCRIPTION { get => PN_DESC; set => PN_DESC = value; }
        public string POKAYOKE { get => SN_VALIDATION_REGEX; set => SN_VALIDATION_REGEX = value; }
    }
}
