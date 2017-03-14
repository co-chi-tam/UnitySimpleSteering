using System;
using System.Collections;
using System.Collections.Generic;

namespace SEEffect
{
    public class SEEffectParam
    {
        private string m_Method;
		private Dictionary<string, object> m_Params;

        public string Method {
            get { return m_Method; }
            set { m_Method = value; }
        }

		public Dictionary<string, object> Params {
            get { return m_Params; }
            set { m_Params = value; }
        }

        public SEEffectParam()
        {
            m_Method = string.Empty;
            m_Params = null;
        }
    }
}

