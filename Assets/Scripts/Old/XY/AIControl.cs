using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour {
	
	public XYControllerMove xYControllerMove;
	public XYShooter xYShooter;
	
	// Use this for initialization
	void Start () {
		xYControllerMove = GetComponent<XYControllerMove>();
		xYShooter = GetComponent<XYShooter>();
		}
	
	// Update is called once per frame
	void Update () {
		if(xYControllerMove.isGrounded == true){
		xYControllerMove.moveInput = 1.0F;
		}
		//xYControllerMove.jumpInput = Input.GetButtonDown ("P01B4");
		//xYShooter.shotInput = new Vector3(Input.GetAxis ("P01RightX"),Input.GetAxis ("P01RightY"), 0.0F);
		//xYShooter.fireInput = Input.GetAxis ("P01RT");
	
	}
}
