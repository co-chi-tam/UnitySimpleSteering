using UnityEngine;
using System.Collections;

namespace RPGSurvival {
	public class CBaseMonoBehaviour : MonoBehaviour {

		protected Transform m_Transform;

		protected virtual void Awake ()
		{
			m_Transform = this.transform;
		}

		protected virtual void Start ()
		{
		}

		protected virtual void Update ()
		{
			UpdateBaseTime (Time.deltaTime);
		}

		protected virtual void UpdateBaseTime (float dt)
		{
		}

		protected virtual void FixedUpdate ()
		{
			FixedUpdateBaseTime (Time.fixedDeltaTime);
		}

		protected virtual void FixedUpdateBaseTime (float dt)
		{
		}



	}
}
