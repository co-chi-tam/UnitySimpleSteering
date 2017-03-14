using UnityEngine;
using System.Collections;
using RPGSurvival;

public class CTargetController : CObjectController {

	[SerializeField]	private Camera cameraMain;
	[SerializeField]	private LayerMask layerMask;

	[SerializeField]	private Vector3 center;
	[SerializeField]	private float radius = 45f;
	[SerializeField]	private float speed = 20f;

	void Start () {
	
	}
	
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit raycastHit;
			if (Physics.Raycast (cameraMain.ScreenPointToRay (Input.mousePosition), out raycastHit, 1000f, layerMask)) {
				this.transform.position = raycastHit.point;
			}
		}
	}

}
