using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGSurvival {
	public class CMovableComponent : CBaseComponent {

		public Transform currentTransform;
		public Vector3 targetPosition;
		public LayerMask obstacles;

		public float distance;
		public float radiusBase = 0f;

		private IMovable m_Target;
		private float m_Angle;
		private float m_SpeedThreshold;
		private Vector3 m_Direction;
		private NavMeshAgent m_NavMeshAgent;
		private float[] m_AngleCheckings = new float[] { 0, -15, 15, -45, 45, -90, 90 }; 
		private float[] m_AngleAvoidances = new float[] { 10, 40, -40, 40, -40, 80, -80 }; 
		private float[] m_lengthAvoidances = new float[] { 3f, 3f, 3f, 3f, 3f, 1.5f, 1.5f }; 

		public static Dictionary<string, IMovable> MovableObjects = new Dictionary<string, IMovable> ();

		public CMovableComponent (IMovable movable, NavMeshAgent navMeshAgent) : base()
		{
			m_Angle = 0f;
			m_SpeedThreshold = 1f;
			m_NavMeshAgent = navMeshAgent;
			m_Target = movable;

			if (CMovableComponent.MovableObjects.ContainsKey (m_Target.GetID ()) == false) {
				CMovableComponent.MovableObjects.Add (m_Target.GetID (), m_Target);
			}
		}

		public override void UpdateComponent(float dt) {
			base.UpdateComponent (dt);
		} 

		public void MoveForwardToTarget(float dt) {
			if (DidMoveToTarget() == false) {
				m_Direction = targetPosition - currentTransform.position;
				var forward = currentTransform.forward;
				m_Angle = Mathf.Atan2 (m_Direction.x, m_Direction.z) * Mathf.Rad2Deg;
				DrawRayCast ();
				var position = forward * m_Target.GetMoveSpeed() * dt * m_SpeedThreshold;
				if (position != Vector3.zero) {
					if (m_NavMeshAgent.isOnNavMesh) {
						m_NavMeshAgent.Move (position);
					} else {
						currentTransform.position = Vector3.Lerp (currentTransform.position, currentTransform.position + position, 0.5f);
					}
				}
				currentTransform.rotation = Quaternion.Lerp (currentTransform.rotation, Quaternion.AngleAxis (m_Angle, Vector3.up), 0.1f);
				#if UNITY_EDITOR
				Debug.DrawRay (currentTransform.position, m_Direction, Color.green);	
				#endif
			}
			Reset ();
		}

		public void LookForwardToTarget(Vector3 value) {
			if (currentTransform != null) {
				m_Direction = value - currentTransform.position;
				var forward = currentTransform.forward;
				m_Angle = Mathf.Atan2 (m_Direction.x, m_Direction.z) * Mathf.Rad2Deg;
				currentTransform.rotation = Quaternion.Lerp (currentTransform.rotation, Quaternion.AngleAxis (m_Angle, Vector3.up), 0.1f);
			}
		}

		private void DrawRayCast() {
			var forward = currentTransform.forward;
			var tmpAngle = m_Angle;
			var tmpSpeedThreshold = m_SpeedThreshold;
			for (int i = 0; i < m_AngleCheckings.Length; i++) {
				var rayCast = Quaternion.AngleAxis(m_AngleCheckings[i], currentTransform.up) * forward * m_lengthAvoidances[i];
				RaycastHit rayCastHit;
				if (Physics.Raycast (currentTransform.position + (rayCast.normalized * radiusBase), rayCast, out rayCastHit, m_lengthAvoidances[i], obstacles)) {
					var movableHitName = rayCastHit.collider.gameObject.GetInstanceID ().ToString();
					var avoidance = true;
					if (CMovableComponent.MovableObjects.ContainsKey (movableHitName) == true) {
						avoidance = CMovableComponent.MovableObjects [movableHitName].GetIsObstacle();
					} 
					if (avoidance == true) {
						tmpAngle += m_AngleAvoidances [i] * (1f - (rayCastHit.distance / m_lengthAvoidances[i]));
						tmpSpeedThreshold -= 1f / ((float)m_AngleCheckings.Length / 1.15f);
					}
				} 
				#if UNITY_EDITOR
				Debug.DrawRay (currentTransform.position + (rayCast.normalized * radiusBase), rayCast, Color.white);
				#endif
			}
			m_Angle = tmpAngle;
			m_SpeedThreshold = tmpSpeedThreshold;
		}

		private void Reset() {
			m_SpeedThreshold = 1f;
		}

		public bool DidMoveToTarget() {
			if (currentTransform == null)
				return true;
			m_Direction = targetPosition - currentTransform.position;
			return m_Direction.sqrMagnitude <= distance * distance;
		}

		public bool DidMoveToTarget(Transform target) {
			if (target == null)
				return true;
			if (currentTransform == null)
				return true;
			var direction = target.position - currentTransform.position;
			return direction.sqrMagnitude <= distance * distance;
		}

		public bool DidMoveToTarget(Vector3 target) {
			if (currentTransform == null)
				return true;
			var direction = target - currentTransform.position;
			return direction.sqrMagnitude <= distance * distance;
		}

	}
}
