using UnityEngine;
using System.Collections;

namespace RPGSurvival {
	[RequireComponent(typeof(CapsuleCollider))]
	public class CObjectController : CBaseController {

		protected CapsuleCollider m_CapsuleCollider;
		protected bool m_IsActive = false;

		protected CGameManager m_GameManager;

		protected override void Awake ()
		{
			base.Awake ();
			this.m_CapsuleCollider = this.GetComponent<CapsuleCollider> ();

			m_GameManager = CGameManager.GetInstance ();
		}

		public virtual void Init() {
			m_GameManager.RegisterObject (this.gameObject.name + this.GetID(), this);
		}

		public virtual string GetID() {
			return this.gameObject.GetInstanceID ().ToString();
		}

		public virtual float GetRadius() {
			return m_CapsuleCollider.radius;
		}

		public virtual float GetHeight() {
			return m_CapsuleCollider.height / 2f + m_Transform.position.y;
		}

		public virtual void SetPosition(Vector3 position) {
			m_Transform.position = position;
		}

		public virtual Vector3 GetPosition() {
			return m_Transform.position;
		}

		public virtual void SetActive(bool value) {
			m_IsActive = value;
			this.gameObject.SetActive (value);
		}

		public virtual bool GetActive() {
			return m_IsActive;
		}

		public virtual void SetCurrentStorable(float value) {

		}

		public virtual float GetCurrentStorable() {
			return 0f;
		}

	}
}
