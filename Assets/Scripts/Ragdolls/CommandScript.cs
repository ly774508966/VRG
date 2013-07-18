using UnityEngine;
using System.Collections;

public class CommandScript : MonoBehaviour {
	
	//Scripts
	public SightScript sightScript;
	public ArmRotateScript armRotateScript;
	public MoveCharacterScript moveCharacterScript;
	public GunScript gunScript;
	public CharacterShellScript characterShellScript;
	
	//World
	public GameObject gameMaster;
	
	//Body Parts
	public GameObject leftArm;
	
	//Objectives
	public GameObject currentTarget;
	public Vector3 destinationCoordinates;
	
	//Modes (in order of priority)
	public bool avoidEngagement = false;
	public bool engageTargets = false;
	public bool moveToDestination = false;
	
	//Actions
	public bool raiseWeapon = false;
	public bool lowerWeapon = false;
	
	//States
	public bool weaponReady = false;
	public bool firingWeapon = false;
	
	
	
		
	void Start () {
	destinationCoordinates = transform.position;
	}
	
	void FixedUpdate () {
		
		//If target in sight, raise weapon; otherwise, lower weapon and stop firing
		if(sightScript.observedObject1)
		{
			if(!weaponReady) 
			{
			raiseWeapon = true;
			}
			} else {
			gunScript.inputFireWeapon = false;
			firingWeapon = false;
				if(leftArm.transform.eulerAngles.z > 0.1f)
				{
					lowerWeapon = true;
				}
		}
		
		//Action: Raise weapon
		if(raiseWeapon)
		{
			if(leftArm.transform.eulerAngles.z < 90.0f)
			{
				armRotateScript.inputAimAxis = 1.0f;	
				} else if(leftArm.transform.eulerAngles.z >= 90.0f && leftArm.transform.eulerAngles.z < 90.1f)
			{
				armRotateScript.inputAimAxis = 0.0f;
				raiseWeapon = false;
				weaponReady = true;
			}
		}
			
		//Action: Lower weapon
		if(lowerWeapon)
			{
				weaponReady = false;
				if(leftArm.transform.eulerAngles.z > 0.1f)
				{
					armRotateScript.inputAimAxis = -1.0f;
				} else if(leftArm.transform.eulerAngles.z >= 0 && leftArm.transform.eulerAngles.z < 0.1f)
				{
					armRotateScript.inputAimAxis = 0.0f;
					lowerWeapon = false;
				}
			}
			
		
		//A1. Avoid engagement
		if(avoidEngagement)
		{
			//Avoidance behavior
		//A2. Engage targets
		} else if(engageTargets && sightScript.observedObject1)
		{
		//Engagement behavior
			//B1. If outside of range, move into position
		if(Mathf.Abs (transform.root.position.x - sightScript.observedObject1.transform.position.x) > gunScript.weaponRange)
			{
				if(moveCharacterScript.inputMoveAxis != 1.0f)
				{
					moveCharacterScript.inputMoveAxis = 1.0f;
				}
			} else {
				if(moveCharacterScript.inputMoveAxis != 0)
				{
					moveCharacterScript.inputMoveAxis = 0.0f;
				}
			}
	
			//B2. If in position and weapon ready, start firing weapon
			if(moveCharacterScript.inputMoveAxis == 0.0f && weaponReady && !firingWeapon)
				{
					firingWeapon = true;
					Debug.Log (Time.frameCount + "FireInput" + transform.root.gameObject);
					StartCoroutine("ReadyFire");
					//gunScript.inputFireWeapon = true;
				}
		//A3. Move to destination
		} else if(moveToDestination)
		{
		//Move to destination and disable move mode
	if(transform.root.position.x - destinationCoordinates.x < -0.1f || transform.root.position.x - destinationCoordinates.x > 0.1f)
			{
		if(transform.root.position.x - destinationCoordinates.x > 0.1f)
			{
				if(transform.eulerAngles.y != 180.0f)
				{
					transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
				}
			} else if(transform.root.position.x - destinationCoordinates.x < -0.1f)
			{
				if(transform.eulerAngles.y != 0.0f)
				{
					transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
				}
				}
				moveCharacterScript.inputMoveAxis = 1.0f;
			} else {
				moveCharacterScript.inputMoveAxis = 0.0f;
				moveToDestination = false;
			}
		//A4. Stop moving
		} else {
			moveCharacterScript.inputMoveAxis = 0.0f;
		}
			
		
		
		
	}	
	
	void SetDestination(Vector3 _destination)
	{
	destinationCoordinates = _destination;
	moveToDestination = true;
	}
	
	IEnumerator WaitUntilReady(){
		int _i = -1;
		do{
			yield return null;
			_i++;
			Debug.Log (Time.frameCount + "Loop" + transform.root.gameObject + " " + _i);
	yield return null;
		} while(_i < (100 - characterShellScript.nerves));
		
	}
	
	IEnumerator ReadyFire(){
		yield return StartCoroutine("WaitUntilReady");
		gunScript.inputFireWeapon = true;
		//gameMaster.SendMessage ("ActionRequest",gameObject);
	}
}
	
	

