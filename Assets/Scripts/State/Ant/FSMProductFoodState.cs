using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMProductFoodState : FSMControlState
	{
		public FSMProductFoodState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetIsObstacle (false);
			if (m_Controller.GetNest () != null) {
				m_Controller.GetNest ().RemoveLineReturnNest (m_Controller);
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
//			m_Controller.LookAtTarget (dt);
			var totalResourcePertime = m_Controller.GetResourcePerTime (dt);
			m_Controller.SetCurrentStorable (m_Controller.GetCurrentStorable () - totalResourcePertime);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}
