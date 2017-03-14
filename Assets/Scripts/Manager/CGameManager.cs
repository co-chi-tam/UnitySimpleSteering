using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGSurvival {
	public class CGameManager : CMonoSingleton<CGameManager> {

		private Dictionary<string, CObjectController> m_Objects;

		protected override void Awake ()
		{
			base.Awake ();
			m_Objects = new Dictionary<string, CObjectController> ();
		}


		public void RegisterObject(string name, CObjectController controller) {
			m_Objects.Add (name, controller);
		}

		public void UnregisterObject(string name) {
			m_Objects.Remove (name);
		}

	}
}
