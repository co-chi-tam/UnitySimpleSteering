using UnityEngine;
using System.Collections;
using FSM;
using SEEffect;

namespace RPGSurvival {
	public class CWorkerController : CAIController {

		#region Properties

		public float currentStorable = 0;
		public float maxStorable = 10;

		#endregion

		#region Mono Implementation

		protected override void Start ()
		{
			base.Start ();
		}

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

		internal override bool DidFullStorable() {
			base.DidFullStorable ();
			return currentStorable >= maxStorable;
		}

		internal override bool DidProductFoodComplete() {
			base.DidProductFoodComplete ();
			return currentStorable <= 0f;
		}

		#endregion

	}
}
