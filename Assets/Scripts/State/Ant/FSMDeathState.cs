using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMDeathState : FSMControlState
	{
		public FSMDeathState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetActive (false);
			m_Controller.SetHidden (true);
			m_Controller.SetIsObstacle (false);
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
