using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using SEEffect;
using ObjectPool;

namespace RPGSurvival {
	public class CNestController : CObjectController, IContext {

		#region Properties

		[SerializeField]	private string m_StateName;
		[SerializeField]	private string m_ModelPath = "Prefabs/Caterpilla";
		[SerializeField]	private GameObject m_AntRoot;
		[SerializeField]	private int m_MaxMember = 20;
		[SerializeField]	private List<CAIController> m_Members;

		public GameObject signPointObject;

		protected LinkedList<CAIController> m_Frees;
		protected LinkedList<CAIController> m_LineWorkers;
		protected LinkedList<CAIController> m_LineReturnNest;

		protected FSMManager m_FSMManger;
		protected ObjectPool<CAIController> m_ObjectPool;

		[SerializeField]	private SEObjectProperty<float> m_Timer;

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();

			m_ObjectPool = new ObjectPool<CAIController> ();

			m_FSMManger = new FSMManager ();
			m_FSMManger.LoadFSM ("FSM/SimpleNestFSM");

			var idleNestState = new FSMNestIdleState (this);
			var deactiveNestState = new FSMNestDeactiveState (this);

			m_FSMManger.RegisterState ("NestIdleState", idleNestState);
			m_FSMManger.RegisterState ("NestDeactiveState", deactiveNestState);

			m_FSMManger.RegisterCondition ("IsDeactive", IsDeactive);

			m_Members 			= new List<CAIController> ();

			m_Frees 			= new LinkedList<CAIController> ();
			m_LineWorkers 		= new LinkedList<CAIController> ();
			m_LineReturnNest 	= new LinkedList<CAIController> ();

			OnNestInit();
		}

		protected override void UpdateBaseTime (float dt)
		{
			base.UpdateBaseTime (dt);
			m_FSMManger.UpdateState (dt);
			m_StateName = m_FSMManger.currentStateName;
		}

		#endregion

		#region Main methods

		public virtual void OnNestInit() {
			StartCoroutine (HandleSpawnMember ());
		}

		private IEnumerator HandleSpawnMember() {
			var ant = Resources.Load<CAIController> (m_ModelPath);
			for (int i = 0; i < m_MaxMember; i++) {
				var memberController = OnNestSpawnMember (ant);
				memberController.name = "Ant " + (i + 1);
				m_Members.Add (memberController);
				memberController.Init ();
				m_ObjectPool.Create (memberController);
				yield return WaitHelper.WaitForShortSeconds;
			}
		}

		public virtual CAIController OnNestSpawnMember(CAIController instance) {
			var memberController = Instantiate (instance);
			memberController.transform.SetParent (m_AntRoot.transform);
			memberController.SetPosition (m_Transform.position);
			memberController.SetNest (this);
			return memberController;
		}

		public void AddLineWorker(CAIController value) {
			if (m_LineReturnNest.Contains (value) == true)
				return;
			if (m_LineWorkers.Contains (value) == true)
				return;
			m_Frees.Remove (value);
			m_LineReturnNest.Remove (value);
			m_LineWorkers.AddLast (value);
		}

		public void RemoveLineWorker(CAIController value) {
			if (m_LineReturnNest.Contains (value) == true)
				return;
			if (m_LineWorkers.Contains (value) == false)
				return;
			m_Frees.AddLast (value);
			m_LineReturnNest.Remove (value);
			m_LineWorkers.Remove (value);
		}

		public void AddLineReturnNest(CAIController value) {
			if (m_LineWorkers.Contains (value) == true)
				return;
			if (m_LineReturnNest.Contains (value) == true) {
				return;
			}
			m_Frees.Remove (value);
			m_LineWorkers.Remove (value);
			m_LineReturnNest.AddLast (value);
		}

		public void RemoveLineReturnNest(CAIController value) {
			if (m_LineWorkers.Contains (value) == true)
				return;
			if (m_LineReturnNest.Contains (value) == false)
				return;
			m_Frees.AddLast (value);
			m_LineWorkers.Remove (value);
			m_LineReturnNest.Remove (value);
		}

		public void FreeLine(CAIController value) {
			m_Frees.AddLast (value);
			m_LineWorkers.Remove (value);
			m_LineReturnNest.Remove (value);
		}

		#endregion

		#region Getter && Setter 

		public Vector3 GetSignPoint() {
			return signPointObject.transform.position;
		}

		#endregion

		#region FSM

		internal bool IsDeactive() {
			return false;
		}

		internal bool HaveFreeMember() {
			return m_Frees.Count > 0;	
		}

		#endregion

	}
}
