using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Stocktake.Class
{
    public class base64
    {
        private string m_originalStr;

        public string OriginalStr
        {
            get { return m_originalStr; }
            set { m_originalStr = value; }
        }
        private string m_base64Str;

        public string Base64Str
        {
            get { return m_base64Str; }
            set { m_base64Str = value; }
        }
        public base64(string orStr)
        {

            this.OriginalStr = orStr;
            byte[] bytes = Encoding.UTF8.GetBytes(OriginalStr);
            this.Base64Str = Convert.ToBase64String(bytes);

        }
        public string decode(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
    }
}