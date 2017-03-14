using UnityEngine;
using System.Collections;
using SEEffect;

namespace RPGSurvival {
	public class CBaseData : SEEffectInfo {

		private string m_ID;

		public string ID { 
			get { return m_ID; } 
			set { m_ID = value; } 
		}

		public CBaseData ()
		{
			m_ID = string.Empty;
		}

		public static CBaseData Clone(CBaseData instance) {
			var tmp = new CBaseData ();
			tmp.ID = instance.ID;
			return tmp;
		}

	}
}
