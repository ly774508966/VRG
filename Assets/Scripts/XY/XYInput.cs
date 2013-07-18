using UnityEngine;
using System.Collections;

public class XYInput : MonoBehaviour {
	
	public XYControllerMove xYControllerMove;
	public XYShooter xYShooter;
	public float moveInput;
	
	
	// Use this for initialization
	void Start () {
		xYControllerMove = GetComponent<XYControllerMove>();
		xYShooter = GetComponent<XYShooter>();
		}
	
	// Update is called once per frame
	void Update () {
		xYControllerMove.moveInput = Input.GetAxis("P01LeftX");
		xYControllerMove.jumpInput = Input.GetButtonDown ("P01B4");
		xYShooter.shotInput = new Vector3(Input.GetAxis ("P01RightX"),Input.GetAxis ("P01RightY"), 0.0F);
		xYShooter.fireInput = Input.GetAxis ("P01RT");
	
	}
}
