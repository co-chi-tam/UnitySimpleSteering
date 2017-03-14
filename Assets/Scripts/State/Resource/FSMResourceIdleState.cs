using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMResourceIdleState : FSMBaseState
	{
		protected CResourceController m_Controller;

		public FSMResourceIdleState(IContext context) : base (context)
		{
			m_Controller = context as CResourceController;
		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetActive (true);
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}
