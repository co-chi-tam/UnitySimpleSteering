using UnityEngine;
using System;
using System.Collections;

namespace RPGSurvival {
	public class CBaseComponent {

		protected GameObject m_Target;

		public CBaseComponent ()
		{
			m_Target = null;
		}

		public CBaseComponent (GameObject target)
		{
			m_Target = target;
		}

		public virtual void UpdateComponent(float dt) {
			
		}

	}
}
