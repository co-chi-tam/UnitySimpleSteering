using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SEEffect;
using TinyJSON;

namespace RPGSurvival {
	public class CBugData : CBaseData {

		private SEObjectProperty<float> m_MoveSpeed;

		public float MoveSpeed {
			get { return m_MoveSpeed.Value; }
			set { m_MoveSpeed.Value = value; }
		}

		public int currentHealth;
		public int maxHealth;
		public float signRadius;
		public int popular;

		public CBugData ()
		{
			this.m_MoveSpeed = new SEObjectProperty<float>("moveSpeed");
			this.currentHealth = 0;
			this.maxHealth = 0;
			this.popular = 0;

		
		}

		public static CBugData Parse(Dictionary<string, object> instance) {
			var tmp = new CBugData ();


			return tmp;
		}

		public static CBugData Clone(CBugData instance) {
			var tmp = new CBugData ();
			tmp.ID = instance.ID;
			tmp.MoveSpeed = instance.MoveSpeed;




			return tmp;
		}

	}
}
