using UnityEngine;
using System.Collections;

public class InputController3 : MonoBehaviour {
	
	public Vector3 moveInput = new Vector3(0,0,0);
	public Vector3 shotInput = Vector3.zero;
	public float fireInput;

	
	CharacterMover characterMover;
	Shooter shooter;
	
	// Use this for initialization
	void Start () {
	characterMover = GetComponent<CharacterMover>();
	shooter = GetComponent<Shooter>();
	}
	
	// Update is called once per frame
	void Update () {
	moveInput = new Vector3(Input.GetAxis ("P03LeftX"),0,Input.GetAxis ("P03LeftY"));
	characterMover.moveDirection = moveInput;
	shotInput = new Vector3(Input.GetAxis ("P03RightX"),0,Input.GetAxis ("P03RightY"));
	shooter.shotVector = shotInput;
		fireInput = Input.GetAxis ("P03RT");
		shooter.isFiring = fireInput > 0.5F;
	}
}
