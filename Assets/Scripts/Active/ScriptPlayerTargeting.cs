using UnityEngine;
using System.Collections;

public class ScriptPlayerTargeting : MonoBehaviour {

	//Draw visible ray with length equal to character's weapon, which oscillates z rotation so that ray passes over the enemy
	//Take clicks as input and return break box (if any)
	//Use break box to run new result on hit location
	//Pass result through game master (and physics controller)

	public float speedConstant = 0.5F;

	public float cooldown = 1;
	public float cooldownTimer = 0;

	int activeLayer = 9;
	int layerMask;

	public ScriptCharacterSheet scriptCharacterSheet;
	public ScriptGameMaster scriptGameMaster;

	public LineRenderer lineRenderer;
	public KeyCode playerKeyCode;

	public float zOffset = -1.6F; //No idea why this must be -1.6; should be 0

	bool shotInputReady = true;
	bool shotLoaded = false;
	bool shotFired = false;

	//Z rotation angles
	float startAngle = -45F;
	float finishAngle = 45F;
	public float currentAngle = 0;
	Ray currentRay;

	bool oscillationIsPaused = false;

	enum OscillationMode
	{
		Ascending,
		Descending
		//Paused
	}

	OscillationMode oscillationMode;

	// Use this for initialization
	void Start () {

		scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();

		layerMask = 1 << activeLayer;

		lineRenderer = GetComponent<LineRenderer>();

		if(Random.value >= 0.5)
		{
			oscillationMode = OscillationMode.Ascending;
		}
		else
		{
			oscillationMode = OscillationMode.Descending;
		}

		currentAngle = (int)Mathf.Floor (Random.value * (finishAngle - startAngle) - finishAngle);

		if(scriptCharacterSheet == scriptGameMaster.selectedSheet)
		{
			playerKeyCode = KeyCode.Q;
		} else if(scriptCharacterSheet == scriptGameMaster.opposingSheet)
		{
			playerKeyCode = KeyCode.P;
		}
		else
		{
			Debug.Log("Invalid Player");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(shotFired)
		{
			cooldownTimer += Time.deltaTime;
			if(cooldownTimer >= cooldown)
			{
				shotFired = false;
				shotInputReady = true;
				lineRenderer.SetColors (Color.green, Color.green);
			}
		}

		if(scriptCharacterSheet.activeItem != null)
		{
		//Set currentAngle
			if(!oscillationIsPaused)
			{
			if(oscillationMode == OscillationMode.Ascending)
		{
			if(currentAngle <= finishAngle)
			{
				currentAngle += speedConstant;
			}
			else
			{
				oscillationMode = OscillationMode.Descending;
				currentAngle -= speedConstant;
			}
		}
			else if(oscillationMode == OscillationMode.Descending)
		{
			if(currentAngle >= startAngle)
			{
				currentAngle -= speedConstant;
			}
			else
			{
				oscillationMode = OscillationMode.Ascending;
				currentAngle += speedConstant;
			}
		}
			else
			{
					Debug.Log ("Invalid oscillation mode");
			}
			}
			else
			{
				//Oscillation paused, do not change angle
			}


			//Update ray
			currentRay = GetSelectionRay(currentAngle);
			//Debug.DrawRay(hotRay.origin, hotRay.direction
			  //            * scriptCharacterSheet.activeItem.itemStatProfile.maxRangeAspect
			    //         , Color.green);

			//Draw line
			lineRenderer.SetPosition(0, currentRay.origin);
			lineRenderer.SetPosition(1, currentRay.direction * scriptCharacterSheet.activeItem.itemStatProfile.maxRangeAspect + currentRay.origin);
			//Debug.Log (hotRay.origin.ToString () + "<>" + hotRay.direction.ToString());
		}

		//Input
		if(Input.GetKeyDown(playerKeyCode))
		{
			if(shotInputReady)
			{
				shotLoaded = true;
			}
		}

		if(Input.GetKey(playerKeyCode) && shotLoaded)
		{
			//shotInputReady = false;
			oscillationIsPaused = true;
		}
		else
		{
			oscillationIsPaused = false;
		}

		if(Input.GetKeyUp (playerKeyCode) && shotLoaded)
		{
			//Fire shot
			RaycastHit hit;
			if(Physics.Raycast(currentRay, out hit, scriptCharacterSheet.activeItem.itemStatProfile.maxRangeAspect, layerMask))
			{
				if(hit.collider.gameObject.tag == "Raycast")
				{

					Debug.Log ("Hit " + hit.collider.gameObject.name);
				}
				else
				{
					Debug.Log (hit.collider.gameObject.name);
				}
			}


			//Set as not ready
			shotInputReady = false;
			shotLoaded = false;
			shotFired = true;
			lineRenderer.SetColors(Color.red, Color.red);
			cooldownTimer = 0;

		}

	}

	Ray GetSelectionRay(float angleInDegrees)
	{
		//Convert degrees to radians
		float angle = angleInDegrees * Mathf.PI / 180;

		//Get weapon range
		float weaponRange = scriptCharacterSheet.activeItem.itemStatProfile.maxRangeAspect;


		float y = Mathf.Sin (angle) * weaponRange;
		float x = Mathf.Cos (angle) * weaponRange;
		//Debug.Log ((Mathf.Cos (angle) * weaponRange).ToString () + " is equal to " + x.ToString());
		if(playerKeyCode == KeyCode.Q)
		{
		Vector3 rayDirection = new Vector3(x, y, transform.position.z + zOffset);
		return new Ray(transform.position, rayDirection);
		}
		else if(playerKeyCode == KeyCode.P)
		{
			Vector3 rayDirection = new Vector3(-x, y, transform.position.z + zOffset);
			return new Ray(transform.position, rayDirection);
		}
		else
		{
			Debug.Log ("Invalid Character");
			return new Ray();
		}
	}

	void PassShotInfo()
	{

	}
}
