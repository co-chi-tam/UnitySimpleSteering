using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMControlState : FSMBaseState
	{
		protected CAIController m_Controller;

		public FSMControlState(IContext context) : base (context)
		{
			m_Controller = context as CAIController;
		}

		public override void StartState()
		{
			base.StartState ();
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
