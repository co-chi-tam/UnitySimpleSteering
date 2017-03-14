using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using SEEffect;

namespace RPGSurvival {
	public class CResourceController : CObjectController, IContext {

		#region Properties

		[SerializeField]	private string m_StateName;

		public float currentStorable = 100;
		public float maxStorable = 100;

		protected FSMManager m_FSMManger;
		protected SEEffectManager m_EffectManager;

		private Parabola m_Jumper;
		public bool m_Jump;

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();

			m_EffectManager = new SEEffectManager ();
			m_EffectManager.LoadEffect ("Effect/ResourceEffect");

			m_EffectManager.RegisterCondition ("CanActiveEffect", CanActiveEffect);

			m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);

			m_FSMManger = new FSMManager ();
			m_FSMManger.LoadFSM ("FSM/SimpleResourceFSM");

			var idleResourceState = new FSMResourceIdleState (this);
			var deactiveResourceState = new FSMResourceDeactiveState (this);

			m_FSMManger.RegisterState ("ResourceIdleState", idleResourceState);
			m_FSMManger.RegisterState ("ResourceDeactiveState", deactiveResourceState);

			m_FSMManger.RegisterCondition ("IsDeactive", IsDeactive);

			m_Jumper = this.GetComponent<Parabola> ();
		}

		protected override void UpdateBaseTime (float dt)
		{
			base.UpdateBaseTime (dt);
			m_FSMManger.UpdateState (dt);
			m_StateName = m_FSMManger.currentStateName;
			if (m_Jump) {
				m_Jumper.JumpTo (Vector3.zero, null, () => {
					m_Jump = false;
				});
			}
		
		}

		#endregion

		#region Main methods

		#endregion

		#region Getter && Setter 

		public override void SetCurrentStorable(float value) {
			base.SetCurrentStorable (value);
			currentStorable = value;
		}

		public override float GetCurrentStorable() {
			base.GetCurrentStorable ();
			return currentStorable;
		}

		#endregion

		#region FSM

		internal bool IsDeactive() {
			return currentStorable <= 0f;
		}

		#endregion

		#region Effect

		internal virtual bool CanActiveEffect(Dictionary<string, object> pars) {
			return GetActive();
		}

		internal virtual void PrintDebug(Dictionary<string, object> pars) {
			#if UNITY_EDITOR
			foreach (var item in pars)
			{
				Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
			}
			#endif
		}

		#endregion

	}
}
