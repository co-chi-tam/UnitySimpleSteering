using System;
using System.Collections;
using System.Collections.Generic;

namespace SEEffect
{
	public class SEYesNoNode: SEBaseEffectNode
	{
        private SEEffectParam m_EffectParam;
		private List<SEBaseEffectNode> m_YesMethods;
		private List<SEBaseEffectNode> m_NoMethods;

		public List<SEBaseEffectNode> YesMethods {
			get { return m_YesMethods; }
			set { m_YesMethods = value; }
		}

		public List<SEBaseEffectNode> NoMethods {
			get { return m_NoMethods; }
			set { m_NoMethods = value; }
		}

        public SEEffectParam EffectParam
        {
            get { return m_EffectParam; }
            set { m_EffectParam = value; }
        }

		public SEYesNoNode () : base ()
		{
            EffectParam = new SEEffectParam();
			m_YesMethods = new List<SEBaseEffectNode> ();
			m_NoMethods = new List<SEBaseEffectNode> ();                                     
		}
	}
}

