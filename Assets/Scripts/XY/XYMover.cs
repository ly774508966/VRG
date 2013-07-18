using UnityEngine;
using System.Collections;

public class XYMover : MonoBehaviour {
	
	
	public float moveInput = 0.0F;
	public float moveSpeed = 1.0F;
	public bool jumpInput = false;
	public float jumpPower = 0.10F;
	public float gravityAcceleration = -0.01F;
	public Vector3 currentVelocity = Vector3.zero;
	public bool isGrounded;
	public bool isBlockedLeft;
	public bool isBlockedRight;
	public bool isBlockedAbove;
	public float pushRayLength = 0.75F;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		/*
		//Create push raycasts
		//Down
		RaycastHit hitDown;
		if(Physics.Raycast(transform.position,Vector3.down,out hitDown, pushRayLength)){
			isGrounded = true;
		} else {
			isGrounded = false;
		}
		
		//Left
		Debug.DrawRay(transform.position,new Vector3(-0.75F, 0.0F, 0.0F),Color.green);
			RaycastHit hitLeft;
		if(Physics.Raycast(transform.position,Vector3.left, out hitLeft, pushRayLength)){
			isBlockedLeft = true;
		} else {
			isBlockedLeft = false;
		}
		
		*/
		
		
		if(isGrounded){
			//Handle grounded movement
			//Move horizontally
			currentVelocity.x = moveInput * moveSpeed;
			//Prevent movement through floor
			currentVelocity.y = Mathf.Clamp (currentVelocity.y, 0.0F, Mathf.Infinity);
			//Jump
			if(jumpInput){
				currentVelocity.y += jumpPower;	
				}
		} else {
			//Fall due to gravity
		currentVelocity.y += gravityAcceleration;
		}
		//Restrict movement through walls
		if(isBlockedLeft){
			currentVelocity.x = Mathf.Clamp (currentVelocity.x, 0.0F, Mathf.Infinity);
		}
		if(isBlockedRight){
			currentVelocity.x = Mathf.Clamp (currentVelocity.x, -Mathf.Infinity, 0.0F);
		}
		if(isBlockedAbove){
			currentVelocity.y = Mathf.Clamp (currentVelocity.y, -Mathf.Infinity, 0.0F);
		}
		
		transform.Translate(currentVelocity);
	
		
		
	}
	
	void OnCollisionEnter(Collision hit){
		if(hit.collider.gameObject.tag == "Solid"){
			foreach(ContactPoint contact in hit.contacts){
				
				if(contact.normal.x > 0.75F){
					isBlockedLeft = true;
				}
				if(contact.normal.y > 0.75F){
					isGrounded = true;
				}
				if(contact.normal.y < -0.75F){
					isBlockedAbove = true;	
				}
				if(contact.normal.x < -0.75F){
					isBlockedRight = true;	
				}
				Debug.Log (contact.normal);
			}	
		}
	}
		
		
	
	void OnCollisionExit(Collision hit){
		//isGrounded = false;
		//isBlockedLeft = false;
		//isBlockedRight = false;
		//isBlockedAbove = false;
			/*	if(hit.collider.gameObject.tag == "Solid"){
			foreach(ContactPoint contact in hit.contacts){
				
				if(contact.normal.x > 0.5F){
					isBlockedLeft = false;
				}
				if(contact.normal.y > 0.5F){
					isGrounded = false;
				}
				if(contact.normal.y < -0.5F){
					isBlockedAbove = false;	
				}
				if(contact.normal.x < -0.5F){
					isBlockedRight = false;	
				}
			}
		}
		*/
	}

}
		
