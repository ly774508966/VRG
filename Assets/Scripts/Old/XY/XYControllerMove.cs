using UnityEngine;
using System.Collections;

public class XYControllerMove : MonoBehaviour {
	
	public float jumpPower = 5.0F;
	public float moveSpeed = 5.0F;
	public Vector3 currentVelocity = Vector3.zero;
	public bool isGrounded = false;
	public float gravity = -9.8F;
	public float moveInput = 0.0F;
	public bool jumpInput = false;

	
	CharacterController controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if((controller.collisionFlags & CollisionFlags.Below) != 0){
			isGrounded = true;	
		} else {
			isGrounded = false;
		}
		if(isGrounded){
		currentVelocity.x = moveInput * moveSpeed;
		if (jumpInput == true){
			currentVelocity.y += jumpPower;
		}
		currentVelocity.y = Mathf.Clamp(currentVelocity.y, 0.0F, Mathf.Infinity);
			
		} else {
			currentVelocity.x = moveInput * moveSpeed / 2.0F;
		}
		currentVelocity.y += gravity * Time.deltaTime;
		controller.Move(currentVelocity * Time.deltaTime);
	}
	
	
}
