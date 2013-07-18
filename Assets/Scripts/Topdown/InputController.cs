using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	
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
		moveInput = new Vector3(Input.GetAxis ("P01LeftX"),0,Input.GetAxis ("P01LeftY"));
		characterMover.moveDirection = moveInput;
		shotInput = new Vector3(Input.GetAxis ("P01RightX"),0,Input.GetAxis ("P01RightY"));
		shooter.shotVector = shotInput;
		fireInput = Input.GetAxis ("P01RT");
		shooter.isFiring = fireInput > 0.5F;
	}
}
