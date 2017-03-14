using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace RPGSurvival {
	[RequireComponent(typeof(NavMeshAgent))]
	public class CAIController : CObjectController, IContext, IMovable {

		#region Properties

		[SerializeField]	private string m_StateName;
		[SerializeField]	private CNestController m_Nest;
		[SerializeField]	private CObjectController m_Target;
		[SerializeField]	private LayerMask m_ObstacleLayerMasks;
		[SerializeField]	private LayerMask m_HavestLayerMasks;
		[SerializeField]	private float m_MoveSpeed = 5f;

		private LayerMask m_CurrentLayerMask;
		private NavMeshAgent m_NavMeshAgent;
		private FSMManager m_FSMManager;
		private CBugData m_Data;
		private CMovableComponent m_MovableComponent;
		private bool m_IsObstacle = true;

		public LinkedList<CObjectController> havestObjects;

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();

			m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
			havestObjects = new LinkedList<CObjectController> ();

			m_FSMManager = new FSMManager ();
			m_FSMManager.LoadFSM ("FSM/SimpleWorkerFSM");

			var idleState 			= new FSMIdleState (this);
			var moveState 			= new FSMMoveState (this);
			var waitingState 		= new FSMWaitingState (this);
			var waitingCommandState = new FSMWaitCommandState (this);
			var havestState 		= new FSMHavestState (this);
			var goNestState 		= new FSMGoNestState (this);
			var productFoodState 	= new FSMProductFoodState (this);
			var deathState 			= new FSMDeathState (this);

			m_FSMManager.RegisterState ("IdleState", idleState);
			m_FSMManager.RegisterState ("MoveState", moveState);
			m_FSMManager.RegisterState ("WaitingState", waitingState);
			m_FSMManager.RegisterState ("WaitingCommandState", waitingCommandState);
			m_FSMManager.RegisterState ("HavestState", havestState);
			m_FSMManager.RegisterState ("GoNestState", goNestState);
			m_FSMManager.RegisterState ("ProductFoodState", productFoodState);
			m_FSMManager.RegisterState ("DeathState", deathState);

			m_FSMManager.RegisterCondition ("HaveTarget", HaveTarget);
			m_FSMManager.RegisterCondition ("HaveHavest", HaveHavest);
			m_FSMManager.RegisterCondition ("DidMoveToSignPoint", DidMoveToSignPoint);
			m_FSMManager.RegisterCondition ("DidMoveToTarget", DidMoveToTarget);
			m_FSMManager.RegisterCondition ("DidMoveToNest", DidMoveToNest);
			m_FSMManager.RegisterCondition ("DidFullStorable", DidFullStorable);
			m_FSMManager.RegisterCondition ("DidProductFoodComplete", DidProductFoodComplete);
			m_FSMManager.RegisterCondition ("IsDeath", IsDeath);

			m_MovableComponent = new CMovableComponent (this, m_NavMeshAgent);
			m_MovableComponent.currentTransform = m_Transform;
			m_MovableComponent.obstacles = m_ObstacleLayerMasks;
			m_MovableComponent.radiusBase = 0f;

			m_CurrentLayerMask = this.gameObject.layer;
		}

		protected override void Start ()
		{
			base.Start ();
		}

		protected override void UpdateBaseTime (float dt)
		{
			base.UpdateBaseTime (dt);
			m_StateName = m_FSMManager.currentStateName;
			m_FSMManager.UpdateState (dt);
		}

		#endregion

		#region Main methods

		public virtual void MoveToTarget(float dt) {
			m_MovableComponent.MoveForwardToTarget (dt);
		}

		public virtual void MoveToTarget(float dt, Vector3 target) {
			m_MovableComponent.targetPosition = target;
			m_MovableComponent.MoveForwardToTarget (dt);
		}

		public virtual void LookAtTarget(float dt) {
			m_MovableComponent.LookForwardToTarget (m_Target.transform.position);
		}

		#endregion

		#region Getter && Setter

		public virtual float GetMoveSpeed() {
			return m_MoveSpeed;
		}

		public virtual void SetTarget(CObjectController target) {
			if (target == this.gameObject)
				return;
			if (target == null)
				return;
			m_Target = target;
			m_MovableComponent.targetPosition = target.transform.position;
		}

		public virtual void SetTarget(Vector3 target) {
			m_MovableComponent.targetPosition = target;
		}

		public virtual CObjectController GetTarget() {
			return m_Target;
		}

		public virtual Vector3 GetTargetPosition() {
			if (m_Target == null)
				return this.GetPosition();
			return m_Target.GetPosition ();
		}

		public virtual void SetNest(CNestController value) {
			m_Nest = value as CNestController;
			m_MovableComponent.targetPosition = value.GetSignPoint ();
		}

		public virtual CNestController GetNest() {
			return m_Nest;
		}

		public virtual void SetHidden(bool value) {
			if (value) {
				this.gameObject.layer = LayerMask.NameToLayer ("Hidden");
			} else {
				this.gameObject.layer = m_CurrentLayerMask;
			}
		}

		public virtual bool GetHidden() {
			return this.gameObject.layer == LayerMask.NameToLayer ("Hidden");
		}

		public virtual void SetIsObstacle(bool value) {
			m_IsObstacle = value;
		}

		public override string GetID() {
			base.GetID ();
			return this.gameObject.GetInstanceID ().ToString();
		}

		public virtual bool GetIsObstacle() {
			return m_IsObstacle;
		}

		public virtual float GetResourcePerTime(float dt) {
			return 50f / 60f * dt;
		}

		#endregion

		#region FSM

		internal virtual bool HaveTarget() {
			if (m_Target != null) {
				m_MovableComponent.targetPosition = m_Target.transform.position;
			}
			return m_Target != null;
		}

		internal virtual bool HaveHavest() {
			var colliderHavests = Physics.OverlapSphere (m_Transform.position, 5f, m_HavestLayerMasks);
			if (colliderHavests.Length > 0) {
				havestObjects.Clear ();
				for (int i = 0; i < colliderHavests.Length; i++) {
					var colliderObj = colliderHavests [i].GetComponent<CObjectController> ();
					if (colliderObj != null && colliderObj.GetActive()) {
						havestObjects.AddLast (colliderObj);
					}
				}
				if (havestObjects.Count > 0) {
					return true;
				} 
				return false;
			}
			return false;
		}

		internal virtual bool DidMoveToSignPoint() {
			m_MovableComponent.distance = GetRadius();
			m_MovableComponent.targetPosition = m_Nest.GetSignPoint ();
			return m_MovableComponent.DidMoveToTarget(m_Nest.GetSignPoint());
		}

		internal virtual bool DidMoveToTarget() {
			m_MovableComponent.distance = m_Target.GetRadius() + GetRadius();
			return m_MovableComponent.DidMoveToTarget();
		}

		internal virtual bool DidMoveToNest() {
			m_MovableComponent.distance = m_Nest.GetRadius() + GetRadius();
			m_MovableComponent.targetPosition = m_Nest.GetPosition();
			return m_MovableComponent.DidMoveToTarget(m_Nest.GetPosition());
		}

		internal virtual bool DidFullStorable() {
			return false;
		}

		internal virtual bool DidProductFoodComplete() {
			return false;
		}

		internal virtual bool IsDeath() {
			return false;
		}

		#endregion

	}
}
