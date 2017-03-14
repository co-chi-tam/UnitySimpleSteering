using System;
using System.Collections;
using System.Collections.Generic;

namespace SEEffect
{
	public class SEExcuteEffectNode : SEBaseEffectNode
	{
        private List<SEEffectParam> m_ExcuteMethods;

        public List<SEEffectParam> ExcuteMethods {
			get { return m_ExcuteMethods; }
			set { m_ExcuteMethods = value; }
		}

		public SEExcuteEffectNode () : base ()
		{
            m_ExcuteMethods = new List<SEEffectParam> ();
		}
	}
}

