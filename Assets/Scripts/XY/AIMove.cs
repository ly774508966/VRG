using UnityEngine;
using System.Collections;

public class AIMove : MonoBehaviour {
	
	public float jumpPower = 5.0F;
	public float moveSpeed = 5.0F;
	public Vector3 currentVelocity = Vector3.zero;
	public bool isGrounded = false;
	public float gravity = -9.8F;
	public float moveDirection = 0.0F;
	public bool jumpInput = false;
	public bool hasJustCollided = false;

	
	CharacterController controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		moveDirection = Random.value * 2.0F - 1;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Check if grounded
		if((controller.collisionFlags & CollisionFlags.Below) != 0){
			isGrounded = true;	
		} else {
			isGrounded = false;
		}
		
		//Reverse direction if touching wall
		if((controller.collisionFlags & CollisionFlags.Sides) != 0 && !hasJustCollided){
			moveDirection *= -1;
			hasJustCollided = true;
			StartCoroutine("CollisionTimer");
		}
		
		
		
		
		if(isGrounded){
			currentVelocity.x = moveDirection * moveSpeed;
		
			if (jumpInput == true){
			currentVelocity.y += jumpPower;
			}
			currentVelocity.y = Mathf.Clamp(currentVelocity.y, 0.0F, Mathf.Infinity);
			
		} else {
			currentVelocity.x = moveDirection / 5.0F;
			currentVelocity.y += gravity * Time.deltaTime;
		}
		
		controller.Move(currentVelocity * Time.deltaTime);
	}
	
	IEnumerator CollisionTimer(){
	yield return new WaitForSeconds(0.1F);
	hasJustCollided = false;
	}
	
}
