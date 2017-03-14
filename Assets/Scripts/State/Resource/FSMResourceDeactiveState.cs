using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMResourceDeactiveState : FSMBaseState
	{
		protected CResourceController m_Controller;

		public FSMResourceDeactiveState(IContext context) : base (context)
		{
			m_Controller = context as CResourceController;
		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetActive (false);
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
