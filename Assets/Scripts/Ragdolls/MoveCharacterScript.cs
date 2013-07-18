using UnityEngine;
using System.Collections;

public class MoveCharacterScript : MonoBehaviour {
	
	public float moveSpeed = 1.0f;
	public float inputMoveAxis = 0.0f;
	public float horizontalPosition;
	//public bool facingRight = false;
	//public bool facingLeft = false;
	
	
	// Use this for initialization
	void Start () {
		
		/*
		if(transform.rotation.y == 0) {
			facingRight = true;
			facingLeft = false;
		}	else {	
			facingLeft = true;
			facingRight = false;
		}
		*/
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(inputMoveAxis != 0f) {
		float horizontalPosition = inputMoveAxis * Time.deltaTime * moveSpeed;
		transform.Translate (horizontalPosition, 0, 0);
		}
	}
	/*
	void ExecuteFaceLeft () {
	transform.eulerAngles = new Vector3(0,180,0);
	facingLeft = true;
	facingRight = false;
	}
	
	void ExecuteFaceRight () {
	transform.eulerAngles = new Vector3(0,0,0);
	facingLeft = false;
	facingRight = true;
	}
	*/
}
