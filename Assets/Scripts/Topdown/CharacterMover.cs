using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour {

public Vector3 moveDirection = new Vector3(0,0,0);	
public float moveSpeed = 3.0f;
	
CharacterController characterController;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	characterController = GetComponent<CharacterController>();
	characterController.Move(moveDirection * moveSpeed * Time.deltaTime); 
	}
}

