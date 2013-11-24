using UnityEngine;
using System.Collections;

public class ScriptPlayerTargeting : MonoBehaviour {

	//Draw visible ray with length equal to character's weapon, which oscillates z rotation so that ray passes over the enemy
	//Take clicks as input and return break box (if any)
	//Use break box to run new result on hit location
	//Pass result through game master (and physics controller)

	public float weaponRange = 500.0F;
	//public int theAngle = 45;
	//public int segments = 10;

	public float zOffset = -1.6F; //No idea why this must be -1.6; should be 0

	//bool shotInputReady = true;

	//Z rotation angles
	int startAngle = -45;
	int finishAngle = 45;
	public int currentAngle = 0;

	bool isAscending = true;
	bool isDescending = false;

	//public int angleLerpSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//if(Time.frameCount % 5 == 0)
	//	{
		if(isAscending)
		{
			if(currentAngle <= finishAngle)
			{
				currentAngle += 1;
			}
			else
			{
				isAscending = false;
				isDescending = true;
				currentAngle -= 1;
			}
		}
		else if(isDescending)
		{
			if(currentAngle >= startAngle)
			{
				currentAngle -= 1;
			}
			else
			{
				isDescending = false;
				isAscending = true;
				currentAngle += 1;
			}
	//	}
		}
		//if(Input.GetKeyDown (KeyCode.R))
		  // {
			//RaycastSweep();
		//}
		//Update ray position
		//Ray indicatorRay = new Ray(transform.position, new Vector3(

		//Draw ray
		//Debug.DrawRay(indicatorRay.origin, indicatorRay.direction, Color.green);

		//if(shotInputReady)
		//{

		Ray hotRay = GetSelectionRay(currentAngle);
		Debug.DrawRay(hotRay.origin, hotRay.direction * weaponRange, Color.green);

		//}

	}
	/*
	void RaycastSweep()
		{
			Vector3 startPosition = transform.position;
			Vector3 targetPosition = Vector3.zero;

			int startAngle = (int)(-theAngle * 0.5);
			int finishAngle = (int) (theAngle * 0.5);

		int rayIncrement = (int)(theAngle / segments);

		RaycastHit hit;

		for(int i = startAngle; i < finishAngle; i+= rayIncrement)
		{
			targetPosition = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * distance;

			if(Physics.Linecast(startPosition, targetPosition, out hit))
			{
				Debug.Log ("Hit" + hit.collider.gameObject.name);
			}

			Debug.DrawLine (startPosition, targetPosition, Color.green);
		}
		}
		*/


	//Input start z rotation angle, finish z rotation angle, radius, speedConstant
	//Input- angle, radius
	//Output- x, y
	//Output- new Ray(transform.position, currentRayDirection, Color.green)
	//Lerp angle 0 to 1, 1 to 0 repeating
	Ray GetSelectionRay(int angleInDegrees)
	{
		//Convert degrees to radians
		float angle = angleInDegrees * Mathf.PI / 180;

		float y = Mathf.Sin (angle) * weaponRange;
		float x = Mathf.Sqrt (Mathf.Pow (weaponRange, 2) - Mathf.Pow (y, 2));
		Debug.Log ((Mathf.Cos (angle) * weaponRange).ToString () + " is equal to " + x.ToString());
		Vector3 rayDirection = new Vector3(x, y, transform.position.z + zOffset);
		Debug.Log (transform.position.ToString () + " <> " + rayDirection.ToString ());
		return new Ray(transform.position, rayDirection);
	}
}
