using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour {
	
	public Vector3 destinationCoordinates;
	public bool setMovement = true;
	public ArmRotateScript armRotateScript;
	public MoveCharacterScript moveCharacterScript;
	public GunScript gunScript;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
			
	moveCharacterScript.inputMoveAxis = Mathf.Abs (Input.GetAxis ("Walk"));
		
	if(Input.GetAxis("Walk") != 0.0f)
		{
			if(Input.GetAxis ("Walk") > 0.0f)
			{
				transform.eulerAngles = new Vector3 (0.0f,0.0f,0.0f);
			} else if(Input.GetAxis ("Walk") < 0.0f)
			{
				transform.eulerAngles = new Vector3 (0.0f,180.0f,0.0f);
			}
			moveCharacterScript.inputMoveAxis = Mathf.Abs (Input.GetAxis ("Walk"));
		}
	
	armRotateScript.inputAimAxis = Input.GetAxis ("Vertical");
		
	if(Input.GetButtonDown ("Fire1"))
		{
			gunScript.inputFireWeapon = true;
		}
	if(Input.GetButtonUp("Fire1"))
		{
			gunScript.inputFireWeapon = false;
		}
	}
	
}
	
	

