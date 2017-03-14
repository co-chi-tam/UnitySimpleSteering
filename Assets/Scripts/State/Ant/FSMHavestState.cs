using UnityEngine;
using System.Collections;
using FSM;

namespace RPGSurvival {
	public class FSMHavestState : FSMControlState
	{

		public FSMHavestState(IContext context) : base (context)
		{
			
		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetIsObstacle (false);
			m_Controller.SetTarget (m_Controller.havestObjects.First.Value);
			if (m_Controller.GetNest () != null) {
				m_Controller.GetNest ().RemoveLineWorker (m_Controller);
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.LookAtTarget (dt);
			var resourceTorable = m_Controller.GetTarget ().GetCurrentStorable ();
			if (resourceTorable > 0) {
				var totalResourcePertime = m_Controller.GetResourcePerTime (dt);
				m_Controller.GetTarget ().SetCurrentStorable (resourceTorable - totalResourcePertime);
				m_Controller.SetCurrentStorable(m_Controller.GetCurrentStorable() + totalResourcePertime);
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}
